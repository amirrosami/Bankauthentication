using BankAuthentication.Domain;
using BankAuthentication.Domain.Entities;

namespace RabbitMQConsumer.Repository
{
    public class UserRepository
    {
        private readonly BankDbContext _db;
        public UserRepository(BankDbContext db)
        {
            _db = db;
        }
       

        public async Task<Users> GetUserBy(string username)
        {
           return  _db.Users
                .Where(x=>x.UserName==username)
                .FirstOrDefault();
        }

       public async Task UpdateUser(Users user)
        {
            _db.Users.Update(user);
           await _db.SaveChangesAsync();
        }
    }
   
}
