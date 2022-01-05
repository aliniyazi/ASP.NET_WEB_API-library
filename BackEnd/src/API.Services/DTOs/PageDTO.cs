using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DataAccess.Models;

namespace API.Services.DTOs
{
    public class PageDTO
    {
        public BookDTO[] data { get; set; }
        public int recordsTotal { get; set; }
        public PageDTO(BookDTO[] books, int recordsTotal)
        {
            this.data = books;
            this.recordsTotal = recordsTotal;
        }
    }
}
