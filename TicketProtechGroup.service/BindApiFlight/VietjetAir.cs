using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.service.BindApiFlight.RootFlight;
using TicketProtechGroup.service.DbService;
using Newtonsoft.Json;
namespace TicketProtechGroup.service.BindApiFlight

{
    public class VietjetAir
    {
        public static GroupFlight GetGroupFlightVietJets(RootVietJets root, int fareId, int waytype, int countPax)
        {
            var airportRepository = new AirportRepository(); // khởi tạo tạm thời
            GroupFlight result = new GroupFlight();
            FareOption fareOption = GetFareClass(root.fareOptions, countPax);
            Console.WriteLine(fareOption);
            if (fareOption != null && fareOption.fareClass != null)
            {
                result.FareDataId = fareId;
                result.BookingKey = fareOption.bookingKey;
                result.FlightServiceSearch = FlightServiceSearch.VietjetAir;
                result.PriceDomestic = GetPriceDomesticVJ(fareOption);
                result.BgRow = string.Empty;
                result.FareAdt = GetFareAdtVJ(fareOption);
                result.TaxAdt = GetTaxAdtVJ(fareOption);
                result.FareChd = GetFareChdVJ(fareOption);
                result.TaxChd = GetTaxChd(fareOption);
                result.FareInf = GetFareInfVJ(fareOption);
                result.TaxInf = GetTaxInf(fareOption);
                result.FeeAdt = GetFeeAdtVJ(fareOption);
                result.FeeChd = GetFeeChdVJ(fareOption);
                result.FeeInf = GetFeeInfVJ(fareOption);
                result.TicketClassDomestic = fareOption.fareClass.description;
                result.FlightRef = fareOption.availability;
                result.ListChangBays = GetListChangBayVJ(root.flights, fareOption.fareClass.description);
                result.MainFlightNumber = "VJ" + root.flights[0].flightNumber;
                result.MainAirlineCode = "VJ";
                result.MainAirlineName = "VietJet Air";
                result.MainDepartureAirportCode = root.flights[0].departure.airport.code;
                result.MainDepartureAirportName = root.flights[0].departure.airport.name;
                var departureAirportRow = airportRepository.GetAirportByCode(root.flights[0].departure.airport.code);
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                result.MainDepartureTime = Convert.ToDateTime(root.flights[0].departure.scheduledTime).ToString("HH:mm");
                result.Plane = root.flights[0].aircraftModel.name;
                result.MainDepartureDate = Convert.ToDateTime(root.flights[0].departure.scheduledTime);
                result.MainArrivalAirportCode = root.flights[root.flights.Count - 1].arrival.airport.code;
                result.MainArrivalAirportName = root.flights[root.flights.Count - 1].arrival.airport.name;
                var arrivalAirportRow = airportRepository.GetAirportByCode(root.flights[root.flights.Count - 1].arrival.airport.code);
                result.MainArrivalCity = arrivalAirportRow.CityName;
                result.MainArrivalCountry = arrivalAirportRow.CountryName;
                result.MainArrivalTime = Convert.ToDateTime(root.flights[root.flights.Count - 1].arrival.scheduledTime).ToString("HH:mm");
                result.MainArrivalDate = Convert.ToDateTime(root.flights[root.flights.Count - 1].arrival.scheduledTime);
                var h = Convert.ToInt16(root.enRouteHours);
                var m = Convert.ToInt16((Convert.ToDecimal(root.enRouteHours) - h) * 60);
                result.Duration = h + "h" + m + "m"; ;
                result.TotalMinute = Convert.ToInt16(Convert.ToDecimal(root.enRouteHours) * 60);
                result.Stop = Convert.ToInt16(root.numberOfStops);
                if (waytype == 0)
                    result.WayType = WayType.OutBound;
                else
                    result.WayType = WayType.InBound;
                switch (fareOption.fareClass.code.Split('_')[1])
                {
                    case "ECO":
                        result.RecommendationNumber = "7 Kg hành lý xách tay";
                        result.AllowanceBaggage = "0 Kg hành lý ký gửi";
                        result.Note = "<ul class=\"none-style\">";
                        result.Note += "<li><b>Bao gồm:</b></li>";
                        result.Note += "<li> 7 Kg hành lý xách tay</li>";
                        result.Note += "<li><b>Chưa bao gồm:</b></li>";
                        result.Note += "<li> Hành lý ký gửi(tùy chọn)</li>";
                        result.Note += "<li> Chọn trước chỗ ngồi</li>";
                        result.Note += "<li> Phí thay đổi chuyến bay, chặng bay, ngày bay</li>";
                        result.Note += "<li> Chênh lệch tiền vé khi thay đổi(nếu có)</li>";
                        result.Note += "</ul>";
                        result.ReturnTicket = "Không hoàn hủy";
                        break;
                    case "DLX":
                        result.RecommendationNumber = "7 Kg hành lý xách tay";
                        result.AllowanceBaggage = "20 Kg hành lý ký gửi";
                        result.Note = "<ul class=\"none-style\">";
                        result.Note += "<li>7 Kg hành lý xách tay</li>";
                        result.Note += "<li>20 Kg hành lý ký gửi</li>";
                        result.Note += "<li>Ưu tiên làm thủ tục check -in</li>";
                        result.Note += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                        result.Note += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                        result.Note += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                        result.Note += "</ul>";
                        result.ReturnTicket = "Có thể hoàn hủy";
                        break;
                    case "SBoss":
                        result.RecommendationNumber = "7 Kg hành lý xách tay";
                        result.AllowanceBaggage = "20 Kg hành lý ký gửi";
                        result.Note = "<ul class=\"none-style\">";
                        result.Note += "<li>7 Kg hành lý xách tay</li>";
                        result.Note += "<li>20 Kg hành lý ký gửi</li>";
                        result.Note += "<li>Ưu tiên làm thủ tục check -in</li>";
                        result.Note += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                        result.Note += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                        result.Note += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                        result.Note += "</ul>";
                        result.ReturnTicket = "Có thể hoàn hủy";
                        break;
                    default:
                        result.RecommendationNumber = "0 Kg hành lý xách tay";
                        result.AllowanceBaggage = "0 Kg hành lý ký gửi";
                        result.Note = "<ul class=\"none-style\">";
                        result.Note += "<li>0 Kg hành lý xách tay</li>";
                        result.Note += "<li>0 Kg hành lý ký gửi</li>";
                        result.Note += "<li>Không được thay đổi chuyến bay, chặng bay, ngày bay</li>";
                        result.Note += "</ul>";
                        result.ReturnTicket = "Không hoàn hủy";
                        break;
                }
                return result;
            }
            else
                return null;
        }
        private static FareOption GetFareClass(List<FareOption> lFareot, int countPax)
        {
            if (lFareot == null || lFareot.Count == 0)
                return null;

            FareOption result = new FareOption();
            string fareCl = string.Empty;

            if (!string.IsNullOrEmpty(fareCl))
            {
                foreach (var f in lFareot)
                {
                    string check = f.fareClass.code.Split('_')[1];
                    if (fareCl.Equals(check))
                    {
                        result = f;
                        break;
                    }
                }
            }
            else
            {
                foreach (var f in lFareot)
                {
                    if (f.fareValidity != null && f.fareValidity.valid && !f.fareValidity.soldOut && !f.fareValidity.noFare)
                    {
                        if (f.availability >= countPax)
                        {
                            result = f;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static decimal GetPriceDomesticVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.chargeType.code.Equals("FA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFareAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.adult && f.chargeType.code.Equals("FA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.adult && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.adult && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;

        }
        private static decimal GetFareChdVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.child && f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeChdVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.child && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxChd(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.child && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;

        }
        private static decimal GetFareInfVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.infant && f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeInfVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.infant && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxInf(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f => {
                if (f.passengerApplicability.infant && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;
        }
        private static List<Segment> GetListChangBayVJ(List<Flight> flights, string ticketClass)
        {
            var airportRepository = new AirportRepository(); // khởi tạo tạm thời

            List<Segment> result = new List<Segment>();
            flights.ForEach(f => {
                Segment s = new Segment();
                s.FlightNumber = f.flightNumber;
                s.AirlineCode = f.airlineCode.code;
                s.AirlineName = "VietJetAir";
                DateTime departureTime = Convert.ToDateTime(f.departure.scheduledTime);
                DateTime arrivalTime = Convert.ToDateTime(f.arrival.scheduledTime);
                TimeSpan beweenTime = arrivalTime - departureTime;
                double TotalMinute = beweenTime.TotalMinutes;
                var h = Convert.ToInt16(TotalMinute / 60);
                var m = Convert.ToInt16(TotalMinute - h * 60);
                s.Duration = h + "h" + m + "m";
                s.OperatingAirlineCode = "VJ";
                s.OperatingAirlineName = "VietJetAir";
                var departureAirportRow = airportRepository.GetAirportByCode(f.departure.airport.code);
                s.DepartureAirportCode = f.departure.airport.code;
                s.DepartureAirportName = f.departure.airport.name;
                s.DepartureTerminal = string.Empty;
                s.DepartureDate = departureTime;
                s.DepartureTime = departureTime.ToString("HH:mm");
                s.DepartureCity = departureAirportRow.CityName;
                s.DepartureCountry = departureAirportRow.CountryName;
                s.ArrivalAirportCode = f.arrival.airport.code;
                var arrivalAirportRow = airportRepository.GetAirportByCode(f.arrival.airport.code);
                s.ArrivalAirportName = f.arrival.airport.name;
                s.ArrivalTerminal = string.Empty;
                s.ArrivalDate = arrivalTime;
                s.ArrivalTime = arrivalTime.ToString("HH:mm");
                s.ArrivalCity = arrivalAirportRow.CityName;
                s.ArrivalCountry = arrivalAirportRow.CountryName;
                s.TicketClass = ticketClass;
                s.AircraftCode = f.aircraftModel.name.Length < 4 ? "A" + f.aircraftModel.name : f.aircraftModel.name;
                s.AircraftName = f.aircraftModel.name.Length < 4 ? "A" + f.aircraftModel.name : f.aircraftModel.name;
                s.SeatRemain = 0;
                s.SegmentStop = "1";
                result.Add(s);
            });
            return result;
        }
        public static RootVietJets GetAirlineResponseRowVJ(string jsonContent, string bookingKey)
        {
            RootVietJets result = new RootVietJets();
            var root = JsonConvert.DeserializeObject<RootVietJets[]>(jsonContent);
            foreach (var r in root)
            {
                foreach (var f in r.fareOptions)
                {
                    if (f.bookingKey.ToLower().Equals(bookingKey))
                    {
                        result = r;
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
