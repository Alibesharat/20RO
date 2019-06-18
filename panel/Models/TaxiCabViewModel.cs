using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Models
{
    public class TaxiCabViewModel
    {
        public List<ServiceRequset> Passngers { get; set; }

        public TaxiService TaxiCab { get; set; }
    }
}
