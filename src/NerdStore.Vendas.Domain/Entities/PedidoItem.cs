using System;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Domain.Helpers;

namespace NerdStore.Vendas.Domain.Entities
{
    public class PedidoItem : BaseEntity
    {
        public string Nome { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid id,Guid produtoId,string nome, decimal valorUnitario, int quantidade)
        {
            Id = id;
            Nome = nome;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            
            ChecandoQuantidade();
        }

        public PedidoItem AtualizarQuantidade(int quantidade)
        {
            Quantidade += quantidade;
            
            return this;
        }

        public void ChecandoQuantidade()
        {
            var isInRange = Quantidade >= PedidoItemHelper.MIN_UNIDADE_PRODUTO 
                            && 
                            Quantidade <= PedidoItemHelper.MAX_UNIDADE_PRODUTO;
        
            if (!isInRange)
                throw new DomainException($"A quantidade do item {Nome} deve ser entre " +
                                          $"{PedidoItemHelper.MIN_UNIDADE_PRODUTO} a {PedidoItemHelper.MAX_UNIDADE_PRODUTO}");
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        // public override bool EhValido()
        // {
        //     var validator = new PedidoItemValidator();
        //
        //     var validation = validator.Validate(this);
        //
        //     if (validation.IsValid)
        //         return true;
        //
        //     ValidationResult = validation;
        //
        //     return false;
        // }
    }
}