﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RizSoft.Acme.Domain.Models
{
    public partial class TagsProduct
    {
        public int IdTag { get; set; }
        public int IdProduct { get; set; }

        public virtual Product IdProductNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}