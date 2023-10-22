namespace BankAuthentication.Domain.Entities
{
    public class Users
    {
        public int  UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string NationalCode { get; set; }
        public string ImgId1 { get; set; }
        public string ImgId2 { get; set; }
        public string Ip { get; set; }
        public int StatusCode { get; set; }

        public Users()
        {
        }

        public Users(string userName, string fullName, string emailAddress, string nationalCode, string imgUrl1, string imgUrl2, string ip)
        {
            UserName = userName;
            FullName = fullName;
            EmailAddress = emailAddress;
            NationalCode = nationalCode;
            ImgId1 = imgUrl1;
            ImgId2 = imgUrl2;
            Ip = ip;
        }
    }
}
