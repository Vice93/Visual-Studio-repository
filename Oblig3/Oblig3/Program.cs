using System.IO;
using System;
using System.IO.Compression;
using System.Diagnostics;

namespace zip
{
    public class Program
    {
        private static string directoryPath = @"c:\temp";
        private static Stopwatch stopWatch = new Stopwatch();
        public static void Main()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
            Compress(directorySelected);
            foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
            {
                stopWatch.Start();
                Decompress(fileToDecompress);
            }
            Console.WriteLine("Finished compression and decompression.");
            Console.ReadLine();
        }

        public static void Compress(DirectoryInfo directorySelected)
        {
            foreach (FileInfo fileToCompress in directorySelected.GetFiles())
            {
                stopWatch.Start();
                FileStream originalFileStream = fileToCompress.OpenRead();
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz");
                        {
                            GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
                            {
                                originalFileStream.CopyTo(compressionStream);

                            }
                        }
                        FileInfo info = new FileInfo(directoryPath + "\\" + fileToCompress.Name + ".gz");
                        stopWatch.Stop();
                        Console.WriteLine("Compressed {0} from {1} to {2} bytes. Time spent: {3}",
                        fileToCompress.Name, fileToCompress.Length.ToString(), info.Length.ToString(), stopWatch.ElapsedMilliseconds);
                        stopWatch.Reset();
                    }

                }
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {
            FileStream originalFileStream = fileToDecompress.OpenRead();
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                FileStream decompressedFileStream = File.Create(newFileName);
                {
                    GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress);
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        stopWatch.Stop();
                        Console.WriteLine("Decompressed: {0}. Time spent: {1}", fileToDecompress.Name, stopWatch.ElapsedMilliseconds);
                        stopWatch.Reset();
                    }
                }
            }
        }
    }
}
