namespace Autumn.File
{
    using System;
    //=============================================================================================
    /// <summary>
    /// A wrapper <see cref="Exception"/> so that any fault thrown is easier to catch.
    /// </summary>
    /// <created>LNDL</created>
    //=====================================================================================LNDL====
    [System.Diagnostics.DebuggerDisplay("ProcessException Message = {Message}")]
    public class ProcessException : Exception
    {
        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class.
        /// </summary>
        //=================================================================================LNDL====
        public ProcessException()
        { }

        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a 
        /// specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        //=========================================================================================
        public ProcessException(string message) : base(message)
        { }

        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessException"/> class with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerexception">
        /// The exception that is the cause of the current exception, or a <c>null</c> reference 
        /// (<b>Nothing</b> in Visual Basic) if no inner exception is specified.
        /// </param>
        //=========================================================================================
        public ProcessException(string message, Exception innerexception) : base(message, innerexception)
        { }
        #endregion

        #region PUBLIC ACCESSORS
        #endregion
    }
}
