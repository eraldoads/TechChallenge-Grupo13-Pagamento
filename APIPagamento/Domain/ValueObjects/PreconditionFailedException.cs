namespace Domain.ValueObjects
{
    /// <summary>
    /// Inicializa uma nova instância da classe PreconditionFailedException com uma mensagem de erro e o nome do parâmetro que causa a exceção.
    /// </summary>
    /// <param name="message">A mensagem de erro que explica o motivo da exceção.</param>
    /// <param name="paramName">O nome do parâmetro que causa a exceção.</param>
    public class PreconditionFailedException(string message, string paramName) : ArgumentException(message, paramName)
    {
    }
}
