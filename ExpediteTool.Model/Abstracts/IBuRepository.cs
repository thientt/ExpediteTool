using ExpediteTool.Model.DataTransfer;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBuRepository : IRepository<BuDto>
    {
        /// <summary>
        /// Gets the total bu with actived.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TotalBuDto> GetTotalBuWithActived();

        /// <summary>
        /// Checks the allocation and actual.
        /// </summary>
        /// <param name="buid">The buid.</param>
        /// <returns></returns>
        bool CheckAllocationAndActual(int buid);
    }
}
