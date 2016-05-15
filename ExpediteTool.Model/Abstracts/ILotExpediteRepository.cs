using ExpediteTool.Model.DataTransfer;
using System;
using System.Collections.Generic;

namespace ExpediteTool.Model.Abstracts
{
    public interface ILotExpediteRepository : IRepository<LotExpediteDto>
    {
        /// <summary>
        /// Moves the sorting.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="moveType">Type of the move.</param>
        /// <returns></returns>
        ActionResult MoveSorting(int id, MoveType moveType);

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ActionResult UpdateStatus(int id, StatusType status, string userName);

        /// <summary>
        /// Updates the comment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="BuID">The bu identifier.</param>
        /// <returns></returns>
        ActionResult UpdateComment(int id, string comment, string userName, out int BuID);

        /// <summary>
        /// Gets the count pending.
        /// </summary>
        /// <returns></returns>
        int GetCountPendingHotLots();

        /// <summary>
        /// Gets the count actived.
        /// </summary>
        /// <returns></returns>
        int GetCountActivedHotLots();

        /// <summary>
        /// Gets the count locked.
        /// </summary>
        /// <returns></returns>
        int GetCountClosedHotLots();

        /// <summary>
        /// Gets the hot lot data by status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="startRowIndex">Start index of the row.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        List<LotExpediteDto> GetHotLotDataByStatus(StatusType statusType, int startRowIndex, int pageSize);

        /// <summary>
        /// Gets the count pending by bu.
        /// </summary>
        /// <param name="BUId">The bu identifier.</param>
        /// <returns></returns>
        int GetCountPendingByBu(int BUId);

        /// <summary>
        /// Gets the hot lot data pending by bu.
        /// </summary>
        /// <param name="BuId">The bu identifier.</param>
        /// <returns></returns>
        List<LotExpediteDto> GetHotLotDataByBuAndStatus(int BuId, StatusType status);

        /// <summary>
        /// Updates the reason and request out date.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        ActionResult UpdateReasonAndRequestOutDate(ReasonDto reason);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IEnumerable<LotExpediteDto> SearchAll(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        ActionResult Delete(int id, string userName);

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        ActionResult AddBulk(List<LotExpediteDto> items, string currentUser);
    }
}
