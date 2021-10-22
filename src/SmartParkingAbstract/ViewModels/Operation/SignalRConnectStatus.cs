namespace SmartParkingAbstract.ViewModels.Operation
{
    public class SignalRConnectStatus
    {
        public string GateName { get; set; }
        public string ConnectionId { get; set; }
        public bool RequestLock { get; set; }
        public bool LockSuccess { get; set; }
        public string Error { get; set; }
    }
}
