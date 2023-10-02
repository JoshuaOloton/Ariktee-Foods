namespace ArikteeFoods.Models.PaystackModels
{
    public class TotalPayInfo
    {
        public ApplicantPayInfo PayInfo { get; set; }
        public String Address { get; set; }
    }


    public class ApplicantPayInfo
    {
        public String Email { get; set; }
        public String Amount { get; set; }
        public String Callback_url { get; set; }

        public ApplicantPayInfo(String email, String amount, String callback_url)
        {
            Email = email;
            Amount = amount;
            Callback_url = callback_url;
        }
    }
}
