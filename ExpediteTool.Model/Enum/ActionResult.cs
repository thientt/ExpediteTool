
/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Model
{
    /// <summary>
    /// 
    /// </summary>
    public enum ActionResult : byte
    {
        /// <summary>
        /// The success
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// The fail
        /// </summary>
        FAIL = 1,
        /// <summary>
        /// The exist
        /// </summary>
        EXIST = 2,
        /// <summary>
        /// The notexist
        /// </summary>
        NOTEXIST = 3,
        /// <summary>
        /// The unknown
        /// </summary>
        UNKNOWN = 4,
        /// <summary>
        /// The deacivate
        /// </summary>
        DEACIVATE = 5,
        /// <summary>
        /// The actived
        /// </summary>
        ACTIVED = 6,
        /// <summary>
        /// The locked
        /// </summary>
        LOCKED = 7,
        /// <summary>
        /// The connection to database
        /// </summary>
        CONNECTION=8,
    }
}
