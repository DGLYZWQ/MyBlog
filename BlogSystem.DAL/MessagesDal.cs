using System.Runtime.Remoting.Messaging;
using BlogSystem.IDAL;
using BlogSystem.Models;

namespace BlogSystem.DAL
{
    public class MessagesDal : BaseDAL<Messages>,IMessagesDal
    {
        public MessagesDal() : base(new BlogSystemContext())
        {

        }
    }
}