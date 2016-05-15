using System;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.DataTransfer
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersInfoDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>
        /// The firstname.
        /// </value>
        public string Firstname { get; set; }
        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        public string Lastname { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public UserStatus Status { get; set; }
        /// <summary>
        /// Gets the show status.
        /// </summary>
        /// <value>
        /// The show status.
        /// </value>
        public string ShowStatus
        {
            get
            {
                string result = "De-Activate";
                switch (Status)
                {
                    case UserStatus.De_activated:
                        result = "Activate";
                        break;
                    case UserStatus.Activated:
                        result = "Lock";
                        break;
                    case UserStatus.Locked:
                        result = "De-Activate";
                        break;
                }
                return result;
            }
        }
        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        /// <value>
        /// The registration date.
        /// </value>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Gets or sets the last login.
        /// </summary>
        /// <value>
        /// The last login.
        /// </value>
        public DateTime LastLogin { get; set; }

        public RoleUserDto Role { get; set; }

        public bool IsConfirmed { get; set; }

        public string Guid { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
    }
}
