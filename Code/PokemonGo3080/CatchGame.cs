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
using GeneralView;

namespace CatchGame {

    /* Catch Game Model */
    public interface ICatchModel {
        Pokemon target { get; }
        int GameState { get; set; }
        int WheelState { get; set; }
        int WheelAngle { get; set; }
        int CatchRate { get; set; }
        int evaluateGameState();
    }

    public class CatchModel : ICatchModel {
        protected int gameState; // -1: escape, 0: processing, 1: success
        protected int wheelState; // -1: Wheel slowing down, 0: Wheel stop, 1: Wheel running
        protected int wheelAngle;
        protected int catchRate;

        public CatchModel(IPokemonGenerator gen) {
            gameState = 0;
            wheelState = 0;
            Player.Instance.GameMode = 2;
            target = gen.GeneratePokemon();
        }

        public Pokemon target { get; protected set; }

        public int GameState {
            get {
                return gameState;
            }
            set {
                if (value >= -1 && value <= 1)
                    gameState = value;
            }
        }

        public int WheelState {
            get {
                return wheelState;
            }
            set {
                if (value >= -1 && value <= 1)
                    wheelState = value;
            }
        }

        public int WheelAngle {
            get {
                return wheelAngle;
            }
            set {
                if (value >= 0 && value <= 360)
                    wheelAngle = value;
            }
        }

        public int CatchRate {
            get {
                return catchRate;
            }
            set {
                if (value < 0) {
                    catchRate = 0;
                } else if (value > 100) {
                    catchRate = 100;
                } else catchRate = value;
            }
        }

        public int evaluateGameState() {
            if (wheelAngle > 275)
                wheelAngle -= 360;
            if (wheelAngle >= 275 - catchRate * 3.6) {
                gameState = 1;
            } else if (wheelAngle >= 275 - (50 + catchRate / 2) * 3.6) {
                gameState = 0;
            } else {
                gameState = -1;
            }
            return gameState;
        }
    }

    /* Catch Game View Interface */
    public interface IWOFWheel {
        void ResetWheel(int catchRate);
        void PrintWheel(int rotateAngle);
        void ClearChildren();
    }

    /* Catch Game View Implementation */
    public class WOFItemBox : GameComboBox {
        public WOFItemBox(ComboBox comboBox) : base(comboBox) { }

        public override void InitializeBox() {
            RemoveAll();
            foreach (Item i in Player.Instance.itemList) {
                if (i is Ball) {
                    Add(i);
                }
            }
        }

        public override bool Add(Object o) {
            if (o is Ball) {
                comboBox.Items.Add(o as Ball);
                return true;
            } else return false;
        }
    }

    public class Category {
        public float Percentage { get; set; }
        public string Title { get; set; }
        public Brush ColorBrush { get; set; }
    }

    public class WOFWheel : GameElement, IWOFWheel {
        protected Canvas mainCanvas;
        protected int blueSize, greySize, purpleSize;
        public WOFWheel(Canvas mainCanvas) : base(mainCanvas) {
            this.mainCanvas = mainCanvas;
        }
        public void ResetWheel(int catchRate) {
            mainCanvas.Children.Clear();
            if (catchRate < 100) {
                if (catchRate < 0)
                    catchRate = 0;
                purpleSize = catchRate;
                blueSize = (100 - catchRate) / 2;
                greySize = 100 - purpleSize - blueSize;
                PrintWheel(0);
            } else {
                purpleSize = 100; blueSize = 0; greySize = 0;
                Ellipse circle = new Ellipse();
                circle.Width = 350;
                circle.Height = 350;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9370DB"));
                circle.Fill = mySolidColorBrush;
                mainCanvas.Children.Add(circle);
            }
        }

