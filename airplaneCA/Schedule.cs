using System;

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
