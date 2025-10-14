using Newtonsoft.Json;
using ProtechGroup.Application.Interfaces;
using ProtechGroup.Application.Services;
using ProtechGroup.Domain;
using ProtechGroup.Domain.DTOs;
using ProtechGroup.Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace ProtechGroup.Application.Common
{
    public class VietJetsMethod : IVietJetsMethod
    {
        private readonly IAirportService _iairportService;
        private readonly IServiceFeeService _iserviceFeeService;
        public VietJetsMethod(IAirportService iairportService, IServiceFeeService iserviceFeeService)
        {
            _iairportService = iairportService;
            _iserviceFeeService = iserviceFeeService;
        }
        public FlightResultOutput BuildFlightResultVietJets(RootVietJets[] alineVJ, int countPax, bool isDomestric)
        {
            try
            {
                var flightResultOutput = new FlightResultOutput();
                flightResultOutput.IsFlightDomestic = true;
                flightResultOutput.BlockItems = new List<BlockItem>();
                flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
                var airline = new FlightResultOutput.Airline();
                airline.AirlineName = "VietJetAir";
                airline.AirlineCode = "VJ";

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
                            var gf = GetGroupFlightVietJets(alineVJ[i], i, 0, countPax, isDomestric);
                            if (gf != null)
                                blockItem.FlightOutBounds.Add(gf);
                        }
                        else
                        {
                            var gf = GetGroupFlightVietJets(alineVJ[i], i, 1, countPax, isDomestric);
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
            catch
            {
                return null;
            }
        }
        public GroupFlight GetGroupFlightVietJets(RootVietJets root, int fareId, int wattype, int countPax, bool isDomestric)
        {
            GroupFlight result = new GroupFlight();
            string depDate = root.flights[0].departure.scheduledTime.Split(' ')[0].Split('-')[2] + "/" +
                             root.flights[0].departure.scheduledTime.Split(' ')[0].Split('-')[1] + "/" +
                             root.flights[0].departure.scheduledTime.Split(' ')[0].Split('-')[0] + " " +
                             root.flights[0].departure.scheduledTime.Split(' ')[1].Split(':')[0] + ":" +
                             root.flights[0].departure.scheduledTime.Split(' ')[1].Split(':')[1] + ":00";
            double timecheck = CoreUtils.GetHourDifference(depDate);
            int beforday = 0;
            if (timecheck <= 24)
            {
                beforday = 1;
            }
            var serviceFee = _iserviceFeeService.GetServiceFeeByAgBfdDo(isDomestric, 0, beforday);
            var priceBreakDownFlightVJ = GetPriceBreakDownFlightVJ(root.fareOptions, countPax, serviceFee.Price);
            if (priceBreakDownFlightVJ != null)
            {
                result.FareDataId = fareId;
                result.BookingKey = priceBreakDownFlightVJ[0].BookingKey;
                result.FlightServiceSearch = FlightServiceSearch.VietjetAir;
                result.PriceDomestic = priceBreakDownFlightVJ[0].TotallPriceAdt;
                result.BgRow = string.Empty;
                result.PriceBreakDowns = priceBreakDownFlightVJ;
                result.TicketClassDomestic = priceBreakDownFlightVJ[0].FareClass;
                result.FlightRef = int.Parse(root.flights[0].flightNumber);
                result.ListSegment = GetListSegmentVJ(root.flights, priceBreakDownFlightVJ[0].FareClass);
                result.MainFlightNumber = "VJ" + root.flights[0].flightNumber;
                result.MainAirlineCode = "VJ";
                result.MainAirlineName = "VietJet Air";
                result.MainDepartureAirportCode = root.flights[0].departure.airport.code;
                result.MainDepartureAirportName = root.flights[0].departure.airport.name;
                var departureAirportRow = _iairportService.GetAirportByCode(root.flights[0].departure.airport.code);
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                result.MainDepartureTime = Convert.ToDateTime(root.flights[0].departure.scheduledTime).ToString("HH:mm");
                result.Plane = root.flights[0].aircraftModel.name;
                result.MainDepartureDate = Convert.ToDateTime(root.flights[0].departure.scheduledTime);
                result.MainArrivalAirportCode = root.flights[root.flights.Count - 1].arrival.airport.code;
                result.MainArrivalAirportName = root.flights[root.flights.Count - 1].arrival.airport.name;
                var arrivalAirportRow = _iairportService.GetAirportByCode(root.flights[root.flights.Count - 1].arrival.airport.code);
                result.MainArrivalCity = arrivalAirportRow.CityName;
                result.MainArrivalCountry = arrivalAirportRow.CountryName;
                result.MainArrivalTime = Convert.ToDateTime(root.flights[root.flights.Count - 1].arrival.scheduledTime).ToString("HH:mm");
                result.MainArrivalDate = Convert.ToDateTime(root.flights[root.flights.Count - 1].arrival.scheduledTime);

                var h = Convert.ToInt16(root.enRouteHours);
                var m = Convert.ToInt16((Convert.ToDecimal(root.enRouteHours) - h) * 60);
                result.Duration = h + "h" + m + "m"; ;
                result.TotalMinute = Convert.ToInt16(Convert.ToDecimal(root.enRouteHours) * 60);
                result.Stop = Convert.ToInt16(root.numberOfStops);
                if (wattype == 0)
                    result.WayType = WayType.OutBound;
                else
                    result.WayType = WayType.InBound;
                return result;
            }
            else
                return null;
        }
        public List<PriceBreakDownFlight> GetPriceBreakDownFlightVJ(List<FareOption> lFareot, int countPax, decimal serviceFee)
        {
            var result = new List<PriceBreakDownFlight>();
            foreach (var f in lFareot)
            {
                if (f.fareValidity.valid && !f.fareValidity.soldOut && !f.fareValidity.noFare)
                {
                    if (f.availability >= countPax)
                    {
                        var price = new PriceBreakDownFlight();
                        price.BookingKey = f.bookingKey;
                        price.FareClass = f.fareClass.description;
                        price.FareBasis = f.fareClass.code;
                        price.ClassName = f.bookingCode.description;
                        price.CabinClass = f.cabinClass.description;
                        price.SeatAvailablity = f.availability;
                        price.TotallPriceAdt = 0;
                        price.DiscountAdt = 0;
                        price.TaxAdt = 0;
                        price.FareBaseAdt = 0;
                        price.FeeAdt = 0;
                        price.TotallPriceChd = 0;
                        price.DiscountChd = 0;
                        price.TaxChd = 0;
                        price.FareBaseChd = 0;
                        price.FeeChd = 0;
                        price.TotallPriceInf = 0;
                        price.DiscountInf = 0;
                        price.TaxInf = 0;
                        price.FareBaseInf = 0;
                        price.FeeInf = 0;
                        foreach (var faOp in f.fareCharges)
                        {
                            if (faOp.chargeType.code.Equals("FA"))
                            {
                                if (faOp.passengerApplicability.adult)
                                {
                                    price.TotallPriceAdt = Convert.ToDecimal(faOp.currencyAmounts[0].totalAmount) + serviceFee;
                                    price.DiscountAdt = Convert.ToDecimal(faOp.currencyAmounts[0].discountAmount);
                                    price.TaxAdt = Convert.ToDecimal(faOp.currencyAmounts[0].taxAmount);
                                    price.FareBaseAdt = Convert.ToDecimal(faOp.currencyAmounts[0].baseAmount);
                                    price.FeeAdt = serviceFee;
                                }
                                if (faOp.passengerApplicability.child)
                                {
                                    price.TotallPriceChd = Convert.ToDecimal(faOp.currencyAmounts[0].totalAmount) + serviceFee;
                                    price.DiscountChd = Convert.ToDecimal(faOp.currencyAmounts[0].discountAmount);
                                    price.TaxChd = Convert.ToDecimal(faOp.currencyAmounts[0].taxAmount);
                                    price.FareBaseChd = Convert.ToDecimal(faOp.currencyAmounts[0].baseAmount);
                                    price.FeeChd = serviceFee;
                                }
                                if (faOp.passengerApplicability.infant)
                                {
                                    price.TotallPriceInf = Convert.ToDecimal(faOp.currencyAmounts[0].totalAmount) + serviceFee;
                                    price.DiscountInf = Convert.ToDecimal(faOp.currencyAmounts[0].discountAmount);
                                    price.TaxInf = Convert.ToDecimal(faOp.currencyAmounts[0].taxAmount);
                                    price.FareBaseInf = Convert.ToDecimal(faOp.currencyAmounts[0].baseAmount);
                                    price.FeeInf = serviceFee;
                                }
                            }
                        }
                        switch (f.fareClass.code.Split('_')[1])
                        {
                            case "ECO":
                                price.RecommendationNumber = "7 Kg hành lý xách tay";
                                price.AllowanceBaggage = "0 Kg hành lý ký gửi";
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "<li><b>Bao gồm:</b></li>";
                                price.Condition += "<li> 7 Kg hành lý xách tay</li>";
                                price.Condition += "<li><b>Chưa bao gồm:</b></li>";
                                price.Condition += "<li> Hành lý ký gửi(tùy chọn)</li>";
                                price.Condition += "<li> Chọn trước chỗ ngồi</li>";
                                price.Condition += "<li> Phí thay đổi chuyến bay, chặng bay, ngày bay</li>";
                                price.Condition += "<li> Chênh lệch tiền vé khi thay đổi(nếu có)</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Không hoàn hủy";
                                break;
                            case "DLX":
                                price.RecommendationNumber = "7 Kg hành lý xách tay";
                                price.AllowanceBaggage = "20 Kg hành lý ký gửi";
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "<li>7 Kg hành lý xách tay</li>";
                                price.Condition += "<li>20 Kg hành lý ký gửi</li>";
                                price.Condition += "<li>Ưu tiên làm thủ tục check -in</li>";
                                price.Condition += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                                price.Condition += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                                price.Condition += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "SBoss":
                                price.RecommendationNumber = "7 Kg hành lý xách tay";
                                price.AllowanceBaggage = "20 Kg hành lý ký gửi";
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "<li>7 Kg hành lý xách tay</li>";
                                price.Condition += "<li>20 Kg hành lý ký gửi</li>";
                                price.Condition += "<li>Ưu tiên làm thủ tục check -in</li>";
                                price.Condition += "<li>Ưu tiên chọn chỗ ngồi yêu thích (không áp dụng các hàng ghế dành cho Skyboss)</li>";
                                price.Condition += "<li>Miễn phí thay đổi chuyến bay, chặng bay, ngày bay (thu chênh lệch tiền vé nếu có)</li>";
                                price.Condition += "<li>Bảo hiểm Deluxe_Flight Care (chưa áp dụng cho các chuyến bay do Thai Vietjet khai thác)</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            default:
                                price.RecommendationNumber = "0 Kg hành lý xách tay";
                                price.AllowanceBaggage = "0 Kg hành lý ký gửi";
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "<li>0 Kg hành lý xách tay</li>";
                                price.Condition += "<li>0 Kg hành lý ký gửi</li>";
                                price.Condition += "<li>Không được thay đổi chuyến bay, chặng bay, ngày bay</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Không hoàn hủy";
                                break;
                        }
                        result.Add(price);
                    }
                }
            }
            return result;
        }
        public List<Segment> GetListSegmentVJ(List<Flight> flights, string ticketClass)
        {

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
                var departureAirportRow = _iairportService.GetAirportByCode(f.departure.airport.code);
                s.DepartureAirportCode = f.departure.airport.code;
                s.DepartureAirportName = f.departure.airport.name;
                s.DepartureTerminal = string.Empty;
                s.DepartureDate = departureTime;
                s.DepartureTime = departureTime.ToString("HH:mm");
                s.DepartureCity = departureAirportRow.CityName;
                s.DepartureCountry = departureAirportRow.CountryName;
                s.ArrivalAirportCode = f.arrival.airport.code;
                var arrivalAirportRow = _iairportService.GetAirportByCode(f.arrival.airport.code);
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
        public RootVietJets GetAirlineResponseRowVJ(string jsonContent, string bookingKey)
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
