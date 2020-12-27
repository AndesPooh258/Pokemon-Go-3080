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

namespace GeneralView {

    /* General View Interface */
    public interface IGameElement {
        bool show();
        bool hide();
    }

    public interface IGameText {
        void ChangeText(string str);
    }

    public interface IComboBox {
        Object Selected { get; }
        bool SetDefault();
        void InitializeBox();
        bool Add(Object o);
        bool ClearSelected();
        bool RemoveSelected();
        void RemoveAll();
        void Refresh();
    }

    public interface IPokemonListBox {
        bool Add(Object o);
        bool Remove(Object o);
        Pokemon GetSelectedPokemon();
    }

    public interface IImageBox {
        bool ChangeImage(Pokemon p);
    }

    public interface IHPBar {
        bool ChangePokemon(Pokemon p);
        bool SetValue();
    }

    /* General View Implementation */
    public class GameElement : IGameElement {
        protected object element;
        public GameElement(Object element) {
            this.element = element;
        }
        public bool show() {
            UIElement target = element as UIElement;
            if (target != null) {
                target.Visibility = Visibility.Visible;
                return true;
            } else return false;
        }
        public bool hide() {
            UIElement target = element as UIElement;
            if (target != null) {
                target.Visibility = Visibility.Hidden;
                return true;
            } else return false;
        }
    }

    public class GameText : GameElement, IGameText {

        protected TextBlock textBlock = null;
        protected TextBox textBox = null;
        protected Button button = null;

        public GameText(TextBlock textBlock) : base(textBlock) {
            this.textBlock = textBlock;
        }
        public GameText(TextBox textBox) : base(textBox) {
            this.textBox = textBox;
        }

        public GameText(Button button) : base(button) {
            this.button = button;
        }

        public void ChangeText(string str) {
            if (textBlock != null) {
                textBlock.Text = str;
            } else if (textBox != null) {
                textBox.Text = str;
            } else if (button != null) {
                button.Content = str;
            } else return;
        }
    }

    public class GameComboBox : GameElement, IComboBox {
        protected ComboBox comboBox;
        public Object Selected { get { return comboBox.SelectedValue; } }

        protected GameComboBox(ComboBox comboBox) : base(comboBox) {
            this.comboBox = comboBox;
            RemoveAll();
            InitializeBox();
        }
        public bool SetDefault() {
            if (comboBox.Items.Count > 0) {
                comboBox.SelectedValue = comboBox.Items[0];
                return true;
            } else return false;
        }
        public virtual void InitializeBox() {
            return;
        }
        public virtual bool Add(Object o) {
            comboBox.Items.Add(o);
            return true;
        }

        public bool ClearSelected() {
            if (comboBox.SelectedValue != null) {
                comboBox.SelectedValue = null;
                return true;
            } else return false;
        }
        public bool RemoveSelected() {
            if (comboBox.SelectedValue != null) {
                comboBox.Items.Remove(comboBox.SelectedValue);
                return true;
            } else return false;
        }
        public void RemoveAll() {
            while (comboBox.Items.Count > 0)
                comboBox.Items.Remove(comboBox.Items[0]);
        }
        public void Refresh() {
            Object o = comboBox.SelectedValue;
            InitializeBox();
            comboBox.SelectedValue = o;
        }
    }

    public class GeneralPokemonBox : GameComboBox {
        public GeneralPokemonBox(ComboBox comboBox) : base(comboBox) { }
        public override void InitializeBox() {
            RemoveAll();
            foreach (Pokemon p in Player.Instance.pokemonList) {
                Add(p);
            }
        }
        public override bool Add(Object o) {
            if (o is Pokemon) {
                comboBox.Items.Add(o as Pokemon);
                return true;
            } else return false;
        }
    }

    public class GeneralItemBox : GameComboBox {
        public GeneralItemBox(ComboBox comboBox) : base(comboBox) { }

        public override void InitializeBox() {
            RemoveAll();
            foreach (Item i in Player.Instance.itemList) {
                Add(i);
            }
        }

        public override bool Add(Object o) {
            if (o is Heal) {
                comboBox.Items.Add(o as Heal);
                return true;
            } else if (o is ReviveItem) {
                comboBox.Items.Add(o as ReviveItem);
                return true;
            } else if (o is BoostItem) {
                comboBox.Items.Add(o as BoostItem);
                return true;
            } else if (o is Ball) {
                comboBox.Items.Add(o as Ball);
                return true;
            } else return false;
        }
    }

    public class GameImageBox : GameElement, IImageBox {
        protected Image image;
        public GameImageBox(Image image) : base(image) {
            this.image = image;
        }
        public bool ChangeImage(Pokemon p) {
            try {
                image.Source = new BitmapImage(new Uri(p.imageSource, UriKind.Relative));
                return true;
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
    }

    public class GameHPBar : GameElement, IHPBar {
        protected ProgressBar progressBar;
        protected Pokemon p;
        public GameHPBar(ProgressBar progressBar) : base(progressBar) {
            this.progressBar = progressBar;
            progressBar.Minimum = 0;
        }

        public bool ChangePokemon(Pokemon p) {
            if (p != null) {
                this.p = p;
                progressBar.Maximum = p.actualHP;
                return true;
            } else return false;
        }

        public bool SetValue() {
            if (p != null) {
                progressBar.Value = p.HP;
                return true;
            } else {
                progressBar.Value = 0;
                return false;
            }
        }
    }
}
