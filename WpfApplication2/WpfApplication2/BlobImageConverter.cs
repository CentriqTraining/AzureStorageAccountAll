using AzureStorageTools;
using AzureStorageTools.BlobHandlers;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApplication2
{
    public class BlobImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AzureStorageAccount acct = new AzureStorageAccount();

            var handler = acct.GetContainer("azurestoragetest")
               .BlobHandler<BlockBlobHandler, CloudBlockBlob>();

            Stream blobFile = handler.DownloadBlobAsStream((string)value);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = blobFile;
            image.EndInit();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
