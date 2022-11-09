namespace Autumn.File
{
    //=============================================================================================
    /// <summary>
    /// Specifies a configured path with a type of <see cref="ProcessConfigurationPathType"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationPath Type=[{PathType}] Value=[{Value}]")]
    internal abstract class ProcessConfigurationPath
    {
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationPath"/> class.
        /// </summary>
        //=========================================================================================
        protected ProcessConfigurationPath()
        { this.PathType = ProcessConfigurationPathType.Unknown; }

        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationPath"/> class.
        /// </summary>
        //=========================================================================================
        protected ProcessConfigurationPath(ProcessConfigurationPathType type)
        { this.PathType = type; }

        //=========================================================================================
        /// <summary>Gets or sets the value of this path instance.</summary>
        //=========================================================================================
        public string Value {  get; set; }

        //=========================================================================================
        /// <summary>Gets the type of the path as <see cref="ProcessConfigurationPathType"/>.</summary>
        //=========================================================================================
        public ProcessConfigurationPathType PathType { get; private set; }
    }

    //=============================================================================================
    /// <summary>
    /// Specifies a configured path type as <see cref="ProcessConfigurationPathType.Destination"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationDestinationPath Type=[{PathType}] CreateFolders=[{Folders}] Value=[{Value}]")]
    internal sealed class ProcessConfigurationDestinationPath : ProcessConfigurationPath
    {
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationDestinationPath"/> class.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationDestinationPath() : base(ProcessConfigurationPathType.Destination)
        { }

        //=========================================================================================
        /// <summary>
        /// If <c>true</c>, a new folder will be created for each file into which the file will be moved.
        /// </summary>
        //=========================================================================================
        public bool Folders { get; set; }
    }

    //=============================================================================================
    /// <summary>
    /// Specifies a configured path type as <see cref="ProcessConfigurationPathType.Log"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationLogPath Type=[{PathType}] Enabled=[{Enabled}] Value=[{Value}]")]
    internal sealed class ProcessConfigurationLogPath : ProcessConfigurationPath
    {
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationLogPath"/> class.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationLogPath() : base(ProcessConfigurationPathType.Log)
        { }

        //=========================================================================================
        /// <summary>If <c>false</c>, no log file will be written.</summary>
        //=========================================================================================
        public bool Enabled { get; set; }
    }

    //=============================================================================================
    /// <summary>
    /// Specifies a configured path type as <see cref="ProcessConfigurationPathType.Rejected"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationRejectedPath Type=[{PathType}] Append=[{AppendDate}] Format=[{Format}] Value=[{Value}]")]
    internal sealed class ProcessConfigurationRejectedPath : ProcessConfigurationPath
    {
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationRejectedPath"/> class.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationRejectedPath() : base(ProcessConfigurationPathType.Rejected)
        { }

        //=========================================================================================
        /// <summary>
        /// If <c>true</c>, the rejected path name will have the full date and time appended to the end. 
        /// Example rejected-20150203-223345213 (2-3-2015 22:33:45:213)
        /// </summary>
        //=========================================================================================
        public bool AppendDate { get; set; }

        //=========================================================================================
        /// <summary>
        /// If <see cref="AppendDate"/> is <c>true</c>, this is the date and time format to append to 
        /// each rejected file.
        /// </summary>
        //=========================================================================================
        public string Format { get; set; }
    }

    //=============================================================================================
    /// <summary>
    /// Specifies a configured path type as <see cref="ProcessConfigurationPathType.Source"/> and the 
    /// <see cref="ProcessConfigurationPath.Value"/> as the path itself.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationSourcePath Type=[{PathType}] Recursive=[{Recursive}] Value=[{Value}]")]
    internal sealed class ProcessConfigurationSourcePath : ProcessConfigurationPath
    {
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessConfigurationSourcePath"/> class.
        /// </summary>
        //=========================================================================================
        public ProcessConfigurationSourcePath() : base(ProcessConfigurationPathType.Source)
        { }

        //=========================================================================================
        /// <summary>If <c>true</c>, this path and all sub-directories within the path will be read.</summary>
        //=========================================================================================
        public bool Recursive { get; set; }
    }
}
