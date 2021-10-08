using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Borealis.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Email adresa")]
        [DataType(DataType.EmailAddress)]
        public string EMailAddress { get; set; }

        [DisplayName("Lozinka")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
