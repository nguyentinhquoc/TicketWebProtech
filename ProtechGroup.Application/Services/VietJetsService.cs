using ProtechGroup.Application.Interfaces;
using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Domain.Interfaces;
using ProtechGroup.Infrastructure.FlightProviders;
using System;
using System.Threading.Tasks;

namespace ProtechGroup.Application.Services
{
    public class VietJetsService : IVietJetsService
    {
        private readonly IVietJetsProvider _vietjetsProvider;
        private readonly ISearchWSHistoryRepository _searchWSHistoryRepository;
        public VietJetsService(IVietJetsProvider vietjetsProvider, 
                                ISearchWSHistoryRepository searchWSHistoryRepository)
        {
            _vietjetsProvider = vietjetsProvider;
            _searchWSHistoryRepository = searchWSHistoryRepository;
        }
        public async Task<UserSessionVJ> GetUserSessionsVietJets()
        {
            return await _vietjetsProvider.GetUserSessionsVietJets();
        }
        public async Task<RootVietJets[]> GetAlinesVietJets(string strRequest)
        {
            var searchHis = _searchWSHistoryRepository.GetSearchWSHistoryByAirlineCode("VJ");
            string accessToken = string.Empty;
            if (searchHis != null && !string.IsNullOrEmpty(searchHis.AccessToken))
            {
                accessToken = searchHis.AccessToken;
            }
            else
            {
                var userVj = await GetUserSessionsVietJets();
                var enty = new SearchWSHistoryMod()
                {
                    AccessToken = userVj.accessToken,
                    DateTimeBlock = DateTime.Now.AddMinutes(3),
                    AirlineCode = "VJ"
                };
                var tokenAcc = _searchWSHistoryRepository.Insert(enty);
                accessToken = tokenAcc.AccessToken;
            }
            return await _vietjetsProvider.GetAlinesVietJets(strRequest, accessToken);
        }
    }
}
