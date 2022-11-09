namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// In this process, the state of <see cref="ProcessConfigurationDestinationPath.Folders"/> is 
    /// determined and; if <c>true</c>, the destination folder specific to that file is created. If 
    /// an image, the folder name will be the date on which the image was taken (EXIF Date Time Taken);
    /// otherwise, the folder name is that of the file name without the file extension.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessFolderName Instance Key=[{Key}] Initialized=[{__isinitialized}]")]
    internal sealed class ProcessFolderName : AbstractFileProcess
    {
        #region PRIVATE MEMBERS
        private const string __foldernameformat = "yyyy MM dd";
        private static readonly int [] __exifdatetaken = { 0x009003, 0x000132, 0x009004 }; //<--{DateTime Taken, DateTime Modified, DateTime Digitized}

        //=========================================================================================
        /// <summary>The destination folder name is the file name without the file extension.</summary>
        //=========================================================================================
        private readonly Action<FileMetadataContext> __fileaction = instance => 
        {
            instance.Endpoint = instance.Name.TrimLastIndexNonAlphaNumeric(String.Empty);
        };
        //=========================================================================================
        /// <summary>If an IMAGE, the destination folder name is the EXIF Date Time Taken.</summary>
        //=========================================================================================
        private readonly Action<FileMetadataContext> __imageaction = instance =>
        {
            try
            {
                using(FileStream __stream = new FileStream(instance.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using(Image __image = Image.FromStream(__stream, false, false))
                {
                    int __exiftagfound = -1;

                    for(int __index = 0; __index < __exifdatetaken.Length; __index++)
                    {
                        if(-1 == Array.IndexOf(__image.PropertyIdList, __exifdatetaken[__index])) { continue; }
                        __exiftagfound = __exifdatetaken[__index]; break;
                    }

                    if(-1 == __exiftagfound)
                    {
                        instance.Status = FileStatus.Rejected; instance.Comment = Resources.ExceptionExifDateTagNotFound;
                        return;
                    }

                    PropertyItem __property = __image.GetPropertyItem(__exiftagfound);
                    string __datetakentext  = Encoding.UTF8.GetString(__property.Value, 0, __property.Value.Length - 1);

                    DateTime __datetaken;
                    if(!DateTime.TryParseExact(__datetakentext, Resources.FormatExifDateTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out __datetaken))
                    {
                        instance.Status = FileStatus.Rejected; instance.Comment = Resources.ExceptionExifDateParseFail.FormatInvariant(__datetakentext);
                        return;
                    }

                    instance.Endpoint = __datetaken.ToString(__foldernameformat);
                }
            }
            catch(ArgumentException e)
            {
                instance.Status = FileStatus.Rejected; instance.Comment = Resources.ExceptionImageFormat.FormatInvariant(instance.Name, e.Message);
            }
            
        };
        #endregion

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// The new instance of the <see cref="ProcessFolderName"/> class created.
        /// </summary>
        //=========================================================================================
        public ProcessFolderName() { }
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
        /// </notes>
        //=========================================================================================
        protected override IEnumerable<FileMetadataContext> Process(ProcessConfiguration source)
        {

            if(null == source) { this.ThrowException(Resources.ExceptionArgumentEmpty.FormatInvariant(StringsParamNames.Source), null, source.ContinueOnError); }

            IEnumerable<ProcessConfigurationDestinationPath> __paths = source.Paths
                                                                             .FindAll(path => path.PathType == ProcessConfigurationPathType.Destination)
                                                                             .As<ProcessConfigurationPath, ProcessConfigurationDestinationPath>();
            if(__paths.Any(path => !path.Folders)) { return(source.Files); }

            if(source.IsParallel)
            {
                Parallel.ForEach(source.Files, instance =>
                {
                    Action<FileMetadataContext> __chosenaction = instance.IsImage() ? __imageaction : __fileaction;
                                                __chosenaction(instance);
                });

                return(source.Files);
            }

            source.Files.Each(instance =>
            {
                Action<FileMetadataContext> __chosenaction = instance.IsImage() ? __imageaction : __fileaction;
                                            __chosenaction(instance);
            });

            return(source.Files);
        }
        // ReSharper restore PossibleNullReferenceException
        #endregion
    }
}
