using System;
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
using System.Windows.Threading;
using System.Threading;
using ItemSpace;
using PokemonWorld;
using BattleGame;
using CatchGame;
using GeneralView;

namespace Navigation {
    /* Navigation Game Model */
    public class Spot {
        protected double x;
        protected double y;

        public double X {
            get { return x; }
            set {
                if (value > 0.0 && value < 750.0)
                    x = value;
            }
        }

        public double Y {
            get { return y; }
            set {
                if (value > 0.0 && value < 380.0)
                    y = value;
            }
        }

        private Random rand = new Random();

        public Spot() {
            shuffleCoordinate();
        }

        public Spot(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public void shuffleCoordinate() {
            do {
                x = rand.Next(0, 720);
                y = rand.Next(0, 360);
            } while (x >= 676 && y >= 335);
        }

        public void shuffleCoordinate(int xMin, int xMax, int yMin, int yMax) {
            x = rand.Next(xMin, xMax);
            y = rand.Next(yMin, yMax);
        }
    }

    public interface IMapModel {
        Spot PlayerPlace { get; }
        Spot CatchPokemonPlace { get; }
        Spot GymBattlePlace { get; }
        Spot GetItemPlace { get; }
    }

    public class MapModel : IMapModel {
        public Spot PlayerPlace { get; protected set; }
        public Spot CatchPokemonPlace { get; protected set; }
        public Spot GymBattlePlace { get; protected set; }
        public Spot GetItemPlace { get; protected set; }

        public MapModel() {
            PlayerPlace = new Spot(603, 345);
            Thread.Sleep(25);
            CatchPokemonPlace = new Spot(153, 112);
            Thread.Sleep(25);
            GymBattlePlace = new Spot(185, 234);
            Thread.Sleep(25);
            GetItemPlace = new Spot(187, 360);
        }
    }

    /* Navigation Game View Implementation*/
    public class MapCanvas : GameElement {
        protected Canvas canvas;
        protected Rectangle trainer;
        protected TextBlock CatchPokemonBlock;
        protected TextBlock GymBattleBlock;
        protected TextBlock GetItemBlock;

        public MapCanvas(Canvas canvas, Rectangle trainer, TextBlock CatchPokemonBlock, TextBlock GymBattleBlock, TextBlock GetItemBlock) : base(canvas) {
            this.canvas = canvas;
            this.trainer = trainer;
            this.CatchPokemonBlock = CatchPokemonBlock;
            this.GymBattleBlock = GymBattleBlock;
            this.GetItemBlock = GetItemBlock;
        }

        public void SetPlayer(double x, double y) {
            Canvas.SetLeft(trainer, x);
            Canvas.SetTop(trainer, y);
        }

        public void SetItem(double x, double y, string target) {
            TextBlock textBlock;
            switch (target) {
                case "pokemon":
                    textBlock = CatchPokemonBlock;
                    break;
                case "gym":
                    textBlock = GymBattleBlock;
                    break;
                case "item":
                    textBlock = GetItemBlock;
                    break;
                default:
                    return;
            }
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
        }
    }

    /* Navigation Game Presenter */
    public class MapPresenter : GamePresenter {
        protected IMapModel model;
        protected MapCanvas canvas;
        protected List<IGameElement> GameElements = new List<IGameElement>();
        protected ItemFactory igen;
        protected BattlePresenter battlePresenter;
        protected CatchPresenter catchPresenter;

        public MapPresenter(MapModel model, MapCanvas canvas, Button ViewPokemonButton, ItemFactory igen, BattlePresenter battlePresenter, CatchPresenter catchPresenter) {
            // inter-presenter connection
            this.battlePresenter = battlePresenter;
            this.catchPresenter = catchPresenter;

            // initialize model
            this.model = model;
            this.igen = igen;

            // initialize view interface
            this.canvas = canvas;

            // initialize game view
            GameElements.Add(canvas);
            GameElements.Add(new GameElement(ViewPokemonButton));
            InitializeTimer();
        }

        /* Timer-related function */
        protected void InitializeTimer() {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(ControlView);
            timer.Tick += new EventHandler(MovePlayer);
            timer.Tick += new EventHandler(PositionObserver);
            timer.Start();
        }

        protected void ControlView(object sender, EventArgs e) {
            if (Player.Instance.GameMode == 1)
                showAll(GameElements);
            else hideAll(GameElements);
        }

        protected void MovePlayer(object sender, EventArgs e) {
            if (Player.Instance.GameMode == 1) {
                if (Keyboard.IsKeyDown(Key.Left)) {
                    model.PlayerPlace.X -= .05;
                }
                if (Keyboard.IsKeyDown(Key.Up)) {
                    model.PlayerPlace.Y -= .05;
                }
                if (Keyboard.IsKeyDown(Key.Down)) {
                    model.PlayerPlace.Y += .05;
                }
                if (Keyboard.IsKeyDown(Key.Right)) {
                    model.PlayerPlace.X += .05;
                }
                canvas.SetPlayer(model.PlayerPlace.X, model.PlayerPlace.Y);
            }
        }

        protected void PositionObserver(object sender, EventArgs e) {
            if (Player.Instance.GameMode == 1) {
                if (GetDistance(model.PlayerPlace, model.CatchPokemonPlace) < 30.0) {
                    MessageBox.Show("A Pokemon Encountered!");
                    catchPresenter.restart();
                    model.CatchPokemonPlace.shuffleCoordinate();
                    canvas.SetItem(model.CatchPokemonPlace.X, model.CatchPokemonPlace.Y, "pokemon");
                } else if (GetDistance(model.PlayerPlace, model.GymBattlePlace) < 30.0) {
                    battlePresenter.restart();
                    model.GymBattlePlace.shuffleCoordinate();
                    canvas.SetItem(model.GymBattlePlace.X, model.GymBattlePlace.Y, "gym");
                } else if (GetDistance(model.PlayerPlace, model.GetItemPlace) < 30.0) {
                    MessageBox.Show(GetItem());
                    model.GetItemPlace.shuffleCoordinate();
                    canvas.SetItem(model.GetItemPlace.X, model.GetItemPlace.Y, "item");
                }
            }
        }

        /* Helper functions */
        protected static double GetDistance(Spot spot1, Spot spot2) {
            return Math.Sqrt(Math.Pow(spot1.X - spot2.X, 2) + Math.Pow(spot1.Y - spot2.Y, 2));
        }

        protected string GetItem() {
            string result = "Item Obtained:\n";
            for (int i = 0; i < 3; i++) {
                Item item = igen.ProduceItem();
                Player.Instance.itemList.Add(item);
                Thread.Sleep(25);
                result += item.ToString() + "\n";
            }
            return result;
        }
    }
}