using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DataAccess.Contracts
{
    public interface IDeletable 
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
