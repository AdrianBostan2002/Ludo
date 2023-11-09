using Ludo.Domain.Interfaces;

namespace Ludo.Business.UseCases.TestUseCase
{
    public class TestHandler : IRequestHandler<TestRequest, string>
    {
        public async Task<string> Handle(TestRequest request)
        {
            return await Task.FromResult("message");
        }
    }
}