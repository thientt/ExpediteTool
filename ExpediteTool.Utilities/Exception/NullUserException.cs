using System;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class NullUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullUserException"/> class.
        /// </summary>
        public NullUserException():base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NullUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NullUserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullUserException"/> class.
        /// </summary>
        /// <param name="messsage">The messsage.</param>
        /// <param name="ex">The ex.</param>
        public NullUserException(string messsage, Exception ex)
            : base(messsage, ex)
        {

        }
    }
}
