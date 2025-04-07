using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Factories
{
    public class RoleFactory
    {
        public static Role Map(IdentityRole identityRole)
        {
            if (identityRole == null)
            {
                return null;
            }

            var role = new Role
            {
                Id = identityRole.Id,
                Name = identityRole.Name,
            };

            return role;
        }
    }
}