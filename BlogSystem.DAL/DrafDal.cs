using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class DrafDal : BaseDAL<Draft>,IDraftDal
    {
        public DrafDal() : base(new BlogSystemContext())
        {

        }
    }
}