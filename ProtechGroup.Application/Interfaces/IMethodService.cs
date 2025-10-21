using ProtechGroup.Domain;
using System.Threading.Tasks;


namespace ProtechGroup.Application.Interfaces
{
    public interface IMethodService
    {
        Task<FlightResultOutput> GetFlightDomestic(int sessionId);
    }
}

