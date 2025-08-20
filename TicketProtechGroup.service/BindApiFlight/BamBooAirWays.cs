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
        internal static FlightResultOutput Build(RootBamboo alineBB, int countPax)
        {
            var flightResultOutput = new FlightResultOutput();
            flightResultOutput.IsFlightDomestic = true;
            flightResultOutput.BlockItems = new List<BlockItem>();
            flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
            var airline = new FlightResultOutput.Airline();
            airline.AirlineName = "BamBooAirways";
            airline.AirlineCode = "QH";

            flightResultOutput.Airlines.Add(airline);
            BlockItem blockItem = new BlockItem();
            blockItem.FlightOutBounds = new List<GroupFlight>();
            blockItem.FlightInBounds = new List<GroupFlight>();
            if (alineBB != null && alineBB.data.Count > 0)
            {
                for (int i = 0; i < alineBB.data[0].trip_info.Count; i++)
                {
                    var gf = BamBooAirWays.GetGroupFlightBamBoo(alineBB.data[0].trip_info[i], 0, alineBB.id, countPax);
                    if (gf != null)
                        blockItem.FlightOutBounds.Add(gf);
                }
                if (alineBB.data.Count > 1)
                {
                    for (int i = 0; i < alineBB.data[1].trip_info.Count; i++)
                    {
                        var gf = BamBooAirWays.GetGroupFlightBamBoo(alineBB.data[1].trip_info[i], 1, alineBB.id, countPax);
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


        public static GroupFlight GetGroupFlightBamBoo(TripInfo tripInfo, int waytype, string keyroot, int countPax)
        {
            var airportRepository = new AirportRepository();
            GroupFlight result = new GroupFlight();
            List<ListHangVe> listHangVes = new List<ListHangVe>();
            List<BookingClass> classBamBoos = getBookingClassBB(tripInfo.booking_class, countPax);
            if (classBamBoos != null)
            {
                result.FareDataId = tripInfo.flight_segment_group_id;
                result.FlightServiceSearch = FlightServiceSearch.BamBooAirWays;
                result.MainFlightNumber = "QH" + tripInfo.segment_info[0].flight_info.flight_number;
                result.MainAirlineCode = "QH";
                result.MainAirlineName = "BamBooAirways";
                result.MainDepartureAirportCode = tripInfo.segment_info[0].departure_info.airport_code;
                var departureAirportRow = airportRepository.GetAirportByCode(tripInfo.segment_info[0].departure_info.airport_code);
                result.MainDepartureAirportName = departureAirportRow.CityName;
                result.MainDepartureCity = departureAirportRow.CityName;
                result.MainDepartureCountry = departureAirportRow.CountryName;
                result.MainDepartureTime = Convert.ToDateTime(tripInfo.segment_info[0].departure_info.datetime).ToString("HH:mm");
                result.Plane = tripInfo.segment_info[0].aircraft_info.type;
                result.MainDepartureDate = Convert.ToDateTime(tripInfo.segment_info[0].departure_info.datetime);
                result.MainArrivalAirportCode = tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.airport_code;
                var arrivalAirportRow = airportRepository.GetAirportByCode(tripInfo.segment_info[tripInfo.segment_info.Count - 1].arrival_info.airport_code);
                result.MainArrivalAirportName = arrivalAirportRow.CityName;
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
                result.BgRow = string.Empty;
                if (waytype == 0)
                    result.WayType = WayType.OutBound;
                else
                    result.WayType = WayType.InBound;

                foreach (var classBamBoo in classBamBoos)
                {
                    var listHangVe = new ListHangVe();

                    listHangVe.ListChangBays = GetListChangBayQH(tripInfo.segment_info, classBamBoo.cabin_class);
                    listHangVe.BookingKey = classBamBoo.group_fare[0].fare_class + "-" + classBamBoo.group_fare[0].fare_basis + "-" + keyroot;
                    listHangVe.PriceDomestic = classBamBoo.pricing.pax_pricing_info[0].display_fare.amount - result.Discount;
                    listHangVe.Discount = classBamBoo.pricing.pax_pricing_info[0].discount.amount;
                    listHangVe.FareAdt = GetFareQH(classBamBoo, "ADULT");
                    listHangVe.TaxAdt = GetTaxFeeQH(classBamBoo, "ADULT");
                    listHangVe.FareChd = GetFareQH(classBamBoo, "CHILD");
                    listHangVe.TaxChd = GetTaxFeeQH(classBamBoo, "CHILD");
                    listHangVe.FareInf = GetFareQH(classBamBoo, "INFANT");
                    listHangVe.TaxInf = GetTaxFeeQH(classBamBoo, "INFANT");
                    listHangVe.FeeAdt = 0;
                    listHangVe.FeeChd = 0;
                    listHangVe.FeeInf = 0;
                    listHangVe.TicketClassDomestic = classBamBoo.pricing.fare_type;
                    listHangVe.FlightRef = int.Parse(tripInfo.segment_info[0].flight_info.flight_number);
                    switch (classBamBoo.pricing.fare_type)
                    {
                        case "EconomySaverMax":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "Hành lý ký gửi	Trả phí";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note += "    <li>Hành lý ký gửi	Trả phí</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)	Không áp dụng</li>";
                            listHangVe.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)Không áp dụng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            listHangVe.Note += "</ul>";
                            listHangVe.ReturnTicket = "Không hoàn hủy";
                            break;
                        case "EconomySaver":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note += "    <li>Hành lý ký gửi 20 kg</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) 270,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            listHangVe.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) Không áp dụng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) Không áp dụng</li>";
                            listHangVe.Note += "</ul>";
                            listHangVe.ReturnTicket = "Không hoàn hủy";
                            break;
                        case "EconomySmart":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note += "    <li>Hành lý ký gửi	20 kg</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) 270,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 550,000 VNĐ/người/chặng + chênh lệch (nếu có)</li>";
                            listHangVe.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 550,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "</ul>";
                            listHangVe.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "EconomyFlex":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "20 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note += "    <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note += "    <li>Hành lý ký gửi	20 kg</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trước giờ khởi hành tối thiểu 03 tiếng) Miễn phí + chênh lệch (nếu có)</li>";
                            listHangVe.Note += "    <li>Thay đổi chuyến bay/ hành trình (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)	Miễn phí + chênh lệch (nếu có)</li>";
                            listHangVe.Note += "    <li>Đổi tên (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "    <li>Hoàn vé (Trước giờ khởi hành tối thiểu 03 tiếng) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "   	<li>Hoàn vé (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) 350,000 VNĐ/người/chặng</li>";
                            listHangVe.Note += "</ul>";
                            listHangVe.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "PremiumSmart":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "30 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note = "    <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note = "    <li>Hành lý ký gửi	30 kg</li>";
                            listHangVe.Note = "    <li>Thay đổi chuyến bay/ hành trình 270,000 VNĐ/người/chặng + chênh lệch(Trước giờ khởi hành tối thiểu 03 tiếng) (nếu có)</li>";
                            listHangVe.Note = "    <li>Thay đổi chuyến bay/ hành trình 270,000 VNĐ/người/chặng + chênh lệch(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành) (nếu có)</li>";
                            listHangVe.Note = "    <li>Đổi tên 350,000 VNĐ/người/chặng(Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)</li>";
                            listHangVe.Note = "    <li>Hoàn vé 350,000 VNĐ/người/chặng(Trước giờ khởi hành tối thiểu 03 tiếng)</li>";
                            listHangVe.Note = "    <li>Hoàn vé 350,000 VNĐ/người/chặng(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            listHangVe.Note = "</ul>";
                            listHangVe.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        case "PremiumFlex":
                            listHangVe.RecommendationNumber = "7 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "30 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note = "     <li>Hành lý xách tay 7 kg</li>";
                            listHangVe.Note = "     <li>Hành lý ký gửi	30 kg</li>";
                            listHangVe.Note = "     <li>Thay đổi chuyến bay/ hành trình (Miễn phí + chênh lệch(Trước giờ khởi hành tối thiểu 03 tiếng) (nếu có)</li>";
                            listHangVe.Note = "     <li>Thay đổi chuyến bay/ hành trình (Miễn phí + chênh lệch (nếu có)(Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            listHangVe.Note = "     <li>Đổi tên 350,000 VNĐ/người/chặng (Trước giờ khởi hành đầu tiên trên vé tối thiểu 03 tiếng)</li>";
                            listHangVe.Note = "     <li>Hoàn vé	350,000 VNĐ/người/chặng (Trước giờ khởi hành tối thiểu 03 tiếng)</li>";
                            listHangVe.Note = "     <li>Hoàn vé	350,000 VNĐ/người/chặng (Trong vòng 03 tiếng trước giờ khởi hành và sau giờ khởi hành)</li>";
                            listHangVe.Note = " </ul>";
                            listHangVe.ReturnTicket = "Có thể hoàn hủy";
                            break;
                        default:
                            listHangVe.RecommendationNumber = "0 Kg hành lý xách tay";
                            listHangVe.AllowanceBaggage = "0 Kg hành lý ký gửi";
                            listHangVe.Note = "<ul class=\"none-style\">";
                            listHangVe.Note += "<li>0 Kg hành lý xách tay</li>";
                            listHangVe.Note += "<li>0 Kg hành lý ký gửi</li>";
                            listHangVe.Note += "<li>Không được thay đổi chuyến bay, chặng bay, ngày bay</li>";
                            listHangVe.Note += "</ul>";
                            listHangVe.ReturnTicket = "Không hoàn hủy";
                            break;
                    }

                    result.ListHangVes.Add(listHangVe);
                }
                    return result; 
            }
            else
                return null;
        }
        private static List<Segment> GetListChangBayQH(List<SegmentInfo> segment_info, string ticketClass)
        {
            List<ListHangVe> listHangVes = new List<ListHangVe>();

            List<Segment> result = new List<Segment>();
                var airportRepository = new AirportRepository();
            segment_info.ForEach(f =>
            {
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
        private static List<BookingClass> getBookingClassBB(List<BookingClass> bookingclass, int countPax)
        {
            List<BookingClass> result = new List<BookingClass>();

            foreach (var b in bookingclass)
            {
                if (b.seat_availablity > 0 && b.seat_availablity >= countPax)
                {
                    result.Add(b);
                }
            }

            return result;
        }

    }

}
