using System;
using ProtechGroup.Infrastructure.Setting;
using ProtechGroup.Infrastructure.HttpClients;
using ProtechGroup.Domain.Interfaces;
using Newtonsoft.Json;
using ProtechGroup.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;


namespace ProtechGroup.Infrastructure.FlightProviders
{
    public class VietJetsProvider : IVietJetsProvider
    {
        public async Task<UserSessionVJ> GetUserSessionsVietJets()
        {
            try
            {
                string author = "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(ApiVietJetsSetting.username + 
                                                                                        ":" + ApiVietJetsSetting.password));
                var headers = new Dictionary<string, string>
                            {
                                { "apikey", ApiVietJetsSetting.apikey },
                                { "Authorization", author }
                            };

                string urlPost = ApiVietJetsSetting.urlVietjets + "/flight/userSessions";
                string response = await ApiClient.PostMethodHttpClientAddHeader(urlPost, string.Empty, headers);
                return JsonConvert.DeserializeObject<UserSessionVJ>(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi gọi API VietJets: " + ex.Message, ex);
            }
        }
        public async Task<RootVietJets[]> GetAlinesVietJets(string strRequest, string accessToken)
        {
            try
            {
                var headers = new Dictionary<string, string>
                            {
                                { "Authorization", "Bearer " + accessToken},
                                { "apikey", ApiVietJetsSetting.apikey }
                            };
                string strGet = ApiVietJetsSetting.urlVietjets + "/flight/travelOptions" + strRequest;
                string response = await ApiClient.GetMethodHttpClientAddHeader(strGet, headers);
                return JsonConvert.DeserializeObject<RootVietJets[]>(response);
            }
            catch (Exception ex) {
                throw new Exception("Lỗi khi gọi API VietJets: " + ex.Message, ex);
            }
               
        }
    }
}
