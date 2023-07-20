using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountScraping.Models
{
    public class MovementModel
    {
        public DateTime MovementDate { get; set; }
        public decimal MovementSum { get; set; }
        public string MovementDesc { get; set; }
    }
}
