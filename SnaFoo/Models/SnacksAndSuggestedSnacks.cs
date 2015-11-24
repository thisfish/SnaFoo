using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnaFoo.Models
{
    public class SnacksAndSuggestedSnacks
    {
        public List<Snack> Snacks { get; set; }
        public List<SuggestedSnack> SuggestedSnacks { get; set; }
    }
}