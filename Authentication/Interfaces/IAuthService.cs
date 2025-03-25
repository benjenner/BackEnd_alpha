using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResult> SignInAsync(SignInForm form);

        Task<IdentityResult> SignUpAsync(SignUpForm form);
    }
}