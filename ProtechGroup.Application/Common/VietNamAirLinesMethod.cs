using ProtechGroup.Application.Interfaces;
using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace ProtechGroup.Application.Common
{
    public class VietNamAirLinesMethod : IVietNamAirLinesMethod
    {
        private readonly IAirportService _iairportService;
        private readonly IServiceFeeService _iserviceFeeService;
        public VietNamAirLinesMethod(IAirportService iairportService, IServiceFeeService iserviceFeeService)
        {
            _iairportService = iairportService;
            _iserviceFeeService = iserviceFeeService;
        }
        public FlightResultOutput BuildFlightResultVietNamAirLines(RootVNA alineVNA, int countPax, bool isDomestric)
        {
            var flightResultOutput = new FlightResultOutput();
            flightResultOutput.IsFlightDomestic = true;
            flightResultOutput.BlockItems = new List<BlockItem>();
            flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
            var airline = new FlightResultOutput.Airline();
            airline.AirlineName = "VietNamAirLines";
            airline.AirlineCode = "VN";
            flightResultOutput.Airlines.Add(airline);
            BlockItem blockItem = new BlockItem();
            blockItem.FlightOutBounds = new List<GroupFlight>();
            blockItem.FlightInBounds = new List<GroupFlight>();
            if (alineVNA != null)
            {
                if (alineVNA.ListGroup != null && alineVNA.ListGroup.Count > 0)
                {
                    foreach (var avn in alineVNA.ListGroup[0].ListAirOption)
                    {
                        var grvna = GetGroupFlightVietNamAirLines(avn, -1, 0, alineVNA.Session, isDomestric, countPax);
                        if (grvna != null)
                            blockItem.FlightOutBounds.Add(grvna);
                    }
                    if (alineVNA.ListGroup.Count > 1)
                    {
                        foreach (var avn in alineVNA.ListGroup[1].ListAirOption)
                        {
                            var grvna = GetGroupFlightVietNamAirLines(avn, -1, 1, alineVNA.Session, isDomestric, countPax);
                            if (grvna != null)
                                blockItem.FlightInBounds.Add(grvna);
                        }
                        blockItem.IsRoundTrip = true;
                    }
                }

            }
            flightResultOutput.BlockItems.Add(blockItem);
            return flightResultOutput;
        }
        public GroupFlight GetGroupFlightVietNamAirLines(ListAirOptionVNA airOption, int FareId, int waytype, 
                                                        string sesionId, bool isDomestric, int countPax)
        {
            
            GroupFlight result = new GroupFlight();
            string mDepTime = airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(0, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(2, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(4, 4) + " " + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(9, 2) + ":" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(11, 2) + ":00";
            string mArrTime = airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(0, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(2, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(4, 4) + " " + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(9, 2) + ":" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(11, 2) + ":00";
            double timecheck = CoreUtils.GetHourDifference(mDepTime);
            int beforday = 0;
            if (timecheck <= 24)
            {
                beforday = 1;
            }
            var serviceFee = _iserviceFeeService.GetServiceFeeByAgBfdDo(isDomestric, 0, beforday);
            var priceBreakDownFlightVN = GetPriceBreakDownFlightVN(airOption.ListFareOption, serviceFee.Price, countPax);
            if (priceBreakDownFlightVN != null)
            {
                result.FareDataId = airOption.OptionId;
                result.BookingKey = sesionId + "_" + airOption.ListFlightOption[0].OptionId;
                result.FlightServiceSearch = FlightServiceSearch.VietnamAirline;
                result.BgRow = string.Empty;
                result.PriceBreakDowns = priceBreakDownFlightVN;
               result.TicketClassDomestic = string.Empty;
                result.FlightRef = int.Parse(airOption.ListFlightOption[0].ListFlight[0].FlightNumber);
                var listChangbay = GetListChangBayVN(airOption.ListFlightOption[0].ListFlight[0].ListSegment, priceBreakDownFlightVN[0].ClassName);
                result.ListSegment = listChangbay;
                result.MainFlightNumber = "VN" + airOption.ListFlightOption[0].ListFlight[0].FlightNumber;
                result.MainAirlineCode = airOption.ListFlightOption[0].ListFlight[0].Operator;
                if (result.MainAirlineCode.Equals("BL"))
                    result.MainAirlineName = "Pacific Airlines";
                else
                    result.MainAirlineName = "Vietnam Airlines";
                result.MainDepartureAirportCode = airOption.ListFlightOption[0].ListFlight[0].StartPoint;
                var departureAirportRow = _iairportService.GetAirportByCode(airOption.ListFlightOption[0].ListFlight[0].StartPoint);
                result.MainDepartureAirportName = departureAirportRow.AirportNameVN;
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                
                result.MainDepartureTime = Convert.ToDateTime(mDepTime).ToString("HH:mm");
                result.Plane = listChangbay[0].AircraftName;
                result.MainDepartureDate = Convert.ToDateTime(mDepTime);
                result.MainArrivalAirportCode = airOption.ListFlightOption[0].ListFlight[0].EndPoint;
                var arrivalAirportRow = _iairportService.GetAirportByCode(airOption.ListFlightOption[0].ListFlight[0].EndPoint);
                result.MainArrivalAirportName = arrivalAirportRow.AirportNameVN;
                result.MainArrivalCity = arrivalAirportRow.CityName;
                result.MainArrivalCountry = arrivalAirportRow.CountryName;
                result.MainArrivalTime = Convert.ToDateTime(mArrTime).ToString("HH:mm");
                result.MainArrivalDate = Convert.ToDateTime(mArrTime);

                TimeSpan beweenTime = result.MainArrivalDate - result.MainDepartureDate;
                double TotalMinute = beweenTime.TotalMinutes;
                var h = Convert.ToInt16(TotalMinute / 60);
                var m = Convert.ToInt16(TotalMinute - h * 60);
                result.Duration = h + "h" + m + "m"; ;
                result.TotalMinute = Convert.ToInt16(TotalMinute);
                result.Stop = Convert.ToInt16(airOption.ListFlightOption[0].ListFlight[0].ListSegment.Count - 1);
                if (waytype == 0)
                    result.WayType = WayType.OutBound;
                else
                    result.WayType = WayType.InBound;
               
            }
            return result;
        }
        public List<PriceBreakDownFlight> GetPriceBreakDownFlightVN(List<ListFareOptionVNA> fareOpt, decimal serviceFee,
                                                                    int countPax)

        {
            var result = new List<PriceBreakDownFlight>();
            foreach (var fare in fareOpt)
            {
                if (countPax <= fare.Availability)
                {
                    var price = new PriceBreakDownFlight();
                    price.BookingKey = fare.OptionId.ToString();
                    price.FareClass = fare.FareClass;
                    price.FareBasis = fare.ListFarePax[0].ListFareInfo[0].FareBasis;
                    price.ClassName = fare.ListFarePax[0].ListFareInfo[0].FareFamily;
                    price.CabinClass = fare.ListFarePax[0].ListFareInfo[0].CabinName;
                    price.SeatAvailablity = fare.Availability;
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
                    foreach(var farePax in fare.ListFarePax)
                    {
                        if (farePax.PaxType.Equals("ADT"))
                        {
                            price.TotallPriceAdt = Convert.ToDecimal(farePax.TotalFare) + serviceFee;
                            price.DiscountAdt = 0;
                            price.TaxAdt = Convert.ToDecimal(farePax.Taxes);
                            price.FareBaseAdt = Convert.ToDecimal(farePax.BaseFare);
                            price.FeeAdt = serviceFee;
                        }
                        if (farePax.PaxType.Equals("CHD"))
                        {
                            price.TotallPriceChd = Convert.ToDecimal(farePax.TotalFare) + serviceFee;
                            price.DiscountChd = 0;
                            price.TaxChd = Convert.ToDecimal(farePax.Taxes);
                            price.FareBaseChd = Convert.ToDecimal(farePax.BaseFare);
                            price.FeeChd = serviceFee;
                        }
                        if (farePax.PaxType.Equals("INF"))
                        {
                            price.TotallPriceInf = Convert.ToDecimal(farePax.TotalFare) + serviceFee;
                            price.DiscountInf = 0;
                            price.TaxInf = Convert.ToDecimal(farePax.Taxes);
                            price.FareBaseInf = Convert.ToDecimal(farePax.BaseFare);
                            price.FeeInf = serviceFee;
                        }
                        try
                        {
                            if (farePax.ListFareInfo[0].HandBaggage != null)
                                price.RecommendationNumber = "Hành lý xách tay Không quá " + farePax.ListFareInfo[0].HandBaggage.Split('x')[1];
                            else
                                price.RecommendationNumber = "Không bao gồm hành lý xách tay";
                        }
                        catch
                        {
                            price.RecommendationNumber = "Không bao gồm hành lý sách tay";
                        }
                        try
                        {
                            if (farePax.ListFareInfo[0].FreeBaggage.Equals("0kg"))
                                price.AllowanceBaggage = "Không bao gồm hành lý ký gửi";
                            else
                                price.AllowanceBaggage = $"Có 01 kiện hành lý ký gửi ({farePax.ListFareInfo[0].FreeBaggage.Split('x')[1]})";
                        }
                        catch
                        {
                            price.AllowanceBaggage = farePax.ListFareInfo[0].FreeBaggage;
                        }
                        try
                        {
                            if (farePax.ListFareInfo[0].HandBaggage != null)
                                price.RecommendationNumber = "Hành lý xách tay Không quá " + farePax.ListFareInfo[0].HandBaggage.Split('x')[1];
                            else
                                price.RecommendationNumber = "Không bao gồm hành lý xách tay";
                        }
                        catch
                        {
                            price.RecommendationNumber = "Không bao gồm hành lý xách tay";
                        }
                        try
                        {
                            if (farePax.ListFareInfo[0].FreeBaggage.Equals("0kg"))
                                price.AllowanceBaggage = "Không bao gồm hành lý ký gửi";
                            else
                                price.AllowanceBaggage = $"Có 01 kiện hành lý ký gửi ({farePax.ListFareInfo[0].FreeBaggage.Split('x')[1]})";
                        }
                        catch
                        {
                            price.AllowanceBaggage = farePax.ListFareInfo[0].FreeBaggage;
                        }
                        switch (farePax.ListFareInfo[0].FareFamily)
                        {
                            case "Economy Super Lite":
                                price.RecommendationNumber = "7 Kg hành lý xách tay";
                                price.AllowanceBaggage = "Hành lý ký gửi Trả phí";
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                price.Condition += "    <li>Không bao gồm hành lý ký gửi</li>";
                                price.Condition += "    <li>Không được phép thay đổi</li>";
                                price.Condition += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                price.Condition += "    <li>Không được phép hoàn/hủy vé</li>";
                                price.Condition += "    <li>Tích lũy 10% số dặm</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Không hoàn hủy";
                                break;
                            case "Economy Lite":
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                price.Condition += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                price.Condition += "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                price.Condition += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                price.Condition += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                price.Condition += "    <li>Tích lũy 60% số dặm</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Được phép Hoàn/hủy vé mất phí";
                                break;
                            case "Economy Classic":
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                price.Condition += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                price.Condition += "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                price.Condition += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                price.Condition += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                price.Condition += "    <li>Tích lũy 80% số dặm</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Economy Flex":
                                price.Condition = "<ul class=\"none-style\">";
                                price.Condition += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                price.Condition += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                price.Condition += "    <li>Được phép Thay đổi miễn phí + Chênh lệch giá vé (nếu có)</li>";
                                price.Condition += "    <li>Được sử dụng quầy thủ tục ưu tiên";
                                price.Condition += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                price.Condition += "    <li>Tích lũy 100% số dặm</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Business Classic":
                                price.Condition += "<ul class=\"none-style\">";
                                price.Condition += "    <li>Hành lý xách tay: Không quá 18kg (2 kiện, mỗi kiện 9kg)</li>";
                                price.Condition += "    <li>Có 01 kiện hành lý ký gửi (32kg)</li>";
                                price.Condition += "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                price.Condition += "    <li>Được phép Đổi chuyến tại sân bay mất phí</li>";
                                price.Condition += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                price.Condition += "    <li>Tích lũy 150% số dặm</li>";
                                price.Condition += "</ul>";
                                price.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Business Flex":
                                price.Condition += "<ul class=\"none-style\">";
                                price.Condition += "     <li>Hành lý xách tay: Không quá 18kg (2 kiện, mỗi kiện 9kg)</li>";
                                price.Condition += "     <li>Có 01 kiện hành lý ký gửi (32kg)</li>";
                                price.Condition += "     <li>Được phép Thay đổi miễn phí + Chênh lệch giá vé (nếu có)</li>";
                                price.Condition += "     <li>Được phép Đổi chuyến tại sân bay mất phí</li>";
                                price.Condition = "     <li>Được phép Hoàn/hủy vé mất phí</li>";
                                price.Condition += "     <li>Tích lũy 200% số dặm</li>";
                                price.Condition += " </ul>";
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
        public List<Segment> GetListChangBayVN(List<ListSegmentVNA> listSegment, string className)
        {
            var result = new List<Segment>();
            foreach (var seg in listSegment)
            {

                Segment s = new Segment();
                s.FlightNumber = seg.FlightNumber;

                s.AirlineCode = seg.Airline;
                s.AirlineName = "Vietnam Airlines";
                string depTime = seg.DepartDate.Substring(0, 2) + "/" + seg.DepartDate.Substring(2, 2) + "/" + seg.DepartDate.Substring(4, 4) + " " + seg.DepartDate.Substring(9, 2) + ":" + seg.DepartDate.Substring(11, 2) + ":00";
                string arrTime = seg.ArriveDate.Substring(0, 2) + "/" + seg.ArriveDate.Substring(2, 2) + "/" + seg.ArriveDate.Substring(4, 4) + " " + seg.ArriveDate.Substring(9, 2) + ":" + seg.ArriveDate.Substring(11, 2) + ":00";

                DateTime departureTime = Convert.ToDateTime(depTime);
                DateTime arrivalTime = Convert.ToDateTime(arrTime);
                double TotalMinute = Convert.ToDouble(seg.Duration);
                var h = Convert.ToInt16(TotalMinute / 60);
                var m = Convert.ToInt16(TotalMinute - h * 60);
                s.Duration = h + "h" + m + "m";
                s.OperatingAirlineCode = seg.Operator;
                if (seg.Operator.Equals("BL"))
                    s.OperatingAirlineName = "Pacific Airlines";
                else
                    s.OperatingAirlineName = "Vietnam Airlines";
                var departureAirportRow = _iairportService.GetAirportByCode(seg.StartPoint);
                s.DepartureAirportCode = seg.StartPoint;
                s.DepartureAirportName = departureAirportRow.AirportNameVN;
                s.DepartureTerminal = string.Empty;
                s.DepartureDate = departureTime;
                s.DepartureTime = departureTime.ToString("HH:mm");
                s.DepartureCity = departureAirportRow.CityName;
                s.DepartureCountry = departureAirportRow.CountryName;
                s.ArrivalAirportCode = seg.EndPoint;
                var arrivalAirportRow = _iairportService.GetAirportByCode(seg.EndPoint);
                s.ArrivalAirportName = arrivalAirportRow.AirportNameVN;
                s.ArrivalTerminal = string.Empty;
                s.ArrivalDate = arrivalTime;
                s.ArrivalTime = arrivalTime.ToString("HH:mm");
                s.ArrivalCity = arrivalAirportRow.CityName;
                s.ArrivalCountry = arrivalAirportRow.CountryName;
                s.TicketClass = className;
                s.AircraftCode = seg.Equipment;
                s.AircraftName = "A" + seg.Equipment;
                s.SeatRemain = 0;
                s.SegmentStop = seg.Equipment.ToString();
                result.Add(s);
            }
            return result;
        }
    }
}
