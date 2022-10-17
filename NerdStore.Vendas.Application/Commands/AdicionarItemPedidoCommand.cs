using System;
using FluentValidation;
using NerdStore.Vendas.Core.Data;
using NerdStore.Vendas.Domain.Helpers;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command<bool>
    {
        public string Name { get;  set; }
        public decimal Valor { get;  set; }
        public Guid ClienteId { get;  set; }
        public Guid ProdutoId { get;  set; }
        public int Quantidade { get;  set; }

        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string name, decimal valor,  int quantidade)
        {
            Name = name;
            Valor = valor;
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool EhValido()
        {
            var validator = new AdicionarItemPedidoValidator();
            
            ValidationResult = validator.Validate(this);

            return ValidationResult.IsValid;
        }
    }

    public class AdicionarItemPedidoValidator : AbstractValidator<AdicionarItemPedidoCommand>
    {
        public static string IdClienteErrorMsg => "Id do cliente inválido";
        public static string IdProdutoErrorMsg => "Id do produto inválido";
        public static string NomeErrorMsg => "o nome do produto não foi informado";
        public static string QuantidadeMinErrorMsg => $"A quantidade minima de um item é {PedidoItemHelper.MIN_UNIDADE_PRODUTO}";
        public static string QuantidadeMaxErrorMsg => $"A quantidade máxima de um item é {PedidoItemHelper.MAX_UNIDADE_PRODUTO}";
        public static string ValorErrorMsg => "o valor do item precisa ser maior do que 0";
        
        public AdicionarItemPedidoValidator()
        {
            RuleFor(p => p.ClienteId)
                .NotEmpty()
                .WithMessage(IdClienteErrorMsg)
                .NotNull()
                .WithMessage(IdClienteErrorMsg);

            RuleFor(p => p.ProdutoId)
                .NotEmpty()
                .WithMessage(IdProdutoErrorMsg)
                .NotNull()
                .WithMessage(IdProdutoErrorMsg);

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage(NomeErrorMsg)
                .NotNull()
                .WithMessage(NomeErrorMsg);
            
            RuleFor(p => p.Quantidade)
                .GreaterThanOrEqualTo(PedidoItemHelper.MIN_UNIDADE_PRODUTO)
                .WithMessage(QuantidadeMinErrorMsg)
                .LessThanOrEqualTo(PedidoItemHelper.MAX_UNIDADE_PRODUTO)
                .WithMessage(QuantidadeMaxErrorMsg);

            RuleFor(p => p.Valor)
                .GreaterThan(0)
                .WithMessage(ValorErrorMsg);
        }
    }
}