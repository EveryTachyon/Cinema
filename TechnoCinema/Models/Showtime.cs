using System;
using System.ComponentModel.DataAnnotations;

// TODO
// Mudelite linkimine, et oleks public Film Film, mitte public string Film. sama Room ja Location
// UI muudatsed, nt dropdown/select valik filmile, ruumile, kinole
// Vabu kohti arvutab showtime.room.maxseats - showtime.tickets
// tickets sees eemalda roomid ja movieid vaid hoopis showtime.id

//Frieren the boooomm data 
namespace TechnoCinema.Models
{
    public class Showtime
    {
        public int Id { get; set; }
        public string KinoNimi { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public Room Room { get; set; }
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