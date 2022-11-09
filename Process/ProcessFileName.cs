namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// Loops through the <see cref="FileMetadataContext"/> in the <see cref="ProcessConfiguration"/> 
    /// and inspects that file name which is compared to the <see cref="ProcessConfigurationReplacements"/> 
    /// characters; if a character or string is found, the original [Key} is replaced with the corresponding 
    /// [value]. Optionally, duplicated spaces are replaced.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessFileName Instance Key=[{Key}] Initialized=[{__isinitialized}]")]
    internal sealed class ProcessFileName : AbstractFileProcess
    {
        #region PRIVATE MEMBERS
        #endregion

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// The new instance of the <see cref="ProcessFileName"/> class created.
        /// </summary>
        //=========================================================================================
        public ProcessFileName() { }
        #endregion

        #region PROTECTED METHODS
        // ReSharper disable PossibleNullReferenceException
        //=========================================================================================
        /// <summary>
        /// For each <see cref="FileMetadataContext.Name"/>, inspects the file name and compares each 
        /// character to the <see cref="ProcessConfigurationReplacements"/> characters; if a character 
        /// or string is found, the original [Key] is replaced with the corresponding [value].
        /// </summary>
        //=========================================================================================
        protected override IEnumerable<FileMetadataContext> Process(ProcessConfiguration source)
        {
            if(null == source) { this.ThrowException(Resources.ExceptionArgumentEmpty.FormatInvariant(StringsParamNames.Source)); }

            if(0 == source.Replacements.Count) { return(source.Files); }

            if(source.IsParallel && source.Files.Count >= source.Threshold)
            {
                Parallel.ForEach(source.Files, instance => ProcessFileName.ReplaceChars(instance, source));
                return(source.Files);
            }

            source.Files.Each(instance => ProcessFileName.ReplaceChars(instance, source));
            return(source.Files);
        }
        // ReSharper restore PossibleNullReferenceException
        #endregion

        #region PRIVATE METHODS
        //=========================================================================================
        /// <summary>
        /// For a large count of files, this method is considerably faster than String.Replace
        /// </summary>
        //=========================================================================================
        private static void ReplaceChars(FileMetadataContext value, ProcessConfiguration source)
        {
            StringBuilder __builder = new StringBuilder(value.Name.TrimLastIndexNonAlphaNumeric(String.Empty), value.Name.Length * 0x000002);
            source.Replacements.Each(replacement => __builder.Replace(replacement.Key, replacement.Value));

            if(source.Replacements.Normalize)
            {
                __builder.RemoveDiacritics();
                __builder.RemoveControlCharacters();
                __builder.RemoveRepeatedWhitespace(); //<-- not comfortable with such linear invocations
            }

            string __modified = __builder.ToString();
            if(__modified.Equals(value.Name, StringComparison.CurrentCultureIgnoreCase)) { return; }

            value.OriginalName = value.Name;
            value.Name = __modified;
        }
        #endregion
    }
}
