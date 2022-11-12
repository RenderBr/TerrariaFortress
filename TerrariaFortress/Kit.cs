using System;
using System.Collections.Generic;
using System.Text;

namespace TerrariaFortress
{
    public class Kit
    {
        public string name { get; set; }

        public int health { get; set; }

        public List<Tuple<int, int>> items { get; set; }
    
        public List<int> accessories { get; set; }

        public List<int> armor { get; set; }
    
        public List<int> buffs { get; set; }

        public Kit(string name, List<Tuple<int, int>> items, List<int> accessories, List<int> armor)
        {
            this.name = name;
            this.items = items;
            this.accessories = accessories;
            this.armor = armor;
        }
    }
}
