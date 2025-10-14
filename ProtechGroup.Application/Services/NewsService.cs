using System.Collections.Generic;
using ProtechGroup.Application.Interfaces;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Domain.Interfaces;

namespace ProtechGroup.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly INewRepository _newRepository;
        public NewsService(INewRepository newRepository)
        {
            _newRepository = newRepository;
        }
        public IEnumerable<NewsMod>  GetAllNews()
        {
            return _newRepository.gettAllNews();
        }
    }
}
