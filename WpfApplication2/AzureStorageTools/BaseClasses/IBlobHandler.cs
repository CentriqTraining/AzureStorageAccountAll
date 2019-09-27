using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools.BaseClasses
{
    public interface IBlobHandler
    {
        ICloudBlob GetBlobReference(string blobName);
    }
}
