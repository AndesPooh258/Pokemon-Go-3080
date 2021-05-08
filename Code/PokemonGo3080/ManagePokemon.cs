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
using PokemonSpace;
using ItemSpace;
using PokemonWorld;

namespace ManagePokemon {

    /* Manage Pokemon Presenter */
    public class ManagePresenter : GamePresenter{
        protected List<IGameElement> GameElements = new List<IGameElement>();
        protected IGameText PokemonName;
        protected IGameText basicInfo;
        protected IGameText pokemonStat;
        protected IGameText pokemonMove;
        protected IComboBox pokemonBox;
        protected IComboBox itemBox;
        protected IImageBox player_pkm_image;
        protected IHPBar player_pkm_hp;

        public ManagePresenter(GameText PokemonName, GameText basicInfo, GameText pokemonStat, GameText pokemonMove, GeneralPokemonBox pokemonBox, GeneralItemBox itemBox,
                               GameImageBox player_pkm_image, GameHPBar player_pkm_hp, Button lv_up_Button, Button evolve_Button, 
                               Button item_Button, Button run_button, Button release_button) {
            /* initialize view interface */
            this.PokemonName = PokemonName;
            this.basicInfo = basicInfo;
            this.pokemonStat = pokemonStat;
            this.pokemonMove = pokemonMove;
            this.pokemonBox = pokemonBox;
            this.itemBox = itemBox;
            this.player_pkm_image = player_pkm_image;
            this.player_pkm_hp = player_pkm_hp;

            /* initialize game view */
            GameElements.Add(PokemonName);
            GameElements.Add(basicInfo);
            GameElements.Add(pokemonStat);
            GameElements.Add(pokemonMove);
            GameElements.Add(player_pkm_image);
            GameElements.Add(player_pkm_hp);
            GameElements.Add(new GameElement(lv_up_Button));
            GameElements.Add(new GameElement(evolve_Button));
            GameElements.Add(new GameElement(item_Button));
            GameElements.Add(new GameElement(run_button));
            GameElements.Add(new GameElement(release_button));
            initializeGameView();

        }

        protected void initializeGameView() {
            pokemonBox.InitializeBox();
            itemBox.InitializeBox();
            pokemonBox.SetDefault();
            itemBox.SetDefault();
            UpdatePokemonInfo();
            showAll(GameElements);
        }

        /* Generate Pokemon information */
        public bool UpdatePokemonInfo() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            if (selectedPokemon != null) {
                player_pkm_image.ChangeImage(selectedPokemon);
                player_pkm_hp.ChangePokemon(selectedPokemon);
                player_pkm_hp.SetValue();
                PokemonName.ChangeText(selectedPokemon.name);
                basicInfo.ChangeText(GenerateBasicInfo(selectedPokemon));
                pokemonStat.ChangeText(GenerateStat(selectedPokemon));
                pokemonMove.ChangeText(GenerateMoveInfo(selectedPokemon));
                return true;
            } else return false;
        }

        protected string GenerateBasicInfo(Pokemon p) {
            string info = "";
            info += "Species: " + p.ToString() + "\n";
            info += "Level: " + p.level + " \n";
            info += "Type: " + p.type + " \n";
            return info;
        }

        protected string GenerateStat(Pokemon p) {
            string stat = "";
            stat += "HP: " + p.HP + "/" + p.actualHP + "\n";
            stat += "Attack: " + p.actualAttack + "\n";
            stat += "Defence: " + p.actualDefence + "\n";
            stat += "Speed: " + p.actualSpeed;
            return stat;
        }

        protected string GenerateMoveInfo(Pokemon p) {
            string info = "";
            info += "Move 1: " + p.moveSet[0].ToString() + "\n";
            info += "Power: " + p.moveSet[0].power.ToString() + " Accuracy: " + p.moveSet[0].accuracy.ToString() + "\n";
            info += "Move 2: " + p.moveSet[1].ToString() + "\n";
            info += "Power: " + p.moveSet[1].power.ToString() + " Accuracy: " + p.moveSet[1].accuracy.ToString() + "\n";
            return info;
        }

        /* Player's Operation */
        public bool LevelUp() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            Item selectedItem = itemBox.Selected as Item;
            if (selectedPokemon != null && selectedItem is RareCandy) {
                return UseItem();
            } else return false;
        }

        public bool Evolve() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            Item selectedItem = itemBox.Selected as Item;
            if (selectedPokemon != null && selectedItem is EvolveStone) {
                return UseItem();
            } else return false;
        }

        public bool Heal() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            Item selectedItem = itemBox.Selected as Item;
            if (selectedPokemon != null && (selectedItem is Heal || selectedItem is ReviveItem)) {
                return UseItem();
            } else return false;
        }

        protected bool UseItem() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            Item selectedItem = itemBox.Selected as Item;
            if (selectedPokemon != null && selectedItem != null) {
                if (selectedItem.OnUse(selectedPokemon)) {
                    // remove selected item if it is used successfully
                    Player.Instance.itemList.Remove(selectedItem);
                    itemBox.RemoveSelected();
                    UpdatePokemonInfo();
                    pokemonBox.Refresh();
                    return true;
                } else return false;
            } else return false;
        }

        public bool Rename(string str) {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            if (selectedPokemon != null) {
                if (str.Length <= 12) {
                    selectedPokemon.Rename(str);
                    return true;
                } else {
                    MessageBox.Show("The maximum length of a name is 12!");
                    return false;
                }
            } else return false;
        }

        public bool Release() {
            Pokemon selectedPokemon = pokemonBox.Selected as Pokemon;
            // player cannot release pokemon if he has only 1 pokemon
            if (Player.Instance.pokemonList.Count > 1) {
                string message = "Are you sure to release " + selectedPokemon.name + "?";
                if (MessageBox.Show(message, "Release Pokemon", MessageBoxButton.OKCancel) == MessageBoxResult.OK) {
                    Player.Instance.pokemonList.Remove(selectedPokemon);
                    pokemonBox.RemoveSelected();
                    pokemonBox.SetDefault();
                    return true;
                } else return false;
            } else return false;
        }

        public bool restart() {
            if (Player.Instance.GameMode == 1) {
                Player.Instance.GameMode = 4;
                initializeGameView();
                return true;
            } else return false;
        }

        public bool run() {
            Player.Instance.GameMode = 1;
            hideAll(GameElements);
            return true;
        }
    }
}