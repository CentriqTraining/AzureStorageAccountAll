﻿using AzureStorageTools.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BlobHandlers
{
    public class BlockBlobHandler : BlobHandler<CloudBlockBlob>
    {
        public BlockBlobHandler(CloudBlobContainer container) : base(container) { }

        public override async Task DeleteBlobAsync(string blobName)
        {
            await GetBlobReference(blobName).DeleteIfExistsAsync();
        }
        public override async Task UploadBlobAsync(string blobName, Stream stream)
        {
            await GetBlobReference(blobName).UploadFromStreamAsync(stream);
        }
        public override async Task UploadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).UploadFromFileAsync(fileName);
        }
        public override async Task UploadBlobAsync(string blobName, byte[] buffer)
        {
            await GetBlobReference(blobName).UploadFromByteArrayAsync(buffer, 0, buffer.Length);
        }
        public override async Task DownloadBlobAsync(string blobName, string fileName)
        {
            await GetBlobReference(blobName).DownloadToFileAsync(fileName, FileMode.CreateNew);
        }

        public override void DeleteBlob(string blobName)
        {
            GetBlobReference(blobName).DeleteIfExists();
        }
        public override void UploadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).UploadFromFile(fileName);
        }
        public override void UploadBlob(string blobName, Stream stream)
        {
            GetBlobReference(blobName).UploadFromStream(stream);
        }
        public override void UploadBlob(string blobName, byte[] buffer)
        {
            GetBlobReference(blobName).UploadFromByteArray(buffer, 0, buffer.Length);
        }
        public override void DownloadBlob(string blobName, string fileName)
        {
            GetBlobReference(blobName).DownloadToFile(fileName, FileMode.CreateNew);
        }
        public override Stream DownloadBlobAsStream(string blobName)
        {
            MemoryStream strm = new MemoryStream();
            GetBlobReference(blobName).DownloadToStream(strm);
            return strm;
        }

        protected override CloudBlockBlob GetBlobReference(string blobName)
        {
            return _Container.GetBlockBlobReference(blobName);
        }
    }
}
