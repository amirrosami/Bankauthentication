using BankAuthentication.Domain;
using BankAuthentication.Domain.Entities;

namespace BankAuthentication.Repository
{
    public class UserRepository
    {
        private readonly BankContext _db;
        public UserRepository(BankContext db)
        {
            _db = db;
        }
        public async Task<int> InsertUser(Users user)
        {
           await _db.AddAsync(user);
           await _db.SaveChangesAsync();
            return 1;
        }

        public async Task<Users> GetUserBy(string username)
        {
           return  _db.Users
                .Where(x=>x.UserName==username)
                .FirstOrDefault();
        }


    }
    public class UserViewModel
    {
        public string MyProperty { get; set; }
    }
}
