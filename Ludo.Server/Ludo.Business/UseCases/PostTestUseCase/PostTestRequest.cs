using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.PostTestUseCase
{
    public class PostTestRequest : IRequest<PostTestResponse>
    {
        public string Person { get; set; }
    }
}