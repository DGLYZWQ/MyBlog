using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class DataShowDal : BaseDAL<DataShow>,IDataShowDal
    {
        public DataShowDal() : base(new BlogSystemContext())
        {

        }
    }
}