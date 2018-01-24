using AzureStorageTools.Interfaces;
using Microsoft.Azure.KeyVault;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageTools
{
    public class AzureStorageAccount
    {
        private CloudStorageAccount _Acct;
        private CloudBlobClient _Client;
        public AzureStorageAccount()
        {
            string AZConnectionString = ConfigurationManager
                            .ConnectionStrings["AzureStorageAccount"]
                            .ConnectionString;
            _Acct = CloudStorageAccount.Parse(AZConnectionString);

            _Client = _Acct.CreateCloudBlobClient();

        }
        public BlobContainerHandler GetContainer(string containerName)
        {
            return new BlobContainerHandler(GetBlobContainer(containerName));
        }
        public IEnumerable<CloudBlobContainer> ListContainers()
        {
            return _Client.ListContainers();
        }
        public async Task<ObservableCollection<string>> ListContainersAsync()
        {
            BlobContinuationToken token = null;
            ObservableCollection<string> _List = new ObservableCollection<string>();

            do
            {
                ContainerResultSegment resultset = await _Client.ListContainersSegmentedAsync(token);
                foreach (var item in resultset.Results)
                {
                    _List.Add(item.Name);
                }
            } while (token != null);

            return _List;
        }

        private CloudBlobContainer GetBlobContainer(string containerName)
        {
            var container = _Client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            return container;
        }
    }
}
