using AutoMapper;
using ProtechGroup.Domain.Entities;
using AutoMapper.QueryableExtensions;
using ProtechGroup.Domain.Interfaces;
using ProtechGroup.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Infrastructure.Repositories
{
    public class NewRepository : INewRepository
    {
        private readonly ApplicationDbContext _newContext;
        private readonly IMapper _newmapper;
        public NewRepository(ApplicationDbContext context, IMapper mapper)
        {
            _newContext = context;
            _newmapper = mapper;
        }

        public IEnumerable<NewsMod> gettAllNews()
        {
            var today = DateTime.Today;
            return _newContext.News
                .ProjectTo<NewsMod>(_newmapper.ConfigurationProvider)
                .Take(8)
                .ToList();
        }
    }
}
