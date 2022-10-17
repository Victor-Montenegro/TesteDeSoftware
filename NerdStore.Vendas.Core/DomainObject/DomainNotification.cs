using System;
using MediatR;
using NerdStore.Vendas.Core.Messages;

namespace NerdStore.Vendas.Core.Data
{
    public class DomainNotification : Message,INotification
    {
        public string Key { get; private set; }
        public int Version { get; private set; }
        public string Value { get; private set; }
        public DateTime TimesTamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        

        public DomainNotification(DateTime timesTamp, Guid domainNotificationId, string key, string value)
        {
            Key = key;
            Version = 1;
            Value = value;
            TimesTamp = timesTamp;
            DomainNotificationId = domainNotificationId;
        }
    }
}