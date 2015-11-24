using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnaFoo.Models
{
    public class SuggestedSnack : Snack
    {
        public int Votes { get; set; }
    }
}