namespace TechnoCinema.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Room Room { get; set; }
        public string Type { get; set; }
        public double PriceMultiplier { get; set; }
    }
}