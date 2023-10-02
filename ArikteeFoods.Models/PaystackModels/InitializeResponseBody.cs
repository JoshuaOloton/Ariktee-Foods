namespace ArikteeFoods.Models.PaystackModels
{
    public class InitializeResponseBody
    {
        public bool Status { get; set; }
        public String Message { get; set; }
        public InitializeResponseData Data { get; set; }
    }

    public class InitializeResponseData
    {
        public String Authorization_url { get; set; }
        public String Access_code { get; set; }
        public String Reference { get; set; }
    }
}
