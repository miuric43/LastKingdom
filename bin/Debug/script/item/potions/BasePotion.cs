using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LKCamelot.model;
namespace LKCamelot.script.item
{
    public enum PotionEffect
    {
    }

    public class BasePotion : Item
    {
        private PotionEffect m_PotionEffect;
        public PotionEffect PotionEffect
        {
            get
            {
                return m_PotionEffect;
            }
            set
            {
                m_PotionEffect = value;
          //      InvalidateProperties();
            }
        }
        /*

         */
        public override void Use(Player player)
        {
            Delete(player);
        }

        public BasePotion(int itemID) : base(itemID)
        {
        }

        public BasePotion(Serial serial) : base(serial)
        {
        }

        
   /*     public enum OreTypeB
        {
            GB = 1,
            GN = 2,
            GG = 3,
        }*/
      //  public int Hits = 0;
      //  public virtual int XP { get { return 0; } }
        public override bool Stackable { get { return true; } }

    /*    public virtual void SetSprite()
        {
            if (Stage == (int)OreTypeB.GG)
                m_ItemID = 1;
            if (Stage == (int)OreTypeB.GN)
                m_ItemID = 2;
            if (Stage == (int)OreTypeB.GB)
                m_ItemID = 3;
        }

        public virtual void DropOre(Player player)
        {
            
            
                var roll = Util.Random(1, 100);
               if (roll <= 20) Stage = (int)OreTypeB.GG;
               if (roll <= 50 && roll > 20) Stage = (int)OreTypeB.GN;
               if (roll > 50) Stage = (int)OreTypeB.GB;

                var tempitem = this.Inventory(player);
               (tempitem as BasePotion).SetSprite();
                if (tempitem.Quantity == 1)
                {

                    {
                        player.client.SendPacket(new LKCamelot.model.AddItemToInventory2(tempitem).Compile());
                        World.NewItems.TryAdd((tempitem as script.item.Item).m_Serial, (tempitem as script.item.Item));
                    }
                }
            }*/
        
        public override string ParsedStats
        {
            get
            {
                string ret = "";
                ret += Name;
                if (Quantity != 0)
                    ret += "수량: " + (Quantity - 1) + "\n\t" + "     ";
                if (FlavorText != null)
                    ret += "\n\t  " + FlavorText + "\n\t";
                return ret;
            }
        }
    }
}
