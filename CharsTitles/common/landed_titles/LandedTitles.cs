using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pdoxcl2Sharp;

namespace CharsTitles
{
    public class LandedTitle
    {
        public List<LandedTitle> Children = new List<LandedTitle>();

        public string Name
        {
            set;get;
        }

        //public string Parent
        //{
        //    set;get;
        //}

        //public LandedTitle(string name, string parent)
        //{
        //    Name = name;
        //    Parent = parent;
        //}
    }

    public class LandedTitles : IParadoxRead
    {
        public static List<LandedTitle> Itmes = new List<LandedTitle>(); 

        public void TokenCallback(ParadoxParser parser, string token)
        {
            LandedTitle c = new LandedTitle();
            if(token.StartsWith("e_"))
            {
                c.Name = token;
                var k = new K();
                parser.Parse(k);
                if (k.Name != null && k.Name != string.Empty)
                {
                    c.Children.Add(k);
                }
                LandedTitles.Itmes.Add(c);
            }
        }

        public class K : LandedTitle, IParadoxRead 
        {
            public void TokenCallback(ParadoxParser parser, string token)
            {
                if (token.StartsWith("k_"))
                {
                    Name = token;
                    var d = parser.Parse(new D());
                    if (d.Name != null && d.Name != string.Empty)
                    {
                        this.Children.Add(d);
                    }
                }
            }

            public class D : LandedTitle, IParadoxRead
            {
                public void TokenCallback(ParadoxParser parser, string token)
                {
                    if (token.StartsWith("d_"))
                    {
                        Name = token;
                        var c = parser.Parse(new C());
                        if (c.Name != null && c.Name != string.Empty)
                        {
                            this.Children.Add(c);
                        }
                    }
                }

                public class C : LandedTitle, IParadoxRead
                {
                    public void TokenCallback(ParadoxParser parser, string token)
                    {
                        if (token.StartsWith("c_"))
                        {
                            Name = token;
                        }
                    }
                }
            }
        }
    }
}