        public void PrintWheel(int rotateAngle) {
            float pieWidth = 350, pieHeight = 350, centerX = pieWidth / 2, centerY = pieHeight / 2, radius = pieWidth / 2;
            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;
            List<Category> Categories = new List<Category>()
            {
                new Category
                {
                    Title = "Purple",
                    Percentage = purpleSize,
                    ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9370DB")),
                },

                new Category
                {
                    Title = "Blue",
                    Percentage = blueSize,
                    ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#87CEFA")),
                },

                new Category
                {
                    Title = "Grey",
                    Percentage = greySize,
                    ColorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#708090")),
                },
           };

            // draw pie
            float angle = 0, prevAngle = 0;
            foreach (var category in Categories) {
                double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                angle = category.Percentage * (float)360 / 100 + prevAngle;

                double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                double arcWidth = radius, arcHeight = radius;
                bool isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment() {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc,
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), false);

                var pathFigure = new PathFigure(
                   new Point(centerX, centerY),
                   new List<PathSegment>()
                   {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                   },
                  true);

                var pathFigures = new List<PathFigure>() { pathFigure, };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new Path() {
                    Fill = category.ColorBrush,
                    Data = pathGeometry,
                };
                RotateTransform rt = new RotateTransform(rotateAngle, centerX, centerY);
                path.RenderTransform = rt;
                mainCanvas.Children.Add(path);

                prevAngle = angle;
            }
        }
        public void ClearChildren() {
            mainCanvas.Children.Clear();
        }
    }

    /* Catch Game Presenter */
    public class CatchPresenter : GamePresenter {
        protected ICatchModel model;
        protected IPokemonGenerator gen;
        protected List<IGameElement> GameElements = new List<IGameElement>();
        protected IGameText textBlock;
        protected IWOFWheel gameWheel;
        protected IComboBox pokemonBox;
        protected IComboBox itemBox;
        protected IImageBox image;

        public CatchPresenter(IPokemonGenerator gen, WOFWheel gameWheel, GameText textBlock, GeneralPokemonBox pokemonBox, WOFItemBox itemBox,
                            GameImageBox image, Rectangle indicator, Button spin_button, Button stop_button, Button escape_button, TextBlock ColorExplanation) {
            // initialize model
            this.gen = gen;
            this.model = new CatchModel(gen);

            // initialize view interface
            this.gameWheel = gameWheel;
            this.textBlock = textBlock;
            this.pokemonBox = pokemonBox;
            this.itemBox = itemBox;
            this.image = image;

            // initialize game view
            GameElements.Add(gameWheel);
            GameElements.Add(textBlock);
            GameElements.Add(image);
            GameElements.Add(new GameElement(indicator));
            GameElements.Add(new GameElement(spin_button));
            GameElements.Add(new GameElement(stop_button));
            GameElements.Add(new GameElement(escape_button));
            GameElements.Add(new GameElement(ColorExplanation));
            initializeGameView();
        }

        protected void initializeGameView() {
            textBlock.ChangeText(model.target.ToString() + " encountered!");
            pokemonBox.InitializeBox();
            itemBox.InitializeBox();
            pokemonBox.SetDefault();
            itemBox.SetDefault();
            Ball selectedItem = itemBox.Selected as Ball;
            if (selectedItem != null) {
                ChangeWheel(selectedItem.catchMultiplier);
            } else ChangeWheel(0);
            image.ChangeImage(model.target);
            showAll(GameElements);
        }

        public bool RollWheel() {
            // prevent double clicking
            Ball selectedItem = itemBox.Selected as Ball;
            if (selectedItem != null && model.GameState == 0 && model.WheelState == 0) {
                model.WheelState = 1;
                counter = 0;
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromSeconds(0.01);
                dispatcherTimer.Start();
                Player.Instance.itemList.Remove(selectedItem);
                itemBox.RemoveSelected();
                return true;
            } else return false;
        }

        public bool StopWheel() {
            // prevent stoping a stopped wheel
            if (model.GameState == 0 && model.WheelState == 1) {
                model.WheelState = -1;
                return true;
            } else return false;
        }

        public bool ChangeItem() {
            Ball b = itemBox.Selected as Ball;
            if (b != null) {
                ChangeWheel(b.catchMultiplier);
                return true;
            } else return false;
        }

        protected bool ChangeWheel(double catchMultiplier) {
            if (model.GameState == 0) {
                model.CatchRate = (int)(model.target.catchRate * catchMultiplier);
                gameWheel.ResetWheel(model.CatchRate);
                return true;
            } else return false;
        }

        public bool run() {
            if (model.WheelState == 0) {
                model.GameState = -1;
                textBlock.ChangeText("You run successfully!");
                itemBox.ClearSelected();
                hideAll(GameElements);
                Player.Instance.GameMode = 1;
                return true;
            } else return false;
        }

        public bool restart() {
            if (Player.Instance.GameMode == 1) {
                model = new CatchModel(gen);
                initializeGameView();
                return true;
            } else return false;
        }

        protected System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        protected int counter;

        protected void dispatcherTimer_Tick(object sender, EventArgs e) {
            model.WheelAngle = (counter * 10) % 360;
            if (model.WheelState == -1) {
                dispatcherTimer.Interval += TimeSpan.FromSeconds(0.002);
                if (dispatcherTimer.Interval > TimeSpan.FromSeconds(0.075)) {
                    model.WheelState = 0;
                    dispatcherTimer.Tick -= dispatcherTimer_Tick;
                    dispatcherTimer.Stop();
                    int state = model.evaluateGameState();
                    if (state == 1) {
                        Player.Instance.pokemonList.Add(model.target);
                        pokemonBox.Add(model.target);
                        MessageBox.Show("You catched " + model.target.ToString() + "!");
                        Player.Instance.GameMode = 1;
                        hideAll(GameElements);
                    } else if (state == 0) {
                        textBlock.ChangeText("It left the ball!");
                        ChangeWheel(0);
                    } else {
                        MessageBox.Show("The pokemon escaped!");
                        Player.Instance.GameMode = 1;
                        hideAll(GameElements);
                    }
                    itemBox.SetDefault();
                    return;
                }
            }
            counter++;
            gameWheel.PrintWheel(model.WheelAngle);
        }
    }
}
