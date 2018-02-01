using OpenQA.Selenium.Chrome;
using SiteMapper;
using SiteMapper.Output;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void button1_Click(object sender, EventArgs e)
        {
            Paths.siteToMapAddress = textBox1.Text;
            Paths.savingDataPath = textBox2.Text + "/";

            ChromeDriver driver = new ChromeDriver();
            objectiveMethod = new ObjectiveMethod(driver, Paths.siteToMapAddress);
            objectiveMethod.Run();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "//C:/Users/Bartek/Documents/GitHub/SiteMapper/TestingSite/Index.html";
            Paths.siteToMapAddress = textBox1.Text;
            textBox2.Text = Environment.SpecialFolder.Desktop.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in objectiveMethod.listOfNodes)
            {
                richTextBox1.Text += item.Name + "\n";
                foreach (var webElement in item.Links)
                {
                    richTextBox1.Text += item.ToString() + "\n";
                }
            }
            richTextBox1.Text += "\n";
        }
    }
}
