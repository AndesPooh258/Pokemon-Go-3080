using System;
using System.Collections.Generic;
using PokemonSpace;
using ItemSpace;
using GeneralView;

namespace PokemonWorld {
    public class Player {
        public List<Pokemon> pokemonList;
        public List<Item> itemList;
        private static Player instance;
        public static Player Instance {
            get {
                if (instance == null)
                    instance = new Player();
                return instance;
            }
        }

        public void StartGame() { }

        // gamemode: 1(Navigation), 2(Capture), 3(Gym-Battle), 4(See info)
        protected int gameMode;
        public int GameMode {
            get {
                return gameMode;
            }
            set {
                if (value >= 1 && value <= 4)
                    gameMode = value;
            }
        }

        // constructor
        private Player() {
            pokemonList = new List<Pokemon>();
            pokemonList.Add(new Pikachu(5, 0));
            itemList = new List<Item>();
            for (int i = 0; i < 5; i++) {
                itemList.Add(new PokeBall());
            }
            for (int i = 0; i < 3; i++) {
                itemList.Add(new Potion());
            }
            gameMode = 1;
        }
    }

    public class GamePresenter {
        protected void showAll(List<IGameElement> list) {
            foreach (IGameElement i in list) {
                i.show();
            }
        }

        protected void hideAll(List<IGameElement> list) {
            foreach (IGameElement i in list) {
                i.hide();
            }
        }
    }
}