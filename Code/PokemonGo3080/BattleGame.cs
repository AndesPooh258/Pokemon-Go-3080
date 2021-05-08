using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using GeneralView;
using MoveSpace;
using PokemonSpace;
using ItemSpace;
using PokemonWorld;

namespace BattleGame {

    /* Battle Game Model */
    public class BattleTurn {
        // general information
        private Random rand = new Random();
        protected Pokemon[] p = new Pokemon[2];
        protected int[] Move = new int[2]; // 0: Move 1, 1: Move 2, 2: Use Potion, 3: Use Super Potion, 4: Hyper Potion
        protected Move[,] MoveArr = new Move[2, 2];
        protected bool[] Attack = new bool[2];
        protected string output = "";

        // constructor
        public BattleTurn(Pokemon p0, Pokemon p1, int p0Move, int p1Move) {
            p[0] = p0; p[1] = p1;
            Move[0] = p0Move; Move[1] = p1Move;
            // extract move from dictionary
            for (int i = 0, j = 0; i < 2; i++, j = 0) {
                foreach (Move move in p[i].moveSet) {
                    MoveArr[i, j] = move;
                    j++;
                }
            }
        }

        public void RunBattleTurn() {
            // healing operation is in first priority
            for (int i = 0; i < 2; i++) {
                if (p[i].HP <= 0) {
                    return;
                } else HealOperation(i);
            }
            // compare actual speed of two pokemon
            if (p[0].actualSpeed >= p[1].actualSpeed) {
                AttackOperation(0, 1);
                AttackOperation(1, 0);
            } else {
                AttackOperation(1, 0);
                AttackOperation(0, 1);
            }
        }

        protected void AttackOperation(int attacker, int defencer) {
            if (Attack[attacker] == false) {
                return;
            } else {
                output += p[attacker].name + " use " + MoveArr[attacker, Move[attacker]] + " on " + p[defencer].name;
                int damage = DamageCalculation(attacker, defencer);
                if (damage == 0) {
                    output += ", but missed! ";
                } else {
                    p[defencer].HP -= damage;
                    output += ". ";
                    // output += ", making " + damage + " damages. ";
                }
                // the defender is fainted when HP <= 0, HP cannot be lower than 0
                if (p[defencer].HP <= 0) {
                    output += p[defencer].name + " fainted. ";
                    p[defencer].HP = 0;
                    Attack[defencer] = false;
                }
            }
        }

        protected void HealOperation(int number) {
            if (Move[number] >= 2) {
                Attack[number] = false;
                switch (Move[number]) {
                    case 2:
                        output += p[number].name + " use Potion. ";
                        break;
                    case 3:
                        output += p[number].name + " use Super Potion. ";
                        break;
                    case 4:
                        output += p[number].name + " use Hyper Potion. ";
                        break;
                    default:
                        break;
                }
            } else Attack[number] = true;
        }

        // damage calculation following https://bulbapedia.bulbagarden.net/wiki/Damage
        protected int DamageCalculation(int attacker, int defencer) {
            int result = rand.Next(100);
            // check whether the move is missed
            if (result < MoveArr[attacker, Move[attacker]].accuracy) {
                double modifier = rand.NextDouble() * 0.15 + 0.85;
                int damage = (int)((((0.4 * p[attacker].level + 2) * MoveArr[attacker, Move[attacker]].power
                    * p[attacker].actualAttack / p[defencer].actualDefence) / 50.0 + 2) * modifier);
                if (damage <= 0) {
                    // minimum damage is 1
                    return 1;
                } else return damage;
            } else return 0;
        }

        // convert general information into a string
        public override string ToString() {
            return output;
        }
    }

    public interface IBattleModel {
        int GameState { get; set; }
        Pokemon playerPokemon { get; }
        Pokemon masterPokemon { get; }
        bool InitiateGame(List<Pokemon> playerPokemonList);
        string RunGymBattleTurn(int playerMove);
        List<Pokemon> GetMasterPokemonList();
    }

