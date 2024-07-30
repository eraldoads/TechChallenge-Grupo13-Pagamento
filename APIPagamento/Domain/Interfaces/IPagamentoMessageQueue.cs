namespace Domain.Interfaces
{
    public interface IPagamentoMessageQueue
    {
        event Func<string, Task> MessageReceived;
        Task StartListening();
        void ReenqueueMessage(string message);
    }
}
