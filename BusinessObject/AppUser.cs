using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }    
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Post> ?Posts { get; set; }
    }
}
