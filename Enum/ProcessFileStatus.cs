namespace Autumn.File
{
    //=============================================================================================
    /// <summary>Specifies the <see cref="FileMetadataContext"/> status.</summary>
    //=============================================================================================
    public enum FileStatus : byte
    {
        /// <summary>The current file exists; no errors.</summary>
        Exists    = 0x000003,
        /// <summary>The current file exists but access was denied.</summary>
        Denied    = 0x000005,
        /// <summary>The current file was rejected.</summary>
        Duplicate = 0x000007,
        /// <summary>The current file could not be found.</summary>
        Missing   = 0x00000B,
        /// <summary>The current file was rejected.</summary>
        Rejected  = 0x000011,
        /// <summary>The current file status is unknown; this is an error condition.</summary>
        Unknown   = 0x0000ff
    };
}
