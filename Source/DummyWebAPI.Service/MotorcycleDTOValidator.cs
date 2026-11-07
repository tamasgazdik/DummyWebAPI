using DummyWebAPI.Core;
using DummyWebAPI.Core.Models;

namespace DummyWebAPI.Service
{
    // this whole class could be static or implemented inside the MotorcycleService but whatever
    // just want to add some functionality and open a PR
    public sealed class MotorcycleDTOValidator : IMotorcycleDTOValidator
    {
        public bool IsValid(MotorcycleDTO motorcycleDTO)
        {
            return IsModelValid(motorcycleDTO.Model) &&
                   IsManufactoringYearValid(motorcycleDTO.ManufactoringYear) &&
                   IsDisplacementInCCValid(motorcycleDTO.DisplacementInCC);
        }

        private static bool IsManufactoringYearValid(int manufactoringYear)
        {
            if (Constants.ManifacturingYear.MIN < manufactoringYear &&
                manufactoringYear <= Constants.ManifacturingYear.MAX)
            {
                return true;
            }

            return false;
        }

        private static bool IsDisplacementInCCValid(int displacementInCC)
        {
            if (displacementInCC <= 0 || Constants.Displacement.MAX < displacementInCC)
            {
                return false;
            }

            return true;
        }

        private static bool IsModelValid(string model)
        {
            return !string.IsNullOrWhiteSpace(model);
        }
    }
}
