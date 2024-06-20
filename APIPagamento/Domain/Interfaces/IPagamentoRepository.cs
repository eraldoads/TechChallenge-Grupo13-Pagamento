using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<PagamentoInput> PostPagamento(PagamentoInput pagamento);
        Task<Pagamento> PutPagamento(Pagamento pagamento);
        Task<Pagamento?> GetPagamentoByIdPedido(int idPedido);

        Task<Pagamento?> GetPagamentoById(string idPagamento);
    }
}
