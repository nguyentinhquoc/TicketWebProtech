using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Domain.Interfaces;
using ProtechGroup.Infrastructure.Contexts;

namespace ProtechGroup.Infrastructure.Repositories
{
	public class HotelRepository : IHotelRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		public HotelRepository(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<HotelMod> GetAllHotels()
		{
			return _context.Hotels
				.ProjectTo<HotelMod>(_mapper.ConfigurationProvider)
				.ToList();
		}
	}
}


