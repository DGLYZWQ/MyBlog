using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class ViewsDal : BaseDAL<Views>, IViewsDal
    {
        public ViewsDal() : base(new BlogSystemContext())
        {

        }
    }
}
