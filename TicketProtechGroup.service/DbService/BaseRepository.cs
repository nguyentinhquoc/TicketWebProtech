using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.data;

namespace TicketProtechGroup.service.DbService
{
    public class BaseRepository
    {
        public SkytourWebEntities2 db;
        public BaseRepository()
        {
            db = new SkytourWebEntities2();
        }
    }
}
