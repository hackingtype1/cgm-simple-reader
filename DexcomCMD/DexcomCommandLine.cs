using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using CommandLine.Parsing;

namespace DexcomCMD
{
    class DexcomCommandLine
    {
        private static readonly HeadingInfo HeadingInfo = new HeadingInfo("G4_CMD_Tool", "0.0");

        /// <summary>
        /// Command Line arguments
        /// </summary>
        class Options
        {
            [Option('f', "file", DefaultValue = @"C:\G4Output.csv",
              HelpText = "Output path and filename e.g. c:\\output.csv")]
            public string OutputFile { get; set; }

            [Option('t', "interval", DefaultValue = "30000",
              HelpText = "Time (in milliseconds) between G4 Reads")]
            public string Time { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        /// <summary>
        /// CommandLine Main
        /// </summary>
        /// <param name="args">Input parameters</param>
        static void Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);

            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
            {
                Run(options);
            }
        }

        /// <summary>
        /// Basic run through, timer setup for read and data save from G4
        /// </summary>
        /// <param name="options">Parsed command line parameters</param>
        private static void Run(Options options)
        {

            if (!string.IsNullOrEmpty(options.Time))
            {
                HeadingInfo.WriteMessage(string.Format("G4 will be polled every {0} milliseconds", options.Time));
            }

            if (!string.IsNullOrEmpty(options.OutputFile))
            {
                HeadingInfo.WriteMessage(string.Format("Writing G4 data to: {0}", options.OutputFile));
            }
            else
            {
                HeadingInfo.WriteMessage("G4 data could not be written.");
                Console.WriteLine("[...]");
            }

            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler((sender, e) => ReadAndSaveEGV(sender, e, options));
            myTimer.Interval = Int32.Parse(options.Time);
            myTimer.Start();

            while (Console.Read() != 'q')
            {
                ;    // do nothing...
            }

        }

        /// <summary>
        /// Primary logic for read and save from G4
        /// </summary>
        private static void ReadAndSaveEGV(object sender, ElapsedEventArgs e, Options options)
        {
            HeadingInfo.WriteMessage("Retrieving Database Pages from G4.");
            EGVDownloader.DownloadData download = new EGVDownloader.DownloadData();

            if (download.DidDownload)
            {
                HeadingInfo.WriteMessage(string.Format("Saving G4 data to: {0}", options.OutputFile ));
                EGVDownloader.BasicEGV glucoseValues = new EGVDownloader.BasicEGV(download.ReceiverData);
                glucoseValues.SaveEGVasCSV(options.OutputFile);

            }
            else
            {
                HeadingInfo.WriteMessage("G4 data could not be retrieved.");
            }
            HeadingInfo.WriteMessage("Type 'q' (and Enter) to exit task");
        }
        
    }
}
