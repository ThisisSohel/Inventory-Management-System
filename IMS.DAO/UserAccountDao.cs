﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAO
{
    class UserAccountDao
    {
        public interface IUserAccountDao
        {
            Task DbConnection();
        }
    }
}
