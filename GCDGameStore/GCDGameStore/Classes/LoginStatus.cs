using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDGameStore.Classes
{
    public class LoginStatus
    {
        private readonly IHttpContextAccessor _accessor;

        public LoginStatus (IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public bool IsNotLoggedIn()
        {
            var httpContext = _accessor.HttpContext;
            var memberId = httpContext.Session.GetString("MemberId");

            if (memberId == null || memberId == "")
            {
                return true;
            }

            return false;
        }

        public void MemberLogin(string memberId)
        {
            var httpContext = _accessor.HttpContext;

            httpContext.Session.SetString("Login", "true");
            httpContext.Session.SetString("MemberId", memberId);
        }

        public void MemberLogout()
        {
            var httpContext = _accessor.HttpContext;

            httpContext.Session.SetString("Login", "false");
            httpContext.Session.SetString("MemberId", "");
        }

        public bool IsEmployee()
        {
            var httpContext = _accessor.HttpContext;

            if (httpContext.Session.GetString("EmployeeLogin") == "true")
            {
                return true;
            }

            return false;
        }

        public void EmployeeLogin()
        {
            var httpContext = _accessor.HttpContext;
            httpContext.Session.SetString("EmployeeLogin", "true");
        }

        public void EmployeeLogout()
        {
            var httpContext = _accessor.HttpContext;
            httpContext.Session.SetString("EmployeeLogin", "false");
        }
    }
}
