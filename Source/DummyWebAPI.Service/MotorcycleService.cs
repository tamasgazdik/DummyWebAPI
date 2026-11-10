using DummyWebAPI.Core;
using DummyWebAPI.Core.Models;
using Microsoft.Extensions.Configuration;

namespace DummyWebAPI.Service
{
    public sealed class MotorcycleService : IMotorcycleService
    {
        private IEnumerable<Motorcycle> motorcycles = new List<Motorcycle>();

        private int currentID = 0;

        private readonly IMotorcycleDTOValidator motorcycleDTOValidator;

        public MotorcycleService(
            IConfiguration configuration,
            IMotorcycleDTOValidator motorcycleDTOValidator)
        {
            this.motorcycleDTOValidator = motorcycleDTOValidator;

            // if the configured value is a positive number, then we go with that one.
            // if it's zero, then we'll have no motorcycles in the list to start with.
            // otherwise we'll start with a default of 5 motorcycles in the list.
            if (int.TryParse(configuration.GetSection("NumberOfMotorcyclesToStartWith").Value, out int numberOfMotorcyclesToStartWith) &&
                numberOfMotorcyclesToStartWith > 0)
            {
                InitializeMotorcyclesList(numberOfMotorcyclesToStartWith);
            }
            else if (numberOfMotorcyclesToStartWith == 0) 
            {
                return;
            }
            else
            {
                InitializeMotorcyclesList(5);
            }
        }

        public Task<MotorcycleDTO?> GetMotorcycleAsync(int ID, CancellationToken cancellationToken = default)
        {
            var motorcycle = GetMotorcycleById(ID);
            return Task.FromResult(motorcycle?.ConvertToMotorcycleDTO());
        }

        public Task<List<MotorcycleDTO>> GetAllMotorcyclesAsync(CancellationToken cancellationToken = default)
        {
            var motorcycleDTOs = new List<MotorcycleDTO>();
            foreach (var motorcycle in motorcycles)
            {
                var motorcycleDTO = motorcycle.ConvertToMotorcycleDTO();
                motorcycleDTOs.Add(motorcycleDTO);
            }
            return Task.FromResult(motorcycleDTOs);
        }

        public Task<int> CreateMotorcycleAsync(MotorcycleDTO motorcycleDTO, CancellationToken cancellationToken = default)
        {
            if (IsValidMotorcycleDTO(motorcycleDTO))
            {
                var motorcycle = Motorcycle.CreateMotorcycleFromDTO(motorcycleDTO, ++currentID);
                
                motorcycles = motorcycles.Append(motorcycle);

                return Task.FromResult(currentID);
            }
            else
            {
                return Task.FromResult(-1);
            }
        }

        public async Task<bool> UpdateMotorcycleAsync(int ID, MotorcycleDTO motorcycleDTO, CancellationToken cancellationToken = default)
        {
            var motorcycle = GetMotorcycleById(ID);
            if (motorcycle == null ||
                !IsValidMotorcycleDTO(motorcycleDTO))
            {
                return false;
            }

            motorcycle.Brand = motorcycleDTO.Brand;
            motorcycle.Model = motorcycleDTO.Model;
            motorcycle.ManufactoringYear = motorcycleDTO.ManufactoringYear;
            motorcycle.Color = motorcycleDTO.Color;
            motorcycle.Category = motorcycleDTO.Category;
            motorcycle.EngineType = motorcycleDTO.EngineType;
            motorcycle.Transmission = motorcycleDTO.Transmission;
            motorcycle.DisplacementInCC = motorcycleDTO.DisplacementInCC;

            return true;
        }

        public async Task<int> DeleteMotorcycleAsync(int ID, CancellationToken cancellationToken = default)
        {
            var motorcycle = GetMotorcycleById(ID);
            if (motorcycle is null)
            {
                return -1;
            }

            motorcycles = motorcycles.Where(motorcycle => motorcycle.ID != ID);

            return ID;
        }

        private void InitializeMotorcyclesList(int numberOfMotorcyclesToInit)
        {
            for (int i = 1; i <= numberOfMotorcyclesToInit; i++)
            {
                var motorcycle = new Motorcycle
                {
                    ID = i,
                    Brand = GetRandomEnumValue<MotorcycleBrand>(),
                    Model = "RandomModelNumber",
                    ManufactoringYear = Random.Shared.Next(Constants.ManifacturingYear.MIN, Constants.ManifacturingYear.MAX + 1),
                    Color = GetRandomEnumValue<Color>(),
                    Category = GetRandomEnumValue<Category>(),
                    EngineType = GetRandomEnumValue<EngineType>(),
                    Transmission = GetRandomEnumValue<Transmission>(),
                    DisplacementInCC = GetRandomDisplacement()
                };

                motorcycles = motorcycles.Append(motorcycle);
                currentID++;
            }
        }

        private static T GetRandomEnumValue<T>() where T: struct, Enum
        {
            var enumValues = Enum.GetValues<T>();

            return enumValues[Random.Shared.Next(enumValues.Length)];
        }

        private static int GetRandomDisplacement()
        {
            return CommonDisplacement.Values[Random.Shared.Next(CommonDisplacement.Values.Length)];
        }

        private Motorcycle? GetMotorcycleById(int ID)
        {
            return motorcycles.FirstOrDefault(motorcycle => motorcycle.ID == ID);
        }

        private bool IsValidMotorcycleDTO(MotorcycleDTO motorcycle)
        {
            return motorcycleDTOValidator.IsValid(motorcycle);
        }
    }
}
