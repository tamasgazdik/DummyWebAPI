using DummyWebAPI.Core;
using DummyWebAPI.Core.Models;
using FluentValidation;

namespace DummyWebAPI.Service
{
    // this whole class could be static or implemented inside the MotorcycleService but whatever
    // just want to add some functionality and open a PR
    public sealed class MotorcycleDTOValidator : AbstractValidator<MotorcycleDTO>, IMotorcycleDTOValidator
    {
        public MotorcycleDTOValidator()
        {
            RuleFor(x => x.ManufactoringYear)
                .GreaterThan(Constants.ManifacturingYear.MIN)
                .LessThanOrEqualTo(Constants.ManifacturingYear.MAX)
                .WithMessage($"Manufacturing year must be between {Constants.ManifacturingYear.MIN} and {Constants.ManifacturingYear.MAX + 1}");

            RuleFor(x => x.DisplacementInCC)
                .GreaterThan(0)
                .LessThanOrEqualTo(Constants.Displacement.MAX)
                .WithMessage($"Displacement in CC must be between {0} and {Constants.Displacement.MAX + 1}");

            RuleFor(x => x.Model)
                .Must(NotBeNullOrWhitespace)
                .WithMessage("Model cannot be null or whitespace");
        }

        public bool IsValid(MotorcycleDTO motorcycleDTO)
        {
            return Validate(motorcycleDTO).IsValid;
        }

        private static bool NotBeNullOrWhitespace(string model)
        {
            return !string.IsNullOrWhiteSpace(model);
        }
    }
}
