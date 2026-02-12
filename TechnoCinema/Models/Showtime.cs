using System;
using System.ComponentModel.DataAnnotations;



//Frieren the boooomm data 
namespace TechnoCinema.Models
{
    public class Showtime
    {
        public int Id { get; set; }
        public string KinoNimi { get; set; } = string.Empty;
        public int Saal { get; set; } = 0;
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Formaat { get; set; } = "2D / 3D / 4D";
        public string Keel { get; set; } = "Eesti";
        public string Subtiitrid { get; set; } = "Puuduvad";
        public int VabuKohti { get; set; }
        public string Film { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }

    }
}