namespace Application.Interfaces
{
    public interface IPagamentoMessageService : IDisposable
    {
        Task ReceberMensagens();
    }
}
