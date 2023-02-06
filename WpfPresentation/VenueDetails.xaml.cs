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
using DataObjects;

namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for VenueDetails.xaml
    /// </summary>
    public partial class VenueDetails : Window
    {
        private VenueVM _venue;

        public VenueDetails(VenueVM venue)
        {
            _venue = venue;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblVenueName.Content = _venue.VenueName;
            txtTermsOfUse.Text = _venue.TermsOfUse;
            lblPhoneNumber.Content = _venue.PhoneNumber;
        }
    }
}
