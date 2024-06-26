namespace Application.Interfaces
{
    public interface IPagamentoScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
