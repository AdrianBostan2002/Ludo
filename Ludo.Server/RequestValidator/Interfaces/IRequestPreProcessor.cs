namespace Ludo.RequestValidator.Interfaces
{
    public interface IRequestPreProcessor<in TRequest> where TRequest : notnull
    {
        Task Process(TRequest request, CancellationToken cancellationToken);
    }
}