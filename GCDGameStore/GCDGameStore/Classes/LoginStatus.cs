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
        private const string MEMBER_ID = "MemberId";
        private const string LOGIN = "Login";
        private const string EMPLOYEE_LOGIN = "EmployeeLogin";
        private const string EMPLOYEE_ID = "EmployeeId";

        public LoginStatus (IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public bool IsNotLoggedIn()
        {
            var httpContext = _accessor.HttpContext;
            var memberId = httpContext.Session.GetString(MEMBER_ID);

            if (memberId == null || memberId == "")
            {
                return true;
            }

            return false;
        }

        public void MemberLogin(string memberId)
        {
            var httpContext = _accessor.HttpContext;

            httpContext.Session.SetString(LOGIN, "true");
            httpContext.Session.SetString(MEMBER_ID, memberId);
        }

        public void MemberReset(string memberId)
        {
            var httpContext = _accessor.HttpContext;

            httpContext.Session.SetString(LOGIN, "reset");
            httpContext.Session.SetString(MEMBER_ID, memberId);
        }

        public bool MemberIsResetting()
        {
            var httpContext = _accessor.HttpContext;

            return httpContext.Session.GetString(LOGIN) == "reset";
        }

        public void MemberLogout()
        {
            var httpContext = _accessor.HttpContext;

            httpContext.Session.SetString(LOGIN, "false");
            httpContext.Session.SetString(MEMBER_ID, "");
        }

        /// <summary>
        ///     Returns an int of the memberId, if session value is null or empty, returns -1 instead.
        /// </summary>
        /// <returns></returns>
        public int GetMemberId()
        {
            var httpContext = _accessor.HttpContext;
            var memberId = httpContext.Session.GetString(MEMBER_ID);

            if (memberId != null && memberId != "")
            {
                return Convert.ToInt32(memberId);
            }

            return -1;
        }

        public bool IsEmployee()
        {
            var httpContext = _accessor.HttpContext;

            if (httpContext.Session.GetString(EMPLOYEE_LOGIN) == "true")
            {
                return true;
            }

            return false;
        }

        public void EmployeeLogin(string employeeId)
        {
            var httpContext = _accessor.HttpContext;
            httpContext.Session.SetString(EMPLOYEE_LOGIN, "true");
            httpContext.Session.SetString(EMPLOYEE_ID, employeeId);
        }

        public void EmployeeLogout()
        {
            var httpContext = _accessor.HttpContext;
            httpContext.Session.SetString(EMPLOYEE_LOGIN, "false");
            httpContext.Session.SetString(EMPLOYEE_ID, "");
        }


        /// <summary>
        ///     Returns an int of the employeeId, if session value is null or empty, returns -1 instead.
        /// </summary>
        /// <returns></returns>
        public int GetEmployeeId()
        {
            var httpContext = _accessor.HttpContext;
            var employeeId = httpContext.Session.GetString(EMPLOYEE_ID);

            if (employeeId != null && employeeId != "")
            {
                return Convert.ToInt32(employeeId);
            }

            return -1;
        }
    }
}
