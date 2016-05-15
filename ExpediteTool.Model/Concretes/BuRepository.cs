using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.DataTransfer;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ExpediteTool.Utilities;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.Concretes
{
    /// <summary>
    /// 
    /// </summary>
    public class BuRepository : IBuRepository
    {
        private ILogService _logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuRepository"/> class.
        /// </summary>
        /// <param name="logService">The log service.</param>
        public BuRepository(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BuDto> FindAll()
        {
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var items = (from item in context.Hotlot_BUs
                                 select new BuDto()
                                 {
                                     BuId = item.BUId,
                                     BuName = item.BUName,
                                     Description = item.Description,
                                 }).ToList();
                    return items;
                }
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Inserts the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ActionResult Add(BuDto TEntity, string userName)
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
        public ActionResult Update(BuDto TEntity, string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the total bu with actived.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TotalBuDto> GetTotalBuWithActived()
        {
            StringBuilder sqlCommand = new StringBuilder();
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                sqlCommand.AppendLine("SELECT");
                sqlCommand.AppendLine("BUName,");
                sqlCommand.AppendLine("(SELECT LotAllocation FROM HotLot_BUAllocation WHERE BUId = bu.BUId) As LotAllocation,");
                sqlCommand.AppendLine("(SELECT COUNT(*) FROM HotLot_Data WHERE BUId= bu.BUId AND [Status] =0) AS Actual");
                sqlCommand.AppendLine("FROM Hotlot_BUs bu");
                return context.Database.SqlQuery<TotalBuDto>(sqlCommand.ToString()).ToList<TotalBuDto>();
            };

        }

        /// <summary>
        /// Checks the allocation and actual.
        /// </summary>
        /// <param name="buid">The buid.</param>
        /// <returns>
        /// return true if actual < alloction other then the false
        /// </returns>
        public bool CheckAllocationAndActual(int buid)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                int actual = context.HotLot_Data.Count(x => x.IsDeleted == false && x.BUId == buid && x.Status == 0);
                int allocation = context.HotLot_BUAllocation.Where(x => x.BUId == buid).Sum(x => x.LotAllocation);
                if (actual >= allocation)
                    return false;
                else
                    return true;

            }
        }
    }
}
