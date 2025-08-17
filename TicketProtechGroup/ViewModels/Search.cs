using System;
using System.Collections.Generic;
namespace TicketProtechGroup.ViewModels
{
    public class ChangBayViewModel
    {
        public string FlightNumber { get; set; }
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string Duration { get; set; }
        public string OperatingAirlineCode { get; set; }
        public string OperatingAirlineName { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureTerminal { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureCountry { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalTerminal { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalCountry { get; set; }
        public string TicketClass { get; set; }
        public string AircraftCode { get; set; }
        public string AircraftName { get; set; }
        public int SeatRemain { get; set; }
        public string SegmentStop { get; set; }
    }

    public class HangVeViewModel
    {
        public string ReturnTicket { get; set; }
        public string AllowanceBaggage { get; set; }
        public string RecommendationNumber { get; set; }
        public string BookingKey { get; set; }
        public decimal PriceDomestic { get; set; }
        public decimal FareAdt { get; set; }
        public decimal TaxAdt { get; set; }
        public decimal FareChd { get; set; }
        public decimal TaxChd { get; set; }
        public decimal FareInf { get; set; }
        public decimal TaxInf { get; set; }
        public decimal FeeAdt { get; set; }
        public decimal FeeChd { get; set; }
        public decimal FeeInf { get; set; }
        public string TicketClassDomestic { get; set; }
        public int FlightRef { get; set; }
        public string Note { get; set; }
        public List<ChangBayViewModel> ListChangBays { get; set; }
    }

    public class SearchViewModel
    {
        public int FareDataId { get; set; }
        public int FlightServiceSearch { get; set; }
        public string BgRow { get; set; }
        public string Plane { get; set; }
        public decimal Discount { get; set; }
        public string MainFlightNumber { get; set; }
        public string MainAirlineCode { get; set; }
        public string MainAirlineName { get; set; }
        public string MainDepartureAirportCode { get; set; }
        public string MainDepartureAirportName { get; set; }
        public string MainDepartureCity { get; set; }
        public string MainDepartureCountry { get; set; }
        public string MainDepartureTime { get; set; }
        public DateTime MainDepartureDate { get; set; }
        public string MainArrivalAirportCode { get; set; }
        public string MainArrivalAirportName { get; set; }
        public string MainArrivalCity { get; set; }
        public string MainArrivalCountry { get; set; }
        public string MainArrivalTime { get; set; }
        public DateTime MainArrivalDate { get; set; }
        public string Duration { get; set; }
        public int TotalMinute { get; set; }
        public int Stop { get; set; }
        public int WayType { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMarkupPrivate { get; set; }
        public List<HangVeViewModel> ListHangVes { get; set; }
    }
}