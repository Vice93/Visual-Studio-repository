using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace Oblig2.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private static string filePath;
        private static StorageFile file;



        private async void FilePicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                fileSelected.Text = "Name: " + file.Name;
                filePath = file.Path;
            }
            else
            {
                fileSelected.Text = "Operation cancelled";
            }
        }

        private async Task CompressFile()
        {
            Task<String> compressFile = Compress();

            compressedInfo.Text = await compressFile;
        }

        private async Task<String> Compress()
        {
            IAsyncOperation<IRandomAccessStream> OpenAsync(FileAccessMode accessMode, StorageOpenOptions options);
            IRandomAccessStream inFile = await OpenAsync(ReadWrite, );
            //OpenAsync = await file.OpenStreamForReadAsync();
            {
                // Create the compressed file.
                Stream outFile = File.Create(file.Name + ".gz");
                {
                    GZipStream Compress = new GZipStream(outFile, CompressionMode.Compress);
                    {
                        // Copy the source file into 
                        // the compression stream.
                        inFile.CopyTo(Compress);

                    Debug.WriteLine("Finished compression");

                        return "Compressed " + file.Name + " to " + outFile.Length.ToString();
                    }
                }
            }
        }


        private static void Decompress(string filePath)
        {
            
        }

        private void CompressBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CompressFile();
        }

        private void DeCompressBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (file != null)
            {
                Decompress(filePath);
            }
        }

        private void pickFile_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FilePicker();
        }
    }
}