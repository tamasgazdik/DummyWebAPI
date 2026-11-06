using DummyWebAPI.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DummyWebAPI.Service.UnitTests
{
    public class MotorcycleServiceTests
    {
        private static MotorcycleService motorcycleService;

        private const int NUMBER_OF_MOTORCYCLES = 5;

        private static Mock<IConfiguration> configurationMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            configurationMock = new Mock<IConfiguration>();

            var configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.Setup(cSM => cSM.Value).Returns(NUMBER_OF_MOTORCYCLES.ToString());

            configurationMock.Setup(cM => cM.GetSection(It.IsAny<string>())).Returns(configurationSectionMock.Object);
        }

        [SetUp]
        public void Setup()
        {
            motorcycleService = new MotorcycleService(configurationMock.Object);
        }

        [Test]
        public async Task GetAllMotorcyclesAsync_MethodIsCalled_CorrectNumberOfItemsAreReturned()
        {
            // arrange

            // act
            var result = await motorcycleService.GetAllMotorcyclesAsync();

            // assert
            Assert.That(result, Has.Count.EqualTo(NUMBER_OF_MOTORCYCLES));
        }

        [Test]
        public async Task GetMotorcycleAsync_IDExists_ReturnsSomeObject()
        {
            // arrange

            // act
            var result = await motorcycleService.GetMotorcycleAsync(3);

            // assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetMotorcycleAsync_IDDoesNotExist_ReturnsNull()
        {
            // arrange

            // act
            var result = await motorcycleService.GetMotorcycleAsync(12);

            // assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateMotorcycleAsync_MotorcycleIsProvided_MotorcycleIsAdded()
        {
            const MotorcycleBrand cfMoto = MotorcycleBrand.CFMoto;
            const string model = "Dummy Model";
            const int manufactoringYear = 2000;
            const Color color = Color.Black;
            const Category category = Category.Touring;
            const EngineType engineType = EngineType.InlineFour;
            const Transmission transmission = Transmission.Automatic;
            const int displacementInCC = 650;

            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = cfMoto,
                Model = model,
                ManufactoringYear = manufactoringYear,
                Color = color,
                Category = category,
                EngineType = engineType,
                Transmission = transmission,
                DisplacementInCC = displacementInCC
            };

            // act
            var result = await motorcycleService.CreateMotorcycleAsync(motorcycleDTO);

            // assert
            Assert.That(result, Is.EqualTo(6));
        }
    }
}
