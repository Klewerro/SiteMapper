using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper.Output
{
    public static class ConsoleOutput
    {
        public static int counterOfSites = 0;

        public static void Print(SiteNode node)
        {
            counterOfSites++;
            Console.WriteLine($"!Current site tittle: {node.Name} |{counterOfSites}");
            foreach (var element in node.Links)
            {
                Console.WriteLine(element.Text);
            }
            Console.WriteLine();

        }

        public static void PrintAlsoToTxtFile(SiteNode node, string pathToFile)
        {
            string pathWithFilename = pathToFile + "consoleLog.txt";

            if (counterOfSites == 1)
            {
                File.WriteAllText(pathWithFilename, null);
            }


            using (StreamWriter writer = File.AppendText(pathWithFilename))
            {
                writer.AutoFlush = true;
                writer.WriteLine($"!Current site tittle: {node.Name} |{counterOfSites}");
                foreach (var element in node.Links)
                {
                    writer.WriteLine(element.Text);
                }
                writer.WriteLine();
                writer.Flush();
            }
        }

        public static void ConsoleOutputToTxt(string path)
        {
            Console.WriteLine($@"Entire output will be saved in location: {path}, and file: consoleLog.txt");

            var streamwriter = new StreamWriter(path + "consoleLog.txt");
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
        }

        public static string PrintSingleNodeToForm(SiteNode node)
        {
            string value = "";
            counterOfSites++;
            value += ($"!Current site tittle: {node.Name} |{counterOfSites}");
            foreach (var element in node.Links)
            {
                value += (element.Text + "\n");
            }
            value += "\n";
            return value;
        }

        //public static string PrintToForm(List<SiteNode> nodes)
        //{
        //    string value = "";
        //    foreach (var node in nodes)
        //    {
        //        value += ($"!Current site tittle: {node.Name} |{counterOfSites} \n");
        //        foreach (var element in node.Links)
        //        {
        //            value += (element.ToString() + "\n");
        //        }
        //        value += "\n";
        //    }

            
            
        //    return value;
        //}
    }
}
