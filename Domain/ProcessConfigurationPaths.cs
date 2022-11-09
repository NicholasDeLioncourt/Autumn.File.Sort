namespace Autumn.File
{
    using System;
    using System.Collections.Generic;

    //=============================================================================================
    /// <summary>
    /// Specifies a list of <see cref="ProcessConfigurationPath"/> instances.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationPaths Count=[{Count} ProcessConfigurationPath instances]")]
    internal sealed class ProcessConfigurationPaths : List<ProcessConfigurationPath>
    {
        //=========================================================================================
        /// <summary>
        /// Gets the <see cref="IEnumerable{ProcessConfigurationPath}"/> matching the provided 
        /// <see cref="ProcessConfigurationPathType"/>. This accessor may not be set; any attempt 
        /// will result in a <see cref="System.NotImplementedException"/>.
        /// </summary>
        /// <param name="type">The <see cref="ProcessConfigurationPathType"/>.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">An attempt to invoke SET was made.</exception>
        //=========================================================================================
        public IEnumerable<ProcessConfigurationPath> this[ProcessConfigurationPathType type]
        {
            get { return(this.FindAll(path => path.PathType == type)); }
            set { throw new NotImplementedException();  }
        }  
    }
}
