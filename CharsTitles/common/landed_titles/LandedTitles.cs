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

        public LandedTitle parent;

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

    public class Empires : IParadoxRead
    {
        public List<LandedTitle> Itmes = new List<LandedTitle>(); 

        public void TokenCallback(ParadoxParser parser, string token)
        {
            try
            {
                if (token.StartsWith("e_"))
                {
                    var e = parser.Parse(new K());
                    e.Name = token;
                    this.Itmes.Add(e);
                }
            }
            catch(Exception ex)
            {
                //throw ex;
                return;
            }
        }

        public class K : LandedTitle, IParadoxRead 
        {
            public void TokenCallback(ParadoxParser parser, string token)
            {
                try
                {
                    if (token.StartsWith("k_"))
                    {
                        var k = parser.Parse(new D());
                        k.Name = token;
                        k.parent = this;
                        this.Children.Add(k);
                    }
                }
                catch(Exception ex)
                {
                    //throw ex;
                    return;
                }
            }

            public class D : LandedTitle, IParadoxRead
            {
                public void TokenCallback(ParadoxParser parser, string token)
                {
                    try
                    {
                        if (token.StartsWith("d_"))
                        {
                            var d = parser.Parse(new C());
                            d.Name = token;
                            d.parent = this;
                            this.Children.Add(d);
                        }
                    }
                    catch(Exception ex)
                    {
                        //throw ex;
                        return;
                    }
                }

                public class C : LandedTitle, IParadoxRead
                {
                    public void TokenCallback(ParadoxParser parser, string token)
                    {
                        try
                        {
                            if (token.StartsWith("c_"))
                            {
                                var c = parser.Parse(new B());
                                c.Name = token;
                                c.parent = this;
                                this.Children.Add(c);
                            }
                        }
                        catch(Exception ex)
                        {
                            //throw ex;
                            return;
                        }
                    }

                    public class B : LandedTitle, IParadoxRead
                    {
                        public void TokenCallback(ParadoxParser parser, string token)
                        {
                            try
                            {
                                if (token.StartsWith("b_"))
                                {
                                    LandedTitle l = new LandedTitle();
                                    l.Name = token;
                                    l.parent = this;
                                    this.Children.Add(l);
                                }
                            }
                            catch(Exception ex)
                            {
                                //throw ex;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
