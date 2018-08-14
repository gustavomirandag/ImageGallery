using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService
{


    public class TableService
    {
        private class Image : TableEntity
        {
            public string Url { get; set; }

            public Image(Guid id, string url)
            {
                this.PartitionKey = "EDS";
                this.RowKey = id.ToString();
                Url = url;
            }

            public Image()
            {
            }
        }

        public void AddImage(string url)
        {
            //Obtem referência a StorageAccount do Azure
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(StorageService.Properties.Settings.Default.StorageConnectionString);

            //Cria um CloudTableClient a partir do StorageAccount
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //Obtem referência a tabela de imagens
            CloudTable table = tableClient.GetTableReference("Images");

            //Create the table if it doesn't exist.
            table.CreateIfNotExists();

            //--- Create the Batch Operation ---
            TableBatchOperation batchOperation = new TableBatchOperation();

            //Cria um Image(TableEntity) e adiciona a tabela
            Image image = new Image(Guid.NewGuid(), url);

            //Adiciona a imagem no Batch
            batchOperation.Insert(image);
            //----------------------------------

            //Executa o BatchOperation
            table.ExecuteBatch(batchOperation);
        }

        public IEnumerable<string> GetAllImages()
        {
            //Obtem referência a StorageAccount do Azure
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(StorageService.Properties.Settings.Default.StorageConnectionString);

            //Cria um CloudTableClient a partir do StorageAccount
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Images");

            //Obtem referência a tabela de imagens
            TableQuery<Image> query = new TableQuery<Image>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "EDS"));

            IEnumerable<Image> images = table.ExecuteQuery(query);

            IEnumerable<string> urls = null;
            try
            {
                //Isso é a mesma coisa que o SELECT (select é uma função do LINQ)
                //List<string> urls = new List<string>();
                //foreach(var current in images)
                //    urls.Add(current.Url);

                if (images.Count() > 0)
                    urls = images.Select(i => i.Url);
            }
            catch
            {
                urls = new List<string>();
            }

            return urls;
        }

        public void DeleteImage(string url)
        {
            //Obtem referência a StorageAccount do Azure
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(StorageService.Properties.Settings.Default.StorageConnectionString);

            //Cria um CloudTableClient a partir do StorageAccount
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("Images");

            //Obtem referência a tabela de imagens
            TableQuery<Image> query = new TableQuery<Image>()
                .Where(TableQuery.GenerateFilterCondition("Url", QueryComparisons.Equal, url));

            var imagesToDelete = table.ExecuteQuery(query);

            TableBatchOperation operationToDelete = new TableBatchOperation();
            foreach (var currentImage in imagesToDelete)
            {
                operationToDelete.Delete(currentImage);
            }
            table.ExecuteBatch(operationToDelete);
        }
    }
}
