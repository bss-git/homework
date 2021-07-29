using Homework.Events.RabbitMQ;
using Homework.Updates.Dto;
using Homework.Updates.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public class UpdatesMessageBus
    {
        private List<IUpdatesSubscriber> _subs = new List<IUpdatesSubscriber>();

        private Channel<UpdateMessage> _events = Channel.CreateUnbounded<UpdateMessage>();

        private const string _exchangeName = "update_messages";
        private readonly IModel _rabbitCahnnel;
        private readonly string _queueName;
        private EventingBasicConsumer _consumer;

        public UpdatesMessageBus(RabbitChannelFactory rabbit)
        {
            _rabbitCahnnel = rabbit.GetChannel();
            _rabbitCahnnel.ExchangeDeclare(exchange: _exchangeName,
                                    type: "direct");
            
            _queueName = _rabbitCahnnel.QueueDeclare().QueueName;

            Task.Run(HanleEventsTask);

            SubscribeRabbit();
        }

        public void Subscribe(IUpdatesSubscriber subscriber)
        {
            lock (_subs)
            {
                //возможна утечка памяти, так как всегда держит ссылки на подписчиков
                //в реальности нужно использовать слабые ссылки
                _subs.Add(subscriber);
            }
        }

        public void Publish(UpdateMessage updateEvent)
        {
            var body = JsonConvert.SerializeObject(updateEvent);
            _rabbitCahnnel.BasicPublish(_exchangeName, updateEvent.Recepient.ToString(), null, Encoding.UTF8.GetBytes(body).AsMemory());
        }

        public void SubscribeRecipient(string routingKey)
        {
            _rabbitCahnnel.QueueBind(queue: _queueName,
                  exchange: _exchangeName,
                  routingKey: routingKey);
        }

        public void UnSubscribeRecipient(string routingKey)
        {
            _rabbitCahnnel.QueueUnbind(queue: _queueName,
                  exchange: _exchangeName,
                  routingKey: routingKey);
        }

        private void SubscribeRabbit()
        {
            _consumer = new EventingBasicConsumer(_rabbitCahnnel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var updateMessage = JsonConvert.DeserializeObject<UpdateMessage>(message);

                _events.Writer.WriteAsync(updateMessage);
            };

            _rabbitCahnnel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: _consumer);
        }

        private async Task HanleEventsTask()
        {
            await foreach (var updateEvent in _events.Reader.ReadAllAsync())
            {
                foreach(var sub in _subs)
                {
                    sub.HandleEvent(updateEvent);
                }
            }
        }
    }
}
