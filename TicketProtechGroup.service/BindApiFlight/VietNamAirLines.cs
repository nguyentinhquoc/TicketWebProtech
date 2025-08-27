using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.service.BindApiFlight.RootFlight;
using TicketProtechGroup.service.DbService;

namespace TicketProtechGroup.service.BindApiFlight
{
    public class VietNamAirLines
    {
        internal static FlightResultOutput BuildRootVietNameAirline(RootVietNameAirline alineVNA, int CountPax)
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
                        var grvna = VietNamAirLines.GetGroupFlightVietNamAirLines(avn, -1, 0, alineVNA.Session);
                        if (grvna != null)
                            blockItem.FlightOutBounds.Add(grvna);
                    }
                    if (alineVNA.ListGroup.Count > 1)
                    {
                        foreach (var avn in alineVNA.ListGroup[1].ListAirOption)
                        {
                            var grvna = VietNamAirLines.GetGroupFlightVietNamAirLines(avn, -1, 1, alineVNA.Session);
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

        public static GroupFlight GetGroupFlightVietNamAirLines(ListAirOption airOption, int FareId, int waytype, string sesionId)
        {
            var airportRepository = new AirportRepository();
            List<ListHangVe> listHangVes = new List<ListHangVe>();

            List<ListFareOption> fareOptionVNAs = GetListFareOptionVNA(airOption.ListFareOption, FareId);
            GroupFlight result = new GroupFlight();
            if (fareOptionVNAs != null)
            {
                result.FareDataId = airOption.OptionId;
                result.FlightServiceSearch = FlightServiceSearch.VietnamAirline;
                result.Discount = 0;
                result.BgRow = string.Empty;
                var listChangbay = GetListChangBayVN(airOption.ListFlightOption[0].ListFlight[0].ListSegment);
                result.MainFlightNumber = "VN" + airOption.ListFlightOption[0].ListFlight[0].FlightNumber;
                result.MainAirlineCode = airOption.ListFlightOption[0].ListFlight[0].Operator;
                if (result.MainAirlineCode.Equals("BL"))
                    result.MainAirlineName = "Pacific Airlines";
                else
                    result.MainAirlineName = "Vietnam Airlines";
                result.MainDepartureAirportCode = airOption.ListFlightOption[0].ListFlight[0].StartPoint;
                var departureAirportRow = airportRepository.GetAirportByCode(airOption.ListFlightOption[0].ListFlight[0].StartPoint);
                result.MainDepartureAirportName = departureAirportRow.AirportNameVN;
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                string mDepTime = airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(0, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(2, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(4, 4) + " " + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(9, 2) + ":" + airOption.ListFlightOption[0].ListFlight[0].DepartDate.Substring(11, 2) + ":00";
                string mArrTime = airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(0, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(2, 2) + "/" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(4, 4) + " " + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(9, 2) + ":" + airOption.ListFlightOption[0].ListFlight[0].ArriveDate.Substring(11, 2) + ":00";
                result.MainDepartureTime = Convert.ToDateTime(mDepTime).ToString("HH:mm");
                result.Plane = listChangbay[0].AircraftName;
                result.MainDepartureDate = Convert.ToDateTime(mDepTime);
                result.MainArrivalAirportCode = airOption.ListFlightOption[0].ListFlight[0].EndPoint;
                var arrivalAirportRow = airportRepository.GetAirportByCode(airOption.ListFlightOption[0].ListFlight[0].EndPoint);
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



                foreach (var fareOption in fareOptionVNAs)
                {
                    if (fareOption.FareClass != null)
                    {
                        var listHangVe = new ListHangVe();


                        listHangVe.RecommendationNumber = "Hành lý xách tay Không quá " + fareOption.ListFarePax[0].ListFareInfo[0].HandBaggage.Split('x')[1];
                        if (fareOption.ListFarePax[0].ListFareInfo[0].FreeBaggage.Equals("0kg"))
                            listHangVe.AllowanceBaggage = "Không bao gồm hành lý ký gửi";
                        else
                            listHangVe.AllowanceBaggage = $"Có 01 kiện hành lý ký gửi ({fareOption.ListFarePax[0].ListFareInfo[0].FreeBaggage.Split('x')[1]})";

                        listHangVe.ListChangBays = listChangbay;
                        listHangVe.BookingKey = sesionId + "_" + airOption.OptionId + "_" + airOption.ListFlightOption[0].OptionId;
                        listHangVe.FareAdt = GetFareVN(fareOption.ListFarePax, "ADT");
                        listHangVe.PriceDomestic = fareOption.TotalPrice;
                        listHangVe.TaxAdt = GetTaxFeeVN(fareOption.ListFarePax, "ADT");
                        listHangVe.FareChd = GetFareVN(fareOption.ListFarePax, "CHD");
                        listHangVe.TaxChd = GetTaxFeeVN(fareOption.ListFarePax, "CHD");
                        listHangVe.FareInf = GetFareVN(fareOption.ListFarePax, "INF");
                        listHangVe.TaxInf = GetTaxFeeVN(fareOption.ListFarePax, "INF");
                        listHangVe.FeeAdt = 0;
                        listHangVe.FeeChd = 0;
                        listHangVe.FeeInf = 0;
                        listHangVe.TicketClassDomestic = fareOption.ListFarePax[0].ListFareInfo[0].FareFamily;
                        listHangVe.FlightRef = int.Parse(airOption.ListFlightOption[0].ListFlight[0].FlightNumber);
                        switch (fareOption.ListFarePax[0].ListFareInfo[0].FareFamily)
                        {
                            case "Economy Super Lite":
                                listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                                listHangVe.AllowanceBaggage = "Hành lý ký gửi	Trả phí";
                                listHangVe.Note += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                listHangVe.Note += "    <li>Không bao gồm hành lý ký gửi</li>";
                                listHangVe.Note += "    <li>Không được phép thay đổi</li>";
                                listHangVe.Note += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                listHangVe.Note += "    <li>Không được phép hoàn/hủy vé</li>";
                                listHangVe.Note += "    <li>Tích lũy 10% số dặm</li>";
                                listHangVe.ReturnTicket = "Không hoàn hủy";
                                break;
                            case "Economy Lite":
                                listHangVe.Note += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                listHangVe.Note += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                listHangVe.Note += "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                listHangVe.Note += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                listHangVe.Note += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                listHangVe.Note += "    <li>Tích lũy 60% số dặm</li>";
                                listHangVe.ReturnTicket = "Được phép Hoàn/hủy vé mất phí";
                                break;
                            case "Economy Classic":
                                listHangVe.Note += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                listHangVe.Note += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                listHangVe.Note += "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                listHangVe.Note += "    <li>Không bao gồm quầy thủ tục ưu tiên</li>";
                                listHangVe.Note += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                listHangVe.Note += "    <li>Tích lũy 80% số dặm</li>";
                                listHangVe.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Economy Flex":
                                listHangVe.Note += "    <li>Hành lý xách tay Không quá 12kg</li>";
                                listHangVe.Note += "    <li>Có 01 kiện hành lý ký gửi (23kg)</li>";
                                listHangVe.Note += "    <li>Được phép Thay đổi miễn phí + Chênh lệch giá vé (nếu có)</li>";
                                listHangVe.Note += "    <li>Được sử dụng quầy thủ tục ưu tiên";
                                listHangVe.Note += "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                listHangVe.Note += "    <li>Tích lũy 100% số dặm</li>";
                                listHangVe.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Business Classic":
                                listHangVe.Note = "    <li>Hành lý xách tay: Không quá 18kg (2 kiện, mỗi kiện 9kg)</li>";
                                listHangVe.Note = "    <li>Có 01 kiện hành lý ký gửi (32kg)</li>";
                                listHangVe.Note = "    <li>Được phép Thay đổi mất phí + Chênh lệch giá vé (nếu có)</li>";
                                listHangVe.Note = "    <li>Được phép Đổi chuyến tại sân bay mất phí</li>";
                                listHangVe.Note = "    <li>Được phép Hoàn/hủy vé mất phí</li>";
                                listHangVe.Note = "    <li>Tích lũy 150% số dặm</li>";
                                listHangVe.ReturnTicket = "Có thể hoàn hủy";
                                break;
                            case "Business Flex":
                                listHangVe.Note = "     <li>Hành lý xách tay: Không quá 18kg (2 kiện, mỗi kiện 9kg)</li>";
                                listHangVe.Note = "     <li>Có 01 kiện hành lý ký gửi (32kg)</li>";
                                listHangVe.Note = "     <li>Được phép Thay đổi miễn phí + Chênh lệch giá vé (nếu có)</li>";
                                listHangVe.Note = "     <li>Được phép Đổi chuyến tại sân bay mất phí</li>";
                                listHangVe.Note = "     <li>Được phép Hoàn/hủy vé mất phí</li>";
                                listHangVe.Note = "     <li>Tích lũy 200% số dặm</li>";
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
                    }
                }
            }
            return result;
        }

        private static List<Segment> GetListChangBayVN(List<ListSegment> listSegment)
        {
            var result = new List<Segment>();
            var airportRepository = new AirportRepository();

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
                var departureAirportRow = airportRepository.GetAirportByCode(seg.StartPoint);
                s.DepartureAirportCode = seg.StartPoint;
                s.DepartureAirportName = departureAirportRow.AirportNameVN;
                s.DepartureTerminal = string.Empty;
                s.DepartureDate = departureTime;
                s.DepartureTime = departureTime.ToString("HH:mm");
                s.DepartureCity = departureAirportRow.CityName;
                s.DepartureCountry = departureAirportRow.CountryName;
                s.ArrivalAirportCode = seg.EndPoint;
                var arrivalAirportRow = airportRepository.GetAirportByCode(seg.EndPoint);
                s.ArrivalAirportName = arrivalAirportRow.AirportNameVN;
                s.ArrivalTerminal = string.Empty;
                s.ArrivalDate = arrivalTime;
                s.ArrivalTime = arrivalTime.ToString("HH:mm");
                s.ArrivalCity = arrivalAirportRow.CityName;
                s.ArrivalCountry = arrivalAirportRow.CountryName;
                //s.TicketClass = className;
                s.AircraftCode = seg.Equipment;
                s.AircraftName = "A" + seg.Equipment;
                s.SeatRemain = 0;
                s.SegmentStop = seg.Equipment.ToString();
                result.Add(s);
            }
            return result;
        }
        //public static ListAirOptionVNA GetAirlineResponseRow(string jsonContent, int OptionId)
        //{
        //    ListAirOptionVNA result = new ListAirOptionVNA();
        //    var rootVNA = JsonConvert.DeserializeObject<RootVNA>(jsonContent);
        //    try
        //    {
        //        foreach (var listGroup in rootVNA.ListGroup)
        //        {
        //            foreach (var group in listGroup.ListAirOption)
        //            {
        //                if (group.OptionId == OptionId)
        //                    return group;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return new ListAirOptionVNA();
        //    }
        //    return result;
        //}
        private static List<ListFareOption> GetListFareOptionVNA(List<ListFareOption> listFareOption, int FareId)
        {
            try
            {
                if (FareId >= 0)
                {
                    // Trả về danh sách các phần tử có OptionId bằng FareId (thường chỉ có 1 phần tử)
                    return listFareOption.Where(x => x.OptionId == FareId).ToList();
                }
                else
                {
                    // Trả về tất cả các phần tử có khả năng chứa đủ số khách (Availability >= tổng số khách)
                    var result = new List<ListFareOption>();
                    foreach (var b in listFareOption)
                    {
                        int paxNum = b.ListFarePax.Sum(x => x.PaxNumb);
                        if (b.Availability >= paxNum)
                            result.Add(b);
                    }
                    return result;
                }
            }
            catch
            {
                return new List<ListFareOption>(); // Trả về mảng rỗng khi lỗi
            }
        }

        private static decimal GetTaxFeeVN(List<ListFarePax> farePax, string v)
        {
            decimal result = 0;
            var far = farePax.Where(x => x.PaxType == v).FirstOrDefault();
            if (far != null)
                result = far.Taxes;
            return result;
        }

        private static decimal GetFareVN(List<ListFarePax> farePax, string v)
        {

            decimal result = 0;
            var far = farePax.Where(x => x.PaxType == v).FirstOrDefault();
            if (far != null)
                result = far.BaseFare;
            return result;
        }
    }

}
