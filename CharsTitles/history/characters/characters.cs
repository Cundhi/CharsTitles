using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pdoxcl2Sharp;

namespace CharsTitles
{
    public class Character //: IParadoxRead
    {
        public string ID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Text
        {
            get
            {
                return string.Format("{0} - {1}", ID, Name);
            }
        }

        //public void TokenCallback(ParadoxParser parser, string token)
        //{
        //    int outToken = 0;
        //    if (int.TryParse(token, out outToken))
        //    {
        //        ID = token;
        //    }
        //    var cn = parser.Parse(new charName());
        //    if (cn != null)
        //    {
        //        Name = cn.Name;
        //    }
        //}

        //private class charName : IParadoxRead
        //{
        //    public string Name
        //    {
        //        get; set;
        //    }

        //    public void TokenCallback(ParadoxParser parser, string token)
        //    {
        //        if (token.ToLower() == "name")
        //        {
        //            Name = parser.ReadString();
        //        }
        //    }
        //}
    }

    public class Characters : IParadoxRead
    {
        public List<Character> Items = new List<Character>();

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Character c = new Character();
            int outToken = 0;
            if (int.TryParse(token, out outToken))
            {
                c.ID = token;
                var cn = parser.Parse(new charName());
                if (cn.Name != null && cn.Name != string.Empty)
                {
                    c.Name = cn.Name;
                }
                Items.Add(c);
            }
            //while (!parser.EndOfStream)
            //{
            //    var cc = parser.Parse(new Character());
            //    if (cc != null)
            //    {
            //        Items.Add(cc);
            //    }
            //}
        }

        private class charName : IParadoxRead
        {
            public string Name
            {
                get;set;
            }

            public void TokenCallback(ParadoxParser parser, string token)
            {
                if(token.ToLower() == "name")
                {
                    Name = parser.ReadString();
                }
            }
        }
    }
}
