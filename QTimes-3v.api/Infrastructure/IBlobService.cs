using System.IO;
using System.Threading.Tasks;

namespace QTimes.api.Infrastructure
{
    public interface IBlobService
    {
        string GetFileUrl(string fileName);

        Task<string> UploadFile(Stream stream, string fileName);
    }

}
