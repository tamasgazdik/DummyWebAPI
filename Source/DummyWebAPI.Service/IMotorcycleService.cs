using DummyWebAPI.Core.Models;

namespace DummyWebAPI.Service
{
    public interface IMotorcycleService
    {
        public Task<MotorcycleDTO?> GetMotorcycleAsync(int ID, CancellationToken cancellationToken = default);

        public Task<List<MotorcycleDTO>> GetAllMotorcyclesAsync(CancellationToken cancellationToken = default);

        public Task<int> CreateMotorcycleAsync(MotorcycleDTO motorcycle, CancellationToken cancellationToken = default);

        public Task<bool> UpdateMotorcycleAsync(int ID, MotorcycleDTO updatedMotorcycle, CancellationToken cancellationToken = default);

        public Task<int> DeleteMotorcycleAsync(int ID, CancellationToken cancellationToken = default);
    }
}
