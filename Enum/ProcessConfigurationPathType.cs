namespace Autumn.File
{
    //=============================================================================================
    /// <summary>Specifies the <see cref="ProcessConfigurationPath"/> classification.</summary>
    //=============================================================================================
    public enum ProcessConfigurationPathType : byte
    {
        Destination = 0,
        Log         = 0x000005,
        Rejected    = 0x000007,
        Source      = 0x000011, 
        Unknown     = 0x0000ff
    };
}
