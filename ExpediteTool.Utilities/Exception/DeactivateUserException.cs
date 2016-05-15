using System;

namespace ExpediteTool.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DeactivateUserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivateUserException"/> class.
        /// </summary>
        public DeactivateUserException()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivateUserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DeactivateUserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivateUserException"/> class.
        /// </summary>
        /// <param name="messsage">The messsage.</param>
        /// <param name="ex">The ex.</param>
        public DeactivateUserException(string messsage, Exception ex)
            : base(messsage, ex)
        {

        }
    }
}
