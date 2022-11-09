using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.File
{
    class Program
    {
        //=========================================================================================
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        //========================================================================================= 
        static void Main(string [] args)
        {
            Console.WriteLine("start");
            // 1. TEST READ CONFIGURATION FILE
            ProcessConfiguration __configuration = ProcessConfigurationReader.Current.Configuration;
            //Console.WriteLine(__configuration.ContinueOnError);

            // 2. CREATE 0KB DUMMY FILES
            //for(int __index = 0; __index < 25; __index++)
            //   { System.IO.File.Create(@"C:\Projects\FileToPath\FileToPathUnitTests\TestFiles\test_type3_{0}.xlsx".FormatInvariant(__index)).Dispose(); }

            // 3. READ FILES
            foreach(FileMetadataContext __result in ProcessFileReader.Current
                                                                     .OnException(fault => Console.WriteLine(fault.Message))
                                                                     .Enumerate(__configuration))
            { __configuration.Files.Add(__result); }
            
            AbstractFileProcess __process_a = new ProcessFileName();
                                __process_a.Enumerate(__configuration);
            AbstractFileProcess __process_b = new ProcessFolderName();
                                __process_b.Enumerate(__configuration);
            AbstractFileProcess __process_c = new ProcessFileMove();
                                __process_c.Enumerate(__configuration);
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
