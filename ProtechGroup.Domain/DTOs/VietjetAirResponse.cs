using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Domain.DTOs
{
    public class AircraftModel
    {
        public string href { get; set; }
        public string key { get; set; }
        public string identifier { get; set; }
        public string name { get; set; }
        public object type { get; set; }
        public int seatingCapacity { get; set; }
        public object timestamp { get; set; }
    }

    public class AirlineCode
    {
        public string href { get; set; }
        public string code { get; set; }
        public object name { get; set; }
        public object description { get; set; }
        public object active { get; set; }
        public object parent { get; set; }
        public object timestamp { get; set; }
    }

    public class AirportVJ
    {
        public string href { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public object latitude { get; set; }
        public object longitude { get; set; }
        public object timezone { get; set; }
        public UtcOffset utcOffset { get; set; }
        public object secure { get; set; }
    }

    public class Arrival
    {
        public string scheduledTime { get; set; }
        public object localScheduledTime { get; set; }
        public object utcScheduledShortTime { get; set; }
        public object localScheduledShortTime { get; set; }
        public object estimatedTime { get; set; }
        public object utcEstimatedShortTime { get; set; }
        public object utcActualOutShortTime { get; set; }
        public object utcActualOffShortTime { get; set; }
        public object utcActualOnShortTime { get; set; }
        public object utcActualInShortTime { get; set; }
        public AirportVJ airport { get; set; }
    }

    public class BookingApplicability
    {
        public bool allPassengers { get; set; }
        public bool primaryPassenger { get; set; }
        public bool optional { get; set; }
    }

    public class BookingCode
    {
        public string href { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public object cabinClass { get; set; }
        public object nesting { get; set; }
        public object published { get; set; }
        public object fareClassDefaultCriteria { get; set; }
        public object seatSelectionCharge { get; set; }
        public object timestamp { get; set; }
    }

    public class CabinClass
    {
        public string href { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class ChargeType
    {
        public string href { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public object saleCode { get; set; }
        public object usageCode { get; set; }
        public object feeCategory { get; set; }
        public int index { get; set; }
        public object timestamp { get; set; }
    }

    public class CityPair
    {
        public string href { get; set; }
        public string identifier { get; set; }
        public object departure { get; set; }
        public object arrival { get; set; }
        public object validConnectionAirports { get; set; }
        public object fareStatuses { get; set; }
        public object chargeStatuses { get; set; }
        public object taxConfiguration { get; set; }
        public int loyaltyPointsEarned { get; set; }
        public object groupBookingCount { get; set; }
        public object routeType { get; set; }
        public object travelOptionCriteria { get; set; }
        public object fares { get; set; }
        public object charges { get; set; }
        public object timestamp { get; set; }
    }

    public class Currency
    {
        public string href { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public bool baseCurrency { get; set; }
        public double currentExchangeRate { get; set; }
        public object format { get; set; }
    }

    public class CurrencyAmount
    {
        public double baseAmount { get; set; }
        public double discountAmount { get; set; }
        public double taxAmount { get; set; }
        public List<TaxRateAmount> taxRateAmounts { get; set; }
        public double totalAmount { get; set; }
        public Currency currency { get; set; }
        public double exchangeRate { get; set; }
    }

    public class Departure
    {
        public string scheduledTime { get; set; }
        public object localScheduledTime { get; set; }
        public object utcScheduledShortTime { get; set; }
        public object localScheduledShortTime { get; set; }
        public object estimatedTime { get; set; }
        public object utcEstimatedShortTime { get; set; }
        public object utcActualOutShortTime { get; set; }
        public object utcActualOffShortTime { get; set; }
        public object utcActualOnShortTime { get; set; }
        public object utcActualInShortTime { get; set; }
        public AirportVJ airport { get; set; }
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

    public class FareClass
    {
        public string href { get; set; }
        public string key { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public object bookingCode { get; set; }
        public object bookingCodeDefault { get; set; }
        public object secondarySelectionDisplay { get; set; }
        public object fareType { get; set; }
        public object bookingStatus { get; set; }
        public object fareRules { get; set; }
        public object fareRestrictions { get; set; }
        public bool seatSale { get; set; }
        public object autoApplyCharges { get; set; }
        public object nonRevenue { get; set; }
        public bool allowWaitlist { get; set; }
        public object lateBookingOverride { get; set; }
        public PassengerApplicability passengerApplicability { get; set; }
        public object advancedBookingDays { get; set; }
        public object journeyApplicability { get; set; }
        public object stayOverCriteria { get; set; }
        public SeatSelectionChargeApplicability seatSelectionChargeApplicability { get; set; }
        public object loyaltyMultiplier { get; set; }
        public object specifiedCommission { get; set; }
        public object distributionChannels { get; set; }
        public object timestamp { get; set; }
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

    public class FareType
    {
        public string href { get; set; }
        public string identifier { get; set; }
        public string description { get; set; }
        public int index { get; set; }
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

    public class Flight
    {
        public string href { get; set; }
        public string key { get; set; }
        public AirlineCode airlineCode { get; set; }
        public string flightNumber { get; set; }
        public object operatingPartnerCarrier { get; set; }
        public object flightType { get; set; }
        public AircraftModel aircraftModel { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public object status { get; set; }
        public object flightStatus { get; set; }
        public object schedule { get; set; }
        public object legs { get; set; }
        public object timestamp { get; set; }
    }

    public class PassengerApplicability
    {
        public bool child { get; set; }
        public bool adult { get; set; }
        public bool infant { get; set; }
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

    public class AirlineVietJets
    {
        public List<RootVietJets> rootVietJets { get; set; }
    }
    public class RootVietJets
    {
        public string href { get; set; }
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

    public class SeatSelectionChargeApplicability
    {
        public bool bookingCode { get; set; }
        public bool seatType { get; set; }
    }

    public class TaxConfiguration
    {
        public object feeCategory { get; set; }
    }

    public class TaxRateAmount
    {
        public string name { get; set; }
        public double amount { get; set; }
    }

    public class UtcOffset
    {
        public string iso { get; set; }
        public double hours { get; set; }
        public int minutes { get; set; }
    }


    public class UserSessionVJ
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string accountActionMessage { get; set; }
        public string daysUntilExpiry { get; set; }
        public bool isPasswordExpiryEnabled { get; set; }
    }
}
