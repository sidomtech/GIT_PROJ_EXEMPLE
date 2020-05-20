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

namespace ClientServiceAgence
{
    /// <summary>
    /// Logique d'interaction pour FiltersWindow.xaml
    /// </summary>
    public partial class FiltersWindow : Window, INotifyPropertyChanged
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

        private ServiceAgence.CriteresRechercheBiensImmobiliers _filtres;

        public ServiceAgence.CriteresRechercheBiensImmobiliers Filtres
        {
            get { return _filtres; }
            set { SetField(ref _filtres, value); }
        }

        #endregion
        public FiltersWindow(ServiceAgence.CriteresRechercheBiensImmobiliers filtres)
        {
            InitializeComponent();
            this.DataContext = this;
            if (filtres == null)                this.Filtres = InitializeFilters();
            else                this.Filtres = CloneFilters(filtres);
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

        private void Apply_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.cbChampTri.SelectedItem != null && this.cbChampTri.SelectedItem != Converters.ComboBoxEmptyItemConverter.Item)
            {
                var t = new ServiceAgence.CriteresRechercheBiensImmobiliers.Tri();
                t.Champ = (ServiceAgence.CriteresRechercheBiensImmobiliers.eChampTri)this.cbChampTri.SelectedValue;
                t.Ordre = (ServiceAgence.CriteresRechercheBiensImmobiliers.eOrdreTri)this.cbOrdreTri.SelectedValue;
                this._filtres.Tris = new List<ServiceAgence.CriteresRechercheBiensImmobiliers.Tri> { t };
            }

            this.DialogResult = true;
            this.Close();
        }
        private void Empty_Button_Click(object sender, RoutedEventArgs e)
        {
            this._filtres = null;
            this.DialogResult = true;
            this.Close();
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        public static ServiceAgence.CriteresRechercheBiensImmobiliers InitializeFilters()
        {
            ServiceAgence.CriteresRechercheBiensImmobiliers criteres = new ServiceAgence.CriteresRechercheBiensImmobiliers();
            criteres.AdresseContient = "";
            criteres.CodePostal = "";
            criteres.DateMiseEnTransaction1 = null;
            criteres.DateMiseEnTransaction2 = null;
            criteres.DateTransaction1 = null;
            criteres.DateTransaction2 = null;
            criteres.DescriptionContient = "";
            criteres.EnergieChauffage = null;
            criteres.MontantCharges1 = -1;
            criteres.MontantCharges2 = -1;
            criteres.NbEtages1 = -1;
            criteres.NbEtages2 = -1;
            criteres.NbPieces1 = -1;
            criteres.NbPieces2 = -1;
            criteres.NumEtage1 = -1;
            criteres.NumEtage2 = -1;
            criteres.Prix1 = -1;
            criteres.Prix2 = -1;
            criteres.Surface1 = -1;
            criteres.Surface2 = -1;
            criteres.TitreContient = "";
            criteres.TransactionEffectuee = null;
            criteres.TypeBien = null;
            criteres.TypeChauffage = null;
            criteres.TypeTransaction = null;
            criteres.Ville = "";
            
            return criteres;
        }
        public static ServiceAgence.CriteresRechercheBiensImmobiliers CloneFilters(ServiceAgence.CriteresRechercheBiensImmobiliers filters)
        {
            ServiceAgence.CriteresRechercheBiensImmobiliers criteres = InitializeFilters();
            criteres.AdresseContient = filters.AdresseContient;
            criteres.CodePostal = filters.CodePostal;
            criteres.DateMiseEnTransaction1 = filters.DateMiseEnTransaction1;
            criteres.DateMiseEnTransaction2 = filters.DateMiseEnTransaction2;
            criteres.DateTransaction1 = filters.DateTransaction1;
            criteres.DateTransaction2 = filters.DateTransaction2;
            criteres.DescriptionContient = filters.DescriptionContient;
            criteres.EnergieChauffage = filters.EnergieChauffage;
            criteres.MontantCharges1 = filters.MontantCharges1;
            criteres.MontantCharges2 = filters.MontantCharges2;
            criteres.NbEtages1 = filters.NbEtages1;
            criteres.NbEtages2 = filters.NbEtages2;
            criteres.NbPieces1 = filters.NbPieces1;
            criteres.NbPieces2 = filters.NbPieces2;
            criteres.NumEtage1 = filters.NumEtage1;
            criteres.NumEtage2 = filters.NumEtage2;
            criteres.Prix1 = filters.Prix1;
            criteres.Prix2 = filters.Prix2;
            criteres.Surface1 = filters.Surface1;
            criteres.Surface2 = filters.Surface2;
            criteres.TitreContient = filters.TitreContient;
            criteres.TransactionEffectuee = filters.TransactionEffectuee;
            criteres.TypeBien = filters.TypeBien;
            criteres.TypeChauffage = filters.TypeChauffage;
            criteres.TypeTransaction = filters.TypeTransaction;
            criteres.Ville = filters.Ville;
            criteres.Tris = filters.Tris;

            return criteres;
        }
        public static bool IsFiltersEmpty(ServiceAgence.CriteresRechercheBiensImmobiliers filters)
        {
            return filters.AdresseContient == "" &&
                   filters.CodePostal == "" &&
                   filters.DateMiseEnTransaction1 == null &&
                   filters.DateMiseEnTransaction2 == null &&
                   filters.DateTransaction1 == null &&
                   filters.DateTransaction2 == null &&
                   filters.DescriptionContient == "" &&
                   filters.EnergieChauffage == null &&
                   filters.MontantCharges1 == -1 &&
                   filters.MontantCharges2 == -1 &&
                   filters.NbEtages1 == -1 &&
                   filters.NbEtages2 == -1 &&
                   filters.NbPieces1 == -1 &&
                   filters.NbPieces2 == -1 &&
                   filters.NumEtage1 == -1 &&
                   filters.NumEtage2 == -1 &&
                   filters.Prix1 == -1 &&
                   filters.Prix2 == -1 &&
                   filters.Surface1 == -1 &&
                   filters.Surface2 == -1 &&
                   filters.TitreContient == "" &&
                   filters.TransactionEffectuee == null &&
                   filters.TypeBien == null &&
                   filters.TypeChauffage == null &&
                   filters.TypeTransaction == null &&
                   filters.Ville == "";
        }
    }
}
