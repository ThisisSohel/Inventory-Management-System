using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class UserRole
    {
        public virtual long Id { get; set; }
        public virtual string RoleName { get; set; }
        public virtual UserRegistration UserRegistration { get; set; }
    }
}
