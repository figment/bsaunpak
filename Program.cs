using System;
using System.Collections.Generic;
using System.Text;
using Nexus.Client.Games.Gamebryo.Tools.BSA;
using System.IO;
namespace bsaunpak
{
    class Program
    {
        static void Usage()
        {
            Console.WriteLine("Usage:  bsaunpak <source.bsa> [dest]");
            Console.WriteLine("");
            Console.WriteLine("  <source.bsa> - Source BSA file to unpack");
            Console.WriteLine("  [dest]       - Destination folder to write to (Default: Current Directory + BSA Name)");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 2)
            {
                Usage();
                System.Environment.Exit(1);
            }
            string infile = args[0];
            if (!File.Exists(infile))
            {
                Console.WriteLine("Unable to locate file '{0}'",infile);
                System.Environment.Exit(1);
            }
            string outdir = args.Length > 1 ? args[1] : Path.Combine(System.Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(infile)) ;
            BSAArchive archive = new BSAArchive(infile);
            foreach (var name in archive.FileNames)
            {
                byte[] data = archive.GetFile(name);
                string fileName = Path.Combine(outdir, name);
                string pathName = Path.GetDirectoryName(fileName);
                try
                {
                    Console.WriteLine(fileName);
                    Directory.CreateDirectory(pathName);
                    using (var file = File.Create(fileName, 4096))
                    {
                        file.Write(data, 0, data.Length);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Unhandled exception while writing '{0}'", name);
                }
            }            
        }
    }
}
