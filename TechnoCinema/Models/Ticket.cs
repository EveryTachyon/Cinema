namespace TechnoCinema.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
        public List<Seat> Seats { get; set; }
        public Room Room { get; set; }
        public DateTime  CreatedAt { get; set; }
        public DateTime PaidAt { get; set; } //null = not paid yet
        
    }
}