    public class BattleModel : IBattleModel {
        protected int gameState; // -1: escape, 0: choosing pokemon, 1: on battle, 2: waiting, 3: player win, 4: player lose
        protected Random rand = new Random();
        protected List<Pokemon> playerPokemonList;
        protected List<Pokemon> masterPokemonList;
        protected BattleTurn bt;
        protected int playerNumOfPkm, masterNumOfPkm, playerCurrentPokemon, masterCurrentPokemon;

        public int GameState {
            get {
                return gameState;
            }
            set {
                if (value >= -1 && value <= 4)
                    gameState = value;
            }
        }

        public Pokemon playerPokemon {
            get {
                if (playerNumOfPkm > 0)
                    return playerPokemonList[playerCurrentPokemon];
                else return null;
            }
        }

        public Pokemon masterPokemon {
            get {
                if (masterNumOfPkm > 0)
                    return masterPokemonList[masterCurrentPokemon];
                else return null;
            }
        }

        public BattleModel(IPokemonGenerator gen) {
            gameState = 0;
            Player.Instance.GameMode = 3;
            masterPokemonList = new List<Pokemon>();
            for (int i = 0; i < 3; i++) {
                masterPokemonList.Add(gen.GeneratePokemon());
            }
        }

        public bool InitiateGame(List<Pokemon> playerPokemonList) {
            if (gameState == 0) {
                this.playerPokemonList = playerPokemonList;
                playerNumOfPkm = playerPokemonList.Count;
                masterNumOfPkm = masterPokemonList.Count;
                playerCurrentPokemon = 0;
                masterCurrentPokemon = 0;
                gameState = 1;
                return true;
            } else return false;
        }

        public string RunGymBattleTurn(int playerMove) {
            string output = "";
            int masterMove = rand.Next(2);

            if (gameState == 1) {
                if (playerPokemon.HP > 0 && masterPokemon.HP > 0) {
                    bt = new BattleTurn(playerPokemon, masterPokemon, playerMove, masterMove);
                    bt.RunBattleTurn();
                    output = bt.ToString();
                }
                if (playerPokemon.HP == 0) {
                    playerNumOfPkm--;
                    playerCurrentPokemon++;
                } else if (masterPokemon.HP == 0) {
                    masterNumOfPkm--;
                    masterCurrentPokemon++;
                }
            }

            if (masterNumOfPkm <= 0) {
                gameState = 3;
            } else if (playerNumOfPkm <= 0) {
                gameState = 4;
            } else gameState = 2;
            return output;
        }

        public List<Pokemon> GetMasterPokemonList() {
            return masterPokemonList;
        }
    }

    /* Battle Game View Implementation */
    public class BattlePokemonBox : GameComboBox {
        public BattlePokemonBox(ComboBox comboBox) : base(comboBox) { }

        public override void InitializeBox() {
            RemoveAll();
            foreach (Pokemon p in Player.Instance.pokemonList) {
                Add(p);
            }
        }

        public override bool Add(Object o) {
            Pokemon p = o as Pokemon;
            if (p != null && p.HP > 0) {
                comboBox.Items.Add(o as Pokemon);
                return true;
            } else return false;
        }
    }

    public class BattleItemBox : GameComboBox {
        public BattleItemBox(ComboBox comboBox) : base(comboBox) { }

        public override void InitializeBox() {
            RemoveAll();
            foreach (Item i in Player.Instance.itemList) {
                if (i is Heal) {
                    Add(i);
                }
            }
        }

        public override bool Add(Object o) {
            if (o is Heal) {
                comboBox.Items.Add(o as Heal);
                return true;
            } else return false;
        }
    }

    public class SelectedPokemonBox : GameElement, IPokemonListBox {
        protected ListBox box;
        protected List<Pokemon> list;

        public SelectedPokemonBox(ListBox box, List<Pokemon> list) : base(box) {
            this.box = box;
            this.list = list;
            box.ItemsSource = list;
            box.Items.Refresh();
        }

