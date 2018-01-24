using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper.IO
{
    class ScreenshotSaving
    {
        private string folderPath;
        private string folderName;


        public ScreenshotSaving(string folderPath, string folderName)
        {
            this.folderPath = folderPath;
            this.folderName = folderName;
            CreateFolderForScreenshots();
        }



        public void SaveScreenshotAsJpg(SiteNode node)
        {
            using (var ms = new MemoryStream(node.Screenshot))
            {
                var img = Image.FromStream(ms);
                img.Save(folderPath + node.Name + ".jpg");
            }
        }

        public void SaveScreenshotAsPng(SiteNode node)
        {
            using (var ms = new MemoryStream(node.Screenshot))
            {
                var img = Image.FromStream(ms);
                img.Save(folderPath + node.Name + ".png");
            }
        }

        private void CreateFolderForScreenshots()
        {
            try
            {
                string fullPath = folderPath + folderName;
                if (Directory.Exists(fullPath) && FolderContainsFiles(fullPath))
                {
                    int number = 2;
                    string pathTmp = fullPath;
                    do
                    {
                        fullPath = pathTmp;
                        fullPath += "_" + number;
                        number++;
                    } while (Directory.Exists(fullPath));

                }
                Directory.CreateDirectory(fullPath);
                
                folderPath = fullPath + @"\";
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR!");
                Console.WriteLine("WRONG PATH TO SCREENSHOT FOLDER!");
                Console.WriteLine("Please close application and change path");
                Environment.Exit(-1);
            }

        }

        private bool FolderContainsFiles(string folderPath)
        {
            int length = Directory.GetFiles(folderPath).Length;
            if (length > 0)
                return true;
            return false; ;
        }
    }

}
