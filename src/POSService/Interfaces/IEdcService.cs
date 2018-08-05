namespace POSService.Interfaces
{
    public interface IEDCService
    {
        TResponse ProcessTransaction<TRequest, TResponse>(TRequest request);
    }
}
