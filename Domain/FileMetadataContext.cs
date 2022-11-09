namespace Autumn.File
{
    using System;
    using System.IO;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// Represents specific information about a file; a simplified cognate to <see cref="System.IO.FileInfo"/>.
    /// </summary>
    /// <created>l. nicholas de lioncourt</created>
    /// <remarks>
    /// A less resource consumptive instance of System.IO.FileInfo, and without repository methods.
    /// </remarks>
    //=============================================================================================
    [Serializable, System.Diagnostics.DebuggerDisplay("FileMetadataContext ID=[{Id}] Name=[{Name}] Status=[{Status}] Path=[{FullPath}]")]
    public class FileMetadataContext
    {
        #region PRIVATE MEMBERS
        private int __id;
        private long ? __size;

        private string __comment;
        private string __contenttype;
        private string __extension;
        private string __fullpath;
        private string __key;
        private string __name;
        private string __originalname;
        private string __originalendpoint;
        private string __endpoint;

        private DateTime ? __created;
        private DateTime ? __modified;

        private FileStatus __status;

        protected static readonly object Exclusive = new object();
        #endregion

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="FileMetadataContext"/> class.
        /// </summary>
        //=========================================================================================
        private FileMetadataContext()
        { }

        //=========================================================================================
        /// <summary>
        /// Returns a new and empty instance of the <see cref="FileMetadataContext"/> class.
        /// </summary>
        //=========================================================================================
        public static FileMetadataContext Initialize()
        {
            return(new FileMetadataContext());
        }

        //=========================================================================================
        /// <summary>
        /// Returns a new instance of the <see cref="FileMetadataContext"/> class.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo"/> for the source file.</param>
        /// <param name="throwifinvalid">
        /// If set to <c>true</c> all exceptions will be thrown; otherwise returns a new and empty 
        /// instance of the <see cref="FileMetadataContext"/> class. Default is <c>true</c>.
        /// </param>
        //=========================================================================================
        public static FileMetadataContext Initialize(FileSystemInfo file, bool throwifinvalid = true)
        {
            if(null == file)
            {
                if(throwifinvalid) { throw new ArgumentNullException(Resources.ExceptionRequireArgumentIsNotNull.FormatInvariant(StringsParamNames.File)); }
                return(FileMetadataContext.Initialize());
            }

            if(!file.Exists)
            {
                if(throwifinvalid) { throw new FileNotFoundException(Properties.Resources.ExceptionFullPathNotValid.FormatInvariant(file.FullName), file.FullName); }
                return(FileMetadataContext.Initialize());
            }

            if((file.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                if(throwifinvalid) { throw new FileNotFoundException(Properties.Resources.ExceptionFullPathNotValid.FormatInvariant(file.FullName), file.FullName); }
                return(FileMetadataContext.Initialize());
            }

            FileMetadataContext __result = new FileMetadataContext
                                        {
                                            Created   = file.CreationTimeUtc,
                                            Modified  = file.LastWriteTimeUtc,
                                            Extension = file.Extension,
                                            FullPath  = file.FullName,
                                            Key       = Path.GetFileNameWithoutExtension(file.Name),
                                            Name      = Path.GetFileNameWithoutExtension(file.Name),
                                            Status    = FileStatus.Exists
                                        };
            return(__result);
        }
        #endregion

        #region PUBLIC ACCESSORS
        //=========================================================================================
        /// <summary>Any message or comment about this file.</summary>
        //=========================================================================================
        public virtual string Comment
        {
            get { return(this.__comment ?? String.Empty); }
            set { lock(Exclusive) this.__comment = value; }
        }

        //=========================================================================================
        /// <summary>
        /// An Internet media type, originally called a MIME type after MIME (Multipurpose Internet Mail 
        /// Extensions) and sometimes a Content-type after the name of a header in several protocols 
        /// whose value is such a type, is a two-part identifier for file formats on the Internet.
        /// </summary>
        //=========================================================================================
        public virtual string ContentType
        {
            get { return(this.__contenttype ?? String.Empty); }
            set { lock(Exclusive) this.__contenttype = value; }
        }

        //=========================================================================================
        /// <summary>
        /// Gets or sets the initial create date and time in UTC if available; or, <c>Date.HasValue == false;</c>
        /// </summary>
        //=========================================================================================
        public virtual DateTime ? Created
        {
            get { return(this.__created); }
            set { lock(Exclusive) this.__created = value; }
        }

        //=========================================================================================
        /// <summary>Returns <c>true</c> if the file exists.</summary>
        //=========================================================================================
        public virtual bool Exists
        {
            get { return(this.Status == FileStatus.Exists); }
        }

        //=========================================================================================
        /// <summary>Gets or sets the path to the file.</summary>
        //=========================================================================================
        public virtual string Endpoint
        {
            get { return(this.__endpoint ?? String.Empty); }
            set
            {
                if(value.IsNullOrEmpty()) { lock(Exclusive) this.__endpoint = String.Empty; return; }
                char __lastchar = value[value.Length - 1];

                lock(Exclusive)
                {
                    if(__lastchar == System.IO.Path.DirectorySeparatorChar || __lastchar == System.IO.Path.AltDirectorySeparatorChar)
                      { this.__endpoint = value; return; }

                    this.__endpoint = String.Concat(value, System.IO.Path.DirectorySeparatorChar);
                }
            }
        }

        //=========================================================================================
        /// <summary>Gets the string representing the extension part of the file.</summary>
        //=========================================================================================
        public string Extension
        {
            get
            {
                lock(Exclusive)
                if(this.__extension.IsNullOrEmpty()) { this.__extension = this.Name.IsNullOrEmpty() ? String.Empty: System.IO.Path.GetExtension(this.Name); }

                return(this.__extension);
            }
            private set
            {
                lock(Exclusive) this.__extension = value;
            }
        }

        //=========================================================================================
        /// <summary>Gets or sets an external identifier for this instance.</summary>
        //=========================================================================================
        public virtual int Id
        {
            get { return(this.__id); }
            set { lock(Exclusive) this.__id = value; }
        }

        //=========================================================================================
        /// <summary>Gets or sets the key of this instance.</summary>
        //=========================================================================================
        public virtual string Key
        {
            get { return(this.__key ?? String.Empty); }
            set { lock(Exclusive) this.__key = value; }
        }

        //=========================================================================================
        /// <summary>Gets or sets the last modified date and time; or, <c>Date.HasValue == false;</c></summary>
        //=========================================================================================
        public virtual DateTime ? Modified
        {
            get { return(this.__modified); }
            set { lock(Exclusive) this.__modified = value; }
        }

        //=========================================================================================
        /// <summary>Gets the current (file) name for this <see cref="FileMetadataContext"/>.</summary>
        //=========================================================================================
        public virtual string Name
        {
            get { return(this.__name ?? String.Empty); }
            set 
            {
                if(value.IsNullOrEmpty()) { return; }
                if(this.__originalname.IsNullOrEmpty()) { this.OriginalName = this.__name; }
                lock(Exclusive) { this.__name = value; }
            }
        }

        //=========================================================================================
        /// <summary>The size of this file in BYTES.</summary>
        //=========================================================================================
        public virtual long Size
        {
            get { return(this.__size.HasValue ? this.__size.Value : 0L); }
            set { lock(Exclusive) this.__size = value; }
        }

        //=========================================================================================
        /// <summary>The <see cref="FileStatus"/>.</summary>
        //=========================================================================================
        public FileStatus Status
        {
            get { return(this.__status); }
            set { lock(Exclusive) this.__status = value; }
        }
        #endregion

        #region INTERNAL ACCESSORS
        //=========================================================================================
        /// <summary>The original path and file name when this instance was created.</summary>
        //=========================================================================================
        protected internal string FullPath
        {
            get { return(this.__fullpath ?? String.Empty); }
            private set { lock(Exclusive) this.__fullpath = value; }
        }

        //=========================================================================================
        /// <summary>If the file was renamed, this is the original file name.</summary>
        //=========================================================================================
        internal virtual string OriginalName
        {
            get { return(this.__originalname.IsNullOrEmpty() ? this.Name : this.__originalname); }
            set { lock(Exclusive) this.__originalname = value; }
        }

        //=========================================================================================
        /// <summary>If the path was renamed, this is the original path name.</summary>
        //=========================================================================================
        internal virtual string OriginalEndpoint
        {
            get { return(this.__originalendpoint.IsNullOrEmpty() ? this.Endpoint : this.__originalendpoint); }
            set
            {
                if(value.IsNullOrEmpty()) { return; }
                if(this.__originalendpoint.IsNullOrEmpty()) { this.OriginalEndpoint = this.__endpoint; }
                lock(Exclusive) { this.__endpoint = value; }
            }
        }
        #endregion

        #region PUBLIC METHODS
        //=========================================================================================
        /// <summary>
        /// Clears all properties, assigning each the default value of its <see cref="Type"/>.
        /// </summary>
        //=========================================================================================
        public virtual void Clear()
        {
            lock(Exclusive)
            {
                this.__comment          = null;
                this.__contenttype      = null;
                this.__created	        = null;
                this.__extension        = null;
                this.__fullpath         = null;
                this.__key		        = null;
                this.__modified	        = null;
                this.__name		        = null;
                this.__originalname     = null;
                this.__originalendpoint = null;
                this.__id		        = default(int);
                this.__status           = FileStatus.Unknown;

                if(this.__size.HasValue) { this.__size = null; }
            }
        }
        #endregion

        #region GETHASHCODE
        //=========================================================================================
        /// <summary> Serves as a hash function for a particular type.</summary>
        //=========================================================================================
        public override int GetHashCode()
        {
            unchecked
            {
                int __result  = (this.ContentType.IsNullOrEmpty() ? 0x000001 : this.ContentType.GetHashCode() * 0x00018d);
                    __result ^= (this.Extension.IsNullOrEmpty() ? 0x000003 : this.Extension.GetHashCode());
                    __result ^= (this.Endpoint.IsNullOrEmpty() ? 0x000005 : this.Endpoint.GetHashCode());
                    __result ^= (this.Created.HasValue ? 0x000007 : this.Created.GetHashCode()) ^
                                (this.Modified.HasValue ? 0x00000B : this.Modified.GetHashCode());
                    __result ^= (this.Comment.IsNullOrEmpty() ? 0x000011 : this.Comment.GetHashCode());
                    __result ^= (this.Key.IsNullOrEmpty() ? 0x00000D : this.Key.GetHashCode()) | this.Id.GetHashCode();
                    __result ^=	(this.Name.IsNullOrEmpty() ? 0x0006CD : (this.Name.GetHashCode() | 
                                 this.Name.ToUpperInvariant().GetHashCode()));
                    __result ^= (this.OriginalName.IsNullOrEmpty() ? 0x001505 : this.OriginalName.GetHashCode());
                    __result ^= (this.OriginalEndpoint.IsNullOrEmpty() ? 0x001507 : this.OriginalEndpoint.GetHashCode());
                    __result ^= (this.FullPath.GetHashCode() ^ this.Status.GetHashCode());
                return(__result);
            }
        }
        #endregion

        #region TO STRING
        //=========================================================================================
        /// <summary>
        /// Returns a <see cref="System.String"/> which represents this instance.
        /// </summary>
        //=========================================================================================
        public override string ToString()
        {
            return(Path.Combine(this.Endpoint, this.Name));
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this <see cref="FileMetadataContext"/> instance.
        /// </summary>
        //=========================================================================================
        public static implicit operator string(FileMetadataContext instance)
        {
            return(null == instance ? String.Empty : instance.ToString());
        }

        //=========================================================================================
        /// <summary>
        /// Returns a <see cref="System.Int64"/> that represents the <see cref="Size"/> instance.
        /// </summary>
        //=========================================================================================
        public static implicit operator Int64(FileMetadataContext instance)
        {
            return(null == instance ? 0L : instance.Size);
        }
        #endregion
    }
}
