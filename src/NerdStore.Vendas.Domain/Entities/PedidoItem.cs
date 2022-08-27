using System;

namespace NerdStore.Vendas.Domain.Entities
{
    public class PedidoItem : BaseEntity
    {
        public string Nome { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId,string nome, decimal valorUnitario, int quantidade)
        {
            Nome = nome;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public PedidoItem AtualizarQuantidade(int quantidade)
        {
            Quantidade += quantidade;
            
            ChecandoQuantidade();
            
            return this;
        }

        private void ChecandoQuantidade()
        {
            int quantityMaxProduct = 15;
            int quantityMinimumProduct = 1;
            var isInRange = Quantidade >= quantityMinimumProduct && Quantidade <= quantityMaxProduct;

            if (isInRange)
                throw new Exception($"A quantidade do item {Nome} deve ser entre 1 a 15");
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}