using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;

namespace AdminVymastiSi.ViewModels
{
    public class MasterPageViewModel : DotvvmViewModelBase
    {
        public string CurrentUser
        {
            get { return Context.GetOwinContext().Authentication.User.Identity.Name; }
        }
        public void SignOut()
        {
            Context.GetOwinContext().Authentication.SignOut();
            Context.RedirectToRoute("Default");
        }
    }
}
