using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketProtechGroup.service.BindApiFlight
{
    public class FlightResultOutput
    {
        public bool IsError;
        public bool IsErrorException;
        public Exception Exception;
        public bool IsFirstTimeRequest;
        public string ErrorMessageOrigin;
        public string ErrorMessageCustom;
        public string SessionIdDTC;
        public List<Airline> Airlines;
        public List<BlockItem> BlockItems;
        public class Airline : IEquatable<Airline>
        {
            public string ImageUrl;
            public string AirlineName;
            public string AirlineCode;
            public bool Equals(Airline other)
            {
                if (AirlineCode == other.AirlineCode)
                    return true;
                return false;
            }
        }
        public bool IsDisplayAvgPrice = false;
        public bool IsFlightDomestic = false;
        public bool IsShowPriceFull = false;
    }
    public class BlockItem
    {
        public List<GroupFlight> FlightInBounds;
        public List<GroupFlight> FlightOutBounds;
        public bool IsRoundTrip;

        public decimal PricePerPerson;
        public decimal BasePricePerPerson = 0;
        public string RecommendationNumber = "0";
        public decimal Tax;
        public decimal TotalPrice;
        public bool IsHasThuongGia = false;
        public string Currency = "VND";

        public bool IsIntersection(BlockItem blockItem)
        {
            bool isHas = false;
            foreach (var a in FlightOutBounds)
            {
                foreach (var b in blockItem.FlightOutBounds)
                {
                    if (IsEqualGroupFlight(a, b))
                    {
                        isHas = true;
                        break;
                    }
                }
                if (isHas)
                    break;
            }
            if (!isHas)
                return false;
            if (IsRoundTrip)
            {
                isHas = false;
                foreach (var a in FlightInBounds)
                {
                    foreach (var b in blockItem.FlightInBounds)
                    {
                        if (IsEqualGroupFlight(a, b))
                        {
                            isHas = true;
                            break;
                        }
                    }
                    if (isHas)
                        break;
                }
            }
            return isHas;
        }

        private bool IsEqualGroupFlight(GroupFlight a, GroupFlight b)
        {
            if (a.MainAirlineCode.Equals(b.MainAirlineCode) && a.MainFlightNumber.Equals(b.MainFlightNumber) && a.MainDepartureDate.Equals(b.MainDepartureDate) &&
                a.MainArrivalDate.Equals(b.MainArrivalDate))
                return true;
            return false;
        }
    }
    public class GroupFlight : IEquatable<GroupFlight>
    {
        public int FareDataId;
        public FlightServiceSearch FlightServiceSearch;
        public string BgRow;
        public string Plane;

        public decimal Discount;
        public string MainFlightNumber;
        public string MainAirlineCode;
        public string MainAirlineName;

        public string MainDepartureAirportCode;
        public string MainDepartureAirportName;
        public string MainDepartureCity;
        public string MainDepartureCountry;
        public string MainDepartureTime;
        public DateTime MainDepartureDate;

        public string MainArrivalAirportCode;
        public string MainArrivalAirportName;
        public string MainArrivalCity;
        public string MainArrivalCountry;
        public string MainArrivalTime;
        public DateTime MainArrivalDate;

        public string Duration; // Duration in XML file (for display)
        public int TotalMinute; // Duration in INT (for sort)
        public int Stop;
        public WayType WayType;
        public bool IsSelected = false;



        public List<ListHangVe> ListHangVes { get; set; } = new List<ListHangVe>();

        // ===================

        public bool IsMarkupPrivate = false;
        public bool Equals(GroupFlight other)
        {
            //            if (this.MainFlightNumber.Equals(other.MainFlightNumber) || this.FlightRef == other.FlightRef)
            //                                    return true;
            //if (FlightServiceSearch != FlightServiceSearch.Amadeus)
            //{
            if (this.MainFlightNumber.Equals(other.MainFlightNumber))
                return true;
            //}
            //else
            //    if (this.FlightRef == other.FlightRef)
            //        return true;
            return false;
        }
    }
    public class ListHangVe
    {
        public string ReturnTicket;

        public string AllowanceBaggage;
        public string RecommendationNumber = "0";

        public string BookingKey;

        public decimal PriceDomestic;

        public decimal FareAdt;

        public decimal TaxAdt;

        public decimal FareChd;
        public decimal Discount;

        public decimal TaxChd;

        public decimal FareInf;

        public decimal TaxInf;

        public decimal FeeAdt;

        public decimal FeeChd;

        public decimal FeeInf;

        public string TicketClassDomestic;

        public decimal FlightRef;

        public string Note;
        public List<Segment> ListChangBays;


    }
    public class Segment
    {
        public string FlightNumber;
        public string AirlineCode;
        public string AirlineName;
        public string Duration;
        public string OperatingAirlineCode;
        public string OperatingAirlineName;
        public string DepartureAirportCode;
        public string DepartureAirportName;
        public string DepartureTerminal;
        public DateTime DepartureDate;
        public string DepartureTime;
        public string DepartureCity;
        public string DepartureCountry;
        public string ArrivalAirportCode;
        public string ArrivalAirportName;
        public string ArrivalTerminal;
        public DateTime ArrivalDate;
        public string ArrivalTime;
        public string ArrivalCity;
        public string ArrivalCountry;
        public string TicketClass;
        public string AircraftCode;
        public string AircraftName;
        public int SeatRemain;
        public string SegmentStop = string.Empty;
    }
    public enum FlightServiceSearch
    {
        Amadeus = 1,
        VietnamAirline = 2,
        Galileo = 3,
        JetStar = 4,
        Abacus = 5,
        BamBooAirWays = 6,
        VietjetAir = 7,
        VietjetAirInternational = 8,
        Datacom = 9,
        TicketDiscount = 10
    }
    public enum WayType
    {
        OutBound = 0,
        InBound = 1,
    }
}