namespace Autumn.File
{
    using System.Collections.Generic;

    //=============================================================================================
    /// <summary>
    /// Specifies a list of <see cref="Dictionary{TKey,TValue}"/> instances; each one representing 
    /// a key-value pair with which the <c>key</c> string will be replaced by the <c>value</c> 
    /// string for each file name.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationReplacements Count=[{Count} Replacements] Normalize=[{Normalize}]")]
    internal sealed class ProcessConfigurationReplacements : Dictionary<string, string>
    {
        //=========================================================================================
        /// <summary>
        /// If <c>true</c>, replace repeated spaces with one space and remove non-printable characters.
        /// </summary>
        //=========================================================================================
        public bool Normalize { get; set; }
    }
}
