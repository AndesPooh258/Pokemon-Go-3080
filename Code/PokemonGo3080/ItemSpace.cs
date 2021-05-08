using System;
using System.Collections.Generic;
using PokemonSpace;
using PokemonWorld;

namespace ItemSpace {

    public abstract class Item {
        public string type, description;
        public abstract bool OnUse(Pokemon p);
        public override string ToString() {
            return type;
        }
    }

    public class Heal : Item {
        public int HPRestore { get; protected set; }
        protected Heal() { }
        public override bool OnUse(Pokemon p) {
            if (Player.Instance.GameMode != 2 && p.HP < p.actualHP && p.HP > 0) {
                p.HP += HPRestore;
                if (p.HP > p.actualHP)
                    p.HP = p.actualHP;
                return true;
            } else return false;
        }
    }

    public class Potion : Heal {
        public Potion() {
            type = "Potion";
            description = "Restores HP that have been lost in battle by 20 HP.";
            HPRestore = 20;
        }
    }

    public class SuperPotion : Heal {
        public SuperPotion() {
            type = "Super Potion";
            description = "Restores HP that have been lost in battle by 50 HP.";
            HPRestore = 50;
        }
    }

    public class HyperPotion : Heal {
        public HyperPotion() {
            type = "Hyper Potion";
            description = "Restores HP that have been lost in battle by 200 HP.";
            HPRestore = 200;
        }
    }

    public class ReviveItem : Item {
        public double Proportion { get; protected set; }
        protected ReviveItem() { }
        public override bool OnUse(Pokemon p) {
            if ((Player.Instance.GameMode == 1 || Player.Instance.GameMode == 4) && p.HP == 0) {
                p.HP = (int) (p.actualHP * Proportion);
                return true;
            } else return false;
        }
    }

    public class Revive : ReviveItem {
        public Revive() {
            type = "Revive";
            description = "Revives a fainted Pokémon and restores half its maximum HP.";
            Proportion = 0.5;
        }
    }

    public class MaxRevive : ReviveItem {
        public MaxRevive() {
            type = "Max Revive";
            description = "Revives a fainted Pokémon, fully restoring its HP.";
            Proportion = 1.0;
        }
    }

    public class Ball : Item {
        public double catchMultiplier { get; protected set; }
        protected Ball() { }
        public override bool OnUse(Pokemon p) {
            if (Player.Instance.GameMode == 2) {
                p.HP = p.actualHP;
                return true;
            } else return false;
        }
    }

    public class PokeBall : Ball {
        public PokeBall() {
            catchMultiplier = 1.0;
            type = "PokeBall";
            description = "A device for catching wild Pokémon. It's thrown like a ball at a Pokémon, comfortably encapsulating its target.";
        }
    }

    public class GreatBall : Ball {
        public GreatBall() {
            catchMultiplier = 1.5;
            type = "Great Ball";
            description = "A good, high-performance Poké Ball that provides a higher success rate for catching Pokémon than a standard Poké Ball.";
        }
    }

    public class UltraBall : Ball {
        public UltraBall() {
            catchMultiplier = 2.0;
            type = "Ultra Ball";
            description = "An ultra-high-performance Poké Ball that provides a higher success rate for catching Pokémon than a Great Ball.";
        }
    }

    public class MasterBall : Ball {
        public MasterBall() {
            catchMultiplier = 100.0;
            type = "Master Ball";
            description = "The best Poké Ball with the ultimate level of performance. With it, you will catch any wild Pokémon without fail.";
        }
    }

    public class BoostItem : Item {
        protected BoostItem() { }

        public override bool OnUse(Pokemon p) {
            return false;
        }
    }

    public class RareCandy : BoostItem {
        public RareCandy() {
            type = "Rare Candy";
            description = "Raises the level of the selected Pokémon by one.";
        }

        public override bool OnUse(Pokemon p) {
            return p.PowerUp();
        }
    }

    public class EvolveStone : BoostItem {
        public EvolveStone() {
            type = "Evolve Stone";
            description = "A Stone used for making Pokémon evolve.";
        }

        public override bool OnUse(Pokemon p) {
            return p.Evolve();
        }
    }

    public class StandardItemGenerator : ItemFactory {
        private Random rand = new Random();
        public Item ProduceItem() {
            int roll = rand.Next(100);
            if (roll < 18)
                return new Potion(); // 18%
            else if (roll < 30)
                return new SuperPotion(); // 12%
            else if (roll < 38)
                return new HyperPotion(); // 8%
            else if (roll < 48)
                return new Revive(); // 10%
            else if (roll < 53)
                return new MaxRevive(); // 5%
            else if (roll < 71)
                return new PokeBall(); // 18%
            else if (roll < 83)
                return new GreatBall(); // 12%
            else if (roll < 89)
                return new UltraBall(); // 6%
            else if (roll < 90)
                return new MasterBall(); // 1%
            else if (roll < 98)
                return new RareCandy(); // 8%
            else
                return new EvolveStone(); // 2%
        }
    }

    public interface ItemFactory {
        Item ProduceItem();
    }
}