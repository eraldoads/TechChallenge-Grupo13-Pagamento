namespace Domain.Interfaces
{
    public interface IPagamentoMessageQueue
    {
        Task<string> ReceberMensagem();
    }
}