        public bool Add(Object o) {
            Pokemon p = o as Pokemon;
            if (p != null && list.Contains(p) == false) {
                list.Add(p);
                box.Items.Refresh();
                return true;
            } else return false;
        }

        public bool Remove(Object o) {
            Pokemon p = o as Pokemon;
            if (p != null && list.Contains(p) == true) {
                list.Remove(p);
                box.Items.Refresh();
                return true;
            } else return false;
        }

        public Pokemon GetSelectedPokemon() {
            return box.SelectedItem as Pokemon;
        }
    }

    /* Battle Game Presenter*/
    public class BattlePresenter : GamePresenter {
        protected IBattleModel model;
        protected IPokemonGenerator gen;
        protected List<IGameElement> State0Elements = new List<IGameElement>();
        protected List<IGameElement> State1Elements = new List<IGameElement>();
        protected IGameText textBlock;
        protected IGameText move0_button;
        protected IGameText move1_button;
        protected IComboBox pokemonBox;
        protected IComboBox itemBox;
        protected IPokemonListBox selectedPokemon;
        protected IImageBox master_pkm_1;
        protected IImageBox master_pkm_2;
        protected IImageBox master_pkm_3;
        protected IImageBox player_pkm_image;
        protected IImageBox master_pkm_image;
        protected IHPBar player_pkm_hp;
        protected IHPBar master_pkm_hp;
        protected List<Pokemon> selectedPokemonList;

        public BattlePresenter(IPokemonGenerator gen, GameText textBlock, BattlePokemonBox pokemonBox, BattleItemBox itemBox, ListBox listBox,
                               GameImageBox master_pkm_1, GameImageBox master_pkm_2, GameImageBox master_pkm_3,
                               GameImageBox player_pkm_image, GameImageBox master_pkm_image, GameHPBar player_pkm_hp, GameHPBar master_pkm_hp,
                               GameText move0_button, GameText move1_button, Button item_button, Button add_button, Button remove_button,
                               Button start_button, Button run_button, TextBlock selectedPokemonBlock, TextBlock gym_master_pokemon) {
            // initialize model
            this.gen = gen;
            this.model = new BattleModel(gen);

            // initialize view interface
            this.textBlock = textBlock;
            this.pokemonBox = pokemonBox;
            this.itemBox = itemBox;
            this.move0_button = move0_button;
            this.move1_button = move1_button;
            this.master_pkm_1 = master_pkm_1;
            this.master_pkm_2 = master_pkm_2;
            this.master_pkm_3 = master_pkm_3;
            this.player_pkm_image = player_pkm_image;
            this.master_pkm_image = master_pkm_image;
            this.player_pkm_hp = player_pkm_hp;
            this.master_pkm_hp = master_pkm_hp;

            // initialize game view
            selectedPokemonList = new List<Pokemon>();
            selectedPokemon = new SelectedPokemonBox(listBox, selectedPokemonList);
            State0Elements.Add(textBlock);
            State0Elements.Add(new GameElement(selectedPokemonBlock));
            State0Elements.Add(new GameElement(listBox));
            State0Elements.Add(new GameElement(add_button));
            State0Elements.Add(new GameElement(remove_button));
            State0Elements.Add(new GameElement(start_button));
            State0Elements.Add(new GameElement(run_button));
            State0Elements.Add(new GameElement(gym_master_pokemon));
            State0Elements.Add(master_pkm_1);
            State0Elements.Add(master_pkm_2);
            State0Elements.Add(master_pkm_3);
            State1Elements.Add(textBlock);
            State1Elements.Add(new GameElement(selectedPokemonBlock));
            State1Elements.Add(new GameElement(listBox));
            State1Elements.Add(player_pkm_image);
            State1Elements.Add(master_pkm_image);
            State1Elements.Add(player_pkm_hp);
            State1Elements.Add(master_pkm_hp);
            State1Elements.Add(move0_button);
            State1Elements.Add(move1_button);
            State1Elements.Add(new GameElement(item_button));
            initializeGameView();
        }

