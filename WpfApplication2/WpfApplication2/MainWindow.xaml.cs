using AzureBlobStorageSample;
using AzureStorageTools;
using AzureStorageTools.BlobHandlers;
using AzureStorageTools.Interfaces;
using Microsoft.Win32;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication2.SetupFiles;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AzureStorageAccount _Acct = null;
        private string _DownloadLocation;
        public MainWindow()
        {
            InitializeComponent();
            //  Performs the work of letting you log in to your azure account
            //  Creates a storage account and downloads the appropriate key to use it
            AzureAccountSetupManager.GetAzureAccount();

            //  Class to abstract the inner workings of dealing with Azure storage accounts
            _Acct = new AzureStorageAccount();

            //  copies some initial images which will then be displayed
            //  in this form
            _DownloadLocation = AzureBlobStorageUsageExampleInitializer.InitDirectory();

            //AzureBlobStorageUsageExampleInitializer.Init(_Acct);

            //  This downloads a list of all blobs
            var blobs = _Acct
                .GetContainer("azurestoragetest")
                .ListBlobs()
                .Select(e => new BlobFile()
                {
                    ContainerName = e.Segments[1].Replace("/", ""),
                    FileName = e.Segments[2].Replace("%20", " ")
                });
            lstBlobs.ItemsSource = blobs;
        }

        private void CmdUploadNormal_Click(object sender, RoutedEventArgs e)
        {
            string blobName = UploadBlob<BlockBlobHandler, CloudBlockBlob>("Normal");
            string DownloadedFilePath = DownloadBlob<BlockBlobHandler, CloudBlockBlob>(blobName);
            Process.Start(DownloadedFilePath);
        }
        private void CmdUploadEncrypted_Click(object sender, RoutedEventArgs e)
        {
            string blobName = UploadBlob<EncryptedBlockBlobHandler, CloudBlockBlob>("Encrypted");
            string DownloadedFilePath = DownloadBlob<BlockBlobHandler, CloudBlockBlob>("Encrypted");
            Process.Start(DownloadedFilePath);
        }
        private void ReDownloadAndDecrypt_Click(object sender, RoutedEventArgs e)
        {
            string DownloadedFilePath = DownloadBlob<EncryptedBlockBlobHandler, CloudBlockBlob>("Encrypted");
            Process.Start(DownloadedFilePath);
        }
        private void CmdUploadPageNormal_Click(object sender, RoutedEventArgs e)
        {

        }
        private void GetSASTokenForBlob_Click(object sender, RoutedEventArgs e)
        {
            var sas = _Acct
                .GetContainer("azurestoragetest")
                .GetDefaultBlobHandler()
                .GetSasToken("Normal");
            System.Diagnostics.Process.Start(sas);

        }

        private string UploadBlob<THandlerType, TBlobType>(string blobName) where THandlerType : BlobHandler<TBlobType>
        {
            string fileName = GetImageFileNameFromUser();
            var container = _Acct.GetContainer("azurestoragetest");
            container.BlobHandler<THandlerType, TBlobType>()
                    .UploadBlob(blobName, File.ReadAllBytes(fileName));
            return blobName;
        }
        private string DownloadBlob<THandlerType, TBlobType>(string blobName) where THandlerType : BlobHandler<TBlobType>
        {
            var container = _Acct.GetContainer("azurestoragetest");
            var DownloadedFilePath = System.IO.Path.Combine(_DownloadLocation, blobName + ".jpg");
            if (File.Exists(DownloadedFilePath))
            {
                File.Delete(DownloadedFilePath);
            }

            container.BlobHandler<THandlerType, TBlobType>()
                    .DownloadBlob(blobName, DownloadedFilePath);
            return DownloadedFilePath;
        }
        private static string GetImageFileNameFromUser()
        {
            string fileName = null;
            var chooser = new OpenFileDialog();
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            chooser.InitialDirectory = dir.Parent.Parent.FullName + @"\WorkingFiles";
            chooser.Title = "Choose an image file to upload";
            chooser.Filter = "Image Files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            chooser.Multiselect = false;
            chooser.ShowDialog();
            if (chooser.FileName != null)
            {
                fileName = chooser.FileName;
            }

            return fileName;
        }

    }
}
