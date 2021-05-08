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
using PokemonSpace;
using ItemSpace;
using PokemonWorld;
using BattleGame;
using CatchGame;
using ManagePokemon;
using Navigation;
using GeneralView;


namespace PokemonGo3080 {
    
    public partial class MainWindow : Window {

        protected Player player;
        protected BattlePresenter battlePresenter;
        protected CatchPresenter catchPresenter;
        protected ManagePresenter managePresenter;
        protected MapPresenter mapPresenter;

        public MainWindow() {
            InitializeComponent();
            InitializeGame();
        }

        public void InitializeGame() {
            Player.Instance.StartGame();
            battlePresenter = new BattlePresenter(new StandardPokemonGenerator(7), new GameText(message), new BattlePokemonBox(pokemonBox), new BattleItemBox(itemBox), selectedPokemonBox,
                                new GameImageBox(master_pkm_1), new GameImageBox(master_pkm_2), new GameImageBox(master_pkm_3),
                                new GameImageBox(player_pkm_image), new GameImageBox(master_pkm_image), new GameHPBar(player_pkm_hp),
                                new GameHPBar(master_pkm_hp), new GameText(move0_button), new GameText(move1_button), item_button,
                                add_button, remove_button, start_button, run_button, selectedPokemonBlock, gym_master_pokemon);
            catchPresenter = new CatchPresenter(new StandardPokemonGenerator(5), new WOFWheel(mainCanvas), new GameText(status),
                             new GeneralPokemonBox(pokemonBox), new WOFItemBox(itemBox), new GameImageBox(image_box), indicator, spin_button, stop_button, run_button, ColorExplanation);
            managePresenter = new ManagePresenter(new GameText(Pokemon_Name), new GameText(basicInfo), new GameText(pokemonStat), new GameText(pokemonMove),
                                      new GeneralPokemonBox(pokemonBox), new GeneralItemBox(itemBox),
                                      new GameImageBox(player_pkm_image), new GameHPBar(player_pkm_hp),
                                      lv_up_Button, evolve_Button, item_button, run_button, release_button);
            mapPresenter = new MapPresenter(new MapModel(), new MapCanvas(TrainerCanvas, Trainer, CatchPokemonBlock, GymBattleBlock, GetItemBlock),
                                            ViewPokemonButton, new StandardItemGenerator(), battlePresenter, catchPresenter);
            battlePresenter.run();
            catchPresenter.run();
            managePresenter.run();
        }

        /* Controls related to Navigation */
        private void View_Pokemon_Button_Click(object sender, RoutedEventArgs e) {
            managePresenter.restart();
        }

        /* Controls related to Catch Game */
        private void itemBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (catchPresenter != null) {
                catchPresenter.ChangeItem();
            } else return;
        }

        private void spin_button_Click(object sender, RoutedEventArgs e) {
            catchPresenter.RollWheel();
        }

        private void stop_button_Click(object sender, RoutedEventArgs e) {
            catchPresenter.StopWheel();
        }

        /* Controls related to Gym Battle */
        private void move0_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.RunTurn(0);
        }

        private void move1_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.RunTurn(1);
        }

        private void item_button_Click(object sender, RoutedEventArgs e) {
            if (Player.Instance.GameMode == 3) {
                battlePresenter.RunTurn(2);
            } else if (Player.Instance.GameMode == 4) {
                managePresenter.Heal();
            } else return;
        }

        private void start_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.StartGame();
        }

        private void add_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.addPokemon(pokemonBox.SelectedItem);
        }

        private void remove_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.RemoveSelected();
        }

        /* Controls related to Navigation */
        private void pokemonBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (managePresenter != null) {
                managePresenter.UpdatePokemonInfo();
            } else return;
        }

        private void lv_up_Button_Click(object sender, RoutedEventArgs e) {
            if (managePresenter != null) {
                managePresenter.LevelUp();
            }
        }

        private void evolve_Button_Click(object sender, RoutedEventArgs e) {
            if (managePresenter != null) {
                managePresenter.Evolve();
            }
        }

        private void release_button_Click(object sender, RoutedEventArgs e) {
            if (managePresenter != null) {
                managePresenter.Release();
            }
        }

        private void PokemonName_TextChanged(object sender, TextChangedEventArgs e) {
            if (managePresenter != null) {
                managePresenter.Rename(Pokemon_Name.Text);
            }
        }

        /* General Controls */
        private void run_button_Click(object sender, RoutedEventArgs e) {
            battlePresenter.run();
            catchPresenter.run();
            managePresenter.run();
        }
    }
}
