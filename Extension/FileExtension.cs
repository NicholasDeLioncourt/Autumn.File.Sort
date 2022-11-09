using System;
using System.Collections.Generic;
using System.Linq;

namespace Autumn.File
{
    using System.IO;
    //=============================================================================================
    /// <summary>System.IO extension methods.</summary>
    /// <created>LNDL</created>
    /// <copyright file="FileExtension.cs" company="De Lioncourt LLC">
    /// L. Nicholas de Lioncourt. All code herein is proprietary. It is OK to use 
    /// this as long as my name remains in this file.
    /// </copyright>
    //=============================================================================================
    internal static class FileExtension
    {
        //=========================================================================================
        /// <summary>
        /// Returns the <see cref="FileSystemInfo.Extension"/> without the leading dot (period); 
        /// as an example the extension [.docx] would be returned as just [docx] and myfile.readme.txt 
        /// would return [txt].
        /// </summary>
        //=========================================================================================
        public static string ExtensionNoDot(this FileSystemInfo file)
        {
            int __position = file.Extension.LastIndexOf(Chars.CharPeriod);
            return(-1 == __position ? file.Extension : file.Extension.Substring(__position + 1));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the <see cref="FileMetadataContext.Extension"/> matches one of the 
        /// popular image type extensions.
        /// </summary>
        //=========================================================================================
        public static bool IsImage(this FileMetadataContext file)
        {
            return(CommonLists.CommonImageExtensions.Any(value => value.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the <see cref="FileSystemInfo.Extension"/> matches one of the 
        /// popular image type extensions.
        /// </summary>
        //=========================================================================================
        public static bool IsImage(this FileSystemInfo file)
        {
            return(CommonLists.CommonImageExtensions.Any(value => value.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)));
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> if the <see cref="FileSystemInfo.Extension"/> is one known within the 
        /// <see cref="System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()"/>.
        /// </summary>
        //=========================================================================================
        public static bool IsKnownImageType(this FileSystemInfo file)
        {
            string __extension = file.Extension;
            if(__extension.IsNullOrEmpty()) { return(false); }

            List<string> __knowntypes = new List<string>(0x000020);
                         __extension  = Chars.CharAsterisk + __extension.ToLowerInvariant();

            System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().Each(codecinfo => __knowntypes
                                                                    .AddRange(codecinfo.FilenameExtension.ToLowerInvariant()
                                                                    .Split(new[]{Chars.CharSemicolon})));
            return(__knowntypes.Any(type => type.Equals(__extension)));
        }
    }
}
