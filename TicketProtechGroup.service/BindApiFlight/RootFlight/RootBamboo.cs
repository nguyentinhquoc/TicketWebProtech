using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProtechGroup.service.BindApiFlight.RootFlight
{

    public class RootBamboo
    {
        public List<BambooData> data { get; set; }
        public string id { get; set; }
        public List<MessageItem> message { get; set; }
        public bool success { get; set; }
    }

    public class MessageItem
    {
        public string message { get; set; }
    }

    public class BambooData
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public string route { get; set; }
        public List<TripInfo> trip_info { get; set; }
    }

    public class TripInfo
    {
        public int flight_segment_group_id { get; set; }
        public bool is_transit { get; set; }
        public List<SegmentInfo> segment_info { get; set; }
        public List<BookingClass> booking_class { get; set; }
    }

    public class SegmentInfo
    {
        public int group_segment_id { get; set; }
        public int segment_id { get; set; }
        public FlightInfo flight_info { get; set; }
        public DepartureInfo departure_info { get; set; }
        public ArrivalInfo arrival_info { get; set; }
        public AircraftInfo aircraft_info { get; set; }
        public bool is_international { get; set; }
    }

    public class FlightInfo
    {
        public string carrier_code { get; set; }
        public string flight_number { get; set; }
        public string flight_date { get; set; }
    }

    public class DepartureInfo
    {
        public string airport_code { get; set; }
        public string terminal { get; set; }
        public string datetime { get; set; }
    }

    public class ArrivalInfo
    {
        public string airport_code { get; set; }
        public string terminal { get; set; }
        public string datetime { get; set; }
    }

    public class AircraftInfo
    {
        public string type { get; set; }
    }

    public class BookingClass
    {
        public int trip_index { get; set; }
        public string booking_class { get; set; }
        public int seat_availablity { get; set; }
        public string cabin_class { get; set; }
        public List<GroupFare> group_fare { get; set; }
        public Pricing pricing { get; set; }
    }

    public class GroupFare
    {
        public int group_segment_id { get; set; }
        public int segment_id { get; set; }
        public string fare_class { get; set; }
        public string fare_basis { get; set; }
    }

    public class Pricing
    {
        public string fare_type { get; set; }
        public List<PaxPricingInfo> pax_pricing_info { get; set; }
    }

    public class PaxPricingInfo
    {
        public string pax_type { get; set; }
        public BaggageAllowance baggage_allowance { get; set; }
        public Money display_fare { get; set; }
        public Money discount { get; set; }
        public Money applied_fare { get; set; }
        public Money tax { get; set; }
        public Money base_fare { get; set; }
        public Money total { get; set; }
    }

    public class BaggageAllowance
    {
        public string weight { get; set; }
        public string unit { get; set; }
    }

    public class Money
    {
        public long amount { get; set; }
        public string currency { get; set; }
    }


}
