using System;
using System.IO;

namespace VPNPatch
{
    class Program
    {
        static public long CountLinesReader(FileInfo file)
        {
            var lineCounter = 0L;
            using (var reader = new StreamReader(file.FullName))
            {
                while (reader.ReadLine() != null)
                {
                    lineCounter++;
                }
                return lineCounter;
            }
        }
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = Environment.GetEnvironmentVariable("appdata");
            DirectoryInfo fl = new DirectoryInfo(Environment.CurrentDirectory + @"\Microsoft\Network\Connections\Cm");
            Console.WriteLine("Current working directory is " + fl.FullName);
            try
            {
                DirectoryInfo[]  subdirectories = fl.GetDirectories();
                foreach (DirectoryInfo dir in subdirectories)
                {
                    Console.WriteLine("Subfolder Name : " + dir.FullName);
                    try
                    {
                        FileInfo[] files = dir.GetFiles("routes.txt", SearchOption.TopDirectoryOnly);
                        foreach (FileInfo f1 in files)
                        {
                            Console.WriteLine("Found target file : {0}", f1.FullName);
                            // Console.WriteLine("Current file has {0} lines", CountLinesReader(f1));
                            if( CountLinesReader(f1) <= 3 ) {
                                Console.WriteLine("This file has less then 3 lines. This file will be modified");
                                // Backup file and save to routes.txt.bak
                                f1.CopyTo(f1.Directory + @"\routes.txt.bak", true);
                                Console.WriteLine("Creating backup file");
                                using (StreamWriter w = f1.AppendText())
                                {
                                    w.WriteLine("ADD 172.16.51.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 172.16.52.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 172.16.72.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 172.16.82.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 211.171.185.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 1.209.185.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 172.16.84.0 MASK 255.255.255.0 default METRIC default IF default");
                                    w.WriteLine("ADD 172.16.94.0 MASK 255.255.255.0 default METRIC default IF default");
                                }
                                Console.WriteLine("The modified file has {0} lines", CountLinesReader(f1));
                            } else
                            {
                                Console.WriteLine("This file has over 3 lines or routes.");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The process failed: {0}", e.ToString());
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                Console.WriteLine("Please check if Azure VPN Software is installed on the system");
            }
        }
    }
}
