using System;

namespace POSService.Interfaces
{
    public interface IConnectionService
    {
        int Send(string data);
        string Receive(TimeSpan timeout);
        void Close();
    }
}
