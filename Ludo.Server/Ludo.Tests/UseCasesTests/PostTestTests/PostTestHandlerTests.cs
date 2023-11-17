using Bogus;
using Ludo.Business.UseCases.PostTestUseCase;

namespace Ludo.Tests.UseCasesTests.PostTestTests
{
    [TestClass]
    public class PostTestHandlerTests
    {
        private readonly PostTestRequest _postTestRequest = new Faker<PostTestRequest>();

        private readonly PostTestResponse _postResponse = new Faker<PostTestResponse>();

        [TestMethod]
        public async Task PostTest_HappyFlow_ReturnsCorrectMessageAsync()
        {
            var expectedType = typeof(PostTestResponse);
            var expected = _postResponse;

            var sut = CreateSutForPostTestHandler();

            var actual = await sut.Handle(_postTestRequest);
            
            Assert.IsInstanceOfType(actual, expectedType);
            Assert.AreEqual(expected.ResponseMessage, actual.ResponseMessage);
        }

        private PostTestHandler CreateSutForPostTestHandler()
        {
            _postTestRequest.Person = "Radu";
            _postResponse.ResponseMessage = "Hello Radu!!";

            return new PostTestHandler();
        }
    }
}
