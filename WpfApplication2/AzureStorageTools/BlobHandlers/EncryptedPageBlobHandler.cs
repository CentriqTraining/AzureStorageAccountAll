using AzureStorageTools.BaseClasses;
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
    public class EncryptedPageBlobHandler : EncryptedBlobHandler<CloudPageBlob>, IBlobHandler
    {
        public EncryptedPageBlobHandler(CloudBlobContainer container) : base(container)
        {

        }

        public override ICloudBlob GetBlobReference(string blobName)
        {
            return _Container.GetPageBlobReference(blobName);
        }
    }
}
