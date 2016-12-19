using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airplaneCA
{
    class Schedule
    {
        public Guid Id;
        public Guid IdFlight;
        public DateTime DepartureDT;
        public DateTime ArrivalDT;

        public override string ToString()
        {
            return Id.ToString() + IdFlight.ToString() + DepartureDT.ToString() + ArrivalDT.ToString();
        }
    }
}
