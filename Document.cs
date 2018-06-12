using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skeleton_PSC_Application
{
    public class Document
    {
        private string _docID;
        private List<string> _timeStamps;

        public Document()
        {
            _docID = "";
            _timeStamps = new List<string>();
        }

        public Document(string line)
        {
            string[] columns = line.Split(',');
            _docID = columns[0];

            _timeStamps = new List<string>();
            for (int i = 1; i < columns.Length; i++)
            {
                string timeStamp = columns[i];
                timeStamp = timeStamp.Replace(" ", "");        // Checks for spaces in input and replaces with no spaces
                timeStamp = timeStamp.Replace("\t", "");      //  Checks for empty time values

                _timeStamps.Add(timeStamp);
            }
        }

        public string DocID
        {
            get
            {
                return _docID;
            }
            set
            {
                _docID = value;
            }
        }

        public List<string> TimeStamps
        {
            get
            {
                return _timeStamps;
            }
        }

        public void AddTimeStamp(string t)
        {
            _timeStamps.Add(t);
        }
    }
}