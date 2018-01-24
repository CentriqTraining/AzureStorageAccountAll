using AzureStorageTools.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BlobHandlers
{
    public class BlockBlobHandler : BlobHandler
    {
        public BlockBlobHandler(CloudBlobContainer container) : base(container) { }

        public override void UploadBlob(string blobName, Stream stream)
        {
            GetBlobReference(blobName).UploadFromStream(stream);
        }
        public override void UploadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).UploadFromFile(fileName);
        }
        public override void UploadBlob(string blobName, byte[] buffer)
        {
            GetBlobReference(blobName).UploadFromByteArray(buffer, 0, buffer.Length);
        }
        public override void DeleteBlob(string blobName)
        {
            GetBlobReference(blobName).DeleteIfExists();
        }
        public override void DownloadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).DownloadToFile(fileName, FileMode.CreateNew);
        }
        private CloudBlockBlob GetBlobReference(string blobName)
        {
            return _Container.GetBlockBlobReference(blobName);
        }
    }

}
