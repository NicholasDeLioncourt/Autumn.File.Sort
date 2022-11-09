namespace Autumn.File
{
    using System.Runtime.InteropServices;
    //=============================================================================================
    /// <summary>Common constants employed by this application.</summary>
    //=====================================================================================LNDL====
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct PunctuationStrings
    {
        /// <summary>The backslash [\]</summary>
        public static readonly string Backslash = "\\";
        /// <summary>The comma [,]</summary>
        public const string Comma = ",";
        /// <summary>The dash [-]</summary>
        public static readonly string Dash = "-";
        /// <summary>An ellipsis is a row of three periods or full stops (...)</summary>
        public static readonly string Ellipsis = "...";
        /// <summary>the equality character</summary>
        public static readonly string Equal = "=";
        /// <summary>the quote character</summary>
        public static readonly string Quote = "\"";
        /// <summary>quoted empty string</summary>
        public static readonly string QuotedEmpty = "\"\"";
        /// <summary>quoted string &quot;{0}&quot;</summary>
        public static readonly string QuotedString = "\"{0}\"";
        /// <summary>The dot, period</summary>
        public static readonly string Period = ".";
        /// <summary>The space character</summary>
        public static readonly string Space = " ";
        /// <summary>Also known as forward slash</summary>
        public static readonly string Virgule = "/";
        /// <summary>number 0 as a string</summary>
        public static readonly string ZeroString = "0";
    }
}
