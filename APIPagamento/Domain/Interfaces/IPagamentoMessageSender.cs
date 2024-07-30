namespace Domain.Interfaces
{
    public interface IPagamentoMessageSender
    {
        void SendMessage(string queueName, string message);
    }
}
