using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.Concretes
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// The log service
        /// </summary>
        private ILogService _logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        public RoleRepository(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleUserDto> FindAll()
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    return (from item in context.HotLot_Roles
                            select new RoleUserDto()
                            {
                                RoleId = item.RoleId,
                                RoleName = item.RoleName,
                                Description = item.Description,
                            }).ToList();
                }
                catch (Exception ex)
                {
                    _logService.Error(ex.Message, ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// Inserts the specified t entity.
        /// </summary>
        /// <param name="TEntity">The t entity.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ActionResult Add(RoleUserDto TEntity, string username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified t entity.
        /// </summary>
        /// <param name="TEntity">The t entity.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ActionResult Update(RoleUserDto TEntity, string username)
        {
            throw new NotImplementedException();
        }
    }
}
