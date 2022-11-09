namespace Autumn.File
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using Autumn.File.Properties;
    // ReSharper disable PossibleNullReferenceException
    //=============================================================================================
    /// <summary>
    /// Probes each <see cref="ProcessConfigurationPathType.Source"/> in the 
    /// <see cref="ProcessConfiguration.Paths"/> and returns <see cref="FileMetadataContext"/> 
    /// instances filtered by <see cref="ProcessConfiguration.Extensions"/>.
    /// </summary>
    /// <remarks>Do not reuse this instance </remarks>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessFileReader Instance Key=[{Key}] Initialized=[{__isinitialized}]")]
    internal sealed class ProcessFileReader : AbstractFileProcess
    {
        #region PRIVATE MEMBERS
        private static bool __isinitialized;

        private static object __synchronize;
        private static ProcessFileReader __reader;
        #endregion

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// Prevents a default instance of the <see cref="ProcessFileReader"/> class from being created.
        /// </summary>
        //=========================================================================================
        private ProcessFileReader() { }

        //=========================================================================================
        /// <summary>Initializes the <see cref="ProcessFileReader"/> class.</summary>
        //=========================================================================================
        static ProcessFileReader() { }
        #endregion

        #region PUBLIC ACCESSORS
        //=========================================================================================
        /// <summary>(Fluent) Gets the Singleton <see cref="ProcessFileReader"/> Instance.</summary>
        //=========================================================================================
        public static ProcessFileReader Current
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref __reader,
                                                  ref __isinitialized,
                                                  ref __synchronize, () =>
                                                  new ProcessFileReader());
                return(__reader);
            }
        }
        #endregion

        #region PUBLIC METHODS
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
        /// <see cref="AbstractFileProcess.ThrowException(string,Autumn.File.FileMetadataContext,bool)"/> 
        /// would interrupt processing; if requested.
        /// </notes>
        //=========================================================================================
        protected override IEnumerable<FileMetadataContext> Process(ProcessConfiguration source)
        {
            if(null == source) { this.ThrowException(Resources.ExceptionArgumentEmpty.FormatInvariant(StringsParamNames.Source)); }
  
            ProcessConfigurationPaths __paths = source.Paths;
            if(null == __paths) { this.ThrowException(Resources.ExceptionArgumentEmpty.FormatInvariant(StringsParamNames.Paths), null, source.ContinueOnError); }

            IEnumerable<ProcessConfigurationSourcePath> __sourcepaths = __paths.FindAll(path => path.PathType == ProcessConfigurationPathType.Source)
                                                                               .As<ProcessConfigurationPath, ProcessConfigurationSourcePath>();
            foreach(ProcessConfigurationSourcePath __path in __sourcepaths)
            {
                DirectoryInfo __source = new DirectoryInfo(__path.Value);

                if(!__source.Exists)
                  { this.ThrowException(Resources.ExceptionSourcePathNotFound.FormatInvariant(__path.Value), null, source.ContinueOnError); continue; }

                foreach(FileSystemInfo __fileinfo in __source.EnumerateFileSystemInfos(Resources.KeyWildcardAllFiles, __path.Recursive ? 
                                                              SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                                                             .Where(fileinfo => source.Extensions
                                                             .Contains(fileinfo.ExtensionNoDot().ToLowerInvariant())))
                { yield return(FileMetadataContext.Initialize(__fileinfo)); }
            }
        }
        #endregion
        // ReSharper restore PossibleNullReferenceException
    }
}
