using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProtechGroup.service.BindApiFlight.RootFlight
{

    public class RootVietJets
    {
        public string key { get; set; }
        public CityPair cityPair { get; set; }
        public string departureDate { get; set; }
        public double enRouteHours { get; set; }
        public int numberOfStops { get; set; }
        public int numberOfChanges { get; set; }
        public List<Flight> flights { get; set; }
        public List<FareOption> fareOptions { get; set; }
        public PromoCodeApplicability promoCodeApplicability { get; set; }
    }

    public class CityPair
    {
        public string identifier { get; set; }
    }

    public class Flight
    {
        public string key { get; set; }
        public AirlineCode airlineCode { get; set; }
        public string flightNumber { get; set; }
        public object operatingPartnerCarrier { get; set; }
        public AircraftModel aircraftModel { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public int availability { get; set; }
        public int infantAvailability { get; set; }
    }

    public class AirlineCode
    {
        public string code { get; set; }
    }

    public class AircraftModel
    {
        public string key { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
    }

    public class Departure
    {
        public string scheduledTime { get; set; }
        public string localScheduledTime { get; set; }
        public Airport airport { get; set; }
    }

    public class Arrival
    {
        public string scheduledTime { get; set; }
        public string localScheduledTime { get; set; }
        public Airport airport { get; set; }
    }

    public class Airport
    {
        public string code { get; set; }
        public string name { get; set; }
        public UtcOffset utcOffset { get; set; }
    }

    public class UtcOffset
    {
        public string iso { get; set; }
        public int hours { get; set; }
        public int minutes { get; set; }
    }

    public class FareOption
    {
        public string bookingKey { get; set; }
        public FareValidity fareValidity { get; set; }
        public FareClass fareClass { get; set; }
        public BookingCode bookingCode { get; set; }
        public CabinClass cabinClass { get; set; }
        public FareType fareType { get; set; }
        public int availability { get; set; }
        public bool cheapestFareType { get; set; }
        public bool cheapestFareOption { get; set; }
        public List<FareCharge> fareCharges { get; set; }
        public bool promoCodeApplied { get; set; }
    }

    public class FareValidity
    {
        public bool valid { get; set; }
        public bool soldOut { get; set; }
        public bool noFare { get; set; }
        public bool invalidAdultAvailability { get; set; }
        public bool invalidChildAvailability { get; set; }
        public bool invalidAvailability { get; set; }
        public bool invalidLayover { get; set; }
        public bool invalidStayover { get; set; }
    }

    public class FareClass
    {
        public string key { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public bool seatSale { get; set; }
        public bool allowWaitlist { get; set; }
        public PassengerApplicability passengerApplicability { get; set; }
        public SeatSelectionChargeApplicability seatSelectionChargeApplicability { get; set; }
    }

    public class PassengerApplicability
    {
        public bool adult { get; set; }
        public bool child { get; set; }
        public bool infant { get; set; }
    }

    public class SeatSelectionChargeApplicability
    {
        public bool bookingCode { get; set; }
        public bool seatType { get; set; }
    }

    public class BookingCode
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class CabinClass
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class FareType
    {
        public string identifier { get; set; }
        public string description { get; set; }
        public int index { get; set; }
    }

    public class FareCharge
    {
        public string description { get; set; }
        public BookingApplicability bookingApplicability { get; set; }
        public PassengerApplicability passengerApplicability { get; set; }
        public ChargeType chargeType { get; set; }
        public List<CurrencyAmount> currencyAmounts { get; set; }
        public TaxConfiguration taxConfiguration { get; set; }
    }

    public class BookingApplicability
    {
        public bool allPassengers { get; set; }
        public bool primaryPassenger { get; set; }
        public bool optional { get; set; }
    }

    public class ChargeType
    {
        public string code { get; set; }
        public string description { get; set; }
        public object feeCategory { get; set; }
        public int index { get; set; }
    }

    public class CurrencyAmount
    {
        public long baseAmount { get; set; }
        public long discountAmount { get; set; }
        public long taxAmount { get; set; }
        public List<TaxRateAmount> taxRateAmounts { get; set; }
        public long totalAmount { get; set; }
        public Currency currency { get; set; }
    }

    public class TaxRateAmount
    {
        public string name { get; set; }
        public long amount { get; set; }
    }

    public class Currency
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class TaxConfiguration
    {
        public object feeCategory { get; set; }
    }

    public class PromoCodeApplicability
    {
        public bool promoCodeRequested { get; set; }
        public PromoCodeValidity promoCodeValidity { get; set; }
        public string promoCode { get; set; }
    }

    public class PromoCodeValidity
    {
        public bool valid { get; set; }
        public bool notApplicable { get; set; }
        public bool noMarket { get; set; }
        public bool invalidFlightDate { get; set; }
        public bool notAvailable { get; set; }
        public bool invalidAvailability { get; set; }
    }

}
