using AzureStorageTools;
using AzureStorageTools.BlobHandlers;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.SetupFiles
{
    public static class AzureBlobStorageUsageExampleInitializer
    {
        public static void Init(AzureStorageAccount _Acct)
        {
            var files = Directory.GetFiles("../../SetupFiles", "*.jpg");
            var handler = _Acct.GetContainer("azurestoragetest")
               .BlobHandler<BlockBlobHandler, CloudBlockBlob>();

            foreach (var item in files)
            {
                handler.UploadBlob(System.IO.Path.GetFileName(item), item);
            }
        }
        public static string InitDirectory()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            FileInfo RunningAppLocation = new FileInfo(asm.Location);
            string _DownloadLocation = System.IO.Path.Combine(RunningAppLocation.DirectoryName, "DownloadedFiles");
            Directory.CreateDirectory(_DownloadLocation);
            return _DownloadLocation;
        }
    }
}
