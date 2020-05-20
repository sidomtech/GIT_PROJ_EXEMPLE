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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace ClientServiceAgence
{
    /// <summary>
    /// Logique d'interaction pour AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Attributs et Propriétés

        private bool _modification;
        private ServiceAgence.BienImmobilier _bien;
        private ObservableCollection<string> _photos;
        private int _indexPhoto;

        public ServiceAgence.BienImmobilier Bien
        {
            get { return _bien; }
            set { SetField(ref _bien, value); }
        }
        public ObservableCollection<string> Photos
        {
            get { return _photos; }
        }
        public int IndexPhoto
        {
            get { return _indexPhoto; }
            set { SetField(ref _indexPhoto, value); }
        }

        #endregion


        public AddEditWindow(ServiceAgence.BienImmobilier bien = null)
        {
            InitializeComponent();
            this.DataContext = this;

            this._modification = (bien != null);
            if (bien == null)
            {
                var b = new ServiceAgence.BienImmobilier();
                b.DateMiseEnTransaction = DateTime.Now.Date;
                b.DateTransaction = null;
                b.TransactionEffectuee = false;
                this.Bien = b;
            }
            else
                this.Bien = bien;

            if (this._bien.PhotosBase64 == null)
                this._bien.PhotosBase64 = new List<string>();

            this._photos = new ObservableCollection<string>();
            foreach (string p in this._bien.PhotosBase64)
            {
                this.Photos.Add(p);
            }

        }

        private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            ValidationTextBox(sender, e, "[^0-9,]+");
        }
        private void IntValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            ValidationTextBox(sender, e, "[^0-9.]+");
        }
        private void ValidationTextBox(object sender, TextCompositionEventArgs e, string regex)
        {
            Regex r = new Regex(regex);
            e.Handled = r.IsMatch(e.Text);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Fichiers images|*.png;*.jpeg;*.jpg;*.gif";
            if (dlg.ShowDialog(this) == true)
            {
                BitmapImage img = new BitmapImage(new Uri(dlg.FileName));
                // Conversion du fichier en base 64
                string str = Converters.Base64StringToBitmapImageConverter.BitmapImageToBase64String(img);
                // Ajout de l'image à la liste
                this._bien.PhotosBase64.Add(str);
                this.Photos.Add(str);
            }
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._indexPhoto < 0) return;
            if (this._indexPhoto >= this._bien.PhotosBase64.Count) return;
            this._bien.PhotosBase64.RemoveAt(this._indexPhoto);
            this.Photos.RemoveAt(this._indexPhoto);
            this._indexPhoto = -1;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            // Gestion de la photo principale
            if (_bien.PhotosBase64.Count > 0)
                _bien.PhotoPrincipaleBase64 = _bien.PhotosBase64[0];
            else
                _bien.PhotoPrincipaleBase64 = "";

            this.DialogResult = true;
            this.Close();
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
