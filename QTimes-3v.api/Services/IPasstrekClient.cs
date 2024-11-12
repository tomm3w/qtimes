using QTimes.api.Models.Pass;
using System.Threading.Tasks;

namespace QTimes.api.Services
{
    public interface IPasstrekClient
    {
        Task<string> GeneratePassUrlAsync(QTimesPassModel passModel);
    }
}
