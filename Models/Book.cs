using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trpo7.Models
{
    public class Book
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } 

        public string Author { get; set; }

        public int PublishYear { get; set; }

        public decimal Price { get; set; } 

        public bool IsAvailable { get; set; }
    }
}
