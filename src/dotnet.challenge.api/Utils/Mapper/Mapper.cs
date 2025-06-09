using dotnet.challenge.api.Utils.Dtos;
using dotnet.challenge.data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dotnet.challenge.api.Utils.Mapper
{
    public static class Mapper
    {
        public static User MapToUser(this UserDto user)
        {
            return new User
            {
                DateOfBirth = DateTime.Parse(user.DateOfBirth),
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = Guid.Parse(user.Id)
            };
        }

        public static UserDto MapToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth.ToString("yyyy-MM-dd")
            };
        }

        public static IEnumerable<UserDto> MapToUsersDto(this IEnumerable<User> users)
        {
            return users.Select(user => user.MapToUserDto());
        }
    }
}
