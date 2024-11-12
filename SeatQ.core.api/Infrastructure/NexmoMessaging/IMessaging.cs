namespace SeatQ.core.api.Infrastructure.NexmoMessaging
{
    public interface IMessaging
    {
        string Send(string from, string toMobileNo, string message);
    }
}