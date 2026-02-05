using System;
using FilaRapida.Models;
using FilaRapida.Repositories;

namespace FilaRapida.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository pedidoRepo;
        
        public PedidoService()
        {
            pedidoRepo = new PedidoRepository();
        }

        public ResultadoPedido ProcessarPedido(Pedido pedido)
        {
            var resultado = new ResultadoPedido();
            
            try
            {
                // Validações de negócio
                if (pedido.Itens.Count == 0)
                {
                    resultado.Sucesso = false;
                    resultado.Mensagem = "Pedido deve conter pelo menos um item";
                    return resultado;
                }
                
                if (pedido.Total <= 0)
                {
                    resultado.Sucesso = false;
                    resultado.Mensagem = "Total do pedido inválido";
                    return resultado;
                }
                
                if (pedido.UsuarioId <= 0)
                {
                    resultado.Sucesso = false;
                    resultado.Mensagem = "Usuário não identificado";
                    return resultado;
                }

                // Salva no banco
                int pedidoId = pedidoRepo.Save(pedido);
                
                resultado.Sucesso = true;
                resultado.PedidoId = pedidoId;
                resultado.Mensagem = $"Pedido #{pedidoId} registrado com sucesso! Total: {pedido.Total:C2}";
                resultado.NumeroPedido = pedidoId;
                resultado.DataHora = DateTime.Now;
                resultado.ValorTotal = pedido.Total;
            }
            catch (Exception ex)
            {
                resultado.Sucesso = false;
                resultado.Mensagem = $"Erro ao processar pedido: {ex.Message}";
            }
            
            return resultado;
        }

        /// <summary>
        /// Classe para resultado do processamento do pedido
        /// </summary>
        public class ResultadoPedido
        {
            public bool Sucesso { get; set; }
            public string Mensagem { get; set; }
            public int PedidoId { get; set; }
            public int NumeroPedido { get; set; }
            public DateTime DataHora { get; set; }
            public decimal ValorTotal { get; set; }
        }
    }
}