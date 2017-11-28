using System;
using System.Collections.Generic;
using System.IO;

namespace MP3Renamer
{
    public class Root
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[1];
                args[0] = "blank";
            }
            
            if (args[0] == "h" || args[0] == "help")
            {
                HelpText();
            }

            string TargetFolder = "";

            if (Directory.Exists(args[0]))
            {
                TargetFolder = args[0];
            }
            else
            {
                TargetFolder = Directory.GetCurrentDirectory();
            }

            string[] FilesList = Directory.GetFiles(TargetFolder);

            int NumberOfFiles = FilesList.Length;
            int CurrentFile = 1;
            foreach (string entry in FilesList)
            {
                Console.Write("[" + CurrentFile.ToString() + @"/" + NumberOfFiles.ToString() + "] ");

                if (!entry.EndsWith(".mp3"))
                {
                    Console.WriteLine("Skipping - not mp3 file");
                }
                else
                {
                    TagLib.File CurrentTrack = TagLib.File.Create(entry);

                    string Artist = CurrentTrack.Tag.FirstPerformer;
                    string Title = CurrentTrack.Tag.Title;

                    if (Artist == null || Artist == "" || Title == "" || Title == null)
                    {
                        Console.WriteLine("Skipping - incomplete id3 tags");
                    }
                    else
                    {
                        string NewFilename = Artist + " - " + Title + ".mp3";
                        string CurrentFilename = Path.GetFileName(entry);
                        string NewPath = entry.Replace(CurrentFilename, NewFilename);

                        File.Move(entry, NewPath);

                        Console.WriteLine("File Renamed");
                    }
                }

                CurrentFile++;
            }

            Console.WriteLine("Completed");
            Console.ReadLine();
        }

        public static void HelpText()
        {
            Console.WriteLine("Usage: MP3Renamer <path>");
            Console.WriteLine("Note the path is optional, we'll just look in the current directory otherwise");
            Environment.Exit(0);
        }
    }
}