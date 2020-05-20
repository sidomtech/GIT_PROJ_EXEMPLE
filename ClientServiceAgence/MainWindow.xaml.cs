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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace ClientServiceAgence
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int NB_BIENS_IMMOBILIERS_PAR_PAGE = 15;
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
        private ServiceAgence.AgenceClient _service;
        private ServiceAgence.CriteresRechercheBiensImmobiliers _filtres;
        private ServiceAgence.BienImmobilierBase.eTypeTransaction _typeTransaction;
        private ServiceAgence.ListeBiensImmobiliers _listeBiens;
        private ServiceAgence.BienImmobilierBase _bienBaseSelectionne;
        private ServiceAgence.BienImmobilier _bienSelectionne;
        private int _indexPhoto;
        private bool _chargementListe;
        private bool _chargementBien;
        /// <summary>
        /// 
        /// </summary>
        public ServiceAgence.CriteresRechercheBiensImmobiliers Filtres
        {
            get { return _filtres; }
            set
            {
                if (SetField(ref _filtres, value))
                {
                    OnPropertyChanged("ListeFiltree");
                    ChargerBiensImmobiliers();
                }
            }
        }
        public bool ListeFiltree
        {
            get 
            {
                if (_filtres == null) return false;
                return !FiltersWindow.IsFiltersEmpty(_filtres);
            }
        }
        public ServiceAgence.BienImmobilierBase.eTypeTransaction TypeTransaction
        {
            get { return _typeTransaction; }
            set { if (SetField(ref _typeTransaction, value)) ChargerBiensImmobiliers(); }
        }
        public ServiceAgence.ListeBiensImmobiliers ListeBiens
        {
            get { return _listeBiens; }
            set { SetField(ref _listeBiens, value); }
        }
        public ServiceAgence.BienImmobilierBase BienBaseSelectionne
        {
            get { return _bienBaseSelectionne; }
            set { if(SetField(ref _bienBaseSelectionne, value)) ChargerDetailsBienImmobilier(); }
        }
        public ServiceAgence.BienImmobilier BienSelectionne
        {
            get { return _bienSelectionne; }
            set
            {
                if (SetField(ref _bienSelectionne, value))
                {
                    OnPropertyChanged("NombrePhotos");
                    this.IndexPhoto = 0;
                }
            }
        }
        public int IndexPhoto
        {
            get { return _indexPhoto; }
            set 
            {
                if (value < 0) value = 0;
                if (value >= this.NombrePhotos) value = this.NombrePhotos - 1;
                if (SetField(ref _indexPhoto, value))
                {
                    OnPropertyChanged("PhotoSelectionnee");
                }
            }
        }
        public int NombrePhotos
        {
            get { return (_bienSelectionne == null) ? 0 : _bienSelectionne.PhotosBase64.Count; }
        }
        public string PhotoSelectionnee
        {
            get { return (_bienSelectionne == null || _indexPhoto < 0 || _indexPhoto >= _bienSelectionne.PhotosBase64.Count) ? "" : _bienSelectionne.PhotosBase64[_indexPhoto]; }
        }
        public bool ChargementListe
        {
            get { return _chargementListe; }
            set { SetField(ref _chargementListe, value); }
        }
        public bool ChargementBien
        {
            get { return _chargementBien; }
            set { SetField(ref _chargementBien, value); }
        }
        
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            this._service = new ServiceAgence.AgenceClient("WSHttpBinding_IAgence");
            this._filtres = null;
            this._bienBaseSelectionne = null;
            this._bienSelectionne = null;
            this._listeBiens = null;
            this.DataContext = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._typeTransaction = ServiceAgence.BienImmobilierBase.eTypeTransaction.Vente;
            ChargerBiensImmobiliers();
        }
        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            ChargerBiensImmobiliers();
        }
        private void Filters_Button_Click(object sender, RoutedEventArgs e)
        {
            FiltersWindow w = new FiltersWindow(this._filtres);
            w.Owner = this;
            if (w.ShowDialog() == true)
                this.Filtres = w.Filtres;
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditWindow w = new AddEditWindow();
            w.Owner = this;
            if (w.ShowDialog() == true)
            {
                // Appel au service WCF
                try
                {
                    ServiceAgence.ResultatBool resultat = null;
                    resultat = this._service.AjouterBienImmobilier(w.Bien);
                    if (resultat.SuccesExecution)
                    {
                        if (resultat.Valeur)
                        {
                            MessageBox.Show(this, "Le bien a été ajouté avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            ChargerBiensImmobiliers();
                        }
                    }
                    else
                        AfficherErreur(resultat);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._bienSelectionne == null) return;
            AddEditWindow w = new AddEditWindow(this._bienSelectionne);
            w.Owner = this;
            if (w.ShowDialog() == true)
            {
                // Appel au service WCF
                try
                {
                    ServiceAgence.ResultatBool resultat = null;
                    resultat = this._service.ModifierBienImmobilier(w.Bien);
                    if (resultat.SuccesExecution)
                    {
                        if (resultat.Valeur)
                        {
                            MessageBox.Show(this, "Le bien sélectionné a été mis à jour avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            ChargerBiensImmobiliers();
                        }
                    }
                    else
                        AfficherErreur(resultat);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this._bienBaseSelectionne == null) return;
            if (MessageBox.Show(this, "Êtes-vous sûr de vouloir supprimer le bien sélectionné ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            // Appel au service WCF
            try
            {
                ServiceAgence.ResultatBool resultat = null;
                resultat = this._service.SupprimerBienImmobilier(this._bienBaseSelectionne.Id.ToString());
                if (resultat.SuccesExecution)
                {
                    if (resultat.Valeur)
                    {
                        MessageBox.Show(this, "Le bien sélectionné a été supprimé avec succès.", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                        ChargerBiensImmobiliers();
                    }
                }
                else
                    AfficherErreur(resultat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Previous_Page_Button_Click(object sender, RoutedEventArgs e)
        {
            ChargerBiensImmobiliers((_listeBiens == null || _listeBiens.Page <= 1) ? 1 : _listeBiens.Page - 1);
        }
        private void Next_Page_Button_Click(object sender, RoutedEventArgs e)
        {
            ChargerBiensImmobiliers((_listeBiens == null || _listeBiens.Page >= _listeBiens.PagesCount) ? _listeBiens.PagesCount : _listeBiens.Page + 1);
        }
        private void Previous_Photo_Button_Click(object sender, RoutedEventArgs e)
        {
            this.IndexPhoto--;
        }
        private void Next_Photo_Button_Click(object sender, RoutedEventArgs e)
        {
            this.IndexPhoto++;
        }



        protected async void ChargerBiensImmobiliers(int page = 1)
        {
            this.ChargementListe = true;

            ServiceAgence.CriteresRechercheBiensImmobiliers criteres = null;
            ServiceAgence.ResultatListeBiensImmobiliers resultat = null;

            if (this._filtres == null) {
                criteres = FiltersWindow.InitializeFilters();
            }
            else
                criteres = this._filtres;

            criteres.TypeTransaction = this._typeTransaction;

            // Appel au service WCF
            try
            {
                resultat = await this._service.LireListeBiensImmobiliersAsync(criteres, page, NB_BIENS_IMMOBILIERS_PAR_PAGE);
                if (resultat.SuccesExecution)
                {
                    this.ListeBiens = resultat.Liste;
                    this.BienBaseSelectionne = null;
                }
                else
                    AfficherErreur(resultat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.ChargementListe = false;
        }

        protected async void ChargerDetailsBienImmobilier()
        {
            this.ChargementBien = true;

            ServiceAgence.ResultatBienImmobilier resultat = null;
            
            if (this._bienBaseSelectionne == null)
            {
                this.BienSelectionne = null;
                this.ChargementBien = false;
                return;
            }

            // Appel au service WCF
            try
            {
                resultat = await this._service.LireDetailsBienImmobilierAsync(this._bienBaseSelectionne.Id.ToString());
                if (resultat.SuccesExecution)
                    this.BienSelectionne = resultat.Bien;
                else
                    AfficherErreur(resultat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.ChargementBien = false;
        }

        protected void AfficherErreur(ServiceAgence.ResultatOperation resultat)
        {
            string message = "";

            if (resultat.ErreursBloquantes.Count > 0)
            {
                message = "Erreurs bloquantes :";
                foreach (ServiceAgence.ResultatOperation.Erreur erreur in resultat.ErreursBloquantes)
                {
                    message += "\r\n" + erreur.Message;
                }
            }

            if (resultat.ErreursNonBloquantes.Count > 0)
            {
                if (message != "") message += "\r\n\r\n\r\n";
                message = "Erreurs non bloquantes :";
                foreach (ServiceAgence.ResultatOperation.Erreur erreur in resultat.ErreursNonBloquantes)
                {
                    message += "\r\n" + erreur.Message;
                }
            }

            if (message != "")
                MessageBox.Show(this, message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }


                
    }
}
