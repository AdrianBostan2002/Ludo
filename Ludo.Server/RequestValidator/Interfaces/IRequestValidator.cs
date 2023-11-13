namespace Ludo.RequestValidator.Interfaces
{
    public interface IRequestValidator
    {
        void ValidateRequest<TRequest>(TRequest request);
    }
}