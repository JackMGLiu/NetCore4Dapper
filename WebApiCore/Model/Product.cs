using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore.Query.ResultOperators.Internal;

namespace WebApiCore.Model
{
    [Dapper.Contrib.Extensions.Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [Write(false)]
        public virtual Category Category { get; set; }
    }
}
