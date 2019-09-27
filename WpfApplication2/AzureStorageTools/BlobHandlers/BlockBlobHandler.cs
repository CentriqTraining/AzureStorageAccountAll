using AzureStorageTools.BaseClasses;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BlobHandlers
{
    public class BlockBlobHandler : BlobHandler<CloudBlockBlob>, IBlobHandler
    {
        public BlockBlobHandler(CloudBlobContainer container) : base(container) { }

        public override ICloudBlob GetBlobReference(string blobName)
        {
            return _Container.GetBlockBlobReference(blobName);
        }
    }
}
