using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.entity
{
    public class KeyWorldEntity
    {
        private string keyWorld;

        private int hotLeve;

        public string KeyWorld
        {
            get
            {
                return keyWorld;
            }

            set
            {
                keyWorld = value;
            }
        }

        public int HotLeve
        {
            get
            {
                return hotLeve;
            }

            set
            {
                hotLeve = value;
            }
        }
    }
}
