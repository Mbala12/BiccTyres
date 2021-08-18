using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSales.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }

    }
}