using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Engine
{
    public class Recipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public int RequiredItemID {  get; set; }
        public int RequiredAmount {  get; set; }
        public int CraftedItemID { get; set; }


        public Recipe(int id, string name, string namePlural, int requiredItemID, int requiredAmount, int craftedItemID)
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
            RequiredItemID = requiredItemID;
            RequiredAmount = requiredAmount;
            CraftedItemID = craftedItemID;
        }
    }
}