using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.Concretes;
using ExpediteTool.Model.DataTransfer;
using System;
using System.Security.Principal;

namespace ExpediteTool.Web
{
    public static class IdentityExtension
    {
        public static UsersInfoDto GetUserInfo(this IIdentity identity, IUserRepository userRepository)
        {
            try
            {
                if (identity != null && identity.IsAuthenticated)
                {
                    return userRepository.GetInfoUser(identity.Name);
                }
            }
            catch
            {
            }
            return null;
        }

        public static ActionResult ChangePassword(this IIdentity identity, IUserRepository userRepository, string oldPass, string newPass)
        {
            return userRepository.ChangePassword(identity.Name, oldPass, newPass);
        }

        public static RoleType IsRole(this IPrincipal principal, IUserRepository userRepository)
        {
            RoleType result = RoleType.NormalUser;
            try
            {
                if (principal.Identity != null && principal.Identity.IsAuthenticated)
                {
                    result = userRepository.Role(principal.Identity.Name);
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool IsRoleAdmin(this IPrincipal principal, IUserRepository userRepository)
        {
            bool result = false;
            try
            {
                if (principal.Identity != null && principal.Identity.IsAuthenticated)
                {
                    var typeRole = userRepository.Role(principal.Identity.Name);
                    if (typeRole == Model.RoleType.SuperAdmin)
                        result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool IsRoleUser(this IPrincipal principal, IUserRepository userRepository)
        {
            bool result = false;
            try
            {
                if (principal.Identity != null && principal.Identity.IsAuthenticated)
                {
                    var typeRole = userRepository.Role(principal.Identity.Name);
                    if (typeRole == Model.RoleType.NormalUser)
                        result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool IsRoleContributor(this IPrincipal principal, IUserRepository userRepository)
        {
            bool result = false;
            try
            {
                if (principal.Identity != null && principal.Identity.IsAuthenticated)
                {
                    var typeRole = userRepository.Role(principal.Identity.Name);
                    if (typeRole == Model.RoleType.Contributor)
                        result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        private static bool isAdmin = false;
        public static bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }

        private static bool isContributor = false;
        public static bool IsContributor
        {
            get
            {
                return isContributor;
            }
            set
            {
                isContributor = value;
            }
        }
    }
}