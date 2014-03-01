using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DexcomWrapperTest
{
    [TestClass]
    public class BasicRun
    {
        [TestMethod]
        public void TestMethod1()
        {
            EGVDownloader.DownloadData download = new EGVDownloader.DownloadData();

            if (download.DidDownload)
            {
                EGVDownloader.BasicEGV glucoseValues = new EGVDownloader.BasicEGV(download.ReceiverData);
                glucoseValues.SaveEGVasCSV(@"c:\test.csv");
                
            }
        }
    }
}
