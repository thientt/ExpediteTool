using System;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class LockUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockUserException"/> class.
        /// </summary>
        public LockUserException()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="LockUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LockUserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockUserException"/> class.
        /// </summary>
        /// <param name="messsage">The messsage.</param>
        /// <param name="ex">The ex.</param>
        public LockUserException(string messsage, Exception ex)
            : base(messsage, ex)
        {

        }
    }
}
