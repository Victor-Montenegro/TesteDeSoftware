using System;
using FluentValidation.Results;

namespace NerdStore.Vendas.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public ValidationResult ValidationResult { get; set; }
    }
}