using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EGVDownloader
{
    public class DownloadData
    {
        /// <summary>
        /// Get the XMLDocument for the G4 data
        /// </summary>
        public XmlDocument ReceiverData 
        { 
            get 
            { return pReceiverData; } 
        }
        
        /// <summary>
        /// Check for connection
        /// </summary>
        public bool ReceiverConnected
        {
            get
            { return new Dexcom.ReceiverApi.ReceiverApi().IsReceiverAttached; }
        }

        /// <summary>
        /// Set for download success
        /// </summary>
        public bool DidDownload = false;

        private XmlDocument pReceiverData = new XmlDocument();

        /// <summary>
        /// Pretty straightforward use of Dexcom's API - receiver data gets parsed as needed
        /// </summary>
        public DownloadData()
        {
            if (ReceiverConnected)
            {
                pReceiverData = Dexcom.ReceiverApi.DownloadReceiverHelper.DownloadReceiver();
                DidDownload = true;
            }
            else
            {
                DidDownload = false;
            }

        }

    }
}
