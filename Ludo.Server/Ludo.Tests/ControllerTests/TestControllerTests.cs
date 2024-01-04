//using Bogus;
//using Ludo.Business.UseCases.PostTestUseCase;
//using Ludo.Business.UseCases.TestUseCase;
//using Ludo.MediatRPattern.Interfaces;
//using Ludo.Server;
//using Ludo.Server.Controllers;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Moq;

//namespace Ludo.Tests.ControllerTests
//{
//    [TestClass]
//    public sealed class TestControllerTests
//    {
//        private readonly Mock<IMediator> _mediatorMock = new();
//        private readonly Mock<IHubContext<TestHub>> _hubContextMock = new();

//        private readonly PostTestRequest _postTestRequest = new Faker<PostTestRequest>().Generate();

//        private string _getResponse = "Message";
//        private PostTestResponse _postTestResponse = new Faker<PostTestResponse>().Generate();

//        [TestMethod]
//        public void Constructor_HappyFlow_ThrowsNothing()
//        {
//            Assert.IsNotNull(() => new TestController(_hubContextMock.Object, _mediatorMock.Object));
//        }

//        [TestMethod]
//        public void Constructor_HubContextIsEmpty_ThrowsArgumentNullException()
//        {
//            Assert.ThrowsException<ArgumentNullException>(() => new TestController(null, _mediatorMock.Object));
//        }

//        [TestMethod]
//        public void Constructor_MediatoRIsEmpty_ThrowsArgumentNullException()
//        {
//            Assert.ThrowsException<ArgumentNullException>(() => new TestController(_hubContextMock.Object, null));
//        }

//        [TestMethod]
//        public async Task GetTest_Always_CallsMediatRAsync()
//        {
//            TestController sut = CreateSutForGet();

//            sut.ControllerContext = new ControllerContext();
//            sut.ControllerContext.HttpContext = new DefaultHttpContext();

//            await sut.GetMediatRAsync();

//            _mediatorMock.Verify(x => x.Send(It.IsAny<TestRequest>()), Times.Once);
//        }

//        [TestMethod]
//        public async Task GetTest_HappyFlow_ReturnCorrectMessage()
//        {
//            TestController sut = CreateSutForGet();

//            sut.ControllerContext = new ControllerContext();
//            sut.ControllerContext.HttpContext = new DefaultHttpContext();

//            IActionResult actualResult = await sut.GetMediatRAsync();

//            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));

//            var actualContent = (actualResult as OkObjectResult)?.Value;
//            Assert.AreEqual("Message", actualContent);
//        }

//        [TestMethod]
//        public async Task PostTest_Always_CallsMediatR()
//        {
//            TestController sut = CreateSutForPost();

//            sut.ControllerContext = new ControllerContext();
//            sut.ControllerContext.HttpContext = new DefaultHttpContext();

//            await sut.PostMediatRAsync(_postTestRequest);

//            _mediatorMock.Verify(x => x.Send(_postTestRequest));
//        }

//        [TestMethod]
//        public async Task PostTest_HappyFlow_ReturnCorrectMessage()
//        {
//            TestController sut = CreateSutForPost();

//            sut.ControllerContext = new ControllerContext();
//            sut.ControllerContext.HttpContext = new DefaultHttpContext();

//            IActionResult actualResult = await sut.PostMediatRAsync(_postTestRequest);

//            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));

//            var actualContent = (actualResult as OkObjectResult)?.Value;

//            if (actualContent is PostTestResponse postTestResponse)
//            {
//                Assert.AreEqual(_postTestResponse.ResponseMessage, postTestResponse.ResponseMessage);
//            }
//            else
//            {
//                Assert.Fail("Unexpected result type.");
//            }
//        }

//        private TestController CreateSutForGet()
//        {
//            _mediatorMock.Setup(x => x.Send(It.IsAny<TestRequest>()))
//                .ReturnsAsync(_getResponse);

//            return new TestController(_hubContextMock.Object, _mediatorMock.Object);
//        }


//        private TestController CreateSutForPost()
//        {
//            _mediatorMock.Setup(x => x.Send(It.IsAny<PostTestRequest>()))
//                .ReturnsAsync(_postTestResponse);

//            _postTestResponse.ResponseMessage = "Hello Steve!!";

//            return new TestController(_hubContextMock.Object, _mediatorMock.Object);
//        }
//    }
//}