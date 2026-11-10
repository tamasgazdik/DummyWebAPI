using DummyWebAPI.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DummyWebAPI.Service.UnitTests
{
    [TestFixture]
    public class MotorcycleServiceTests
    {
        private static MotorcycleService motorcycleService;

        private const int NUMBER_OF_MOTORCYCLES = 5;

        private static Mock<IMotorcycleDTOValidator> motorcycleDTOValidatorMock;

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
            motorcycleDTOValidatorMock = new Mock<IMotorcycleDTOValidator>();
            motorcycleDTOValidatorMock.Setup(m => m.IsValid(It.IsAny<MotorcycleDTO>())).Returns(true);

            motorcycleService = new MotorcycleService(configurationMock.Object, motorcycleDTOValidatorMock.Object);
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
        public async Task CreateMotorcycleAsync_MotorcycleIsValid_MotorcycleIsAdded()
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

        [Test]
        public void CreateMotorcycleAsync_MotorcycleIsInvalid_MotorcycleIsNotAdded()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.CFMoto,
                Model = string.Empty
            };
            motorcycleDTOValidatorMock.Setup(m => m.IsValid(motorcycleDTO)).Returns(false);

            // act
            var result = motorcycleService.CreateMotorcycleAsync(motorcycleDTO).Result;
            
            // assert
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleExistsAndIsValid_MotorcycleIsUpdated()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.CFMoto,
                Model = "Dummy Model"
            };

            // act
            var result = motorcycleService.UpdateMotorcycleAsync(3, motorcycleDTO).Result;

            // assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleExistsButIsInvalid_MotorcycleIsNotUpdated()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.CFMoto,
                Model = string.Empty
            };

            motorcycleDTOValidatorMock.Setup(m => m.IsValid(It.IsAny<MotorcycleDTO>())).Returns(false);

            // act
            var result = motorcycleService.UpdateMotorcycleAsync(3, motorcycleDTO).Result;
            
            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleDoesNotExist_ReturnsFalse()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.CFMoto,
                Model = "Dummy Model"
            };

            // act
            var result = motorcycleService.UpdateMotorcycleAsync(999, motorcycleDTO).Result;

            // assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteMotorcycleAsync_MotorcycleExists_MotorcycleIsDeleted()
        {
            // arrange

            // act
            var result = motorcycleService.DeleteMotorcycleAsync(3).Result;
            
            // assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void DeleteMotorcycleAsync_MotorcycleDoesNotExist_ReturnsMinusOne()
        {
            // arrange
            
            // act
            var result = motorcycleService.DeleteMotorcycleAsync(999).Result;
            
            // assert
            Assert.That(result, Is.EqualTo(-1));
        }
    }
}
