using dotnet.challenge.api.Errors;
using dotnet.challenge.api.Utils.Dtos;
using dotnet.challenge.api.Utils.Forms;
using dotnet.challenge.api.Utils.Mapper;
using dotnet.challenge.data.Cache;
using dotnet.challenge.data.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnet.challenge.api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ISimpleObjectCache<Guid, User> _dataContext;
        private readonly ILogger<UserService> _logger;
        public UserService(ISimpleObjectCache<Guid, User> simpleObjectCache, ILogger<UserService> logger)
        {
            _dataContext = simpleObjectCache;
            _logger = logger;
        }
        public async Task<Result<UserDto, Error>> CreateUserAsync(UserForm userForm)
        {
            var users = await _dataContext.GetAllAsync();
            if (users.Any(user => user.Email.Equals(userForm.Email, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogError($"The user email {userForm.Email} is already registered");
                return Result<UserDto, Error>.Failure(UserErrors.UserEmailIsAlreadyRegistered);
            }
            var userToInsert = new User
            {
                Email = userForm.Email,
                DateOfBirth = DateTime.Parse(userForm.DateOfBirth),
                FirstName = userForm.FirstName,
                LastName = userForm.LastName,
                Id = Guid.NewGuid()
            };
            await _dataContext.AddAsync(userToInsert.Id, userToInsert);
            return Result<UserDto, Error>.Success(userToInsert.MapToUserDto());
        }

        public async Task<Result<UserDto, Error>> DeleteUserAsync(Guid userId)
        {
            var existing = await _dataContext.GetAsync(userId);
            if (existing == null)
            {
                _logger.LogError($"The user with Id {userId} was not found");
                return Result<UserDto, Error>.Failure(UserErrors.UserNotFound);
            }

            await _dataContext.DeleteAsync(userId);
            return Result<UserDto, Error>.Success(new UserDto());
        }

        public async Task<Result<UserDto, Error>> GetUserAsync(Guid userId)
        {
            var user = await _dataContext.GetAsync(userId);
            return user == null
                ? Result<UserDto, Error>.Failure(UserErrors.UserNotFound)
                : Result<UserDto, Error>.Success(user.MapToUserDto());
        }

        public async Task<Result<IEnumerable<UserDto>, Error>> GetUsersAsnyc(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogError($"Invalid pagination params provided. Pagenumber: {pageNumber}, PageSize: {pageSize}");
                return Result<IEnumerable<UserDto>, Error>.Failure(UserErrors.InvalidPagination);
            }

            var users = await _dataContext.GetAllAsync();
            var pagedUsers = users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .MapToUsersDto();

            return Result<IEnumerable<UserDto>, Error>.Success(pagedUsers);
        }

        public async Task<Result<UserDto, Error>> UpdateUserAsync(Guid id, UserForm userForm)
        {
            var existingUser = await _dataContext.GetAsync(id);
            if (existingUser == null)
            {
                _logger.LogError($"The user with Id {id} was not found");
                return Result<UserDto, Error>.Failure(UserErrors.UserNotFound);
            }

            if (!existingUser.Email.Equals(userForm.Email, StringComparison.OrdinalIgnoreCase))
            {
                var users = await _dataContext.GetAllAsync();
                if (users.Any(user => user.Email.Equals(userForm.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogError($"The user email {userForm.Email} is already registered");
                    return Result<UserDto, Error>.Failure(UserErrors.UserEmailIsAlreadyRegistered);
                }
            }

            existingUser.FirstName = userForm.FirstName;
            existingUser.LastName = userForm.LastName;
            existingUser.Email = userForm.Email;
            existingUser.DateOfBirth = DateTime.Parse(userForm.DateOfBirth);

            await _dataContext.UpdateAsync(existingUser.Id, existingUser);
            return Result<UserDto, Error>.Success(existingUser.MapToUserDto());
        }
    }
}
