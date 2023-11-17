using Ludo.Business.UseCases.PostTestUseCase;

namespace Ludo.Tests.UseCasesTests.PostTestTests
{
    [TestClass]
    public class PostTestValidatorTests
    {
        private readonly PostTestRequest _postTestRequest = new();
        private readonly PostTestValidator _validator = new();

        [TestMethod]
        public void PostValidator_HappyFlow_RequestIsValid()
        {
            bool expected = true;
            _postTestRequest.Person = "Mircea";

            var result =  _validator.Validate(_postTestRequest);

            Assert.AreEqual(expected, result.IsValid);
        }

        [TestMethod]
        public void PostValidator_PersonIsEmpty_RequestIsNotValid()
        {
            var expected = false;
            _postTestRequest.Person = "";

            var actual = _validator.Validate(_postTestRequest).IsValid;

            Assert.AreEqual(expected, actual);
        }
    }
}