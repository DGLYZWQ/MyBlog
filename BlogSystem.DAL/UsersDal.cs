using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class UsersDal : BaseDAL<Users>,IUsersDal
    {
        public UsersDal() : base(new BlogSystemContext())
        {

        }
    }
}