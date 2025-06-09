using dotnet.challenge.api.Utils.Dtos;
using dotnet.challenge.api.Utils.Forms;
using dotnet.challenge.data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet.challenge.api.Services.Users
{
    public interface IUserService
    {
        Task<Result<UserDto, Error>> CreateUserAsync(UserForm user);
        Task<Result<UserDto, Error>> UpdateUserAsync(Guid id, UserForm user);
        Task<Result<UserDto, Error>> GetUserAsync(Guid userId);
        Task<Result<IEnumerable<UserDto>, Error>> GetUsersAsnyc(int pageNumber, int pageSize);
        Task<Result<UserDto, Error>> DeleteUserAsync(Guid userId);
    }
}
