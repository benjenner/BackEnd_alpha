using Authentication.Entities;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Factories
{
    public class AppUserFactory
    {
        public static AppUserEntity Map(UserRegistrationForm form)
        {
            if (form == null)
            {
                return null;
            }
            var appUserEntity = new AppUserEntity
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                UserName = form.Email
            };

            return appUserEntity;
        }

        public static AppUserEntity Map(CreateUserForm form)
        {
            if (form == null)
            {
                return null;
            }
            var appUserEntity = new AppUserEntity
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                UserName = form.Email,
                JobTitle = form.JobTitle,
                PhoneNumber = form.PhoneNumber,
                Address = new AppUserAddressEntity
                {
                    StreetName = form.Address,
                    City = form.City,
                    PostalCode = form.PostalCode,
                }
            };

            return appUserEntity;
        }

        public static AppUserEntity Map(UpdateUserForm form, AppUserEntity entity)
        {
            if (form == null)
            {
                return null;
            }

            entity.Id = form.UserId;
            entity.FirstName = form.FirstName;
            entity.LastName = form.LastName;
            entity.Email = form.Email;
            entity.JobTitle = form.JobTitle;
            entity.PhoneNumber = form.PhoneNumber;

            if (entity.Address == null)
            {
                entity.Address = new AppUserAddressEntity
                {
                    UserId = entity.Id,
                };
            }
            entity.Address.UserId = form.UserId;
            entity.Address.City = form.City;
            entity.Address.PostalCode = form.PostalCode;
            entity.Address.StreetName = form.StreetName;

            return entity;
        }

        public static AppUser Map(AppUserEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var appUser = new AppUser
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                JobTitle = entity.JobTitle,
                PhoneNumber = entity.PhoneNumber,
                Role = entity.RoleName,
                Address = entity.Address.StreetName,
                City = entity.Address.City,
                PostalCode = entity.Address.PostalCode,
            };

            return appUser;
        }
    }
}