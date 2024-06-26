﻿using Core.DataClasses;
using Core.Models.UserModels;

namespace BLL.Abstractions.Interfaces.UserInterfaces
{
    public interface IJwtService
    {
        string GenerateJwt(UserModel user);

        OptionalResult<Guid> ValidateJwt(string jwt);
    }
}
