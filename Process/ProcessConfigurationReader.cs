namespace Autumn.File
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>
    /// Finds and thereafter parses the Process.config into an instance of <see cref="ProcessConfiguration"/>.
    /// </summary>
    //=============================================================================================
    [System.Diagnostics.DebuggerDisplay("ProcessConfigurationReader Instance Key=[{Key}] Initialized=[{__isinitialized}]")]
    internal sealed class ProcessConfigurationReader
    {
        #region PRIVATE MEMBERS
        private static bool __isinitialized; //<-- not volatile since used as ref
        private static object __synchronize;

        //==> CONST
        private const string __configsource = @"Process.config";
        private static ProcessConfigurationReader __reader;
        private readonly Lazy<ProcessConfiguration> __configuration = new Lazy<ProcessConfiguration>(() => new ProcessConfiguration(), true); 
        #endregion

        #region CONSTRUCTORS
        //=========================================================================================
        /// <summary>
        /// Prevents a default instance of the <see cref="ProcessConfigurationReader"/> class from being created.
        /// </summary>
        //=========================================================================================
        private ProcessConfigurationReader() { this.Key = new Guid().ToString(); }

        //=========================================================================================
        /// <summary>Initializes the <see cref="ProcessConfigurationReader"/> class.</summary>
        //=========================================================================================
        static ProcessConfigurationReader() {}
        #endregion

        #region PUBLIC ACCESSORS
        //=========================================================================================
        /// <summary>Gets the current <see cref="ProcessConfiguration"/> Instance.</summary>
        //=========================================================================================
        public ProcessConfiguration Configuration
        {
            get
            {
                lock(__synchronize)
                    { if(!this.__configuration.IsValueCreated) { this.ReadConfiguration(); }}

                return(this.__configuration.Value);
            }
        }

        //=========================================================================================
        /// <summary>Gets the Singleton <see cref="ProcessConfigurationReader"/> Instance.</summary>
        //=========================================================================================
        public static ProcessConfigurationReader Current
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref __reader,
                                                  ref __isinitialized,
                                                  ref __synchronize, () =>
                                                  new ProcessConfigurationReader().ReadConfiguration());
                return(__reader);
            }
        }

        //=========================================================================================
        /// <summary>Gets the current <see cref="ProcessConfiguration"/> instance key.</summary>
        //=========================================================================================
        public string Key
        {
            get; private set;
        }
        #endregion

        #region PRIVATE ACCESSORS
        // ReSharper disable ForCanBeConvertedToForeach
        //=========================================================================================
        /// <summary>Returns the full path to the configuration file for this process.</summary>
        //=========================================================================================
        private static string Source
        {
            get
            {
                string [] __paths = { Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), 
                                      Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location),
                                      Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)};

                //===> first try the primary paths
                for(int __index = 0; __index < __paths.Length; __index++)
                {
                    string __path = Path.Combine(__paths[__index], __configsource);
                    if(System.IO.File.Exists(__path)) { return(__path); }
                }
                //==> not found, try the subdirectories
                foreach(string __path in __paths.SelectMany(subdirectory => Directory.GetDirectories(subdirectory)
                                                .Select(current => Path.Combine(current, __configsource))
                                                .Where(System.IO.File.Exists))) { return(__path); }
                throw new FileNotFoundException(Resources.ExceptionProcessConfigNotFound.FormatCulture(__configsource), __configsource);
            }
        }
        // ReSharper restore ForCanBeConvertedToForeach
        #endregion

        #region PRIVATE METHODS
        // ReSharper disable PossibleNullReferenceException
        //=========================================================================================
        /// <summary>
        /// Parses the XML configuration into an instance of <see cref="ProcessConfiguration"/>.
        /// </summary>
        //=========================================================================================
        private ProcessConfigurationReader ReadConfiguration()
        {
            XDocument __document = XDocument.Load(Source, LoadOptions.PreserveWhitespace);

            //==> Node <process />
            XElement __root = __document.Element(CommonStrings.Process);
            if(null == __root) { return(this); }

            ProcessConfiguration __target = __configuration.Value;

            //==> Node <isparallel />
            XElement __parallelelement = __root.Element(StringsParamNames.IsParallel);
                     __parallelelement.IfNotNull(() =>
                     {
                         __target.IsParallel = __parallelelement.ValueAsBool();
                         __target.Threshold  = __parallelelement.Attribute(StringsParamNames.Threshold).ValueAsInt32();
                     });

            //==> Node <continueonerror />
            XElement __continueerrorelement = __root.Element(StringsParamNames.ContinueError);
                     __continueerrorelement.IfNotNull(() => __target.ContinueOnError = __continueerrorelement.ValueAsBool());

            //==> Node <paths />
            XElement __pathselement = __root.Element(StringsParamNames.Paths);
            if(__pathselement.IsNull()) { return(this); }

            XElement __logelement = __pathselement.Element(StringsParamNames.Log);
                     __logelement.IfNotNull(() =>
                     {
                        ProcessConfigurationLogPath __log = new ProcessConfigurationLogPath
                                                            {
                                                                Enabled = __logelement.Attribute(StringsParamNames.Enabled).ValueAsBool(false),
                                                                Value   = __logelement.ValueAsString(() => String.Empty)
                                                            }; 
                                                    __target.Paths.Add(__log);
                     });

            XElement __rejectedelement = __pathselement.Element(StringsParamNames.Rejected);
                     __rejectedelement.IfNotNull(() =>
                     {
                         ProcessConfigurationRejectedPath __rejected = new ProcessConfigurationRejectedPath
                         {
                             AppendDate = __rejectedelement.Attribute(StringsParamNames.AppendDate).ValueAsBool(false),
                             Format     = __rejectedelement.Attribute(StringsParamNames.Format).ValueAsString(() => Resources.FormatRejectedDateTime),
                             Value      = __rejectedelement.ValueAsString(() => String.Empty)
                         };
                         __target.Paths.Add(__rejected);
                     });

            XElement __destinationelement = __pathselement.Element(StringsParamNames.Destination);
                     __destinationelement.IfNotNull(() =>
                     {
                        ProcessConfigurationDestinationPath __destination = new ProcessConfigurationDestinationPath
                                                                            {
                                                                                Folders = __destinationelement.Attribute(StringsParamNames.Folders).ValueAsBool(false),
                                                                                Value = __destinationelement.ValueAsString(() => String.Empty)
                                                                            };
                                                            __target.Paths.Add(__destination);
                     });

            XElement __sourceselement = __pathselement.Element(StringsParamNames.Sources);
                     __sourceselement.IfNotNull(() => __sourceselement.Elements(StringsParamNames.Source).Each(sourceelement =>
                     {
                        ProcessConfigurationSourcePath __source = new ProcessConfigurationSourcePath
                                                                  {
                                                                      Recursive = sourceelement.Attribute(StringsParamNames.Recursive).ValueAsBool(false),
                                                                      Value     = sourceelement.ValueAsString(() => String.Empty)
                                                                  };
                                                       __target.Paths.Add(__source);
                     }));

            //==> Node <extensions />
            XElement __extensionselement = __root.Element(StringsParamNames.Extensions);
                     __extensionselement.IfNotNull(() =>
                     {
                        __target.Extensions.LowerCase = __extensionselement.Attribute("lowercase").ValueAsBool(false);
                        __extensionselement.Elements(StringsParamNames.Extension).Each(sourceelement => __target.Extensions.Add(sourceelement.ValueAsString(() => String.Empty).ToLowerInvariant()));
                     });

            //==> Node <replacements />
            XElement __replacementselement = __root.Element(StringsParamNames.Replacements);
            if(null == __replacementselement) { return(this); }

            __target.Replacements.Normalize = __replacementselement.Attribute(StringsParamNames.Normalize).ValueAsBool(false);

            __replacementselement.Elements(StringsParamNames.Replace).Each(sourceelement =>
            __target.Replacements.Add(sourceelement.Attribute(StringsParamNames.Value).ValueAsString(() => String.Empty),
                                      sourceelement.Attribute(StringsParamNames.With).ValueAsString(()  => Chars.CharSpace.ToString(CultureInfo.InvariantCulture))));

            return(this);
        }
        // ReSharper restore PossibleNullReferenceException
        #endregion 
    }
}
