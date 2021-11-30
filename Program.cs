using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinServInstaller
{
    class Program
    {
        public const string fwFolder = @"C:\Windows\Microsoft.NET\Framework";
        public const string iuName = "InstallUtil.exe";
        public const string cmdName = "cmd.exe";

        private static string GetIUPath(string fwFolder)
        {
            var folders = Directory.GetDirectories(fwFolder, "v*").OrderByDescending(f => f).ToList();

            foreach (var dir in folders)
            {
                if (File.Exists($"{dir}\\{iuName}"))
                {
                    return $"{dir}\\{iuName}";
                }
            }

            return "";
        }

        static void Main(string[] args)
        {
            if (Directory.Exists(fwFolder))
            {
                string iuPath = GetIUPath(fwFolder);

                if (!string.IsNullOrWhiteSpace(iuPath))
                {
                    if (args.Length > 0)
                    {
                        string servicePath = args[0];

                        string iuMode = "-i";

                        #region ASK
                        bool ask = true;
                        ConsoleKeyInfo input;

                        do
                        {
                            Console.Clear();
                            Console.Write($"'{Path.GetFileNameWithoutExtension(servicePath)}' Install (1) / Uninstall (0): ");

                            input = Console.ReadKey();
                            switch (input.Key)
                            {
                                case ConsoleKey.D0:
                                case ConsoleKey.NumPad0:
                                    iuMode = "-u";
                                    ask = false;
                                    break;
                                case ConsoleKey.D1:
                                case ConsoleKey.NumPad1:
                                    iuMode = "-i";
                                    ask = false;
                                    break;
                            }
                        } while (ask);
                        Console.WriteLine();
                        #endregion

                        if (Path.GetExtension(servicePath).ToLower().EndsWith("exe"))
                        {
                            string strCmdText = $"/C {iuPath} {iuMode} {args[0]}";
                            Process.Start(cmdName, strCmdText);

                            Console.Write("Completed!");
                        }
                        else
                        {
                            Console.Write("File must have .exe extension!");
                        }
                    }
                    else
                    {
                        Console.WriteLine(".Net Framework OK!");
                        Console.WriteLine("InstallUtil OK!");
                        Console.WriteLine("Ready to use!");
                        Console.WriteLine("Do not use this application directly.");
                        Console.Write("Drag & Drop windows service exe file to this application icon for installing/uninstalling it.");
                    }
                }
                else
                {
                    Console.Write("InstallUtil tool not found!");
                }
            }
            else
            {
                Console.Write("Framework not found!");
            }

            Console.ReadKey();
        }
    }
}
