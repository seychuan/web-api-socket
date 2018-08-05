using System.Threading.Tasks;

namespace POSService.Interfaces
{
    public interface IEDCService
    {
        string ProcessTransaction(string message);
    }
}
