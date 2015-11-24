using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnaFoo.Models
{
    public class Snack
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Optional { get; set; }
        public string PurchaseLocations { get; set; }
        public int PurchaseCount { get; set; }
        public string LastPurchaseDate { get; set; }
    }
}