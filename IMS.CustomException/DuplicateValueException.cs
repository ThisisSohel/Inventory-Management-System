using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CustomException
{
    public class DuplicateValueException:Exception
    {
        public DuplicateValueException(string name)
            : base("Duplicate value is not Accepted! Please input different value!") 
        { 
        }
    }
}
