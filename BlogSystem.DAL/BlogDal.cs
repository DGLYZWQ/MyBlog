using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class BlogDal : BaseDAL<Blog>,IBlogDal
    {
        public BlogDal() : base(new BlogSystemContext())
        {

        }
    }
}