using OpenQA.Selenium.Chrome;
using SiteMapper;
using SiteMapper.Output;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Northwoods.Go;
using Northwoods.Go.Layout;

namespace SiteMapper
{
    public partial class Form1 : Form
    {

        private ObjectiveMethod objectiveMethod;
        //private ConsoleOutput consoleOutput;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Paths.siteToMapAddress;
            textBox2.Text = Paths.savingDataPath;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            


            //objectiveMethod.Run();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.RootFolder = Environment.SpecialFolder.Desktop;
            textBox2.Text = Environment.SpecialFolder.Desktop.ToString();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = folder.SelectedPath;
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(new List<string>() { "headless" });
                ChromeDriver driver = new ChromeDriver(chromeOptions);
                objectiveMethod = new ObjectiveMethod(driver, Paths.siteToMapAddress);
            }

            if (checkBox1.Checked == false)
            {
                ChromeDriver driver = new ChromeDriver();
                objectiveMethod = new ObjectiveMethod(driver, Paths.siteToMapAddress);
            }




            


            var nodes = objectiveMethod.Run(int.Parse(textBox3.Text));

            //foreach (var item in nodes)
            //{
            //    richTextBox1.Text += item.Name + "\n";
            //    foreach (var webElement in item.Links)
            //    {
            //        richTextBox1.Text += item.ToString() + "\n";
            //    }
            //}
            //richTextBox1.Text += "\n";

            //CreateRootNode(nodes, treeView1);


            //TreeNode root = null;
            //PopulateTree(ref root, nodes);


            //GenerateNodeTreeRcursively(nodes);

        }



        private void CreateRootNode(List<SiteNode> nodes, TreeView treeView)
        {
            //int maxIteration = nodes.Max(n => n.ParentNodeId);
            //int nOfLevel = 0;
            //var currentLevelNodes = nodes.Where(n => n.ParentNodeId == nOfLevel).ToList();

            var rootNode = nodes.Where(x => x.ParentNodeId == 0).FirstOrDefault();
            treeView.Nodes.Add(rootNode.Name);
        }

        private void CreateChildNode(List<SiteNode> nodes, TreeView treeView, int nOfLevel)
        {
            int maxIteration = nodes.Max(n => n.ParentNodeId);
            var currentLevelNodes = nodes.Where(n => n.ParentNodeId == nOfLevel).ToList();

            var x = treeView.Nodes.Find(nodes.Where(n => n.ParentNodeId == nOfLevel - 1).FirstOrDefault().Name, true);
            foreach (var node in currentLevelNodes)
            {
                treeView.TopNode.Nodes.Add(node.Name);
            }
        }

        public void PopulateTree(ref TreeNode root, List<SiteNode> siteNodes)
        {
            if (root == null)
            {
                root = new TreeNode();
                root.Text = "Indexxx";
                root.Tag = null;
                // get all nodes in the list with parent is null
                var nodes = siteNodes.Where(t => t.ParentNodeId == null);
                foreach (var node in nodes)
                {
                    var child = new TreeNode()
                    {
                        Text = node.Name,
                        Tag = node.ParentNodeId,
                    };
                    PopulateTree(ref child, siteNodes);
                    root.Nodes.Add(child);
                }
            }
            else
            {
                var id = (int)root.Tag;
                var nodes = siteNodes.Where(t => t.ParentNodeId == id);
                foreach (var node in nodes)
                {
                    var child = new TreeNode()
                    {
                        Text = node.Name,
                        Tag = node.ParentNodeId,
                    };
                    PopulateTree(ref child, siteNodes);
                    root.Nodes.Add(child);
                }
            }
        }


        //private static TreeNode GenerateNodeTreeRcursively(List<SiteNode> allNodes)
        //{
        //    string rootNodeName = allNodes.Where(x => x.ParentNodeId == 0).FirstOrDefault().Name;
        //    var root = new TreeNode(rootNodeName);
        //    AddNodesRecursively(root, allNodes);
        //    return root;
        //}

        //private static TreeNode AddNodesRecursively(TreeNode rootNode, List<SiteNode> allNodes)
        //{
        //    //int ID = rootNode.Nodes. //jakoś dobrać się do id-rodzica
        //    var current = allNodes.Where(n => n.ParentNodeId == ID);
        //    foreach (var singleNode in current)
        //    {
        //        var child = new TreeNode(singleNode.Name);
        //        rootNode.Nodes.Add(child);
        //        AddNodesRecursively(child, allNodes);
        //    }
        //    return rootNode;

        //}





        private void CreateTree2(List<SiteNode> nodes)
        {
            var imageList = CreateImageList(nodes);

            var rootNode = nodes.Where(x => x.ParentNodeId == 0).FirstOrDefault();
            TreeNode obj = new TreeNode();
            obj.Text = rootNode.Name;
            obj.ImageIndex = 0;
            obj.SelectedImageIndex = 0;

            treeView1.Nodes.Add(obj);



            var nodesX = nodes.Where(x => x.ParentNodeId == 1).ToList();
            foreach (var item in nodesX)
            {
                treeView1.Nodes[0].Nodes.Add(item.Name);
            }

            for (int i = 2; i < 5; i++)
            {
                var nodesX_X = nodes.Where(x => x.ParentNodeId == i).ToList();
                foreach (var item in nodesX_X)
                {
                    treeView1.Nodes[0].Nodes[i - 2].Nodes.Add(item.Name);
                }
            }

        }





        //isn't workin'
        private Icon BytesToIcon(SiteNode node)
        {
            using(MemoryStream ms = new MemoryStream(node.Screenshot))
            {
                return new Icon(ms);
            }
        }

        private ImageList CreateImageList(List<SiteNode> nodes)
        {
            ImageList imageList = new ImageList();

            foreach (var singleNode in nodes)
            {
                imageList.Images.Add(BytesToIcon(singleNode));
            }

            return imageList;

        }



    }
}
