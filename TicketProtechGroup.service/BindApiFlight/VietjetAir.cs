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
         public static FlightResultOutput BuildRootVietJets(RootVietJets[] alineVJ, int countPax)
        {
            var flightResultOutput = new FlightResultOutput();
            flightResultOutput.IsFlightDomestic = true;
            flightResultOutput.BlockItems = new List<BlockItem>();
            flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
            var airline = new FlightResultOutput.Airline();
            airline.AirlineName = "VietJetAir";
            airline.AirlineCode = "VJ";
            airline.ImageUrl = "vietjet.png";
            flightResultOutput.Airlines.Add(airline);
            BlockItem blockItem = new BlockItem();
            blockItem.FlightOutBounds = new List<GroupFlight>();
            blockItem.FlightInBounds = new List<GroupFlight>();
            if (alineVJ != null && alineVJ.Length > 0)
            {
                string departureAirport = alineVJ[0].cityPair.identifier.Split('-')[0];
                string ArrivalAirport = alineVJ[0].cityPair.identifier.Split('-')[1];
                for (int i = 0; i < alineVJ.Length; i++)
                {

                    if (alineVJ[i].cityPair.identifier.Equals(departureAirport + "-" + ArrivalAirport))
                    {
                        var gf = VietjetAir.GetGroupFlightVietJets(alineVJ[i], i, 0, countPax);
                        if (gf != null)
                            blockItem.FlightOutBounds.Add(gf);
                    }
                    else
                    {
                        var gf = VietjetAir.GetGroupFlightVietJets(alineVJ[i], i, 1, countPax);
                        if (gf != null)
                            blockItem.FlightInBounds.Add(gf);
                    }
                }
                if (blockItem.FlightInBounds.Count > 0)
                    blockItem.IsRoundTrip = true;
            }
            flightResultOutput.BlockItems.Add(blockItem);
            return flightResultOutput;
        }


        public static GroupFlight GetGroupFlightVietJets(RootVietJets root, int fareId, int waytype, int countPax)
        {
            var airportRepository = new AirportRepository(); 
            List<ListHangVe> listHangVes = new List<ListHangVe>();
            GroupFlight result = new GroupFlight();
            List<FareOption> fareOptions = GetFareClass(root.fareOptions, countPax);
            if (fareOptions != null && fareOptions.Count > 0)
            {
                result.FareDataId = fareId;
                result.FlightServiceSearch = FlightServiceSearch.VietjetAir;
                result.BgRow = string.Empty;
                result.MainAirlineCode = "VJ";
                result.MainAirlineName = "VietJet Air";
                result.MainDepartureAirportCode = root.flights[0].departure.airport.code;
                result.MainDepartureAirportName = root.flights[0].departure.airport.name;
                var departureAirportRow = airportRepository.GetAirportByCode(root.flights[0].departure.airport.code);
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                result.MainDepartureTime = Convert.ToDateTime(root.flights[0].departure.scheduledTime).ToString("HH:mm");
                result.MainFlightNumber = "VJ" + root.flights[0].flightNumber;
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

                foreach (var fareOption in fareOptions)
                {
                    if (fareOption.fareClass != null)
                    {
                        var listHangVe = new ListHangVe();
                        listHangVe.ListChangBays = GetListChangBayVJ(root.flights, fareOption.fareClass.description);
                        listHangVe.BookingKey = fareOption.bookingKey;
                        listHangVe.Discount = GetDiscountVJ(fareOption);
                        listHangVe.PriceDomestic = GetPriceDomesticVJ(fareOption);
                        listHangVe.FareAdt = GetFareAdtVJ(fareOption);
                        listHangVe.TaxAdt = GetTaxAdtVJ(fareOption);
                        listHangVe.FareChd = GetFareChdVJ(fareOption);
                        listHangVe.TaxChd = GetTaxChd(fareOption);
                        listHangVe.FareInf = GetFareInfVJ(fareOption);
                        listHangVe.TaxInf = GetTaxInf(fareOption);
                        listHangVe.FeeAdt = GetFeeAdtVJ(fareOption);
                        listHangVe.FeeChd = GetFeeChdVJ(fareOption);
                        listHangVe.FeeInf = GetFeeInfVJ(fareOption);
                        listHangVe.TicketClassDomestic = fareOption.fareClass.description;
                        listHangVe.FlightRef = fareOption.availability;
                        switch (fareOption.fareClass.code.Split('_')[1])
                        {
                           case "ECO":
                               listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                               listHangVe.AllowanceBaggage = "0 Kg hành lý ký gửi";
                               listHangVe.Note += "<li><b>Bao gồm:</b></li>";
                               listHangVe.Note += "<li> 7 Kg hành lý xách tay</li>";
                               listHangVe.Note += "<li><b>Chưa bao gồm:</b></li>";
                               listHangVe.Note += "<li> Hành lý ký gửi(tùy chọn)</li>";
                               listHangVe.Note += "<li> Chọn trước chỗ ngồi</li>";
                               listHangVe.Note += "<li> Phí thay đổi chuyến bay, chặng bay, ngày bay</li>";
                               listHangVe.Note += "<li> Chênh lệch tiền vé khi thay đổi(nếu có)</li>";
                               listHangVe.ReturnTicket = "Không hoàn hủy";
                               break;
                           case "DLX":
                               listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                               listHangVe.AllowanceBaggage = "20 Kg hành lý ký gửi";
                               listHangVe.Note += "<li>7 Kg hành lý xách tay</li>";
                               listHangVe.Note += "<li>20 Kg hành lý ký gửi</li>";
                               listHangVe.Note += "<li>Ưu tiên làm thủ tục check -in</li>";
                               listHangVe.Note += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                               listHangVe.Note += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                               listHangVe.Note += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                               listHangVe.ReturnTicket = "Có thể hoàn hủy";
                               break;
                           case "SBoss":
                               listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                               listHangVe.AllowanceBaggage = "20 Kg hành lý ký gửi";
                               listHangVe.Note += "<li>7 Kg hành lý xách tay</li>";
                               listHangVe.Note += "<li>20 Kg hành lý ký gửi</li>";
                               listHangVe.Note += "<li>Ưu tiên làm thủ tục check -in</li>";
                               listHangVe.Note += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                               listHangVe.Note += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                               listHangVe.Note += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                               listHangVe.ReturnTicket = "Có thể hoàn hủy";
                               break;
                           default:
                               listHangVe.RecommendationNumber = "0 Kg hành lý xách tay";
                               listHangVe.AllowanceBaggage = "0 Kg hành lý ký gửi";
                               listHangVe.Note += "<li>0 Kg hành lý xách tay</li>";
                               listHangVe.Note += "<li>0 Kg hành lý ký gửi</li>";
                               listHangVe.Note += "<li>Không được thay đổi chuyến bay, chặng bay, ngày bay</li>";
                               listHangVe.ReturnTicket = "Không hoàn hủy";
                               break;
                        }
                    result.ListHangVes.Add(listHangVe);
                    }
                }

                return result;
            }
            else
                return null;
        }
        private static List<FareOption> GetFareClass(List<FareOption> lFareot, int countPax)
        {
            List<FareOption> result = new List<FareOption>();
            string fareCl = string.Empty;

            if (lFareot == null || lFareot.Count == 0)
                return result;

            if (!string.IsNullOrEmpty(fareCl))
            {
                foreach (var f in lFareot)
                {
                    string check = f.fareClass.code.Split('_')[1];
                    if (fareCl.Equals(check))
                    {
                        result.Add(f);
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
                            result.Add(f);
                        }
                    }
                }
            }

            return result;
        }

        private static decimal GetDiscountVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.chargeType.code.Equals("FA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].discountAmount);
            });
            return result;

        }
        private static decimal GetPriceDomesticVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.chargeType.code.Equals("FA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFareAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.adult && f.chargeType.code.Equals("FA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.adult && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxAdtVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.adult && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;

        }
        private static decimal GetFareChdVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.child && f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeChdVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.child && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxChd(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.child && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;

        }
        private static decimal GetFareInfVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.infant && f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result = Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetFeeInfVJ(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.infant && !f.chargeType.code.Equals("FA") && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].baseAmount);
            });
            return result;

        }
        private static decimal GetTaxInf(FareOption faOp)
        {
            decimal result = 0;
            faOp.fareCharges.ForEach(f =>
            {
                if (f.passengerApplicability.infant && !f.chargeType.code.Equals("SA"))
                    result += Convert.ToDecimal(f.currencyAmounts[0].taxAmount);
            });
            return result;
        }
        private static List<Segment> GetListChangBayVJ(List<Flight> flights, string ticketClass)
        {
            var airportRepository = new AirportRepository(); // khởi tạo tạm thời

            List<Segment> result = new List<Segment>();
            flights.ForEach(f =>
            {
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
