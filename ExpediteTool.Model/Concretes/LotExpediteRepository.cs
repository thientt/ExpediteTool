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
    public class LotExpediteRepository : ILotExpediteRepository
    {
        /// <summary>
        /// The log service
        /// </summary>
        private ILogService _logService;

        /// <summary>
        /// 
        /// </summary>
        public LotExpediteRepository() : this(new LogService()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LotExpediteRepository"/> class.
        /// </summary>
        public LotExpediteRepository(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Moves the sorting.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="moveType">Type of the move.</param>
        /// <returns></returns>
        public ActionResult MoveSorting(int id, MoveType moveType)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    HotLot_Data itemSelf = context.HotLot_Data.Single(w => w.ID == id && w.IsDeleted == false);
                    HotLot_Data itemSibling = null;
                    switch (moveType)
                    {
                        case MoveType.DOWN:
                            itemSibling = context.HotLot_Data.
                                Where(x => x.IsDeleted == false &&
                                      x.Status == (byte)StatusType.PENDING &&
                                      x.BUId == itemSelf.BUId &&
                                      x.LotPriority > itemSelf.LotPriority).
                                 OrderBy(x => x.LotPriority).ToList().FirstOrDefault();
                            break;
                        case MoveType.UP:
                            itemSibling = context.HotLot_Data.
                                    Where(x => x.IsDeleted == false &&
                                        x.BUId == itemSelf.BUId &&
                                       x.Status == (byte)StatusType.PENDING &&
                                       x.LotPriority < itemSelf.LotPriority).
                                   OrderBy(x => x.LotPriority).ToList().LastOrDefault();
                            break;
                    }

                    if (itemSelf == null || itemSibling == null)
                        return ActionResult.FAIL;

                    int priority = itemSelf.LotPriority;
                    itemSelf.LotPriority = itemSibling.LotPriority;
                    itemSibling.LotPriority = priority;

                    context.Entry<HotLot_Data>(itemSelf).State = System.Data.Entity.EntityState.Modified;
                    context.Entry<HotLot_Data>(itemSibling).State = System.Data.Entity.EntityState.Modified;

                    context.SaveChanges();
                    result = ActionResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while sorting", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LotExpediteDto> FindAll()
        {
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var items = (from item in context.HotLot_Data
                                 where item.IsDeleted == false
                                 select new LotExpediteDto
                                 {
                                     ID = item.ID,
                                     ModifiedBy = item.ModifiedBy,
                                     Modified = item.Modified,
                                     IsDeleted = item.IsDeleted,
                                     CreatedBy = item.CreatedBy,
                                     Created = item.Created,
                                     LotId = item.LotId,
                                     Reason = item.Reason,
                                     RequestOutDate = item.RequestOutDate,
                                     Device = item.Device,
                                     Requestor = item.Requestor,
                                     Bu = new BuDto
                                     {
                                         BuId = item.Hotlot_BUs.BUId,
                                         BuName = item.Hotlot_BUs.BUName,
                                         Description = item.Hotlot_BUs.Description,
                                     },
                                     Owner = item.Owner,
                                     ScmEndDate = item.ScmEndDate,
                                     Comment = item.Comment,
                                     Platform = item.Platform,
                                     CurrentOperation = item.CurrentOperation,
                                     Status = (StatusType)item.Status,
                                     LotPriority = item.LotPriority,
                                 }).ToList();
                    return items;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while find all", ex);
                return null;
            }
        }

        /// <summary>
        /// Inserts the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ActionResult Add(LotExpediteDto TEntity, string userName)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    int count = context.HotLot_Data.Count();
                    HotLot_Data item = new HotLot_Data
                    {
                        BUId = TEntity.BUId,
                        IsDeleted = TEntity.IsDeleted,
                        CreatedBy = userName,
                        Created = DateTime.Now,
                        LotId = TEntity.LotId,
                        Reason = TEntity.Reason,
                        RequestOutDate = TEntity.RequestOutDate,
                        ScmEndDate = TEntity.ScmEndDate,
                        Status = (byte)TEntity.Status,
                        CurrentOperation = TEntity.CurrentOperation,
                        Device = TEntity.Device,
                        Owner = TEntity.Owner,
                        Comment = TEntity.Comment,
                        Platform = TEntity.Platform,
                        Requestor = TEntity.Requestor,
                        LotPriority = (count + 1),
                    };
                    context.Entry<HotLot_Data>(item).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    result = ActionResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Insert HotLot_Data", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// Updates the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ActionResult Update(LotExpediteDto TEntity, string userName)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    HotLot_Data item = context.HotLot_Data.Single(x => x.ID == TEntity.ID);

                    item.ModifiedBy = userName;
                    item.Modified = DateTime.Now;
                    item.IsDeleted = TEntity.IsDeleted;
                    item.LotId = TEntity.LotId;
                    item.BUId = TEntity.BUId;
                    item.Reason = TEntity.Reason;
                    item.RequestOutDate = TEntity.RequestOutDate;
                    item.Owner = TEntity.Owner;
                    item.Comment = TEntity.Comment;
                    item.Platform = TEntity.Platform;
                    item.CurrentOperation = TEntity.CurrentOperation;
                    item.Device = TEntity.Device;
                    item.Status = (byte)TEntity.Status;
                    context.Entry<HotLot_Data>(item).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    result = ActionResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Update LotExpedite", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public ActionResult UpdateStatus(int id, StatusType status, string userName)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var single = context.HotLot_Data.Single(w => w.ID == id && w.IsDeleted == false);
                    if (single != null)
                    {
                        single.Status = (byte)status;
                        single.ModifiedBy = userName;
                        single.Modified = DateTime.Now;
                        context.Entry<HotLot_Data>(single).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        result = ActionResult.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Update Status HotLotData", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <param name="userName"></param>
        /// <param name="BuID"></param>
        /// <returns></returns>
        public ActionResult UpdateComment(int id, string comment, string userName, out int BuID)
        {
            BuID = 0;
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var single = context.HotLot_Data.Single(w => w.ID == id && w.IsDeleted == false);
                    if (single != null)
                    {
                        single.Comment = comment;
                        single.ModifiedBy = userName;
                        single.Modified = DateTime.Now;
                        BuID = single.BUId;
                        context.Entry<HotLot_Data>(single).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        result = ActionResult.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Update Comment HotLotData", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// Gets the count pending.
        /// </summary>
        /// <returns></returns>
        public int GetCountPendingHotLots()
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    return context.HotLot_Data.Where(x => x.Status == (byte)StatusType.PENDING).Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the count actived.
        /// </summary>
        /// <returns></returns>
        public int GetCountActivedHotLots()
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    return context.HotLot_Data.Where(x => x.Status == (byte)StatusType.ACTIVED).Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the count locked.
        /// </summary>
        /// <returns></returns>
        public int GetCountClosedHotLots()
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                try
                {
                    return context.HotLot_Data.Where(x => x.Status == (byte)StatusType.CLOSED).Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the hot lot data by status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public List<LotExpediteDto> GetHotLotDataByStatus(StatusType statusType, int startRowIndex, int pageSize)
        {
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    int totalRows = context.HotLot_Data.Count();
                    if (totalRows < (startRowIndex + pageSize))
                        pageSize = totalRows - startRowIndex;

                    var items = (from item in context.HotLot_Data
                                 where item.IsDeleted == false && item.Status == (byte)statusType
                                 orderby item.ID
                                 select new LotExpediteDto
                                 {
                                     ID = item.ID,
                                     ModifiedBy = item.ModifiedBy,
                                     Modified = item.Modified,
                                     IsDeleted = item.IsDeleted,
                                     CreatedBy = item.CreatedBy,
                                     Created = item.Created,
                                     LotId = item.LotId,
                                     Reason = item.Reason,
                                     RequestOutDate = item.RequestOutDate,
                                     Device = item.Device,
                                     Requestor = item.Requestor,
                                     Bu = new BuDto
                                     {
                                         BuId = item.Hotlot_BUs.BUId,
                                         BuName = item.Hotlot_BUs.BUName,
                                         Description = item.Hotlot_BUs.Description,
                                     },
                                     Owner = item.Owner,
                                     ScmEndDate = item.ScmEndDate,
                                     Comment = item.Comment,
                                     Platform = item.Platform,
                                     CurrentOperation = item.CurrentOperation,
                                     Status = (StatusType)item.Status,
                                 }).Skip(startRowIndex).Take(pageSize).ToList();

                    return items;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while find all", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the count pending by bu.
        /// </summary>
        /// <param name="BUId">The bu identifier.</param>
        /// <returns></returns>
        public int GetCountPendingByBu(int BUId)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                return context.HotLot_Data.Where(x => x.Status == 0 && x.BUId == BUId).Count();
            }
        }

        /// <summary>
        /// Gets the hot lot data pending by bu.
        /// </summary>
        /// <param name="BuId">The bu identifier.</param>
        /// <returns></returns>
        public List<LotExpediteDto> GetHotLotDataByBuAndStatus(int BuId, StatusType status)
        {
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var items = (from item in context.HotLot_Data
                                 where item.IsDeleted == false &&
                                 item.Status == (byte)status &&
                                 item.BUId == BuId
                                 orderby item.LotPriority ascending
                                 select new LotExpediteDto
                                 {
                                     ID = item.ID,
                                     ModifiedBy = item.ModifiedBy,
                                     Modified = item.Modified,
                                     IsDeleted = item.IsDeleted,
                                     CreatedBy = item.CreatedBy,
                                     Created = item.Created,
                                     LotId = item.LotId,
                                     Reason = item.Reason,
                                     RequestOutDate = item.RequestOutDate,
                                     Device = item.Device,
                                     Requestor = item.Requestor,
                                     Bu = new BuDto
                                     {
                                         BuId = item.Hotlot_BUs.BUId,
                                         BuName = item.Hotlot_BUs.BUName,
                                         Description = item.Hotlot_BUs.Description,
                                     },
                                     Owner = item.Owner,
                                     ScmEndDate = context.GP_vSCMInvLatestSnapshot.Where(x => x.Lot.Contains(item.LotId)).Select(x => x.SCM_End_Date).FirstOrDefault(),
                                     Comment = item.Comment,
                                     Platform = item.Platform,
                                     CurrentOperation = item.CurrentOperation,
                                     Status = (StatusType)item.Status,
                                 }).ToList();


                    return items;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while find all", ex);
                return null;
            }
        }

        /// <summary>
        /// Updates the reason and request out date.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        public ActionResult UpdateReasonAndRequestOutDate(ReasonDto reason)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var hotLotData = context.HotLot_Data.Single(w => w.ID == reason.IdHotLotData
                                                                && w.IsDeleted == false);
                    if (hotLotData != null)
                    {
                        hotLotData.Reason = reason.Reason;
                        hotLotData.RequestOutDate = reason.RequestOutDate;
                        hotLotData.ModifiedBy = reason.UserNameRequest;
                        hotLotData.Modified = DateTime.Now;
                        context.Entry<HotLot_Data>(hotLotData).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        result = ActionResult.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Update Status LotExpedite", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<LotExpediteDto> SearchAll(string text)
        {
            DateTime dTime;
            string textSearch = String.Empty;

            if (!String.IsNullOrEmpty(text) && text.Split('/').Length == 3)
            {
                string[] splitText = text.Split('/');
                if (splitText[0].Length == 1)
                    textSearch = splitText[0].PadLeft(2, '0') + "/";
                else
                    textSearch = splitText[0] + "/";

                if (splitText[1].Length == 1)
                    textSearch += splitText[1].PadLeft(2, '0') + "/";
                else
                    textSearch += splitText[1] + "/";

                textSearch += splitText[2];
            }

            if (String.IsNullOrEmpty(textSearch))
                textSearch = text;

            bool convert = DateTime.TryParseExact(textSearch, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dTime);

            if (convert)
                return _searchAllHasDateTime(text, dTime);
            else
                return _searchAllNoHasDateTime(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName">username Login</param>
        /// <returns></returns>
        public ActionResult Delete(int id, string userName)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    var hotLotData = context.HotLot_Data.Single(w => w.ID == id
                                                                && w.IsDeleted == false);
                    if (hotLotData != null)
                    {
                        hotLotData.IsDeleted = true;
                        hotLotData.ModifiedBy = userName;
                        hotLotData.Modified = DateTime.Now;
                        context.Entry<HotLot_Data>(hotLotData).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        result = ActionResult.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while delete LotExpedite", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        public ActionResult AddBulk(List<LotExpediteDto> items, string currentUser)
        {
            ActionResult result = ActionResult.FAIL;
            try
            {
                using (ACPReportsEntities context = new ACPReportsEntities())
                {
                    int count = context.HotLot_Data.Count();
                    foreach (var TEntity in items)
                    {
                        HotLot_Data item = new HotLot_Data
                        {
                            BUId = TEntity.BUId,
                            LotId = TEntity.LotId,
                            Reason = TEntity.Reason,
                            RequestOutDate = TEntity.RequestOutDate,
                            ScmEndDate = TEntity.ScmEndDate,
                            Status = (byte)StatusType.PENDING,
                            CurrentOperation = TEntity.CurrentOperation,
                            Device = TEntity.Device,
                            Owner = TEntity.Owner,
                            Comment = TEntity.Comment,
                            Platform = TEntity.Platform,
                            Requestor = currentUser,
                            LotPriority = count,
                            IsDeleted = false,
                            CreatedBy = currentUser,
                            Created = DateTime.Now,
                        };
                        context.Entry<HotLot_Data>(item).State = System.Data.Entity.EntityState.Added;
                        count++;
                    }
                    context.SaveChanges();
                    result = ActionResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error while Insert HotLot_Data", ex);
                result = ActionResult.FAIL;
            }
            return result;
        }

        #region Func Private
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private IEnumerable<LotExpediteDto> _searchAllHasDateTime(string text, DateTime dt)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                var results = (from item in context.HotLot_Data
                               where item.LotId.Contains(text) ||
                               item.Device.Contains(text) ||
                               item.Reason.Contains(text) ||
                               item.Requestor.Contains(text) ||
                               item.Owner.Contains(text) ||
                               item.Comment.Contains(text) ||
                               item.Platform.Contains(text) ||
                               item.CurrentOperation.Contains(text) ||
                               item.RequestOutDate == dt
                               select new LotExpediteDto
                                {
                                    ID = item.ID,
                                    ModifiedBy = item.ModifiedBy,
                                    Modified = item.Modified,
                                    IsDeleted = item.IsDeleted,
                                    CreatedBy = item.CreatedBy,
                                    Created = item.Created,
                                    BUId = item.BUId,
                                    LotId = item.LotId,
                                    Reason = item.Reason,
                                    RequestOutDate = item.RequestOutDate,
                                    Device = item.Device,
                                    Requestor = item.Requestor,
                                    Bu = new BuDto
                                    {
                                        BuId = item.Hotlot_BUs.BUId,
                                        BuName = item.Hotlot_BUs.BUName,
                                        Description = item.Hotlot_BUs.Description,
                                    },
                                    Owner = item.Owner,
                                    ScmEndDate = item.ScmEndDate,
                                    Comment = item.Comment,
                                    Platform = item.Platform,
                                    CurrentOperation = item.CurrentOperation,
                                    Status = (StatusType)item.Status,
                                    LotPriority = item.LotPriority,
                                }).ToList();
                return results;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private IEnumerable<LotExpediteDto> _searchAllNoHasDateTime(string text)
        {
            using (ACPReportsEntities context = new ACPReportsEntities())
            {
                var results = (from item in context.HotLot_Data
                               where item.LotId.Contains(text) ||
                               item.Device.Contains(text) ||
                               item.Reason.Contains(text) ||
                               item.Requestor.Contains(text) ||
                               item.Owner.Contains(text) ||
                               item.Comment.Contains(text) ||
                               item.Platform.Contains(text) ||
                               item.CurrentOperation.Contains(text)
                               select new LotExpediteDto
                               {
                                   ID = item.ID,
                                   ModifiedBy = item.ModifiedBy,
                                   Modified = item.Modified,
                                   IsDeleted = item.IsDeleted,
                                   CreatedBy = item.CreatedBy,
                                   Created = item.Created,
                                   BUId = item.BUId,
                                   LotId = item.LotId,
                                   Reason = item.Reason,
                                   RequestOutDate = item.RequestOutDate,
                                   Device = item.Device,
                                   Requestor = item.Requestor,
                                   Bu = new BuDto
                                   {
                                       BuId = item.Hotlot_BUs.BUId,
                                       BuName = item.Hotlot_BUs.BUName,
                                       Description = item.Hotlot_BUs.Description,
                                   },
                                   Owner = item.Owner,
                                   ScmEndDate = item.ScmEndDate,
                                   Comment = item.Comment,
                                   Platform = item.Platform,
                                   CurrentOperation = item.CurrentOperation,
                                   Status = (StatusType)item.Status,
                                   LotPriority = item.LotPriority,
                               }).ToList();
                return results;
            }
        }
        #endregion
    }
}
