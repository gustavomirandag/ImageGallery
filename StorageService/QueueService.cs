using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService
{
    public class QueueService
    {
        public void AddMessageToQueue(string messageText, string queueName)
        {
            //Obtem referência a StorageAccount do Azure
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(StorageService.Properties.Settings.Default.StorageConnectionString);

            //Cria um CloudQueueClient a partir do StorageAccount
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            //Obtem referência a queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            //Cria a queue se ela não existe
            queue.CreateIfNotExists();

            //Adiciona a mensagem de texto a queue
            queue.AddMessage(new CloudQueueMessage(messageText));
        }

        public string GetNextMessageFromQueue(string queueName)
        {
            //Obtem referência a StorageAccount do Azure
            CloudStorageAccount storageAccount = CloudStorageAccount
                .Parse(StorageService.Properties.Settings.Default.StorageConnectionString);

            //Cria um CloudQueueClient a partir do StorageAccount
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            //Obtem referência a queue
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            //Obtem a próxima mensagem
            CloudQueueMessage message = queue.GetMessage();

            //Retorna a mensagem como string
            return message.AsString;
        }
    }
}
