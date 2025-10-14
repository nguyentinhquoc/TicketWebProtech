using AutoMapper;
using ProtechGroup.Domain.Entities;
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
                
            return _newContext.News.Select(n => new NewsMod
            {
                Id = n.Id,
                NewsGroupID = n.NewsGroupID,
                Name = n.Name,
                Url = n.Url,
                Title = n.Title,
                MetaDescription = n.MetaDescription,
                MetaKeywords = n.MetaKeywords,
                Summary = n.Summary,
                ImageUrl = n.ImageUrl,
                Content = n.Content,

            }).ToList();

        }
    }
}
