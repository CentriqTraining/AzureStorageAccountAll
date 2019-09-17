using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System;

namespace AzureStorageTools.Interfaces
{
    public abstract class BlobHandler<T>
    {
        protected CloudBlobContainer _Container;
        public BlobHandler(CloudBlobContainer container)
        {
            _Container = container;
        }

        public abstract Task DeleteBlobAsync(string blobName);
        public abstract Task UploadBlobAsync(string blobName, string fileName);
        public abstract Task UploadBlobAsync(string blobName, Stream stream);
        public abstract Task UploadBlobAsync(string blobName, byte[] buffer);
        public abstract Task DownloadBlobAsync(string blobName, string fileName);

        public abstract void DeleteBlob(string blobName);
        public abstract void UploadBlob(string blobName, string fileName);
        public abstract void UploadBlob(string blobName, Stream stream);
        public abstract void UploadBlob(string blobName, byte[] buffer);
        public abstract void DownloadBlob(string blobName, string fileName);
        public abstract Stream DownloadBlobAsStream(string blobName);

        public string GetBlobUrl(string blobName)
        {
            return _Container.GetBlobReference(blobName).Uri.AbsoluteUri;
        }
        public string GetSasToken(string blobName)
        {
            CloudBlockBlob blobRef = _Container.GetBlockBlobReference(blobName);

            //  Create an adhoc policy that gives access to this item for 24hours
            SharedAccessBlobPolicy Policy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read
            };

            //  Generate a signature that represents that policy
            var SasToken = blobRef.GetSharedAccessSignature(Policy);

            //  Roll that into a url pointing to the blob
            return blobRef.Uri + SasToken;
        }

        protected abstract T GetBlobReference(string blobName);
    }
}