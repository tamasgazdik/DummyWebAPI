using DummyWebAPI.Controllers;
using DummyWebAPI.Core.Models;
using DummyWebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DummyWebAPI.UnitTests.Controllers
{
    [TestFixture]
    public sealed class MotorcyclesControllerTests
    {
        private static MotorcyclesController motorcyclesController;

        private const int ID = 1;

        private const string CBR_600_RR = "CBR600RR";

        private static Mock<IMotorcycleService> motorcycleServiceMock;

        [SetUp]
        public void SetUp()
        {
            motorcycleServiceMock = new Mock<IMotorcycleService>();
            motorcyclesController = new MotorcyclesController(motorcycleServiceMock.Object);
        }

        [Test]
        public void GetMotorcycleAsync_IDExists_ReturnsOkResultWithMotorcycle()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };

            motorcycleServiceMock.Setup(m => m.GetMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycleDTO);
            
            // act
            var result = motorcyclesController.GetMotorcycleAsync(ID, CancellationToken.None).Result;
            
            // assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(motorcycleDTO));
        }

        [Test]
        public void GetMotorcycleAsync_IDDoesNotExist_ReturnsNotFoundResult()
        {
            // arrange
            motorcycleServiceMock.Setup(m => m.GetMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotorcycleDTO?)null);

            // act
            var result = motorcyclesController.GetMotorcycleAsync(ID, CancellationToken.None).Result;

            // assert
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void GetMotorcyclesAsync_2MotorcyclesAreStored_ReturnsMotorcycles()
        {
            var motorcycleDTOs = new List<MotorcycleDTO>
            {
                new MotorcycleDTO
                {
                    Brand = MotorcycleBrand.Honda,
                    Model = CBR_600_RR
                },
                new MotorcycleDTO
                {
                    Brand = MotorcycleBrand.Yamaha,
                    Model = "YZF-R6"
                }
            };

            motorcycleServiceMock.Setup(m => m.GetAllMotorcyclesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycleDTOs);

            // act
            var result = motorcyclesController.GetMotorcyclesAsync(CancellationToken.None).Result;

            // assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That((IEnumerable<MotorcycleDTO>)((OkObjectResult)result.Result).Value, Is.EquivalentTo(motorcycleDTOs));
        }

        [Test]
        public void GetMotorcyclesAsync_ZeroMotorcyclesAreStored_ReturnsEmptyList()
        {
            // arrange
            motorcycleServiceMock.Setup(m => m.GetAllMotorcyclesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            // act
            var result = motorcyclesController.GetMotorcyclesAsync(CancellationToken.None).Result;

            // assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            Assert.That((IEnumerable<MotorcycleDTO>)((OkObjectResult)result.Result).Value, Is.Empty);
        }

        [Test]
        public void CreateMotorcycleAsync_ValidMotorcycle_ReturnsCreatedAtRouteResult()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };
            
            motorcycleServiceMock.Setup(m => m.CreateMotorcycleAsync(motorcycleDTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ID);
            
            // act
            var result = motorcyclesController.CreateMotorcycleAsync(motorcycleDTO, CancellationToken.None).Result;
            
            // assert
            Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
            Assert.That(((CreatedAtRouteResult)result.Result).Value, Is.EqualTo(ID));
        }

        [Test]
        public void CreateMotorcycleAsync_InvalidMotorcycle_ReturnsBadRequestResult()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };

            motorcycleServiceMock.Setup(m => m.CreateMotorcycleAsync(motorcycleDTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(-1);
            
            // act
            var result = motorcyclesController.CreateMotorcycleAsync(motorcycleDTO, CancellationToken.None).Result;
            
            // assert
            Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleExistsAndIsValid_ReturnsOkResult()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };
            
            motorcycleServiceMock.Setup(m => m.GetMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycleDTO);

            motorcycleServiceMock.Setup(m => m.UpdateMotorcycleAsync(ID, motorcycleDTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            // act
            var result = motorcyclesController.UpdateMotorcycleAsync(ID, motorcycleDTO, CancellationToken.None).Result;
            
            // assert
            Assert.That(result, Is.TypeOf<OkResult>());
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleDoesNotExist_ReturnsNotFoundResult()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };
            motorcycleServiceMock.Setup(m => m.GetMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync((MotorcycleDTO?)null);

            // act
            var result = motorcyclesController.UpdateMotorcycleAsync(ID, motorcycleDTO, CancellationToken.None).Result;

            // assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void UpdateMotorcycleAsync_MotorcycleExistsButIsInvalid_ReturnsBadRequestResult()
        {
            // arrange
            var motorcycleDTO = new MotorcycleDTO
            {
                Brand = MotorcycleBrand.Honda,
                Model = CBR_600_RR
            };

            motorcycleServiceMock.Setup(m => m.GetMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycleDTO);

            motorcycleServiceMock.Setup(m => m.UpdateMotorcycleAsync(ID, motorcycleDTO, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // act
            var result = motorcyclesController.UpdateMotorcycleAsync(ID, motorcycleDTO, CancellationToken.None).Result;
            
            // assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void DeleteMotorcycleAsync_MotorcycleExists_ReturnsOkResult()
        {
            // arrange
            motorcycleServiceMock.Setup(m => m.DeleteMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ID);

            // act
            var result = motorcyclesController.DeleteMotorcycleAsync(ID, CancellationToken.None).Result;

            // assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That(((OkObjectResult)result).Value, Is.EqualTo(ID));
        }

        [Test]
        public void DeleteMotorcycleAsync_MotorcycleDoesNotExist_ReturnsNotFoundResult()
        {
            // arrange
            motorcycleServiceMock.Setup(m => m.DeleteMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(-1);
            
            // act
            var result = motorcyclesController.DeleteMotorcycleAsync(ID, CancellationToken.None).Result;
            
            // assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public void DeleteMotorcycleAsync_ErrorHappenedDuringDeletion_ReturnsBadRequestResult()
        {
            // arrange
            motorcycleServiceMock.Setup(m => m.DeleteMotorcycleAsync(ID, It.IsAny<CancellationToken>()))
                .ReturnsAsync(-1);

            // act
            var result = motorcyclesController.DeleteMotorcycleAsync(ID, CancellationToken.None).Result;
            
            // assert
            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }
    }
}
