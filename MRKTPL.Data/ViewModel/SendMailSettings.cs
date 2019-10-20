namespace MRKTPL.Data.ViewModel
{
    public class SendMailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
    }
}
