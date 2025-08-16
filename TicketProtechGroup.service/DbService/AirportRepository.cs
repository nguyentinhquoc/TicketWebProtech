using System.Linq;
using TicketProtechGroup.data; // Namespace chứa entity Airport

namespace TicketProtechGroup.service.DbService
{
    public class AirportRepository : BaseRepository
    {
        // Lấy Airport theo mã code
        public Airport GetAirportByCode(string code)
        {
            return db.Airports.FirstOrDefault(a => a.AirportCode == code);
        }
    }
}
