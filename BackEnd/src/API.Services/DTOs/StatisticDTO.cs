using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.DTOs
{
    public class StatisticDTO
    {
        public int countAllBooks { get; set; }
        public int countAllGenres { get; set; }
        public int countAllUsers { get; set; }
        public StatisticDTO(int numOfBooks, int numOfGenres, int numOfUsers)
        {
            this.countAllBooks = numOfBooks;
            this.countAllGenres = numOfGenres;
            this.countAllUsers = numOfUsers;
        }

    }
}
