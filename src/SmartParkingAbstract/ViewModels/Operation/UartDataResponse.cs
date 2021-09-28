namespace SmartParkingAbstract.ViewModels.Operation
{
    public class UartDataResponse<T>
    {
        public string Action { get; set; }
        public string GateName { get; set; }
        public T Data { get; set; }
    }
}
