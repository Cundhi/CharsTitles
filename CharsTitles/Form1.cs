﻿using System;
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
                        this.textBox1.Text = path;
                    }
                }
            }
            else
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath;
                    this.textBox1.Text = path;
                }
            }
            //开始读取
            //角色 "\\history\\characters\\characters.txt"
            lstChar.DisplayMember = "Text";
            lstChar.ValueMember = "ID";
            using (FileStream fs = new FileStream(path + "\\history\\characters\\characters.txt", FileMode.Open))
            {
                var cs = ParadoxParser.Parse(fs, new Characters());
                //foreach(var c in cs.Items)
                //{
                //    lstChar.Items.Add(c);
                //}
                lstChar.DataSource = cs.Items;
            }
            if (lstChar.Items.Count > 0)
                lstChar.SelectedIndex = 0;

            //省份 "\\common\\landed_titles"
            LandedTitles landedTitles = new LandedTitles();
            foreach (string p in System.IO.Directory.GetFiles(path + "\\common\\landed_titles"))
            {
                if (p.ToLower().EndsWith(".txt"))
                {
                    using (FileStream fs = new FileStream(p, FileMode.Open))
                    {
                        ParadoxParser.Parse(fs, landedTitles);
                    }
                }
            }
            var root = treeView1.Nodes.Add("World");
            foreach(var i in landedTitles.Itmes)
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
            if(lstChar.SelectedIndex < 0)
            {
                MessageBox.Show("No character selected!");
                return;
            }
            // "\\history\\titles"
            string logs = string.Empty;
            string errlogs = string.Empty;
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
                    Title title1 = new Title();
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        ParadoxParser.Parse(fs, new Title());
                        if (title1.Time == ts && title1.HoldID == id)
                        {
                            logs += string.Format("{0} {3} \"{1}={{holder={2}}}\" existed\r\n", DateTime.Now, ts, id, p + ".txt"); //已存在
                            continue;
                        }
                        if(title1.Time == ts)
                        {
                            errlogs += string.Format("{0} {2} \"{1}\" existed\r\n", DateTime.Now, ts, p + ".txt"); //已存在
                            continue;
                        }
                    }
                }
                Title title = new Title();
                title.Time = ts;
                title.HoldID = id;
                using (FileStream fs = new FileStream(path, FileMode.Append))
                using (ParadoxSaver saver = new ParadoxSaver(fs))
                {
                    title.Write(saver);
                }
                logs += string.Format("{0} {3} \"{1}={{holder={2}}}\" writed\r\n", DateTime.Now, ts, id, p + ".txt"); //成功写文件
            }
            if(logs != string.Empty)
            {
                using (FileStream fs = new FileStream(modPath + "\\history\\titles\\logs.log", FileMode.Append))
                {
                    byte[] buffer = Encoding.Default.GetBytes(logs);
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            if (errlogs != string.Empty)
            {
                using (FileStream fs = new FileStream(modPath + "\\history\\titles\\errlogs.log", FileMode.Append))
                {
                    byte[] buffer = Encoding.Default.GetBytes(errlogs);
                    fs.Write(buffer, 0, buffer.Length);
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
