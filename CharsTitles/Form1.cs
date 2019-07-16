using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pdoxcl2Sharp;

namespace CharsTitles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string modPath;

        private void Button1_Click(object sender, EventArgs e)
        {
            string path = this.textBox1.Text.Trim();
            if(path != string.Empty)
            {
                if(MessageBox.Show("There are characters in the text box, use them as path?", "Prompt", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        path = folderBrowserDialog1.SelectedPath;
                    }
                }
            }
            else
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath;
                }
            }
            //开始读取
            //角色 "\\history\\characters\\characters.txt"
            lstChar.DisplayMember = "Text";
            lstChar.ValueMember = "ID";
            using (FileStream fs = new FileStream(path + "\\history\\characters\\characters.txt", FileMode.Open))
            {
                var cs = ParadoxParser.Parse(fs, new Characters());
                foreach(var c in Characters.Items)
                {
                    lstChar.Items.Add(c);
                }
            }

            //省份 "\\common\\landed_titles"
            foreach (string p in System.IO.Directory.GetFiles(path + "\\common\\landed_titles"))
            {
                if (p.ToLower().EndsWith(".txt"))
                {
                    using (FileStream fs = new FileStream(p, FileMode.Open))
                    {
                        ParadoxParser.Parse(fs, new LandedTitles());
                    }
                }
            }
            var root = treeView1.Nodes.Add("World");
            foreach(var i in LandedTitles.Itmes)
            {
                var ee = root.Nodes.Add(i.Name);
                foreach(var ii in i.Children)
                {
                    var k = ee.Nodes.Add(ii.Name);
                    foreach(var iii in ii.Children)
                    {
                        var d = k.Nodes.Add(iii.Name);
                        foreach(var iiii in iii.Children)
                        {
                            d.Nodes.Add(iiii.Name);
                        }
                    }
                }
            }
            modPath = path;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // "\\history\\titles"
            DateTime t = DateTime.MinValue;
            string ts = this.txtTime.Text.Trim();
            if(!ParadoxParser.TryParseDate(ts, out t))
            {
                MessageBox.Show("Time format error!");
                return;
            }
            string id = this.lstChar.SelectedValue.ToString();
            List<string> ps = new List<string>();
            GetCheckedTitles(treeView1.Nodes[0], ps);
            foreach(string p in ps)
            {
                string path = modPath + "\\history\\titles\\" + p + ".txt";
                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(p, FileMode.Open))
                    {
                        Title title = new Title();
                        ParadoxParser.Parse(fs, new Title());
                        if (title.Time == ts && title.HoldID == id)
                            continue;
                    }
                }
                else
                {
                    Title title = new Title();
                    title.Time = ts;
                    title.HoldID = id;
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    using (ParadoxSaver saver = new ParadoxSaver(fs))
                    {
                        title.Write(saver);
                    }
                }
            }
        }

        private void GetCheckedTitles(TreeNode node, List<string> titles)
        {
            if (node.Checked && node.Text != "World")
                titles.Add(node.Text);
            foreach(TreeNode n in node.Nodes)
            {
                GetCheckedTitles(n, titles);
            }
        }

        private void TreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Parent == null)
            {
                CheckNodes(e.Node);
            }
            else if(e.Button == MouseButtons.Right)
            {
                CheckNodes(e.Node);
            }
        }

        private void CheckNodes(TreeNode parentNod)
        {
            foreach(TreeNode n in parentNod.Nodes)
            {
                n.Checked = parentNod.Checked;
                CheckNodes(n);
            }
        }
    }
}
