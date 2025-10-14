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
    public class SearchInputRepository : ISearchInputRepository
    {
        private readonly ApplicationDbContext _searchInputContext;
        private readonly IMapper _searchInputmapper;
        public SearchInputRepository(ApplicationDbContext context, IMapper mapper)
        {
            _searchInputContext = context;
            _searchInputmapper = mapper;
        }
        public SearchInputMod Insert(SearchInputMod entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var infraEntity = _searchInputmapper.Map<SearchInput>(entity);
            _searchInputContext.SearchInputs.Add(infraEntity);
            _searchInputContext.SaveChanges();
            return entity;
        }

        // Cập nhật
        public void Update(SearchInputMod entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var infraEntity = _searchInputmapper.Map<SearchInput>(entity);
            _searchInputContext.Entry(infraEntity).State = EntityState.Modified;
            _searchInputContext.SaveChanges();
        }

        // Xoá
        public void Delete(int id)
        {
            var entity = _searchInputContext.SearchInputs.Find(id);
            if (entity != null)
            {
                _searchInputContext.SearchInputs.Remove(entity);
                _searchInputContext.SaveChanges();
            }
        }

        // Lấy theo SessionId (bạn yêu cầu GetByKeyId đầu vào sessionId)
        public SearchInputMod GetByKeySessionId(int sessionId)
        {
            var infraEntity = _searchInputContext.SearchInputs.FirstOrDefault(x => x.SessionId == sessionId);
            return _searchInputmapper.Map<SearchInputMod>(infraEntity);
        }
        public int GetNextSessionId()
        {
            int maxSessionId = 0;

            if (_searchInputContext.SearchInputs.Any())
            {
                maxSessionId = _searchInputContext.SearchInputs.Max(x => x.SessionId);
            }

            return maxSessionId + 1;
        }

    }
}
