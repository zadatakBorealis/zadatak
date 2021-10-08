using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Borealis.Models
{
    public class TopTime
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Ime")]
        [Required(ErrorMessage = "Potrebno je unijeti ime!")]
        public string FirstName { get; set; }
        [DisplayName("Prezime")]
        [Required(ErrorMessage = "Potrebno je unijeti prezime!")]
        public string LastName { get; set; }
        [DisplayName("Vrijeme utrke")]
        [Required(ErrorMessage = "Potrebno je unijeti vrijeme utrke!")]
        [RegularExpression(@"[0-2]?[0-9]:[0-9]{1,2}:[0-9]{1,2}",
            ErrorMessage = "Vrijeme mora biti u formatu hh:mm:ss")]
        public string Time { get; set; }
        [DisplayName("Jeste li sigurni da želite obrisati vrijeme? Pritiskom na odbaci " +
            "vrijeme se šalje natrag na odobravanje.")]
        public bool IsApproved { get; set; }

        public bool IsReadyToDelete { get; set; }

        public DateTime TimeAsDate { 
            get {
                if(DateTime.TryParse(Time, out DateTime result))
                {
                    return Convert.ToDateTime(Time);
                }
                return result;
            }
            set {
                Convert.ToDateTime(Time);
            }
        }

        public string CreatedBy { get; set; }
    }
}
