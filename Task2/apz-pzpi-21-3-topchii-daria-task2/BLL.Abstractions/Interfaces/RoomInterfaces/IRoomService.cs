﻿using System.Linq.Expressions;
using Core.DataClasses;
using Core.Models.RoomModels;
using Core.Models.UserModels;

namespace BLL.Abstractions.Interfaces.RoomInterfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomModel>> GetByConditions(params Expression<Func<RoomModel, bool>>[] conditions);

        Task<RoomModel> GetRoomById(Guid id);

        Task<OptionalResult<RoomModel>> Create(RoomCreateModel roomModel);

        Task<OptionalResult<RoomModel>> Update(RoomUpdateModel roomModel);

        Task<ExceptionalResult> Delete(Guid id);

        Task<ExceptionalResult> ApproveRoom(Guid id);
    }
}
