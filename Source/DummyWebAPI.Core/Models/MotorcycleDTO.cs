namespace DummyWebAPI.Core.Models
{
    public class MotorcycleDTO
    {
        public MotorcycleBrand Brand { get; set; }

        public required string Model { get; set; }

        public int ManufactoringYear { get; set; }

        public Color Color { get; set; }

        public Category Category { get; set; }

        public EngineType EngineType { get; set; }

        public Transmission Transmission { get; set; }

        public int DisplacementInCC { get; set; }
    }
}
