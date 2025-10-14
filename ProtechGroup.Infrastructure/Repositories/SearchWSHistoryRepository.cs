using System;
using System.Collections.Generic;
using System.Linq;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Domain.Interfaces;
using ProtechGroup.Infrastructure.Contexts;
using System.Data.Entity;
using AutoMapper;
using ProtechGroup.Infrastructure.Entities;


namespace ProtechGroup.Infrastructure.Repositories
{
    public class SearchWSHistoryRepository : ISearchWSHistoryRepository
    {
        private readonly ApplicationDbContext _searchWSHistoryContext;
        private readonly IMapper _searchWSHistorymapper;
        public SearchWSHistoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _searchWSHistoryContext = context;
            _searchWSHistorymapper = mapper;
        }
        public SearchWSHistoryMod GetSearchWSHistoryByAirlineCode(string airlineCode)
        {
            var infraEntity = _searchWSHistoryContext.SearchWSHistorys.FirstOrDefault(x => x.AirlineCode == airlineCode && x.DateTimeBlock > DateTime.Now);
            return _searchWSHistorymapper.Map<SearchWSHistoryMod>(infraEntity);
        }
        public SearchWSHistoryMod Insert(SearchWSHistoryMod entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var infraEntity = _searchWSHistorymapper.Map<SearchWSHistory>(entity);
            _searchWSHistoryContext.SearchWSHistorys.Add(infraEntity);
            _searchWSHistoryContext.SaveChanges();
            return entity;
        }
    }
}
