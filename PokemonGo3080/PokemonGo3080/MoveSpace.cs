namespace MoveSpace {
    public class Move {
        public string name { get; protected set; }
        public int power { get; protected set; }
        public int accuracy { get; protected set; }
        protected Move() { }

        public override string ToString() {
            return name;
        }
    }

    public class Struggle : Move {
        public Struggle() {
            name = "Struggle";
            power = 50;
            accuracy = 100;
        }
    }

    public class MonMonStruggle : Move {
        public MonMonStruggle() {
            name = "Mon Mon Struggle";
            power = 150;
            accuracy = 90;
        }
    }

    public class Thunderbolt : Move {
        public Thunderbolt() {
            name = "Thunderbolt";
            power = 90;
            accuracy = 100;
        }
    }

    public class IronTail : Move {
        public IronTail() {
            name = "Iron Tail";
            power = 100;
            accuracy = 70;
        }
    }

    public class PyroBall : Move {
        public PyroBall() {
            name = "Pyro Ball";
            power = 120;
            accuracy = 70;
        }
    }

    public class DoubleKick : Move {
        public DoubleKick() {
            name = "Double Kick";
            power = 60;
            accuracy = 100;
        }
    }

    public class HyperVoice : Move {
        public HyperVoice() {
            name = "Hyper Voice";
            power = 90;
            accuracy = 100;
        }
    }

    public class HydroPump : Move {
        public HydroPump() {
            name = "Hydro Pump";
            power = 110;
            accuracy = 80;
        }
    }

    public class SeedFlare : Move {
        public SeedFlare() {
            name = "Seed Flare";
            power = 120;
            accuracy = 85;
        }
    }

    public class EnergyBall : Move {
        public EnergyBall() {
            name = "Energy Ball";
            power = 90;
            accuracy = 100;
        }
    }

    public class AuraWheel : Move {
        public AuraWheel() {
            name = "Aura Wheel";
            power = 110;
            accuracy = 100;
        }
    }

    public class Crunch : Move {
        public Crunch() {
            name = "Crunch";
            power = 80;
            accuracy = 100;
        }
    }

    public class DoomDesire : Move {
        public DoomDesire() {
            name = "Doom Desire";
            power = 140;
            accuracy = 100;
        }
    }

    public class Psychic : Move {
        public Psychic() {
            name = "Psychic";
            power = 90;
            accuracy = 100;
        }
    }

    public class DracoMeteor : Move {
        public DracoMeteor() {
            name = "Draco Meteor";
            power = 130;
            accuracy = 90;
        }
    }

    public class ShadowBall : Move {
        public ShadowBall() {
            name = "Shadow Ball";
            power = 90;
            accuracy = 100;
        }
    }

    public class AuraSphere : Move {
        public AuraSphere() {
            name = "Aura Sphere";
            power = 80;
            accuracy = 100;
        }
    }

    public class MeteorMash : Move {
        public MeteorMash() {
            name = "Meteor Mash";
            power = 90;
            accuracy = 90;
        }
    }
}