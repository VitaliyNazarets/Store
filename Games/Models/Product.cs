﻿using Store.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
	public class Product : IProduct
	{

		public string Name { get; set; }

		public decimal Price { get; set; }
	}
}
