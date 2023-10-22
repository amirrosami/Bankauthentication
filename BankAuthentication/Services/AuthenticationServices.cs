using BankAuthentication.Domain;
using BankAuthentication.Domain.Entities;
using BankAuthentication.ExtraServices;
using BankAuthentication.Repository;

namespace BankAuthentication.Services
{
    public class AuthenticationServices
    {
        private readonly UserRepository _userRepository;
       // private readonly ImageProcessingService _imageService;
        private readonly FileManagementService _fileManagementService;
        private readonly IRabbitMqPublisher _rabbitMqPublisher;
        public AuthenticationServices(UserRepository userRepository,FileManagementService fileManagementService,IRabbitMqPublisher rabbitMqPublisher)
        {   
            _userRepository = userRepository;
            //_imageService = imageService;
            _fileManagementService = fileManagementService;
            _rabbitMqPublisher = rabbitMqPublisher;
        }

        public async Task<int> InsertUser(SendRequestDto requestDto,string ip)
        {
            var imgid1=await _fileManagementService.UploadFile(requestDto.Img1);
            var imgid2 =await _fileManagementService.UploadFile(requestDto.Img2);
            var user = new Users(requestDto.UserName, requestDto.FullName, requestDto.EmailAddress, requestDto.NationalCode,imgid1,imgid2,ip);
            var result= await _userRepository.InsertUser(user);
            if (result==1)
            {
                _rabbitMqPublisher.Publish("bank", requestDto.UserName );
                return 1;
            }
            return 0;
            
        }


        public async Task<string> GetStatus(string userName)
        {
           var user=await _userRepository.GetUserBy(userName);
            if (user == null)
            {
                return "Not Found";
            }
            string status = "";
            switch (user.StatusCode)
            {
                case 0: status = "In Process";break;
                case 1:status = "Accepted"; break;
                case -1:status = "Rejected";break;

            }
            return status;
        } 

        
    }
}
