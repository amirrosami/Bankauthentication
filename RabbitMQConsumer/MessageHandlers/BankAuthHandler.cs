//using BankAuthentication.ExtraServices;
//using BankAuthentication.Repository;
using Microsoft.EntityFrameworkCore;
using RabbitMQConsumer.Repository;
using RabbitMQConsumer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer.MessageHandlers
{
    public class BankAuthHandler
    {
        private readonly UserRepository _userrepository;
        private readonly ImageProcessingService _imageService;
        private readonly FileManagementService _fileService;

        public BankAuthHandler(UserRepository userrepository,ImageProcessingService imageService,FileManagementService fileService)
        {
            _userrepository = userrepository;
            _imageService = imageService;
            _fileService = fileService;
        }

        public async Task<Users>  ChangeStatus(string UserName)
        {
           var user =await _userrepository.GetUserBy(UserName);
            var img1=await _fileService.GetFile(user.ImgId1);
            var img2=await _fileService.GetFile(user.ImgId2);
            if (user is not null)
            {
                var detect1 = _imageService.Detect(img1.Data);
                var detect2= _imageService.Detect(img2.Data);
                if ( detect1.Status==0  || detect2.Status ==0)
                {
                    user.StatusCode = -1;
                    await _userrepository.UpdateUser(user);
                    return user;
                }

              var score=  _imageService.GetSimilarity(detect1.face_id,detect2.face_id);
                if (score is not null && score >80)
                {
                    user.StatusCode= 1;
                    await _userrepository.UpdateUser(user);
                    return user;
                }
                else
                {
                    user.StatusCode = -1;
                    await _userrepository.UpdateUser(user);
                    return user;
                }
               
            }
            return user;

        }
    }
}
