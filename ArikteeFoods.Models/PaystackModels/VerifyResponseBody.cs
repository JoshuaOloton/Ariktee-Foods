namespace ArikteeFoods.Models.PaystackModels
{
    public class VerifyResponseBody
    {
        public bool Status { get; set; }
        public String Message { get; set; }
        public VerifyResponseData Data { get; set; }
    }

    public class VerifyResponseData
    {
        public long Id { get; set; }    
        public String Domain { get; set; }
        public String Status { get; set; }
        public String Reference { get; set; }
        public int Amount { get; set; }
        public String Paid_at { get; set; }
        public String Created_at { get; set; }
    }
}
