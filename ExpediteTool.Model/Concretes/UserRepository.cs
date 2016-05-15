using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Utilities;
using ExpediteTool.Model.DataTransfer;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.Concretes
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// The log service
        /// </summary>
        private ILogService _logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="ExistUserException"></exception>
        public ActionResult Register(UsersInfoDto item)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var single = context.HotLot_Users.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    if (single != null)
                        throw new ExistUserException();

                    HotLot_Users user = new HotLot_Users();
                    user.UserName = item.UserName;
                    user.Firstname = item.Firstname;
                    user.Lastname = item.Lastname;
                    user.Password = AppCipher.EncryptCipher(item.Password, null);
                    user.Email = item.Email;
                    user.RoleId = item.RoleId;
                    user.Status = (byte)item.Status;
                    user.IsComfirmed = item.IsConfirmed;
                    user.Guid = item.Guid;
                    user.RegistrationDate = DateTime.Now;
                    user.LastLogin = DateTime.Now;

                    context.Entry<HotLot_Users>(user).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    return ActionResult.SUCCESS;
                }
                catch (ExistUserException eex)
                {
                    _logService.Error(eex.Message, eex);
                    return ActionResult.EXIST;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return ActionResult.UNKNOWN;
                }
            }
        }

        /// <summary>
        /// User login in the application
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns>
        /// SUCCESS: login success
        /// NOTEXIST: user not exist in application
        /// UNKNOWN: has error in while login
        /// CONNECTION: cannot connect database
        /// </returns>
        /// <exception cref="NullUserException">User not exist</exception>
        /// <exception cref="System.Exception"></exception>
        /// <exception cref="DeactivateUserException">User de-activate</exception>
        /// <exception cref="LockUserException">User lock-out</exception>
        public ActionResult Login(string userName, string pass)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    string passCipher = AppCipher.EncryptCipher(pass, null);
                    var single = context.HotLot_Users.Where(x => x.UserName == userName).FirstOrDefault();
                    if (single == null)
                        throw new NullUserException("User not exist");

                    if (single.Password != passCipher)
                        throw new Exception();

                    if (single.Status == 1)//user deactiving
                        throw new DeactivateUserException("User de-activate");

                    if (single.Status == 2)
                        throw new LockUserException("User lock-out");

                    if (single.IsComfirmed == false)
                        throw new LockUserException("User not comfrim email");

                    if (single != null)
                    {
                        single.LastLogin = DateTime.Now;
                        context.Entry<HotLot_Users>(single).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }

                    return ActionResult.SUCCESS;
                }
                catch (LockUserException lex)
                {
                    _logService.Error(lex.Message, lex);
                    return ActionResult.LOCKED;
                }
                catch (DeactivateUserException dex)
                {
                    _logService.Error(dex.Message, dex);
                    return ActionResult.DEACIVATE;
                }
                catch (NullUserException eex)
                {
                    _logService.Error(eex.Message, eex);
                    return ActionResult.NOTEXIST;
                }
                catch (EntityException eeex)
                {
                    _logService.Error(eeex.Message, eeex);
                    return ActionResult.CONNECTION;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return ActionResult.UNKNOWN;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UsersInfoDto GetInfoUser(string userName)
        {
            UsersInfoDto result = null;
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var item = context.HotLot_Users.Single(x => x.UserName == userName);
                    if (item != null)
                    {
                        result = new UsersInfoDto
                        {
                            UserId = item.UserId,
                            UserName = item.UserName,
                            Firstname = item.Firstname,
                            Lastname = item.Lastname,
                            Email = item.Email,
                            Role = new RoleUserDto
                            {
                                RoleId = item.HotLot_Roles.RoleId,
                                RoleName = item.HotLot_Roles.RoleName,
                                Description = item.HotLot_Roles.Description,
                            },
                            LastLogin = item.LastLogin,
                            RegistrationDate = item.LastLogin,
                            Status = (UserStatus)item.Status,
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsersInfoDto> FindAll()
        {
            IEnumerable<UsersInfoDto> results;
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    results = (from item in context.HotLot_Users
                               select new UsersInfoDto()
                               {
                                   UserId = item.UserId,
                                   UserName = item.UserName,
                                   Firstname = item.Firstname,
                                   Lastname = item.Lastname,
                                   LastLogin = item.LastLogin,
                                   Email = item.Email,
                                   Status = (UserStatus)item.Status,
                                   RoleId = item.RoleId,
                                   Role = new RoleUserDto
                                   {
                                       RoleId = item.HotLot_Roles.RoleId,
                                       RoleName = item.HotLot_Roles.RoleName,
                                       Description = item.HotLot_Roles.Description,
                                   },
                                   RegistrationDate = item.RegistrationDate,
                               }).ToList();

                    return results;
                }
                catch (NullUserException eex)
                {
                    _logService.Error(eex.Message, eex);
                    return null;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ActionResult UserUpdate(UsersInfoDto item)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var single = context.HotLot_Users.Single(x => x.UserId == item.UserId);
                    if (single != null)
                    {
                        single.Firstname = item.Firstname;
                        single.Lastname = item.Lastname;
                        single.Email = item.Email;
                        context.Entry<HotLot_Users>(single).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        return ActionResult.SUCCESS;
                    }
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                }
            }
            return ActionResult.FAIL;
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ActionResult AdminUpdate(int userId, byte status, int role = 0)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var single = context.HotLot_Users.Single(x => x.UserId == userId);
                    if (role > 0)
                        single.RoleId = role;

                    single.Status = status;
                    context.Entry<HotLot_Users>(single).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    return ActionResult.SUCCESS;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return ActionResult.FAIL;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UsersInfoDto Single(int userId)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var single = context.HotLot_Users.Single(x => x.UserId == userId);
                    return new UsersInfoDto()
                    {
                        UserId = single.UserId,
                        UserName = single.UserName,
                        Firstname = single.Firstname,
                        Lastname = single.Lastname,
                        LastLogin = single.LastLogin,
                        Email = single.Email,
                        Status = (UserStatus)single.Status,
                        RoleId = single.RoleId,
                        Role = new RoleUserDto
                        {
                            RoleName = single.HotLot_Roles.RoleName,
                            Description = single.HotLot_Roles.Description,
                        },
                        RegistrationDate = single.RegistrationDate,
                    };
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public RoleType Role(string userName)
        {
            RoleType result = RoleType.NormalUser;

            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    var roleId = context.HotLot_Users.Single(x => x.UserName == userName).RoleId;
                    if (roleId == 1)
                        result = RoleType.NormalUser;
                    if (roleId == 2)
                        result = RoleType.Contributor;
                    if (roleId == 3)
                        result = RoleType.SuperAdmin;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    result = RoleType.NormalUser;
                }
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult ChangePassword(string userName, string oldPass, string newPass)
        {
            ActionResult result = ActionResult.FAIL;

            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    string passCipher = AppCipher.EncryptCipher(oldPass, null);
                    var single = context.HotLot_Users.Single(x => x.UserName == userName);
                    if (single.Password != passCipher)
                        throw new Exception();//pass invalidation

                    passCipher = AppCipher.EncryptCipher(newPass);
                    single.Password = passCipher;

                    context.Entry<HotLot_Users>(single).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    result = ActionResult.SUCCESS;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    result = ActionResult.FAIL;
                }
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="newPass"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(int userID, string newPass, out string userName)
        {
            ActionResult result = ActionResult.FAIL;
            userName = String.Empty;

            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    string passCipher = AppCipher.EncryptCipher(newPass, null);
                    var single = context.HotLot_Users.Single(x => x.UserId == userID);

                    single.Password = passCipher;

                    context.Entry<HotLot_Users>(single).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    result = ActionResult.SUCCESS;
                    userName = single.UserName;
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    result = ActionResult.FAIL;
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ActionResult Add(UsersInfoDto TEntity, string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ActionResult Update(UsersInfoDto TEntity, string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Confirms the register.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public ActionResult ConfirmRegister(string guid)
        {
            var result = ActionResult.NOTEXIST;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var user = context.HotLot_Users.Single(w => w.Guid == guid && w.IsComfirmed == false);
                    user.IsComfirmed = true;
                    context.Entry<HotLot_Users>(user).State = System.Data.Entity.EntityState.Modified;
                    result = context.SaveChanges() > 0 ? ActionResult.SUCCESS : ActionResult.FAIL;
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Requests the recover password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="passwordHas">The password has.</param>
        /// <returns></returns>
        public ActionResult RequestRecoverPassword(string email, string guid, string passwordHas)
        {
            var result = ActionResult.NOTEXIST;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var user = context.HotLot_Users.Single(w => w.Email == email);
                    user.Guid = guid;
                    user.PasswordHasTemp = passwordHas;
                    context.Entry<HotLot_Users>(user).State = System.Data.Entity.EntityState.Modified;
                    result = context.SaveChanges() > 0 ? ActionResult.SUCCESS : ActionResult.FAIL;
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Confirms the recover password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public ActionResult ConfirmRecoverPassword(string email, string guid)
        {
            var result = ActionResult.NOTEXIST;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var user = context.HotLot_Users.Single(w => w.Email == email && w.Guid == guid);
                    user.Guid = null;
                    user.Password = user.PasswordHasTemp;
                    user.PasswordHasTemp = null;

                    context.Entry<HotLot_Users>(user).State = System.Data.Entity.EntityState.Modified;
                    result = context.SaveChanges() > 0 ? ActionResult.SUCCESS : ActionResult.FAIL;
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, ex);
            }

            return result;
        }
    }
}
