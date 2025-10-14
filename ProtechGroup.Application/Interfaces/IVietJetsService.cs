using ProtechGroup.Domain.DTOs;
using System.Threading.Tasks;

namespace ProtechGroup.Application.Interfaces
{
    public interface IVietJetsService
    {
        Task<UserSessionVJ> GetUserSessionsVietJets();
        Task<RootVietJets[]> GetAlinesVietJets(string strRequest);
    }
}
