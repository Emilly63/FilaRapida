using System;
using System.Collections.Generic;

namespace FilaRapida.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime CriadoEm { get; set; }
        public decimal Total { get; set; }
        public List<PedidoItem> Itens { get; set; }
        public Usuario Usuario { get; set; }

        public Pedido()
        {
            Itens = new List<PedidoItem>();
            CriadoEm = DateTime.Now;
            Total = 0;
        }

        public void CalcularTotal()
        {
            Total = 0;
            foreach (var item in Itens)
            {
                item.CalcularSubtotal();
                Total += item.Subtotal;
            }
        }

        public void AdicionarItem(PedidoItem item)
        {
            if (item.Quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que zero");

            var itemExistente = Itens.Find(i => i.ProdutoId == item.ProdutoId);
            if (itemExistente != null)
            {
                itemExistente.Quantidade += item.Quantidade;
                itemExistente.CalcularSubtotal();
            }
            else
            {
                Itens.Add(item);
            }
            
            CalcularTotal();
        }

        public void RemoverItem(int produtoId)
        {
            var item = Itens.Find(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                Itens.Remove(item);
                CalcularTotal();
            }
        }
    }
}
