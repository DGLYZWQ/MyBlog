using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class CategoryDal : BaseDAL<Category>,ICategoryDal
    {
        public CategoryDal() : base(new BlogSystemContext())
        {

        }
    }
}