﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RizSoft.Acme.Domain.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagsProducts = new HashSet<TagsProduct>();
        }

        public int Id { get; set; }
        public string Tag1 { get; set; }

        public virtual ICollection<TagsProduct> TagsProducts { get; set; }
    }
}