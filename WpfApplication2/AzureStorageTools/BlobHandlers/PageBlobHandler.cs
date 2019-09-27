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
    public class PageBlobHandler : BlobHandler<CloudPageBlob>, IBlobHandler
    {
        public PageBlobHandler(CloudBlobContainer container) : base(container) { }

        public override ICloudBlob GetBlobReference(string blobName)
        {
            return _Container.GetPageBlobReference(blobName);
        }
    }
}
