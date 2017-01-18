using Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DBTesting
{
    class MedTrainService
    {
        private MedTrainDBContext _context;

        public MedTrainService(MedTrainDBContext context) 
        { 
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void addUser(User u) 
        {
            _context.Users.Add(u); 
            _context.SaveChanges();
        }

        public User getUser(string UserId)
        {
            return _context.Users.Find(UserId);
        }
    }
}
