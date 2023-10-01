using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwizd.Clients
{
    public class AddEventDTO
    {
        public string id { get; set; }
        public string animal_type { get; set; }
        public string breed { get; set; }
        public string event_type { get; set; }
        public string label { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}
