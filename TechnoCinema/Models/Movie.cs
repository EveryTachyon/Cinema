namespace TechnoCinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public List<string> Subtitles { get; set; } = new List<string>();
        public string AgeRating { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public double Rating { get; set; }
    }
}
