using EasyNetQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jango.MessageCenter.RabbitMq
{
    public abstract class Dequeue : IDequeue
    {
        private readonly IBus _bus;
        private string _queueName;
        public Dequeue(string connString, string queueName)
        {
            _bus = RabbitHutch.CreateBus(connString);
            _queueName = queueName;
        }
        public Task Dispose()
        {
            return Task.Run(() =>
            {
                _bus.Dispose();
            });
        }

        public abstract Task ProcessMessage(string msg);

        async Task IDequeue.Dequeue()
        {
            _bus.Receive<IMessage>(_queueName, (msg) =>
            {
                OnMessage(msg);
            });

        }

        private void OnMessage(IMessage message)
        {
            var msg = JsonConvert.SerializeObject(message);
            ProcessMessage(msg);
        }


    }
}
