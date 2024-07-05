using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPagamentoScopedService
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
