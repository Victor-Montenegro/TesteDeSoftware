using System;
using System.Collections.Generic;
using FluentValidation.Results;
using NerdStore.Vendas.Core.Messages;

namespace NerdStore.Vendas.Core.Data
{
    public abstract class BaseEntity
    {
        private List<Event> _notification;
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public IReadOnlyCollection<Event> Notification => _notification;

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event notification)
        {
            if (_notification == null)
                _notification = new List<Event>();
            
            _notification.Add(notification);
        }
        
        public void LimparEventos()
        {
            _notification.Clear();
        }
        // public abstract bool EhValido();
    }
}