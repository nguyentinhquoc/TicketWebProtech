using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Domain
{
    public enum FlightServiceSearch
    {
        Amadeus = 1,
        VietnamAirline = 2,
        Galileo = 3,
        JetStar = 4,
        BamBooAirWays = 6,
        VietjetAir = 7,
        VietjetAirInternational = 8,
        Datacom = 9,
       
    }
    public enum TripType
    {
        RoundTrip = 1,
        OneWay = 0,
        Bay_Nhieu_Chang = 3,
    }

    public enum WayType
    {
        OutBound = 0,
        InBound = 1,
    }
    public enum TravellerType
    {
        Adult = 1,
        Child = 2,
        Infant = 3,
        Youth = 4,
        Student = 5,
        Senior = 6,
        InfantWithSeat = 7,
    }
    public enum PaymentMethod
    {
        AtOffice = 0,
        TransferBanking = 1,
        PaymentGatewayMSB =2
    }
    public enum Title
    {
        Mr = 1,
        Mrs = 2,
        Dr = 3,
        Chd = 4,
        Inf = 5,
        Ông = 6,
        Bà = 7,
        Anh = 8,
        Chị = 9,
        Bé_Trai = 10,
        Bé_Gái = 12,
        Em_Trai = 11,
        Em_Gái = 13,
    }
}
