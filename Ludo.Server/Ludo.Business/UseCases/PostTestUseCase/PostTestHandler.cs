using Ludo.MediatRPattern.Interfaces;

namespace Ludo.Business.UseCases.PostTestUseCase
{
    public class PostTestHandler : IRequestHandler<PostTestRequest, PostTestResponse>
    {
        public async Task<PostTestResponse> Handle(PostTestRequest request)
        {
            var message = $"Hello {request.Person}!!";

            PostTestResponse response = new PostTestResponse()
            {
                ResponseMessage = message
            };

            return await Task.FromResult(response);
        }
    }
}