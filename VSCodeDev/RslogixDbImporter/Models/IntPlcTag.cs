using Newtonsoft.Json;

namespace RslogixDbImporter.Models
{
    public class IntPlcTag
    {
        public int Id { get; set; }
        public int PlcId { get; set; }
        public string? SymbolName { get; set; }
        public string? Address { get; set; }

        public string? Description { get; set; }
        [JsonIgnore]
        public MicrologixPlc Plc { get; set; } = null!;
    }

    public enum TagType
    {
        Output = 0,
        Input = 1,
        Status = 2,
        Binary = 3,
        Timer = 4,
        Counter = 5,
        Control = 6,
        Integer = 7,
        Float = 8,
    }
}