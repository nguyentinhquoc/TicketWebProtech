using System.Collections.Generic;
using ProtechGroup.Domain.Entities;

namespace ProtechGroup.Application.Interfaces
{
	public interface IHotelService
	{
		IEnumerable<HotelMod> GetAllHotels();
	}
}


