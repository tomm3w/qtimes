using SeatQ.core.api.Models.Pass;
using System.Threading.Tasks;

namespace SeatQ.core.api.Services
{
    public interface IPasstrekClient
    {
        Task<string> GeneratePassUrlAsync(QTimesPassModel passModel);
    }
}
