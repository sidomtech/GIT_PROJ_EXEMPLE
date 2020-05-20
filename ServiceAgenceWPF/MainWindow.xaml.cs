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

using System.ServiceModel;

namespace ServiceAgenceWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, System.ComponentModel.INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        ServiceHost _hote = null;
        public string StatutService
        {
            get
            {
                if (_hote == null) return "Service fermé.";
                switch (_hote.State)
                {
                    case CommunicationState.Closed:
                        return "Service fermé.";
                    case CommunicationState.Closing:
                        return "Service en cours de fermeture...";
                    case CommunicationState.Created:
                        return "Service créé.";
                    case CommunicationState.Faulted:
                        return "Service en erreur.";
                    case CommunicationState.Opened:
                        return "Service ouvert.";
                    case CommunicationState.Opening:
                        return "Service en cours d'ouverture...";
                    default:
                        return "Etat du service inconnu.";
                }
            }
        }

        private void window_Closed(object sender, EventArgs e) {
            this.StartStopServiceHost(true);
        }

        void button_Click(object sender, RoutedEventArgs e) {
            this.StartStopServiceHost();
        }

        void _hote_StateChanged(object sender, EventArgs e) {
            this.OnPropertyChanged("StatutService");
        }


        public void StartStopServiceHost(Boolean appExiting = false) {
            try {
                if (_hote == null) {
                    if (appExiting) return;
                    _hote = new ServiceHost(typeof(ServiceAgence.Agence));
                    _hote.Closed += _hote_StateChanged;
                    _hote.Closing += _hote_StateChanged;
                    _hote.Faulted += _hote_StateChanged;
                    _hote.Opened += _hote_StateChanged;
                    _hote.Opening += _hote_StateChanged;
                }

                if (appExiting)
                    _hote.Close();
                else if (_hote.State != CommunicationState.Opened)
                    _hote.Open();
                else
                    _hote.Close();
            }
            catch (Exception ex) { 
                MessageBox.Show(ex.Message, "Erreur" , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
