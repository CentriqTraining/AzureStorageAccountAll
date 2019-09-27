using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System;

namespace AzureStorageTools.BaseClasses
{
    public abstract class BlobHandler<T>  where T : ICloudBlob 
    {
        protected CloudBlobContainer _Container;
        public BlobHandler(CloudBlobContainer container)
        {
            _Container = container;
        }

        public virtual async Task DeleteBlobAsync(string blobName)
        {
            await GetBlobReference(blobName).DeleteIfExistsAsync();
        }
        public virtual async Task UploadBlobAsync(string blobName, Stream stream)
        {
            await GetBlobReference(blobName).UploadFromStreamAsync(stream);
        }
        public virtual async Task UploadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).UploadFromFileAsync(fileName);
        }
        public virtual async Task UploadBlobAsync(string blobName, byte[] buffer)
        {
            await GetBlobReference(blobName).UploadFromByteArrayAsync(buffer, 0, buffer.Length);
        }
        public virtual async Task DownloadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).DownloadToFileAsync(fileName, FileMode.CreateNew);
        }

        public virtual void DeleteBlob(string blobName)
        {
            GetBlobReference(blobName).DeleteIfExists();
        }
        public virtual void UploadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).UploadFromFile(fileName);
        }
        public virtual void UploadBlob(string blobName, Stream stream)
        {
            GetBlobReference(blobName).UploadFromStream(stream);
        }
        public virtual void UploadBlob(string blobName, byte[] buffer)
        {
            GetBlobReference(blobName).UploadFromByteArray(buffer, 0, buffer.Length);
        }
        public virtual void DownloadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).DownloadToFile(fileName, FileMode.CreateNew);
        }
        public virtual Stream DownloadBlobAsStream(string blobName)
        {
            MemoryStream strm = new MemoryStream();
            GetBlobReference(blobName).DownloadToStream(strm);
            return strm;
        }

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
        public abstract ICloudBlob GetBlobReference(string blobName);
    }
}