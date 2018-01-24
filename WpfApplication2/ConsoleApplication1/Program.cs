using AzureStorageTools;
using AzureStorageTools.BlobHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureStorageAccount acct = new AzureStorageAccount();
            var contlist = GetList(acct);
            
            acct.GetContainer("testing")
               .BlobHandler<EncryptedBlockBlobHandler>()
               .UploadBlob("Encrypted", "Files/textfile1.txt");
            acct.GetContainer("testing")
               .BlobHandler<BlockBlobHandler>()
               .UploadBlob("Non-Encrypted", "Files/textfile1.txt");


            acct.GetContainer("testing")
                .BlobHandler<EncryptedBlockBlobHandler>()
                .DownloadBlob("Encrypted", "Files/dl-textfile.txt");

            IEnumerable<Uri> list = acct
                .GetContainer("testing")
                .ListBlobs();

            var url = acct
                .GetContainer("testing")
                .BlobHandler<EncryptedBlockBlobHandler>()
                .GetBlobUrl("encrypted");

            var sas = acct
                .GetContainer("testing")
                .GetDefaultBlobHandler()
                .GetSasToken("Non-Encrypted");
            System.Diagnostics.Process.Start(sas);
        }

        public static ObservableCollection<string> GetList(AzureStorageAccount acct)
        {
            var t = acct.ListContainersAsync();
            t.Wait();
            return t.Result;
        }
    }
}
