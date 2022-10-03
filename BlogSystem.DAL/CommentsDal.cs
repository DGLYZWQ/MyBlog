using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class CommentsDal : BaseDAL<Comments>,ICommentsDal
    {
        public CommentsDal() : base(new BlogSystemContext())
        {

        }
    }
}