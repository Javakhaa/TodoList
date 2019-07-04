using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models.Account
{
    public class ConfirmNewPassword
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}