using Authentication.Models;
using Domain.Models;
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
        Task<ServiceResult> SignInAsync(SignInForm form);

        Task<ServiceResult> SignUpAsync(SignUpForm form);
    }
}