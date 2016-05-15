
using System.Collections.Generic;

namespace ExpediteTool.Model.Abstracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// Inserts the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ActionResult Add(T TEntity, string userName);

        /// <summary>
        /// Updates the specified t entity.
        /// </summary>
        /// <param name="TEntity">The object entity.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ActionResult Update(T TEntity, string userName);
    }
}
