namespace TechnoCinema.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Location Location  { get; set; }
        public int NumberOfSeats { get; set; }
        public string ScreenType { get; set; }
        public bool SubtitleScreen { get; set; }
        public string Layout { get; set; }
        public List<Seat> Seats { get; set; }
        public double PriceMultiplier { get; set; }

    }
}
