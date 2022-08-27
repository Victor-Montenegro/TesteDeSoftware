using System;

namespace NerdStore.Vendas.Domain
{
    public class Roteiro
    {

        // Desenvolvimento  do dominio de vendas
        
        // Pedido -  Item Pedido - Voucher 
        
        // Um item de um pedido representa um produto e pode conter mais de uma unidade independente
        // da acao, um item precisa ser sempre valido:
        //     Possuir: Id e Nome do produto, quantidade entre 1 e 15 unidades, valor mairo que
        /*
        Um pedido enquanto nao iniciado (processo de pagamento) esta no estado de rascunho e deve pertencer
        a um cliente.
        
        1 - Adicionar Item
            1.1 - Ao adicionar um item e necessario calcular o valor total do pedido
            1.2 - Se um item ja esta na lista entao deve acrescecr a quandidade do item no pedido
            1.3 - O item deve ser entre 1 e 15 unidades do produto
            
        2 - Atualizacao de Item
            2.1 - O item precisa estar na lista para ser atualizado
            2.2 - Um item pode ser atualizado contendo mais ou menos unidades do que anteriomente
            2.3 - Ao atualizar um item e necessario calcular o valor total do pedido
            2.4 - Um item deve permanecer entre 1 e 15 unidades do produto
        
        3 - Remocao de Item
            3.1 - O item precisa estar na lista para ser removido
            3.2 - Ao remover um item e necessario calcular o valor total do pedido
            
            
            Um voucher possui um codigo unico e o desconto pode ser em percentual ou valor fixo
            Usar uma flag indicando que um pedido teve um voucher de desconto aplicado e o valor
            do desconto
            
            
            4 - Aplicar voucher de desconto
                4.1 - O voucjer so pode ser aplicado se estiver valido, para isto:
                    4.1.1 - Deve possuir um codigo
                    4.1.2 - A data de validade e superior a data atual
                    4.1.3 - O voucher esta ativo
                    4.1.4 - O voucher possui quantidade disponivel
                    4.1.5 - Uma das formas de desconto devem estar preenchidas com valor acima de 0
                4.2 - Calcular o desconto conforme tipo do voucher
                    4.2.1 - Voucher com desconto percentual
                    4.2.2 - voucher com desconto em valores (reais)
                4.3 - 0 QUando o valor do desconto ultrapassar o total do pedido o pedido recebe o valor : 0
                4.4 - Apos a aplicacao do voucher o desconto deve ser re-calculado apos toda modificacao da lista de itens
                do pedido.
            
            Pedido Commands - Handler
            
                
                o command handler de pedido ira manipular um command para cada intencao em relacao ao pedido
                em todos os commandos manipulados devem ser verificados:
                    
                    Se o command e valido
                    Se o pedido existe
                    Se o item do pedido existe
                    
                    
                    Na alteracao de estado do pedido:
                        
                        Deve ser feita via repositorio
                        Deve enviar um evento
                    
                    1 - AdicionarItemPedidoCommand
                         1.1 Verificar se e um pedido novo ou em andamento
                         1.2 Verificar se o item ja foi adicionado a lista
                    
        */
    }
}