namespace DummyWebAPI.Core.Models
{
    public sealed class Motorcycle : MotorcycleDTO
    {
        public int ID { get; set; }

        public MotorcycleDTO ConvertToMotorcycleDTO()
        {
            return new MotorcycleDTO
            {
                Brand = Brand,
                Model = Model,
                ManufactoringYear = ManufactoringYear,
                Color = Color,
                Category = Category,
                EngineType = EngineType,
                Transmission = Transmission,
                DisplacementInCC = DisplacementInCC,
            };
        }

        public static Motorcycle CreateMotorcycleFromDTO(MotorcycleDTO motorcycleDTO, int ID)
        {
            return new Motorcycle
            {
                ID = ID,
                Brand = motorcycleDTO.Brand,
                Model = motorcycleDTO.Model,
                ManufactoringYear = motorcycleDTO.ManufactoringYear,
                Color = motorcycleDTO.Color,
                Category = motorcycleDTO.Category,
                EngineType = motorcycleDTO.EngineType,
                Transmission = motorcycleDTO.Transmission,
                DisplacementInCC = motorcycleDTO.DisplacementInCC
            };
        }
    }
}
