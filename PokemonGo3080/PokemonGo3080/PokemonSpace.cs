using System;
using System.Collections.Generic;
using MoveSpace;

namespace PokemonSpace {
    public abstract class Pokemon {
        // general information
        protected string species;
        protected int stage, maxStage;
        protected int baseCatchRate;
        public List<Move> moveSet = new List<Move>();
        private Random rand = new Random();

        public string name { get; protected set; }
        public string type { get; protected set; }
        public string imageSource {get; protected set;}

        public int catchRate { 
            get { return baseCatchRate / (stage + 1); }
        }

        // stats calculation following https://bulbapedia.bulbagarden.net/wiki/Statistic#Determination_of_stats
        public int level, HP;
        protected int baseHP, baseAttack, baseDefence, baseSpeed;
        protected int HPIV, AttackIV, DefenceIV, SpeedIV;

        public int actualHP {
            get { return (int)((baseHP * 2 + HPIV + 63) * level / 100.0 + 10 + level); }
        }

        public int actualAttack {
            get { return (int)((baseAttack * 2 + AttackIV + 63) * level / 100.0 + 5); }
        }

        public int actualDefence {
            get { return (int)((baseDefence * 2 + DefenceIV + 63) * level / 100.0 + 5); }
        }

        public int actualSpeed {
            get { return (int)((baseSpeed * 2 + SpeedIV + 63) * level / 100.0 + 5); }
        }

        // constructor
        protected Pokemon(int level, int stage) {
            this.level = level;
            HPIV = rand.Next(32);
            AttackIV = rand.Next(32);
            DefenceIV = rand.Next(32);
            SpeedIV = rand.Next(32);
            this.stage = stage;
            ComputeBaseInfo(stage);
            HP = actualHP;
            name = species;
        }

        // base information depends on different species
        protected abstract void ComputeBaseInfo(int stage);

        // power up the pokemon if it is not in max level
        public bool PowerUp() {
            if (level < 100) {
                level++;
                return true;
            } else return false;
        }

        // evolve the pokemon if it is not in max stage
        public bool Evolve() {
            if (stage < maxStage) {
                stage++;
                ComputeBaseInfo(stage);
                return true;
            } else return false;
        }

        public void Rename(string str) {
            if (str.Length > 0 && str.Length <= 12) {
                name = str;
            } else name = species;
        }

        // convert general information into a string
        public override string ToString() {
            return species;
        }
    }

