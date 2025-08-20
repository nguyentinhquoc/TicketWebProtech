using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProtechGroup.service.BindApiFlight.RootFlight
{
    public class RootVietNameAirline
    {
        public string Session { get; set; }              // ID phiên làm việc hay truy vấn
        public object Remark { get; set; }                // Ghi chú, có thể null hoặc string
        public List<ListGroup> ListGroup { get; set; }    // Danh sách các nhóm chuyến bay (leg, hành trình)
        public string StatusCode { get; set; }            // Mã trạng thái kết quả trả về từ API
        public bool Success { get; set; }                  // Biến báo thành công hay không của truy vấn
        public object Message { get; set; }                // Thông báo kèm theo (nếu có)
        public string Language { get; set; }               // Ngôn ngữ dữ liệu (vd: "en")
        public string RequestID { get; set; }              // ID của yêu cầu API
        public List<object> ApiQueries { get; set; }       // Danh sách truy vấn API, có thể rỗng
    }

    public class ListGroup
    {
        public int Leg { get; set; }                        // Số thứ tự hành trình (leg)
        public string Journey { get; set; }                 // Mã tuyến bay (ví dụ: HANSGN)
        public string StartPoint { get; set; }              // Sân bay khởi hành (mã)
        public string EndPoint { get; set; }                // Sân bay kết thúc (mã)
        public string DepartDate { get; set; }              // Ngày khởi hành (ddMMyyyy)
        public List<ListAirOption> ListAirOption { get; set; }  // Danh sách các lựa chọn hãng bay
        public CurrencyInfo CurrencyInfo { get; set; }          // Thông tin về đơn vị tiền tệ và tỉ giá
    }

    public class ListAirOption
    {
        public int OptionId { get; set; }                    // ID lựa chọn hãng bay
        public int Leg { get; set; }                          // Số thứ tự hành trình (leg)
        public string Airline { get; set; }                   // Mã hãng hàng không (VD: VN)
        public string System { get; set; }                    // Hệ thống booking (ví dụ: 1A)
        public string Currency { get; set; }                  // Đơn vị tiền tệ (VD: VND)
        public object Remark { get; set; }                     // Ghi chú bổ sung, có thể null
        public List<ListFlightOption> ListFlightOption { get; set; }  // Các lựa chọn chuyến bay thuộc hãng này
        public List<ListFareOption> ListFareOption { get; set; }      // Các lựa chọn giá vé (fare)
    }

    public class ListFlightOption
    {
        public int OptionId { get; set; }                      // ID lựa chọn chuyến bay
        public List<ListFlight> ListFlight { get; set; }       // Danh sách các chuyến bay trong lựa chọn này
    }

    public class ListFlight
    {
        public int Leg { get; set; }                            // Số thứ tự hành trình (leg)
        public string FlightId { get; set; }                    // ID chuyến bay
        public string Airline { get; set; }                      // Mã hãng bay
        public string Operator { get; set; }                     // Hãng vận hành chuyến bay
        public string StartPoint { get; set; }                   // Mã sân bay đi
        public string EndPoint { get; set; }                     // Mã sân bay đến
        public string DepartDate { get; set; }                   // Thời gian khởi hành (yyyyMMdd HHmm)
        public string ArriveDate { get; set; }                   // Thời gian đến (yyyyMMdd HHmm)
        public string FlightNumber { get; set; }                 // Số hiệu chuyến bay
        public int StopNum { get; set; }                          // Số điểm dừng (transit)
        public int Duration { get; set; }                         // Thời gian bay (phút)
        public List<ListSegment> ListSegment { get; set; }       // Danh sách các đoạn bay (segment) trong chuyến bay này
    }

    public class ListSegment
    {
        public int SegmentId { get; set; }                        // ID đoạn bay
        public string Airline { get; set; }                        // Hãng hàng không
        public string Operator { get; set; }                       // Hãng vận hành
        public string StartPoint { get; set; }                     // Mã sân bay đi
        public string EndPoint { get; set; }                       // Mã sân bay đến
        public string DepartDate { get; set; }                     // Thời gian khởi hành
        public string ArriveDate { get; set; }                     // Thời gian đến
        public string StartTerminal { get; set; }                  // Nhà ga khởi hành
        public string EndTerminal { get; set; }                    // Nhà ga đến
        public string FlightNumber { get; set; }                   // Số hiệu chuyến bay
        public string Equipment { get; set; }                       // Loại máy bay (vd: 321)
        public int Duration { get; set; }                           // Thời gian bay (phút)
        public bool HasStop { get; set; }                           // Có điểm dừng kỹ thuật hay không
        public object StopPoint { get; set; }                       // Điểm dừng (nếu có)
        public int StopTime { get; set; }                           // Thời gian dừng (phút)
        public object TechnicalStop { get; set; }                   // Thông tin dừng kỹ thuật (nếu có)
        public bool DayChange { get; set; }                          // Có đổi ngày hay không
        public bool StopOvernight { get; set; }                      // Có dừng qua đêm hay không
        public bool ChangeStation { get; set; }                      // Có đổi sân bay trong cùng thành phố hay không
        public bool ChangeAirport { get; set; }                      // Có đổi sân bay khác trong chuyến
        public object MarriageGrp { get; set; }                      // Nhóm ghép chuyến bay (nếu có)
        public int FlightsMiles { get; set; }                         // Quãng đường bay (dặm)
        public string Status { get; set; }                            // Trạng thái vé (HK, HL,...)
    }

    public class ListFareOption
    {
        public int OptionId { get; set; }                            // ID lựa chọn giá vé
        public bool Refundable { get; set; }                          // Vé có được hoàn hay không
        public string FareClass { get; set; }                         // Lớp giá vé (A, P, ...)
        public int Availability { get; set; }                          // Số vé còn trống
        public object ExpiryDate { get; set; }                         // Ngày hết hạn giá vé (có thể null)
        public long TotalFare { get; set; }                            // Tổng tiền vé gốc chưa gồm thuế
        public long TotalTax { get; set; }                             // Tổng tiền thuế
        public long TotalPrice { get; set; }                           // Tổng giá vé = TotalFare + TotalTax
        public object Remark { get; set; }                              // Ghi chú thêm (nếu có)
        public List<ListFarePax> ListFarePax { get; set; }              // Thông tin giá vé theo loại hành khách
    }

    public class ListFarePax
    {
        public string PaxType { get; set; }                             // Loại hành khách (ADT, CHD, INF)
        public int PaxNumb { get; set; }                                // Số lượng hành khách loại này
        public long TotalFare { get; set; }                             // Tổng tiền vé cho loại hành khách này
        public long BaseFare { get; set; }                              // Giá cơ bản vé chưa thuế
        public long Taxes { get; set; }                                 // Tiền thuế vé
        public List<ListFareItem> ListFareItem { get; set; }            // Chi tiết các khoản phí vé (base, tax,...)
        public List<ListFareInfo> ListFareInfo { get; set; }            // Thông tin chi tiết về giá vé cho từng đoạn bay
        public List<object> ListTaxDetail { get; set; }                 // Chi tiết thuế, có thể rỗng hoặc null
    }

    public class ListFareItem
    {
        public string Code { get; set; }                                // Mã loại phí (TICKET_FARE, TICKET_TAX,...)
        public string Name { get; set; }                                // Tên khoản phí (Base fare, Taxes,...)
        public long Amount { get; set; }                                // Số tiền của khoản phí
    }

    public class ListFareInfo
    {
        public int SegmentId { get; set; }                              // ID đoạn bay liên quan
        public string StartPoint { get; set; }                          // Mã sân bay đi
        public string EndPoint { get; set; }                            // Mã sân bay đến
        public string FareClass { get; set; }                           // Lớp giá vé
        public string FareBasis { get; set; }                           // Mã cơ sở giá vé (fare basis code)
        public string FareFamily { get; set; }                          // Nhóm giá vé (ví dụ Economy Super Lite)
        public string CabinCode { get; set; }                           // Mã khoang (M, Y,...)
        public string CabinName { get; set; }                           // Tên khoang (Economy, Business,...)
        public string HandBaggage { get; set; }                         // Hành lý xách tay miễn phí
        public string FreeBaggage { get; set; }                         // Hành lý ký gửi miễn phí
        public int Availability { get; set; }                           // Số vé còn trong hạng vé này
    }

    public class CurrencyInfo
    {
        public string OriginCurrency { get; set; }                      // Đồng tiền gốc (vd: VND)
        public string OutputCurrency { get; set; }                      // Đồng tiền hiển thị (vd: VND)
        public int Rate { get; set; }                                    // Tỉ giá quy đổi (thường là 1 nếu cùng tiền tệ)
        public int RoundUnit { get; set; }                               // Đơn vị làm tròn số (ví dụ 1 = làm tròn đến đồng)
    }
}
