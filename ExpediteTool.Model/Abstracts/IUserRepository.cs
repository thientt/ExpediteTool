using ExpediteTool.Model.DataTransfer;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserRepository : IRepository<UsersInfoDto>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ActionResult Register(UsersInfoDto item);

        /// <summary>
        /// User login in the application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns>
        /// SUCCESS: login success
        /// NOTEXIST: user not exist in application
        /// UNKNOWN: has error in while login
        /// </returns>
        ActionResult Login(string userName, string pass);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UsersInfoDto GetInfoUser(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ActionResult UserUpdate(UsersInfoDto item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ActionResult AdminUpdate(int userId, byte status, int role = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UsersInfoDto Single(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        RoleType Role(string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        ActionResult ChangePassword(string userName, string oldPass, string newPass);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        ActionResult ResetPassword(int userID, string newPass, out string userName);

        /// <summary>
        /// Confirms the register.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        ActionResult ConfirmRegister(string guid);

        /// <summary>
        /// Requests the recover password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="passwordHas">The password has.</param>
        /// <returns></returns>
        ActionResult RequestRecoverPassword(string email, string guid, string passwordHas);

        /// <summary>
        /// Confirms the recover password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        ActionResult ConfirmRecoverPassword(string email, string guid);
    }
}
