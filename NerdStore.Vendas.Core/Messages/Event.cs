using System;
using MediatR;

namespace NerdStore.Vendas.Core.Messages
{
    public class Event : Message,INotification
    {
        public DateTime TimeSpan { get; set; }

        protected  Event()
        {
            TimeSpan = DateTime.Now;
        }
    }
}