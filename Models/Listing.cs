﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BonksList.Models
{
    public class Listing
    {
        public int Id { get; set; }
        public string? imageUrl { get; set; }
        public string description { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal price { get; set; }

        public Listing()
        {

        }
    }
}