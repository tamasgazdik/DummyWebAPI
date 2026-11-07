using DummyWebAPI.Core.Models;

namespace DummyWebAPI.Service
{
    public interface IMotorcycleDTOValidator
    {
        public bool IsValid(MotorcycleDTO motorcycleDTO);
    }
}
