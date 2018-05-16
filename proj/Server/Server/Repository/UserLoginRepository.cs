using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Repository
{
    public class UserLoginRepository
    {
        private NewsAgencyDBEntities ctx;

        public UserLoginRepository()
        {
            ctx = new NewsAgencyDBEntities();

        }

        public bool Authorize(string userName, string password)
        {
            return ctx.Users.Any(p => p.Username == userName && p.Psssword == password);
        }

        public string GetAuthor(string username)
        {
            return ctx.Users.Single(p => p.Username == username).Name;
        }

    }
}
