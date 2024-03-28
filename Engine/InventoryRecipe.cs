using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Engine
{
    public class InventoryRecipe
    {
        public Recipe Details { get; set; }
        public int Quantity { get; set; }
        public InventoryRecipe(Recipe details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
    }
}