    public class Ditto : Pokemon {
        public Ditto(int level, int stage) : base(level, stage) {
            type = "Normal";
            maxStage = 0;
            baseCatchRate = 30;
            moveSet.Add(new Struggle());
            moveSet.Add(new MonMonStruggle());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Ditto";
                    baseHP = 48; baseAttack = 48; baseDefence = 48; baseSpeed = 48;
                    imageSource = "pkm_ditto.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Pikachu : Pokemon {
        public Pikachu(int level, int stage) : base(level, stage) {
            type = "Electric";
            maxStage = 1;
            baseCatchRate = 50;
            moveSet.Add(new Thunderbolt());
            moveSet.Add(new IronTail());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Pikachu";
                    baseHP = 35; baseAttack = 55; baseDefence = 50; baseSpeed = 90;
                    imageSource = "pkm_pikachu.png";
                    break;
                case 1:
                    species = "Raichu";
                    baseHP = 60; baseAttack = 90; baseDefence = 80; baseSpeed = 110;
                    imageSource = "pkm_raichu.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Scorbunny : Pokemon {
        public Scorbunny(int level, int stage) : base(level, stage) {
            type = "Fire";
            maxStage = 2;
            baseCatchRate = 40;
            moveSet.Add(new PyroBall());
            moveSet.Add(new DoubleKick());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Scorbunny";
                    baseHP = 50; baseAttack = 71; baseDefence = 40; baseSpeed = 69;
                    imageSource = "pkm_scorbunny.png";
                    break;
                case 1:
                    species = "Raboot";
                    baseHP = 65; baseAttack = 86; baseDefence = 60; baseSpeed = 94;
                    imageSource = "pkm_raboot.png";
                    break;
                case 2:
                    species = "Cinderace";
                    baseHP = 80; baseAttack = 116; baseDefence = 75; baseSpeed = 119;
                    imageSource = "pkm_cinderace.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Popplio : Pokemon {
        public Popplio(int level, int stage) : base(level, stage) {
            type = "Water";
            maxStage = 2;
            baseCatchRate = 40;
            moveSet.Add(new HyperVoice());
            moveSet.Add(new HydroPump());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Popplio";
                    baseHP = 50; baseAttack = 66; baseDefence = 56; baseSpeed = 40;
                    imageSource = "pkm_popplio.png";
                    break;
                case 1:
                    species = "Brionne";
                    baseHP = 60; baseAttack = 91; baseDefence = 81; baseSpeed = 50;
                    imageSource = "pkm_brionne.png";
                    break;
                case 2:
                    species = "Primarina";
                    baseHP = 80; baseAttack = 126; baseDefence = 116; baseSpeed = 60;
                    imageSource = "pkm_primarina.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Shaymin : Pokemon {
        public Shaymin(int level, int stage) : base(level, stage) {
            type = "Grass";
            maxStage = 0;
            baseCatchRate = 10;
            moveSet.Add(new SeedFlare());
            moveSet.Add(new EnergyBall());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Shaymin";
                    baseHP = 100; baseAttack = 100; baseDefence = 100; baseSpeed = 100;
                    imageSource = "pkm_shaymin.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Morpeko : Pokemon {
        public Morpeko(int level, int stage) : base(level, stage) {
            type = "Dark";
            maxStage = 0;
            baseCatchRate = 30;
            moveSet.Add(new AuraWheel());
            moveSet.Add(new Crunch());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Morpeko";
                    baseHP = 58; baseAttack = 95; baseDefence = 58; baseSpeed = 97;
                    imageSource = "pkm_morpeko.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Jirachi : Pokemon {
        public Jirachi(int level, int stage) : base(level, stage) {
            type = "Psychic";
            maxStage = 0;
            baseCatchRate = 10;
            moveSet.Add(new DoomDesire());
            moveSet.Add(new Psychic());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Jirachi";
                    baseHP = 100; baseAttack = 100; baseDefence = 100; baseSpeed = 100;
                    imageSource = "pkm_jirachi.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Dreepy : Pokemon {
        public Dreepy(int level, int stage) : base(level, stage) {
            type = "Ghost";
            maxStage = 2;
            baseCatchRate = 20;
            moveSet.Add(new DracoMeteor());
            moveSet.Add(new ShadowBall());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Dreepy";
                    baseHP = 28; baseAttack = 60; baseDefence = 30; baseSpeed = 82;
                    imageSource = "pkm_dreepy.png";
                    break;
                case 1:
                    species = "Drakloak";
                    baseHP = 68; baseAttack = 80; baseDefence = 50; baseSpeed = 102;
                    imageSource = "pkm_drakloak.png";
                    break;
                case 2:
                    species = "Dragapult";
                    baseHP = 88; baseAttack = 120; baseDefence = 75; baseSpeed = 142;
                    imageSource = "pkm_dragapult.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class Riolu : Pokemon {
        public Riolu(int level, int stage) : base(level, stage) {
            type = "Fighting";
            maxStage = 1;
            baseCatchRate = 30;
            moveSet.Add(new AuraSphere());
            moveSet.Add(new MeteorMash());
        }

        protected override void ComputeBaseInfo(int stage) {
            switch (stage) {
                case 0:
                    species = "Riolu";
                    baseHP = 40; baseAttack = 70; baseDefence = 40; baseSpeed = 60;
                    imageSource = "pkm_riolu.png";
                    break;
                case 1:
                    species = "Lucario";
                    baseHP = 70; baseAttack = 115; baseDefence = 70; baseSpeed = 90;
                    imageSource = "pkm_lucario.png";
                    break;
                default:
                    species = "Error";
                    baseHP = -1; baseAttack = -1; baseDefence = -1; baseSpeed = -1;
                    imageSource = "pkm_missingno.png";
                    break;
            }
        }
    }

    public class StandardPokemonGenerator : IPokemonGenerator {
        private Random rand = new Random();
        protected int level;
        public StandardPokemonGenerator(int level) {
            this.level = level;
        }
        public Pokemon GeneratePokemon() {
            int roll = rand.Next(100);
            if (roll < 8)
                return new Ditto(level, 0); // 8%
            else if (roll < 28)
                return new Pikachu(level, 0); // 20%
            else if (roll < 42)
                return new Scorbunny(level, 0); // 14%
            else if (roll < 56)
                return new Popplio(level, 0); // 14%
            else if (roll < 60)
                return new Shaymin(level, 0); // 4%
            else if (roll < 76)
                return new Morpeko(level, 0); // 16%
            else if (roll < 80)
                return new Jirachi(level, 0); // 4%
            else if (roll < 90)
                return new Dreepy(level, 0); // 10%
            else
                return new Riolu( level, 0); // 10%
        }
    }

    public interface IPokemonGenerator {
        Pokemon GeneratePokemon();
    }
}