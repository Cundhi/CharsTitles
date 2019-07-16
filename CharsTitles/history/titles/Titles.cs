using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pdoxcl2Sharp;

namespace CharsTitles
{
    public class Title : IParadoxRead, IParadoxWrite
    {
        public string Time
        {
            get;set;
        }

        public string HoldID
        {
            get;set;
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == Time)
            {
                var hi = parser.Parse(new holdId());
                if (hi.ID != null && hi.ID != string.Empty)
                {
                    HoldID = hi.ID;
                }
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine(string.Empty);
            writer.WriteLine(string.Format("{0}={{holder={1}}}", Time, HoldID));
        }

        private class holdId : IParadoxRead
        {
            public string ID
            {
                get; set;
            }

            public void TokenCallback(ParadoxParser parser, string token)
            {
                if (token.ToLower() == "holder")
                {
                    ID = parser.ReadString();
                }
            }
        }
    }
}
