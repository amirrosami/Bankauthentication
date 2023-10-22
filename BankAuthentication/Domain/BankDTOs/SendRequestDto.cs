namespace BankAuthentication.Domain
{
    public class SendRequestDto
    {
        public string UserName  { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string NationalCode { get; set; }
        public IFormFile Img1 { get; set; }
        public IFormFile Img2 { get; set; }

    }
}
