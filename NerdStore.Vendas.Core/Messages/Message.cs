using System;

namespace NerdStore.Vendas.Core.Messages
{
    public class Message
    {
        public Guid AggregateId { get; set; }
        public string Messagetype { get; set; }

        protected Message()
        {
            Messagetype = GetType().Name;
        }
    }
}