using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.service.BindApiFlight.RootFlight;
using TicketProtechGroup.service.DbService;

namespace TicketProtechGroup.service.BindApiFlight
{

        public class BamBooAirWays
        {


            public static List<GroupFlight> GetGroupFlightBamBoo(TripInfo tripInfo, int waytype, string keyroot)
            {
            var airportRepository = new AirportRepository();


            List<GroupFlight> resultList = new List<GroupFlight>();
            List<BookingClass> classBamBooList = GetAvailableBookingClasses(tripInfo.booking_class);
            if (classBamBooList != null)
            {
                foreach (var classBamBoo in classBamBooList)
                {
                    GroupFlight result = new GroupFlight();
                    result.FareDataId = tripInfo.flight_segment_group_id;
                    result.BookingKey = classBamBoo.group_fare[0].fare_class + "-" + classBamBoo.group_fare[0].fare_basis + "-" + keyroot;
                    result.FlightServiceSearch = FlightServiceSearch.BamBooAirWays;
                    result.Discount = classBamBoo.pricing.pax_pricing_info[0].discount.amount;
                    result.PriceDomestic = classBamBoo.pricing.pax_pricing_info[0].display_fare.amount - result.Discount;
                    result.BgRow = string.Empty;
                    result.FareAdt = GetFareQH(classBamBoo, "ADULT");
                    result.TaxAdt = GetTaxFeeQH(classBamBoo, "ADULT");
                    result.FareChd = GetFareQH(classBamBoo, "CHILD");
                    result.TaxChd = GetTaxFeeQH(classBamBoo, "CHILD");
                    result.FareInf = GetFareQH(classBamBoo, "INFANT");
                    result.TaxInf = GetTaxFeeQH(classBamBoo, "INFANT");
                    result.FeeAdt = 0;
                    result.FeeChd = 0;
                    result.FeeInf = 0;
                    result.TicketClassDomestic = classBamBoo.pricing.fare_type;
                    result.FlightRef = int.Parse(tripInfo.segment_info[0].flight_info.flight_number);
                    result.ListChangBays = GetListChangBayQH(tripInfo.segment_info, classBamBoo.cabin_class);
                    result.MainFlightNumber = "QH" + tripInfo.segment_info[0].flight_info.flight_number;
                    result.MainAirlineCode = "QH";
                    result.MainAirlineName = "BamBooAirways";
                    result.MainDepartureAirportCode = tripInfo.segment_info[0].departure_info.airport_code;
                    var departureAirportRow = airportRepository.GetAirportByCode(tripInfo.segment_info[0].departure_info.airport_code);
                    result.MainDepartureAirportName = departureAirportRow.AirportNameVN;
                    result.MainDepartureCity = departureAirportRow.CityName;
                    result.MainDepartureCountry = departureAirportRow.CountryName;
                    result.MainDepartureTime = Convert.ToDateTime(tripInfo.segment_info[0].departure_info.datetime).ToString("HH:mm");
                    result.Plane = tripInfo.segment_info[0].aircraft_info.type;
                    result.MainDepartureDate = Convert.ToDateTime(tripInfo.segment_info[0].departure_info.datetime);
                    result.MainArrivalAirportCode = tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.airport_code;
                    var arrivalAirportRow = airportRepository.GetAirportByCode(tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.airport_code);
                    result.MainArrivalAirportName = arrivalAirportRow.AirportNameVN;
                    result.MainArrivalCity = arrivalAirportRow.CityName;
                    result.MainArrivalCountry = arrivalAirportRow.CountryName;
                    result.MainArrivalTime = Convert.ToDateTime(tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.datetime).ToString("HH:mm");
                    result.MainArrivalDate = Convert.ToDateTime(tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.datetime);
                    TimeSpan beweenTime = result.MainArrivalDate - result.MainDepartureDate;
                    double TotalMinute = beweenTime.TotalMinutes;
                    var h = Convert.ToInt16(TotalMinute / 60);
                    var m = Convert.ToInt16(TotalMinute - h * 60);
                    result.Duration = h + "h" + m + "m"; ;
                    result.TotalMinute = Convert.ToInt16(TotalMinute);
                    result.Stop = Convert.ToInt16(tripInfo.segment_info.Count - 1);
                    if (waytype == 0)
                        result.WayType = WayType.OutBound;
                    else
                        result.WayType = WayType.InBound;
                    switch (classBamBoo.pricing.fare_type)
                    {
                        case "EconomySaverMax":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "Hành lý ký gửi	Trả phí";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            result.Note += "    <li>Hành lý ký gửi	Trả phí</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)	Không áp dụng</li>";
                            result.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)Không áp dụng</li>";
                            result.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            result.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            result.Note += "</ul>";
                            result.ReturnTicket = "Không hoàn hủy";
                            break;
                        case "EconomySaver":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            result.Note += "    <li>Hành lý ký gửi 20 kg</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) 270,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            result.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            result.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            result.Note += "</ul>";
                            result.ReturnTicket = "Không hoàn hủy";
                            break;
                        case "EconomySmart":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            result.Note += "    <li>Hành lý ký gửi	20 kg</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) 270,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 550,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            result.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 550,000 VNĐ/người/chặng</li>";
                            result.Note += "</ul>";
                            result.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "EconomyFlex":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            result.Note += "    <li>Hành lý ký gửi	20 kg</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) Miễn phí + chênh lệch (nếu có)</li>";
                            result.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)	Miễn phí + chênh lệch (nếu có)</li>";
                            result.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "   	<li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 350,000 VNĐ/người/chặng</li>";
                            result.Note += "</ul>";
                            result.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "PremiumSmart":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "30 Kg hành lý ký gửi";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note = "    <li>Hành lý xách tay 7 kg</li>";
                            result.Note = "    <li>Hành lý ký gửi	30 kg</li>";
                            result.Note = "    <li>Thay đổi chuyến bay/ hành trình 270,000 VNĐ/người/chặng + chênh lệch(Trước giờ khởi hành tối thiểu 03 tiếng) (nếu có)</li>";
                            result.Note = "    <li>Thay đổi chuyến bay/ hành trình 270,000 VNĐ/người/chặng + chênh lệch(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) (nếu có)</li>";
                            result.Note = "    <li>Đổi tên 350,000 VNĐ/người/chặng(Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)</li>";
                            result.Note = "    <li>Hoàn vé 350,000 VNĐ/người/chặng(Trước giờ khởi hành tối thiểu 03 tiếng)</li>";
                            result.Note = "    <li>Hoàn vé 350,000 VNĐ/người/chặng(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            result.Note = "</ul>";
                            result.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "PremiumFlex":
                            result.RecommendationNumber = "7 Kg hành lý xách tay";
                            result.AllowanceBaggage = "30 Kg hành lý ký gửi";
                            result.Note = "<ul class=\"none-style\">";
                            result.Note = "     <li>Hành lý xách tay 7 kg</li>";
                            result.Note = "     <li>Hành lý ký gửi	30 kg</li>";
                            result.Note = "     <li>Thay đổi chuyến bay/ hành trình (Miễn phí + chênh lệch(Trước giờ khởi hành tối thiểu 03 tiếng) (nếu có)</li>";
                            result.Note = "     <li>Thay đổi chuyến bay/ hành trình (Miễn phí + chênh lệch (nếu có)(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            result.Note = "     <li>Đổi tên 350,000 VNĐ/người/chặng (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)</li>";
                            result.Note = "     <li>Hoàn vé	350,000 VNĐ/người/chặng (Trước giờ khởi hành tối thiểu 03 tiếng)</li>";
                            result.Note = "     <li>Hoàn vé	350,000 VNĐ/người/chặng (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            result.Note = " </ul>";
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
                    resultList.Add(result);
                }
                return resultList;
            }
            else
                    return null;
            }
            private static List<Segment> GetListChangBayQH(List<SegmentInfo> segment_info, string ticketClass)
            {
            var airportRepository = new AirportRepository(); // khởi tạo tạm thời

            List<Segment> result = new List<Segment>();
                segment_info.ForEach(f => {
                    Segment s = new Segment();
                    s.FlightNumber = f.flight_info.flight_number;
                    s.AirlineCode = f.flight_info.carrier_code;
                    s.AirlineName = "BamBooAirways";
                    DateTime departureTime = Convert.ToDateTime(f.departure_info.datetime);
                    DateTime arrivalTime = Convert.ToDateTime(f.arrival_info.datetime);
                    TimeSpan beweenTime = arrivalTime - departureTime;
                    double TotalMinute = beweenTime.TotalMinutes;
                    var h = Convert.ToInt16(TotalMinute / 60);
                    var m = Convert.ToInt16(TotalMinute - h * 60);
                    s.Duration = h + "h" + m + "m";
                    s.OperatingAirlineCode = "QH";
                    s.OperatingAirlineName = "BamBooAirways";
                    var departureAirportRow = airportRepository.GetAirportByCode(f.departure_info.airport_code);
                    s.DepartureAirportCode = f.departure_info.airport_code;
                    s.DepartureAirportName = departureAirportRow.AirportNameVN;
                    s.DepartureTerminal = string.Empty;
                    s.DepartureDate = departureTime;
                    s.DepartureTime = departureTime.ToString("HH:mm");
                    s.DepartureCity = departureAirportRow.CityName;
                    s.DepartureCountry = departureAirportRow.CountryName;
                    s.ArrivalAirportCode = f.arrival_info.airport_code;
                    var arrivalAirportRow = airportRepository.GetAirportByCode(f.arrival_info.airport_code);
                    s.ArrivalAirportName = arrivalAirportRow.AirportNameVN;
                    s.ArrivalTerminal = string.Empty;
                    s.ArrivalDate = arrivalTime;
                    s.ArrivalTime = arrivalTime.ToString("HH:mm");
                    s.ArrivalCity = arrivalAirportRow.CityName;
                    s.ArrivalCountry = arrivalAirportRow.CountryName;
                    s.TicketClass = ticketClass;
                    s.AircraftCode = f.aircraft_info.type;
                    s.AircraftName = "A" + f.aircraft_info.type;
                    s.SeatRemain = 0;
                    s.SegmentStop = segment_info.Count.ToString();
                    result.Add(s);
                });
                return result;
            }

            private static decimal GetFareQH(BookingClass classBamBoo, string paxtype)
            {
                decimal result = 0;
                foreach (var pr in classBamBoo.pricing.pax_pricing_info)
                {
                    if (pr.pax_type.Equals(paxtype))
                        result = pr.base_fare.amount;
                }
                return result;
            }
            private static decimal GetTaxFeeQH(BookingClass classBamBoo, string paxtype)
            {
                decimal result = 0;
                foreach (var pr in classBamBoo.pricing.pax_pricing_info)
                {
                    if (pr.pax_type.Equals(paxtype))
                        result = pr.tax.amount;
                }
                return result;
            }
        private static List<BookingClass> GetAvailableBookingClasses(List<BookingClass> bookingclass)
        {
            List<BookingClass> availableBookingClasses = new List<BookingClass>();

            foreach (var b in bookingclass)
            {
                if (b.seat_availablity > 0)
                {
                    availableBookingClasses.Add(b);
                }
            }

            return availableBookingClasses;
        }

    }

}
