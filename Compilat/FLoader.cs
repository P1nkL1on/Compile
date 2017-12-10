using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Compilat
{
    class FLoader
    {
        DirectoryInfo directory;// = new DirectoryInfo(@"codes");
        FileInfo[] files;// = directory.GetFiles("*.txt");
        string[] codeNames;
        int currentCodeName;
        string codeFolder;
        string moduleFolder;

        public FLoader(string codeFolderName, string moduleFolderName)
        {
            codeFolder = codeFolderName;
            moduleFolder = moduleFolderName;

            directory = new DirectoryInfo(@"" + codeFolder);
            files = directory.GetFiles("*.txt");
            codeNames = new string[files.Length];
            currentCodeName = 0;

            for (int i = 0; i < files.Length; i++)
                codeNames[i] = files[i].Name.Remove(files[i].Name.LastIndexOf('.'));

        }

        string SelectIO()
        {
            #region uinput_command;
            string command = "";
            while (command.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("↑ ↓ -- select file from list;\n q  -- exit.\n\n");
                MISC.ConsoleWriteLine(" > Load \'" + codeNames[currentCodeName] + ".txt\'", ConsoleColor.Green);
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(@"" + codeFolder + "/" + codeNames[currentCodeName] + ".txt");
                    Console.WriteLine();
                    foreach (string line in lines)
                        MISC.ConsoleWriteLine(line, ConsoleColor.DarkGreen);
                }
                catch (Exception e)
                {
                    MISC.ConsoleWriteLine("Error in code preview", ConsoleColor.DarkRed);
                }

                var ch = Console.ReadKey(false).Key;
                switch (ch)
                {
                    case ConsoleKey.UpArrow:
                        currentCodeName = (currentCodeName > 0) ? currentCodeName - 1 : codeNames.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        currentCodeName = (currentCodeName < codeNames.Length - 1) ? currentCodeName + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        command = codeNames[currentCodeName];
                        Console.Clear();
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine();
            if (command.ToLower() == "q")
                return "-";
            #endregion;
            return command;
        }

        bool CatFileAndDetectChanges(string codeFolder, string fileName, ref string[] linesWeHave)
        {
            string[] lines = System.IO.File.ReadAllLines(@"" + codeFolder + "/" + fileName + ".txt");
            if (lines.Length != linesWeHave.Length)
            { linesWeHave = lines; return true; }        // giant changes
            for (int i = 0; i < lines.Length; i++)
                if (lines[i] != linesWeHave[i])
                { linesWeHave = lines; return true; }    // some changes
            linesWeHave = lines;
            return false;               // all the same
        }

        ASTTree ReadFileToTree(string codeFolder, string command)
        {
            //while (true)
            //{
            //    Console.WriteLine("a");
            //    Thread.Sleep(1000);
            //}

            string code = "";
            string[] lines = System.IO.File.ReadAllLines(@"" + codeFolder + "/" + command + ".txt");
            //linesOut = lines;
            //
            //string filename = "";
            //Thread thread = new Thread(() => ReadFile(codeFolder, command));
            //thread.Start();
            //
            foreach (string line in lines)
            {
                // if an included module
                if (line.IndexOf("#include") >= 0)
                {
                    try
                    {
                        string moduleName = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);
                        MISC.ConsoleWriteLine("Included: " + moduleName, ConsoleColor.Green);
                        try
                        {
                            string[] modulelines = System.IO.File.ReadAllLines(@"" + moduleFolder + "/" + moduleName);
                            foreach (string moduleline in modulelines)
                            {
                                code += moduleline + "\n";
                                MISC.ConsoleWriteLine(moduleline, ConsoleColor.Yellow);
                            }
                        }
                        catch
                        {
                            MISC.ConsoleWriteLine("Invalid include adress: " + moduleFolder + "/" + moduleName, ConsoleColor.Red);
                        }
                    }
                    catch
                    {
                        MISC.ConsoleWriteLine("Invalid include: " + line, ConsoleColor.Red);
                    }
                }
                else
                {
                    code += line + "\n";
                    Console.WriteLine(line);
                }
            }

            ASTTree t = new ASTTree(code);
            return t;
        }

        public void ShowIO()
        {
            while (true)
            {
                string command = SelectIO();
                string[] fileContent = new string[0];
                try
                {
                    Thread thread = new Thread(() =>
                    {

                        bool changed = CatFileAndDetectChanges(codeFolder, command, ref fileContent);
                        ReadFileToTree(codeFolder, command).Trace();
                        Console.Write("\nWaiting changes...");

                        while (true)
                        {
                            
                            Thread.Sleep(5000);
                            changed = CatFileAndDetectChanges(codeFolder, command, ref fileContent);
                            if (changed)
                            {
                                Console.WriteLine("Changes in file detected. Reloading...");
                                Console.Clear();
                                ReadFileToTree(codeFolder, command).Trace(); Console.Write("\nWaiting changes...");
                            }
                        }
                    });
                    thread.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);    //String.Format("Can not read file @/{0}.txt!", command)
                }
                finally
                {
                    Console.WriteLine("Press <ENTER> to exit...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

        }
    }
}