        protected void initializeGameView() {
            // show gym master's pokemon
            List<Pokemon> masterPokemonList = model.GetMasterPokemonList();
            master_pkm_1.ChangeImage(masterPokemonList[0]);
            master_pkm_2.ChangeImage(masterPokemonList[1]);
            master_pkm_3.ChangeImage(masterPokemonList[2]);
            textBlock.ChangeText("Please select not more than 3 Pokemon.");
            pokemonBox.InitializeBox();
            itemBox.InitializeBox();
            pokemonBox.SetDefault();
            itemBox.SetDefault();
            hideAll(State1Elements);
            showAll(State0Elements);
        }

        public bool addPokemon(Object o) {
            if (model.GameState == 0 && selectedPokemonList.Count < 3)
                return selectedPokemon.Add(o);
            else return false;
        }

        public bool RemoveSelected() {
            Pokemon p = selectedPokemon.GetSelectedPokemon();
            if (p != null) {
                selectedPokemon.Remove(p);
                return true;
            } else return false;
        }

        public bool StartGame() {
            if (selectedPokemonList.Count > 0) {
                bool result = model.InitiateGame(selectedPokemonList);
                hideAll(State0Elements);
                showAll(State1Elements);
                ShowTurnView();
                return result;
            } else return false;
        }

        protected void ShowTurnView() {
            if (model.playerPokemon != null) {
                move0_button.ChangeText(model.playerPokemon.moveSet[0].ToString());
                move1_button.ChangeText(model.playerPokemon.moveSet[1].ToString());
                player_pkm_image.ChangeImage(model.playerPokemon);
                player_pkm_hp.ChangePokemon(model.playerPokemon);
            }
            if (model.masterPokemon != null) {
                master_pkm_image.ChangeImage(model.masterPokemon);
                master_pkm_hp.ChangePokemon(model.masterPokemon);
            }
            player_pkm_hp.SetValue();
            master_pkm_hp.SetValue();
        }

        protected System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        protected int counter;

        public bool RunTurn(int action) {
            if (model.GameState == 1) {
                if (action >= 2) {
                    Item selectedItem = itemBox.Selected as Item;
                    if (selectedItem != null) {
                        switch (selectedItem.ToString()) {
                            case "Potion":
                                action = 2;
                                break;
                            case "Super Potion":
                                action = 3;
                                break;
                            case "Hyper Potion":
                                action = 4;
                                break;
                            default:
                                return false;
                        }
                        selectedItem.OnUse(model.playerPokemon);
                        Player.Instance.itemList.Remove(selectedItem);
                        itemBox.RemoveSelected();
                        itemBox.SetDefault();
                    } else action = 5;
                }
                textBlock.ChangeText(model.RunGymBattleTurn(action));
                counter = 0;
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Start();
                return true;
            } else return false;
        }

        protected void dispatcherTimer_Tick(object sender, EventArgs e) {
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Stop();
            ShowTurnView();
            if (model.GameState > 2) {
                switch (model.GameState) {
                    case 3:
                        MessageBox.Show("Player Win!");
                        break;
                    case 4:
                        MessageBox.Show("Player Lose...");
                        break;
                    default:
                        MessageBox.Show("Error");
                        break;
                }
                run();
            }
            model.GameState = 1;
        }

        public bool restart() {
            if (Player.Instance.GameMode == 1) {
                model = new BattleModel(gen);
                initializeGameView();
                model.GameState = 0;
                while (selectedPokemonList.Count > 0) {
                    selectedPokemonList.Remove(selectedPokemonList[0]);
                }
                return true;
            } else return false;
        }

        public bool run() {
            if (model.GameState != 1) {
                model.GameState = -1;
                Player.Instance.GameMode = 1;
                hideAll(State0Elements);
                hideAll(State1Elements);
                while (selectedPokemonList.Count > 0) {
                    selectedPokemon.Remove(selectedPokemonList[0]);
                }
                return true;
            } else return false;
        }
    }
}
