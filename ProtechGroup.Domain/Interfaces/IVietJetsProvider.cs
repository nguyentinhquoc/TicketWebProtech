using ProtechGroup.Domain.DTOs;
using System.Threading.Tasks;


namespace ProtechGroup.Domain.Interfaces
{
    public interface IVietJetsProvider
    {
        Task<UserSessionVJ> GetUserSessionsVietJets();
        Task<RootVietJets[]> GetAlinesVietJets(string strRequest, string accessToken);
    }
}
