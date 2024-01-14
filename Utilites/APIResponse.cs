namespace MCSHiPPERS_Task.Utilites
{
    public class APIResponse
    {
        public int StatusCode { get; set; } = 200;
        public object Data { get; set; } = new object();
        public List<string> Messages { get; set; } = new List<string>();
    }
}
