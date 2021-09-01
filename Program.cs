using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DeleteDuplicates
{
    class Program
    {

     

        public static string pathLength;
        ConsoleKeyInfo cki;
        double totalSize = 0;
        

        static void Main(string[] args)
        {

            

            string path;
            
            
            if (args.Length > 0)
            {
                path = args[0] as string;
            }
            else
            {
                
                MainStart();
            }

            


           

        }
        private static void MainStart()
        {
            
            string path;
            
                Console.WriteLine("Please insert filepath:");
                path = Console.ReadLine();
                pathLength = path;
                startProgram();
            
        }


        private static void startProgram()
        {
            string path = pathLength;
            ConsoleKeyInfo cki;
            double totalSize = 0;
            if (Directory.Exists(path))
            {
                var fileLists = Directory.GetFiles(path);





                int totalFiles = fileLists.Length;

                List<FileDetails> finalDetails = new List<FileDetails>();
                List<string> ToDelete = new List<string>();
                finalDetails.Clear();
                //loop through all the files by file hash code
                foreach (var item in fileLists)
                {
                    using (var fs = new FileStream(item, FileMode.Open, FileAccess.Read))
                    {
                        finalDetails.Add(new FileDetails()
                        {
                            FileName = item,
                            FileHash = BitConverter.ToString(SHA1.Create().ComputeHash(fs)),
                        });
                    }
                }
                //group by file hash code
                var similarList = finalDetails.GroupBy(f => f.FileHash)
                    .Select(g => new { FileHash = g.Key, Files = g.Select(z => z.FileName).ToList() });


                //keeping first item of each group as is and identify rest as duplicate files to delete
                ToDelete.AddRange(similarList.SelectMany(f => f.Files.Skip(1)).ToList());
                Console.WriteLine("Total duplicate files - {0}", ToDelete.Count);
                //list all files to be deleted and count total disk space to be empty after delete
                if (ToDelete.Count > 0)
                {
                    Console.WriteLine("Files to be deleted - ");
                    foreach (var item in ToDelete)
                    {
                        Console.WriteLine(item);
                        FileInfo fi = new FileInfo(item);
                        totalSize += fi.Length;
                    }
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Total space free up by -  {0}mb", Math.Round((totalSize / 1000000), 6).ToString());
                Console.ForegroundColor = ConsoleColor.White;
                //delete duplicate files
                if (ToDelete.Count > 0)
                {
                    Console.WriteLine("Press C to continue with delete");
                    Console.WriteLine("Press the Escape (Esc) key to quit: \n");
                    do
                    {
                        cki = Console.ReadKey();
                        Console.WriteLine(" --- You pressed {0}\n", cki.Key.ToString());
                        if (cki.Key == ConsoleKey.C)
                        {
                            Console.WriteLine("Deleting files...");
                            ToDelete.ForEach(File.Delete);
                            Console.WriteLine("Files are deleted successfully");
                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine("");
                            Console.WriteLine("Press y for check another folder. Any other key for Exit");
                            cki = Console.ReadKey();
                            Console.WriteLine("");
                            if (cki.Key == ConsoleKey.Y)
                            {
                                MainStart();
                            }
                            else
                            {

                                Console.WriteLine("Console will close in 5 seconds");
                                Thread.Sleep(5000);
                                Environment.Exit(0);
                            }
                        }
                        Console.WriteLine("Press the Escape (Esc) key to quit: \n");
                    } while (cki.Key != ConsoleKey.Escape);
                }
                else
                {
                    Console.WriteLine("No files to delete");
                    //Console.ReadLine();
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("");
                    Console.WriteLine("Press y to check another folder. Any other key for exit");                   
                    cki = Console.ReadKey();
                    Console.WriteLine("");
                    if (cki.Key == ConsoleKey.Y)
                    {
                        MainStart();
                    }
                    else
                    {
                       
                        Console.WriteLine("Console will close in 5 seconds");
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }
                         
                }
            }
            else
            {
                Console.WriteLine("Directory Does not Excsists");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("");
                Console.WriteLine("Press y for check another folder. Any other key for exit");
                cki = Console.ReadKey();
                Console.WriteLine("");
                if (cki.Key == ConsoleKey.Y)
                {
                    MainStart();
                }
                else
                {

                    Console.WriteLine("Console will close in 5 seconds");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                    
                }
                
            }
        }
        
        
    }
}
