using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageService;

namespace UnitTestQueueService
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            QueueService myQueue = new QueueService();
            myQueue.AddMessageToQueue("Gustavo", "myqueue");
            string result = myQueue.GetNextMessageFromQueue("myqueue");

            Assert.AreEqual("Gustavo", result);
        }
    }
}
