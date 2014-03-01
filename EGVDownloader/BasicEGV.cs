using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EGVDownloader
{
    public class BasicEGV
    {

        /// <summary>
        /// Most Recent EGV Record on G4
        /// </summary>
        public Dexcom.ReceiverApi.EGVRecord NewestEGV
        {
            get
            {
                return pAllEGV.Last();
            }
        }

        //Private
        private System.Xml.XmlDocument pReceiverData;
        private List<Dexcom.ReceiverApi.EGVRecord> pAllEGV;

        /// <summary>
        /// Contructor for BasicEGV class object
        /// </summary>
        /// <param name="receiverData">XML formatted data from G4</param>
        public BasicEGV(System.Xml.XmlDocument receiverData)
        {
            pReceiverData = receiverData;
            Dexcom.ReceiverApi.ReceiverDatabaseRecordsParser parser = new Dexcom.ReceiverApi.ReceiverDatabaseRecordsParser();
            parser.Parse(pReceiverData);
            pAllEGV = parser.EgvRecords;
        }

        /// <summary>
        /// Save EGVRecords to CSV file
        /// </summary>
        /// <param name="pnfn">Path and filename to save to</param>
        public void SaveEGVasCSV(string pnfn)
        {

            Type type = typeof(Dexcom.ReceiverApi.EGVRecord);
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            var sb = new StringBuilder();

            // First line contains field names
            foreach (System.Reflection.PropertyInfo prp in properties)
            {
                if (prp.CanRead)
                {
                    sb.Append(prp.Name).Append(',');
                }
            }
            sb.Length--; // Remove last ","
            sb.AppendLine();

            foreach (Dexcom.ReceiverApi.EGVRecord egv in pAllEGV)
            {
                foreach (System.Reflection.PropertyInfo prp in properties)
                {
                    if (prp.CanRead)
                    {
                        sb.Append(prp.GetValue(egv, null)).Append(',');
                    }
                }
                sb.Length--; // Remove last ","
                sb.AppendLine();
            }

            //CURRENTLY: Always overwrites existing file
            using (StreamWriter outfile = new StreamWriter(pnfn))
            {
                outfile.Write(sb.ToString());
            }
        }


    }
}
