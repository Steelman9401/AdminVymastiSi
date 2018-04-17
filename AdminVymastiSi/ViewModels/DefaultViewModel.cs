using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using AdminVymastiSi.Repositories;
using System.ComponentModel.DataAnnotations;

namespace AdminVymastiSi.ViewModels
{
    public class DefaultViewModel : DotvvmViewModelBase
    {
        [Required(ErrorMessage = "Uživatelské jméno nemůže být prázdné!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Heslo nemůže být prázdné!")]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public async Task SignIn()
        {
            ValidationService ValRep = new ValidationService();           
            if (await ValRep.UserExist(Username,Password))
            {
                var identity = CreateIdentity();
                Context.GetAuthentication().SignIn(identity);
                Context.RedirectToUrl("Spider");
            }
            else
            {
                ErrorMessage = "Nesprávné údaje!";
            }
        }

        private ClaimsIdentity CreateIdentity()
        {
            var identity = new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.NameIdentifier,Username),
                new Claim(ClaimTypes.Role, "administrator"),
                },
                DefaultAuthenticationTypes.ApplicationCookie);
            return identity;

        }
    }
}
