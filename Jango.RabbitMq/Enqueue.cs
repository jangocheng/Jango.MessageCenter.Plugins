using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyNetQ;

namespace Jango.MessageCenter.RabbitMq
{
    public class Enqueue : IEnqueue
    {
        private readonly IBus _bus;
        private string _queueName;
        public Enqueue(string connString, string queueName)
        {
            _queueName = queueName;
            _bus = RabbitHutch.CreateBus(connString);
        }
        async Task IEnqueue.Enqueue(IMessage msg)
        {
            await _bus.SendAsync(_queueName, msg);
        }

        async Task IEnqueue.Enqueue(IEnumerable<IMessage> msgs)
        {
            await _bus.SendAsync(_queueName, msgs);
        }

        async Task<long> IEnqueue.ScheduledEnqueue(IMessage msg, System.DateTimeOffset timeOffset)
        {
            throw new NotImplementedException();
        }
    }
}
