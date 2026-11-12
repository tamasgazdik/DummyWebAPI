namespace DummyWebAPI.Core.Models
{
    public sealed record MotorcyclesResponse(int NumberOfResults, IEnumerable<MotorcycleDTO> Motorcycles)
    {
    }
}
