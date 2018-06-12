using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PSCLibrary;
using System.IO;

namespace Skeleton_PSC_Application
{
    public class ConversionLogic
    {
        private string _inputFile;
        private string _outputFile;
        private string _header;                                 // temp copy of the input file header
        private bool _busy;                                     // sentinel if we're reading from file

        public ConversionLogic()
        {
            _inputFile = @"C:\Users\ebell\Erin\input.txt";
            _outputFile = @"C:\Users\ebell\Erin\output.txt";
            _header = "";
        }

        public string InputFile
        {
            get { return _inputFile; }
            set { _inputFile = value; }
        }

        public string OutputFile
        {
            get { return _outputFile; }
            set { _outputFile = value; }
        }

        public bool IsBusy
        {
            get { return _busy; }
        }

        public void ConvertTime()
        {
            _busy = true;


            StreamReader reader = new StreamReader(_inputFile);


            string line = "";
            _header = reader.ReadLine();
            int numCols = _header.Split(',').Length; // Every column
            int expectedNumTimeStamps = numCols - 1;              // Time Columns

            //List<Document> documentsOriginal = new List<Document>();
            List<Document> documentsConverted = new List<Document>();

            while ((line = reader.ReadLine()) != null)
            {
                Document dOrig = new Document(line);
                //documentsOriginal.Add(dOrig);

                Document dConv = Convert(dOrig);
                documentsConverted.Add(dConv);

                //PrintDocs(dOrig, dConv);
            }


            ExportResults(documentsConverted, expectedNumTimeStamps);
            

            reader.Close();


            

            _busy = false;
        }

        private Document Convert(Document d)
        {
            Document convertedTimeStamps = new Document();
            convertedTimeStamps.DocID = d.DocID;
           
            foreach (string timeStamp in d.TimeStamps)
            {
                int index = timeStamp.IndexOf(':');
                string hours = timeStamp.Substring(0, index);   // Example "09:00:00 AM" gives "09"
                string remainder = timeStamp.Substring(index);  // Keeping colon as the first character of the remainder of timestamp, Example "09:00:00 AM" gives ":00:00 AM"
                if (remainder.LastIndexOf(":") == 0)            // If remainder only has one colon, doesnt track seconds so add ":00"
                {
                    remainder += ":00";
                }
                int convertedHours = Int32.Parse(hours);

                if (timeStamp.Contains("AM"))
                {
                    if (convertedHours == 12)
                        convertedHours = 0;

                    remainder = remainder.Replace("AM", "");
                }

                if (timeStamp.Contains("PM"))
                {
                    convertedHours += 12;
                    remainder = remainder.Replace("PM", "");
                }

                if (remainder.LastIndexOf(':') == (remainder.Length - 1))
                    remainder = remainder.Substring(0, remainder.Length - 1);



                string outputHours = String.Format("{0:0}", convertedHours);
                outputHours = outputHours.PadLeft(2, '0');
                string result = outputHours + remainder;

                convertedTimeStamps.AddTimeStamp(result);
            }
            return convertedTimeStamps;
       }

            /**
               foreach (string timeStamp in d.TimeStamps)
               {
                   int index = timeStamp.IndexOf(':');
                   string hours = timeStamp.Substring(0, index);   // Example "09:00:00 AM" gives "09"
                   string remainder = timeStamp.Substring(index);  // Keeping colon as the first character of the remainder of timestamp, Example "09:00:00 AM" gives ":00:00 AM"
                   string result = hours + remainder;


                   convertedTimeStamps.AddTimeStamp(result);
               }
               return convertedTimeStamps;
           }
        */
        private void ExportResults(List<Document> docs, int expectedNumTimeStamps)
        {
            StreamWriter writer = new StreamWriter(_outputFile);

            bool success = true;
            foreach (Document d in docs)
            {
                if (d.TimeStamps.Count != expectedNumTimeStamps)
                {
                    success = false;
                    Console.WriteLine("Invalid number of time stamps for DOCID " + d.DocID);
                    writer.WriteLine("Invalid number of time stamps for DOCID " + d.DocID);
                }
            }

            if (!success)
            {
                writer.Close();
                return;
            }

            writer.WriteLine(_header);
            
            foreach (Document d in docs)
            {
                Console.Write(d.DocID);
                writer.Write(d.DocID);

                foreach (string timestamp in d.TimeStamps)
                {
                    Console.Write("," + timestamp);
                    writer.Write("," + timestamp);

                }
                Console.WriteLine();
                writer.WriteLine();
            }
            writer.Close();
        }

        private string DoubleQuote(string s)
        {
            return "\"" + s + "\"";                                                 // wrap any string, s, in double quotes
        }

        private string AddEndingSlash(string path)
        {
            int lastChar = path.Length - 1;
            if (lastChar >= 0 && path[lastChar] != '\\')        // add an ending slash
                path += "\\";
            return path;
        }
    }
}
