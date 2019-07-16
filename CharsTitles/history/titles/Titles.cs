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
        List<string> Others = new List<string>();
        List<string> Times = new List<string>();

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

            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
