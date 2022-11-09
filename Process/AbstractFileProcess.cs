namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    
    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// Represents some action to be taken upon a list of <see cref="ProcessConfiguration"/> instances 
    /// within the <see cref="ProcessConfiguration"/>.
    /// </summary>
    /// <created>l. nicholas de lioncourt</created>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("AbstractFileProcess Instance Key=[{Key}]")]
    internal abstract class AbstractFileProcess
    {
        private string __key;
        private Action<ProcessException> __onexception;
        private static readonly object __synchronize = new object();

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// Prevents a default instance of the <see cref="AbstractFileProcess"/> class from being created.
        /// </summary>
        //=========================================================================================
        protected AbstractFileProcess() { this.Key = new Guid().ToString(); }
        #endregion

        #region PUBLIC ACCESSORS
        //=========================================================================================
        /// <summary>Gets the current <see cref="AbstractFileProcess"/> instance key.</summary>
        //=========================================================================================
        public string Key
        {
            get           { return(this.__key ?? String.Empty); }
            protected set { lock(__synchronize) this.__key = value; }
        }
        #endregion

        #region PUBLIC METHODS
        //=========================================================================================
        /// <summary>
        /// (Fluent) Sets the recipient of any <see cref="ProcessException"/> instances thrown; if not 
        /// set, the Exception will move up the stack and the current processing will end.
        /// </summary>
        //=========================================================================================
        public virtual AbstractFileProcess OnException(Action<ProcessException> func)
        {
            lock(__synchronize) this.__onexception = func; return(this);
        }

        // ReSharper disable InconsistentNaming
        //=========================================================================================
        /// <summary>
        /// Probes each <see cref="ProcessConfigurationPathType.Source"/> in the 
        /// <see cref="ProcessConfiguration.Paths"/> provided by the <paramref name="source"/> and 
        /// returns <see cref="FileMetadataContext"/> instances filtered by 
        /// <see cref="ProcessConfiguration.Extensions"/>.
        /// </summary>
        /// <notes>
        /// In the following, a check for the <c>Action{ProcessException}</c> or 
        /// <see cref="ProcessConfiguration.ContinueOnError"/> is not necessary since 
        /// <see cref="ThrowException(System.Exception,Autumn.File.FileMetadataContext,bool)"/> 
        /// would interrupt processing; if requested.
        /// </notes>
        //=========================================================================================
        public IEnumerable<FileMetadataContext> Enumerate(ProcessConfiguration source)
        {
            try { lock(__synchronize) return(this.Process(source)); }
            catch(ProcessException) { }
            catch(Exception e)      { this.ThrowException(e, null, source.ContinueOnError); }

            return(null);
        }
        // ReSharper restore InconsistentNaming
        #endregion

        #region PROTECTED METHODS
        //=========================================================================================
        /// <summary>To be written in your implementation.</summary>
        //=========================================================================================
        protected abstract IEnumerable<FileMetadataContext> Process(ProcessConfiguration source);

        //=========================================================================================
        /// <summary>
        /// Throws a <see cref="ProcessException"/> if the <c>Action{ProcessException}</c> is 
        /// <c>null</c> or <see cref="ProcessConfiguration.ContinueOnError"/> is <c>false</c>.
        /// </summary>
        //=========================================================================================
        protected virtual void ThrowException(string message, FileMetadataContext file = null, bool @continue = false)
        {
            // ReSharper disable once PossibleNullReferenceException
            file.IfNotNull(() => { file.Status = FileStatus.Rejected; file.Comment = message; });

            ProcessException __exception = new ProcessException(message);
            if(null == this.__onexception) { throw __exception; }
            if(!@continue) throw __exception; this.__onexception(__exception);
        }

        //=========================================================================================
        /// <summary>
        /// Throws a <see cref="ProcessException"/> if the <c>Action{ProcessException}</c> is 
        /// <c>null</c> or <see cref="ProcessConfiguration.ContinueOnError"/> is <c>false</c>.
        /// </summary>
        //=========================================================================================
        protected virtual void ThrowException(Exception exception, FileMetadataContext file = null, bool @continue = false)
        {
            Exception __exception = exception.GetBaseException();
            this.ThrowException(Resources.FormatExceptionMessage.FormatCulture(__exception.GetType().ToString(), 
                                                                               __exception.Message, 
                                                                               __exception.StackTrace),
                                                                               file,
                                                                               @continue);
        }
        #endregion
    }
}
