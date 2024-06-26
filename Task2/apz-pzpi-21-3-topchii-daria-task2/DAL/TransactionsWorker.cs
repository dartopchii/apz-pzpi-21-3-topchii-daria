﻿using Core.DataClasses;
using DAL.Abstractions.Interfaces;
using Microsoft.Extensions.Logging;

namespace DAL;

public class TransactionsWorker : ITransactionsWorker
{
    private readonly AppDbContext context;

    private readonly ILogger<TransactionsWorker> logger;

    public TransactionsWorker(AppDbContext context, ILogger<TransactionsWorker> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<T> RunAsTransaction<T>(Func<Task<T>> method)
    {
        T result = default;
        await using var transaction = await this.context.Database.BeginTransactionAsync();
        try
        {
            result = await method.Invoke();

            if (result is ExceptionalResult { IsSuccess: false } exceptionalResult)
            {
                await transaction.RollbackAsync();
                this.logger.LogInformation($"Wrong behaviour while executing transaction. Transaction rollbacked. Result message: {exceptionalResult.ExceptionMessage}");
            }
            else
            {
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            this.logger.LogError($"Exception happened while executing transaction. Transaction rollbacked. Exception message: {ex.Message} Exception place: {ex.Source} Exception stack trace {ex.StackTrace}");
            var exceptionalResult = new ExceptionalResult(false, ex.Message);

            result = exceptionalResult is T exceptionalResultT ? exceptionalResultT : result;
        }

        return result;
    }
}