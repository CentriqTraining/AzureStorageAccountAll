using AzureStorageTools.BaseClasses;
using AzureStorageTools.BlobHandlers;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools
{
    public class BlobContainerHandler
    {
        CloudBlobContainer _Container;
        public BlobContainerHandler(CloudBlobContainer container)
        {
            _Container = container;
        }
        public IEnumerable<Uri> ListBlobs()
        {
            return _Container.ListBlobs().Select(b => b.Uri);
        }
        public T BlobHandler<T>() where T : IBlobHandler
        {
            return (T)Activator.CreateInstance(typeof(T), _Container);
        }
        public BlockBlobHandler GetDefaultBlobHandler()
        {
            return new BlockBlobHandler(_Container );
        }
    }

}
