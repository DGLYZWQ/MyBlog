﻿using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.DAL
{
    public class UserMsgDal : BaseDAL<UserMsg>, IUserMsgDal
    {
        public UserMsgDal() : base(new BlogSystemContext())
        {

        }
    }
}
