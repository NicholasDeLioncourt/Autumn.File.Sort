namespace Autumn.File
{
    using System;

    //=============================================================================================
    /// <summary>
    /// Specifies a configured path with a type of <see cref="ProcessConfigurationPathType"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfiguration ContinueOnError=[{ContinueOnError}] IsParallel=[{IsParallel}] Threshold=[{Threshold}] Extensions=[{Extensions.DebugInformation}]")]
    internal sealed class ProcessConfiguration
    {
        #region PRIVATE MEMBERS
        private readonly Lazy<ProcessConfigurationExtensions> __extensions = new Lazy<ProcessConfigurationExtensions>(() => new ProcessConfigurationExtensions(), true);
        private readonly Lazy<FileMetadataContexts> __files                = new Lazy<FileMetadataContexts>(() => new FileMetadataContexts(), true);
        private readonly Lazy<ProcessConfigurationPaths> __paths           = new Lazy<ProcessConfigurationPaths>(() => new ProcessConfigurationPaths(), true);
        private readonly Lazy<ProcessConfigurationReplacements> __replacements = new Lazy<ProcessConfigurationReplacements>(() => new ProcessConfigurationReplacements(), true);
        #endregion

        //=========================================================================================
        /// <summary>
        /// Gets or sets whether to <c>throw</c> an <c>Exception</c> when a path cannot be read or 
        /// found; if <c>false</c>, the process will continue and the valid paths processed.
        /// </summary>
        //=========================================================================================
        public bool ContinueOnError { get; set; }

        //=========================================================================================
        /// <summary>
        /// Specifies a list of file extensions to be processed by this application; if the 
        /// <see cref="ProcessConfigurationExtensions"/> is empty, all file types will be processed.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationExtensions Extensions
        {
            get { return(this.__extensions.Value); }
        }

        //=========================================================================================
        /// <summary>
        /// The list of <see cref="FileMetadataContext"/> instances to be processed.
        /// </summary>
        //=========================================================================================
        public FileMetadataContexts Files
        {
            get { return(this.__files.Value); }
        }

        //=========================================================================================
        /// <summary>
        /// Gets or sets whether to process the files in parallel or one at a time. If <c>true</c>, 
        /// this setting will initiate only if the <see cref="Threshold"/> is equal to or greater 
        /// than the file count.
        /// </summary>
        //=========================================================================================
        public bool IsParallel { get; set; }

        //=========================================================================================
        /// <summary>
        /// The list of paths such as (1) the source paths, (2) the destination path and (3) the log path.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationPaths Paths
        {
            get { return(this.__paths.Value); }
        }

        //=========================================================================================
        /// <summary>
        /// Replaces character and string sequences found in the file and folder names; optionally, 
        /// normalizing the strings (<see cref="ProcessConfigurationReplacements.Normalize"/>.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationReplacements Replacements
        {
            get { return(this.__replacements.Value); }
        }

        //=========================================================================================
        /// <summary>
        /// If the <see cref="Threshold"/> value is equal to or greater than the count of files to be 
        /// processed, the <see cref="IsParallel"/> setting will be read. If the value is 0 and 
        /// <see cref="IsParallel"/> is <c>true</c>, parallelism will be attempted notwithstanding 
        /// the file count. Any value less than 0 will deactivate <see cref="IsParallel"/> as 
        /// <c>false</c>.
        /// </summary>
        //=========================================================================================
        public int Threshold { get; set; }
    }
}
