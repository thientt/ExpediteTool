using System;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class ExistUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExistUserException"/> class.
        /// </summary>
        public ExistUserException():base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExistUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExistUserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistUserException"/> class.
        /// </summary>
        /// <param name="messsage">The messsage.</param>
        /// <param name="ex">The ex.</param>
        public ExistUserException(string messsage, Exception ex)
            : base(messsage, ex)
        {

        }
    }
}
