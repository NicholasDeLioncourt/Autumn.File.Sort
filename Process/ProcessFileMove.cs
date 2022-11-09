namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// Performs the actions of creating the REJECTED and DESTINATION folders; if needed, and moving 
    /// each file in the <see cref="ProcessConfiguration.Files"/> list into its assigned folder.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessFileMove Instance Key=[{Key}] Initialized=[{__isinitialized}]")]
    internal sealed class ProcessFileMove : AbstractFileProcess
    {
        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// The new instance of the <see cref="ProcessFileMove"/> class created.
        /// </summary>
        //=========================================================================================
        public ProcessFileMove() { }
        #endregion

        #region PUBLIC METHODS
        // ReSharper disable PossibleNullReferenceException
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
        /// 
        /// Also implements the <see cref="ProcessConfigurationExtensions.LowerCase"/> setting.
        /// </notes>
        //=========================================================================================
        protected override IEnumerable<FileMetadataContext> Process(ProcessConfiguration source)
        {
            if(null == source)          { this.ThrowException(Resources.ExceptionArgumentEmpty.FormatInvariant(StringsParamNames.Source)); }
            if(0 == source.Files.Count) { return(source.Files); }

            ProcessConfigurationDestinationPath __path = source.Paths
                                                               .FindAll(path => path.PathType == ProcessConfigurationPathType.Destination)
                                                               .As<ProcessConfigurationPath, ProcessConfigurationDestinationPath>()
                                                               .FirstOrDefault(() => new ProcessConfigurationDestinationPath());
            if(!__path.Folders) { return(source.Files); }

            ProcessConfigurationRejectedPath __rejected = source.Paths
                                                                .FindAll(path => path.PathType == ProcessConfigurationPathType.Rejected)
                                                                .As<ProcessConfigurationPath, ProcessConfigurationRejectedPath>()
                                                                .FirstOrDefault(() => new ProcessConfigurationRejectedPath());
            source.Files.Each(file =>
            {
                string __destination = Path.Combine(__path.Value, file.Endpoint);
                string __newfilename = file.Name + (source.Extensions.LowerCase ? file.Extension.ToLowerInvariant() : file.Extension);
                if(!Directory.Exists(__destination)) { Directory.CreateDirectory(__destination); }

                if(File.Exists(Path.Combine(__destination, __newfilename)))
                {
                    file.Status = FileStatus.Rejected; file.Comment = "File [{0}] is a duplicate".FormatCulture(__newfilename);
                }

                if(file.Status == FileStatus.Rejected)
                {
                    __destination = __rejected.Value; if(!Directory.Exists(__destination)) { Directory.CreateDirectory(__destination); }

                    if(__rejected.AppendDate)
                    {
                        __newfilename = Resources.FormatTriple.FormatInvariant(file.Name, DateTimeOffset.Now.ToString(__rejected.Format), source.Extensions.LowerCase ? file.Extension.ToLowerInvariant() : file.Extension); 
                    }
                }

                File.Move(file.FullPath, Path.Combine(__destination, __newfilename));
            });

            return(source.Files);
        }
        // ReSharper restore PossibleNullReferenceException
        #endregion
    }
}
