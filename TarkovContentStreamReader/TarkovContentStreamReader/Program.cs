using ComponentAce.Compression.Libs.zlib;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace TarkovContentStreamReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lets read tarkov's stream and send fake packets");
            Console.WriteLine("1 - Deflate input using zlib");
            Console.WriteLine("2 - Deflate input using eft's dll");
            Boolean quit = false;
            while (!quit)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Insert content string:");
                        string inputstream = Console.ReadLine();
                        try
                        {
                            Console.WriteLine(ReadStream(inputstream));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                        
                        break;

                    case "2":

                        Console.WriteLine("Insert content string:");
                        string inputstream2 = Console.ReadLine();
                        try
                        {
                            Console.WriteLine(deflateTo(inputstream2));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }

                        break;

                    case "q":
                    case "Q":
                        quit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
                Console.WriteLine("1 - Deflate input using zlib");
                Console.WriteLine("2 - Deflate input using eft's dll");
            }
        }

        [DllImport(@"D:\Games\EFT\EscapeFromTarkov_Data\Managed\zlib.net")]
        private static extern IntPtr deflateTo(string s);

        public static string ReadStream(string s)
        {
            return SimpleZlib.Decompress(s);
        }
    }
}
