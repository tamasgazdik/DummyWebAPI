using DummyWebAPI.Core;
using DummyWebAPI.Core.Models;

namespace DummyWebAPI.Service.UnitTests
{
    [TestFixture]
    public sealed class MotorcycleDTOValidatorTests
    {
        private static MotorcycleDTOValidator motorcycleDTOValidator;

        private static MotorcycleDTO motorcycleDTO;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            motorcycleDTOValidator = new MotorcycleDTOValidator();

            motorcycleDTO = new MotorcycleDTO { Model = string.Empty };
        }

        [SetUp]
        public void Setup()
        {
            // setup valid DTO
            motorcycleDTO.ManufactoringYear = Constants.ManifacturingYear.MIN + 10;
            motorcycleDTO.Model = "Test Model";
            motorcycleDTO.DisplacementInCC = CommonDisplacement.Values[0];
        }

        [Test]
        public void IsValid_DTOIsValid_ReturnsTrue()
        {
            // arrange

            // act
            var validationResult = motorcycleDTOValidator.IsValid(motorcycleDTO);

            // assert
            Assert.IsTrue(validationResult);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase("\t")]
        public void IsValid_ModelIsInvalid_ReturnsFalse(string model)
        {
            // arrange
            motorcycleDTO.Model = model;
            

            // act
            var validationResult = motorcycleDTOValidator.IsValid(motorcycleDTO);

            // assert
            Assert.IsFalse(validationResult);
        }

        [Test]
        [TestCase(-10)]
        [TestCase(0)]
        [TestCase(Constants.ManifacturingYear.MIN - 1)]
        [TestCase(2027)]
        public void IsValid_ManifacturingYearIsInvalid_ReturnsFalse(int manifacturingYear)
        {
            // arrange
            motorcycleDTO.ManufactoringYear = manifacturingYear;
            

            // act
            var validationResult = motorcycleDTOValidator.IsValid(motorcycleDTO);

            // assert
            Assert.IsFalse(validationResult);
        }

        [Test]
        [TestCase(-200)]
        [TestCase(0)]
        [TestCase(Constants.Displacement.MAX + 1)]
        public void IsValid_DisplacementInCCIsInvalid_ReturnsFalse(int displacemenetInCC)
        {
            // arrange
            motorcycleDTO.ManufactoringYear = displacemenetInCC;


            // act
            var validationResult = motorcycleDTOValidator.IsValid(motorcycleDTO);

            // assert
            Assert.IsFalse(validationResult);
        }
    }
}
