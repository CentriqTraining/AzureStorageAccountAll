using Microsoft.Azure.KeyVault;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BaseClasses
{
    public abstract class EncryptedBlobHandler<T> : BlobHandler<T> where T: ICloudBlob
    {
        protected BlobRequestOptions _BlobUploadEncryptionOptions;
        protected BlobRequestOptions _BlobDownloadEncryptionOptions;

        public EncryptedBlobHandler(CloudBlobContainer container, string key=null) : base(container)
        {
            RsaKey _Key = null;
            //  We are encrypting these values with the strongest encryption key
            //  on God's green earth:  Chuck Norris
            //  unless someone gives me a better one
            //  like THAT exists!
            if (key == null)
            {
                _Key = new RsaKey("private:ChuckNorris");
            }
            else
            {
                _Key = new RsaKey(key);
            }

            //  Local Resolver holds and keeps track of our key so we can
            //  use it over and over again.
            LocalResolver.Current.Add(_Key);

            //  A Policy is created to tell it what key to use to upload
            BlobEncryptionPolicy policy = new BlobEncryptionPolicy(_Key, null);

            //  A policy is created to tell it what key to use to download
            BlobEncryptionPolicy downloadPolicy = new BlobEncryptionPolicy(null, LocalResolver.Current);

            //  The actual parameter when we upload/download is a BlobRequestOptions object
            //  So we load those in preperation of uploading or downloading
            _BlobUploadEncryptionOptions = new BlobRequestOptions() { EncryptionPolicy = policy };
            _BlobDownloadEncryptionOptions = new BlobRequestOptions() { EncryptionPolicy = downloadPolicy };
        }

        public override async Task DeleteBlobAsync(string blobName)
        {
            await GetBlobReference(blobName).DeleteIfExistsAsync();
        }
        public override async Task UploadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).UploadFromFileAsync(fileName, null, _BlobUploadEncryptionOptions, null);
        }
        public override async Task UploadBlobAsync(string blobName, Stream stream)
        {
            await GetBlobReference(blobName).UploadFromStreamAsync(stream, null, _BlobUploadEncryptionOptions, null);
        }
        public override async Task UploadBlobAsync(string blobName, byte[] buffer)
        {
            await GetBlobReference(blobName).UploadFromByteArrayAsync(buffer, 0, buffer.Length, null, _BlobUploadEncryptionOptions, null);
        }
        public override async Task DownloadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).UploadFromFileAsync(fileName, null, _BlobDownloadEncryptionOptions, null);
        }

        public override void DeleteBlob(string blobName)
        {
            GetBlobReference(blobName).DeleteIfExists();
        }
        public override void UploadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).UploadFromFile(fileName, null, _BlobUploadEncryptionOptions, null);
        }
        public override void UploadBlob(string blobName, Stream stream)
        {
            GetBlobReference(blobName).UploadFromStream(stream, null, _BlobUploadEncryptionOptions, null);
        }
        public override void UploadBlob(string blobName, byte[] buffer)
        {
            GetBlobReference(blobName).UploadFromByteArray(buffer, 0, buffer.Length, null, _BlobUploadEncryptionOptions, null);
        }
        public override void DownloadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).DownloadToFile(fileName, FileMode.Create, null, _BlobDownloadEncryptionOptions, null);
        }
        public override Stream DownloadBlobAsStream(string blobName)
        {
            Stream strm = new MemoryStream();
            GetBlobReference(blobName).DownloadToStreamAsync(strm);
            return strm;
        }

    }
}
