using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpediteTool.Web.Constants
{
    public static class Message
    {
        //User
        public const string MSG_USER_INVALID = "Invalid login attempt";
        public const string MSG_USER_NON_EXITST = "User not exist in system";
        public const string MSG_USER_DE_ACTIVATE = "Account de-activate, contact Administrators please!";
        public const string MSG_USER_LOCKED = "Account lock-out, contact Administrators please!";

        //Database
        public const string MSG_CAN_NOT_DATABASE = "Cannot connect database";
    }
}