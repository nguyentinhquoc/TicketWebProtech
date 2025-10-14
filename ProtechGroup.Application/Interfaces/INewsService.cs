using ProtechGroup.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Application.Interfaces
{
    public interface INewsService
    {
        IEnumerable<NewsMod> GetAllNews();
    }
}
