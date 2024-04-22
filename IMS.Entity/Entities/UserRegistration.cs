using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class UserRegistration
    {
        public virtual long UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; } 
        public virtual string PhoneNumber { get; set; }
        public virtual string Password { get; set; }
        public virtual string ConfirmPassword { get; set; }
        public virtual IList<UserRegistration> UserRegistrations { get; set; }

    }
}
