﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entity.Entities
{
    public class Customer
    {
        public virtual long Id { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerNumber { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string CustomerAddress {  get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual long ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }

    }
}
