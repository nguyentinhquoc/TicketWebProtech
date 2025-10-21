using System.Collections.Generic;
using ProtechGroup.Domain.Entities;

namespace ProtechGroup.Domain.Interfaces
{
    public interface IHotelRepository
    {
        IEnumerable<HotelMod> GetAllHotels();
    }
}
