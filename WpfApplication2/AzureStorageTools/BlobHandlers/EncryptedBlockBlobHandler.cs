using AzureStorageTools.Interfaces;
using Microsoft.Azure.KeyVault;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BlobHandlers
{
    public class EncryptedBlockBlobHandler : BlobHandler
    {
        private BlobRequestOptions _BlobUploadEncryptionOptions = null;
        private BlobRequestOptions _BlobDownloadEncryptionOptions = null;

        public EncryptedBlockBlobHandler(CloudBlobContainer container) : base(container)
        {
            RsaKey _Key = new RsaKey("private:ChuckNorris");
            LocalResolver.Current.Add(_Key);
            BlobEncryptionPolicy policy = new BlobEncryptionPolicy(_Key, null);
            BlobEncryptionPolicy downloadPolicy = new BlobEncryptionPolicy(null, LocalResolver.Current);
            _BlobUploadEncryptionOptions = new BlobRequestOptions() { EncryptionPolicy = policy };
            _BlobDownloadEncryptionOptions = new BlobRequestOptions() { EncryptionPolicy = downloadPolicy };

        }

        public override void DeleteBlob(string blobName)
        {
            GetBlobReference(blobName).DeleteIfExists();
        }

        public override void UploadBlob(string blobName, Stream stream)
        {
            var blob = GetBlobReference(blobName);
            using (stream)
            {
                GetBlobReference(blobName).UploadFromStream(stream, null, _BlobUploadEncryptionOptions, null);
            }
        }
        public override void UploadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).UploadFromFile(fileName, null, _BlobUploadEncryptionOptions, null);
        }
        public override void UploadBlob(string blobName, byte[] buffer)
        {
                GetBlobReference(blobName)
                .UploadFromByteArray(buffer, 0, buffer.Length, 
                                null, _BlobUploadEncryptionOptions, null);
        }

        public override  void DownloadBlob(string blobName, string fileName)
        {
            CloudBlockBlob blob = _Container.GetBlockBlobReference(blobName);

            GetBlobReference(blobName)
                .DownloadToFile(fileName, FileMode.Create, null, _BlobDownloadEncryptionOptions, null);
        }
        private CloudBlockBlob GetBlobReference(string blobName)
        {
            return _Container.GetBlockBlobReference(blobName);
        }

    }

}
