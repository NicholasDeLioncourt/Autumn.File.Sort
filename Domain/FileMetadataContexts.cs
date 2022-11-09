namespace Autumn.File
{
    using System;
    using System.Collections.Generic;

    //=============================================================================================
    /// <summary>
    /// Specifies a list of <see cref="FileMetadataContexts"/> instances.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("FileMetadataContexts Count=[{Count} file instances]")]
    internal sealed class FileMetadataContexts : List<FileMetadataContext>
    {
        //=========================================================================================
        /// <summary>Gets the <see cref="IEnumerable{FileMetadataContexts}"/>.</summary>
        /// <param name="condition">The conditions by which a <see cref="FileMetadataContext"/> is returned.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">An attempt to invoke SET was made.</exception>
        //=========================================================================================
        public IEnumerable<FileMetadataContext> this[Func<FileMetadataContext, bool> condition]
        {
            get { return(this.FindAll(file => condition(file))); }
            set { throw new NotImplementedException();  }
        }  
    }
}
