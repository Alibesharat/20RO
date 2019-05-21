namespace NotifCore.models
{
    public  class ResultSmsViewModel
    {
        public long messageid { get; set; }
        public string message { get; set; }
        public int status { get; set; }
        public string statustext { get; set; }
        public string sender { get; set; }
        public string date { get; set; }
        public int cost { get; set; }
    }
}
