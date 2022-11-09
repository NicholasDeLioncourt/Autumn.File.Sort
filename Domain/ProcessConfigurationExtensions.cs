namespace Autumn.File
{
    using System.Collections.Generic;

    //=============================================================================================
    /// <summary>
    /// Specifies a list of file extensions to be processed by this application; if the 
    /// <see cref="ProcessConfigurationExtensions"/> is empty, all file types will be processed.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationExtensions Count=[{Count}] Values=[{DebugInformation}]")]
    internal sealed class ProcessConfigurationExtensions : List<string>
    {
        //=========================================================================================
        /// <summary>
        /// If <c>true</c>, all file extensions will be converted to lower case.
        /// </summary>
        //=========================================================================================
        public bool LowerCase { get; set; }

        //=========================================================================================
        /// <summary>
        /// Used for the <see cref="System.Diagnostics.DebuggerDisplayAttribute"/>.
        /// </summary>
        //=========================================================================================
        internal string DebugInformation
        {
            get { return(this.ToDelimitedString()); }
        }
    }
}
