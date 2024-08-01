﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IMS.Entity.Entities
{
    public class ProductType
    {
        public virtual long Id { get; set; }
        public virtual string TypeName { get; set; }
        public virtual long CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual long ModifyBy { get; set; }
        public virtual DateTime? ModifyDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
