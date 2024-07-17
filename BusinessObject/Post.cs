using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public int AuthorID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte Status { get; set; }
        public int CategoryId { get; set; }

        public  AppUser? Author { get; set; }
        public  PostCategory ?Category { get; set; }
    }
}
