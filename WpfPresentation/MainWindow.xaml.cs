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
using LogicLayer;
using LogicLayerInterfaces;
using DataObjects;


namespace WpfPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Employee _user = null;
        private IVenueManager venueManager = new VenueManager();
        private List<VenueVM> _venues = null;
        private VenueUseManager venueUseManager = new VenueUseManager();
        private List<VenueUseVM> _venueUses = null;
        private AdCompanyManager adCompanyManager = new AdCompanyManager();
        private List<AdCompanyVM> _adCompanies = null;
        private AdCampaignManager adCampaignManager = new AdCampaignManager();
        private List<AdCampaignVM> _adCampaigns = null;

        public MainWindow(Employee e)
        {
            _user = e;
            InitializeComponent();
            hideAllUserTabs();
            
        }

        private void hideAllUserTabs()
        {
            pnlTabs.Visibility = Visibility.Hidden;
            foreach (var tab in tabsetMain.Items)
            {
                ((TabItem)tab).Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateLists();
            prepareUIForUser();

        }

        private void updateLists()
        {
            try
            {
                _venueUses = venueUseManager.RetreiveVenueUses();
                if (_user.Roles.Contains("Tour Planner"))
                {
                    _venues = venueManager.RetreiveVenues();
                }
                if (_user.Roles.Contains("Ad Planner"))
                {
                    _adCompanies = adCompanyManager.RetreiveAdCompanies();
                    _adCampaigns = adCampaignManager.RetreiveAdCampaigns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }
        }

        private void clearDatGridsAndComboBoxs()
        {
            cmbSelectVenue.Items.Clear();
            cmbSelectVenue.Items.Add("Select a venue to use.");
            cmbSelectVenue.SelectedItem = cmbSelectVenue.Items[0];
            datVenueInfo.ItemsSource = null;
            datVenueInfo.Items.Clear();
            cmbSelectUse.Items.Clear();
            cmbSelectUse.Items.Add("Select a venue use to add a day record to.");
            cmbSelectUse.SelectedItem = cmbSelectUse.Items[0];
            datUses.ItemsSource = null;
            datUses.Items.Clear();
            cmbAdCampaignViewUse.Items.Clear();
            cmbToVenueUse.Items.Clear();
            datAdCampaignStats.ItemsSource = null;
            datAdCampaignStats.Items.Clear();
        }

        private void prepareUIForUser()
        {
            lblWelcome.Content = "Hello " + _user.GivenName + " " + _user.FamilyName;

            foreach (var role in _user.Roles)
            {
                tabViewUses.Visibility = Visibility.Visible;
                switch (role)
                {
                    case "Tour Planner":                     
                        tabAddUse.Visibility = Visibility.Visible;
                        tabAddUse.IsSelected = true;
                        tabVenueInfo.Visibility = Visibility.Visible;
                        tabAddUseDay.Visibility = Visibility.Visible;
                        break;
                    case "Ad Planner":
                        tabAdCompanyInfo.Visibility = Visibility.Visible;
                        tabAdCompanyInfo.IsSelected = true;
                        tabAddAdCampaign.Visibility = Visibility.Visible;
                        tabAdCampaignStats.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }

            }
            pnlTabs.Visibility = Visibility.Visible;

        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogIn logInWindow = new LogIn();
            logInWindow.Show();
            this.Close();
        }

        private void tabAddUse_GotFocus(object sender, RoutedEventArgs e)
        {
            
            foreach (VenueVM venue in _venues) { 
                if(!cmbSelectVenue.Items.Contains(venue))
                cmbSelectVenue.Items.Add(venue);
            }
            dateStart.DisplayDateStart = DateTime.Now.Date;
            dateEnd.DisplayDateStart = DateTime.Now.Date;
        }

        private void btnAddUseSubmit_Click(object sender, RoutedEventArgs e)
        {
            // check if a venue has been selected
            if(cmbSelectVenue.SelectedItem == null || cmbSelectVenue.SelectedItem.ToString().Contains("Select a venue to use."))
            {
                MessageBox.Show("You must select the venue that will be used.");
                cmbSelectVenue.Focus();
                return;
            }

            // check if a start date has been selected
            if(dateStart.SelectedDate == null)
            {
                MessageBox.Show("You must select a start date for the venue use.");
                dateStart.Focus();
                return;
            }

            // check if an end date has been selected
            if (dateEnd.SelectedDate == null)
            {
                MessageBox.Show("You must select an end date for the venue use.");
                dateEnd.Focus();
                return;
            }

            // check if the end date is after the start date
            if(dateEnd.SelectedDate <= dateStart.SelectedDate)
            {
                MessageBox.Show("The end date must be after the start date.");
                dateEnd.Focus();
                return;
            }

            var result = MessageBox.Show("Verify the information you entered is correct?", "Confirm Add Venue Use", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                if(venueUseManager.AddVenueUse((VenueVM)cmbSelectVenue.SelectedItem, (DateTime)dateStart.SelectedDate, (DateTime)dateEnd.SelectedDate, _user)){
                    MessageBox.Show("The venue use has been added", "Success");
                }
                else
                {
                    MessageBox.Show("Addition Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }
            updateLists();
            clearDatGridsAndComboBoxs();
        }

        private void tabVenueInfo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (datVenueInfo.Items.Count == 0)
            {
                datVenueInfo.ItemsSource = _venues;
                try
                {
                    datVenueInfo.Columns.RemoveAt(5);
                    datVenueInfo.Columns.RemoveAt(9);
                    datVenueInfo.Columns.RemoveAt(8);
                    // Reordering columns so they appear in the order VenueName,StreetAddress,ZipCode,City,State,AverageTicketsSold,AverageRevenue,LastUsedOn
                    DataGridColumn temp = datVenueInfo.Columns[3];
                    datVenueInfo.Columns.RemoveAt(3);
                    datVenueInfo.Columns.Add(temp);
                    temp = datVenueInfo.Columns[3];
                    datVenueInfo.Columns.RemoveAt(3);
                    datVenueInfo.Columns.Add(temp);
                    temp = datVenueInfo.Columns[0];
                    datVenueInfo.Columns.RemoveAt(0);
                    datVenueInfo.Columns.Add(temp);
                    temp = datVenueInfo.Columns[0];
                    datVenueInfo.Columns.RemoveAt(0);
                    datVenueInfo.Columns.Add(temp);
                    temp = datVenueInfo.Columns[0];
                    datVenueInfo.Columns.RemoveAt(0);
                    datVenueInfo.Columns.Add(temp);
                }
                catch (ArgumentOutOfRangeException) { }

                
            }
            
        }

        private void datVenueInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            VenueVM selectedVenue = (VenueVM)datVenueInfo.SelectedItem;
            VenueDetails details = new VenueDetails(selectedVenue);
            details.ShowDialog();
        }

        private void tabAddUseDay_GotFocus(object sender, RoutedEventArgs e)
        {
            foreach (VenueUseVM venueUse in _venueUses)
            {
                if (!cmbSelectUse.Items.Contains(venueUse))
                    cmbSelectUse.Items.Add(venueUse);
            }
        }

        private void btnAddUseDaySubmit_Click(object sender, RoutedEventArgs e)
        {
            if(cmbSelectUse.SelectedItem == null || cmbSelectUse.SelectedItem.ToString().Contains("Select a venue use to add a day record to."))
            {
                MessageBox.Show("You must select the venue use to add the days record to.");
                cmbSelectUse.Focus();
                return;
            }

            if (dateDate.SelectedDate == null)
            {
                MessageBox.Show("You must select a date for to enter data for.");
                dateDate.Focus();
                return;
            }

            if(dateDate.SelectedDate < ((VenueUse)cmbSelectUse.SelectedItem).StartDate || 
                dateDate.SelectedDate > ((VenueUse)cmbSelectUse.SelectedItem).EndDate)
            {
                MessageBox.Show("The date selected is not within the selected use. Choose a date within the venue use or select another use.");
                dateDate.Focus();
                return;
            }

            if (txtTicketsSold.Text == null || txtTicketsSold.Text == "")
            {
                MessageBox.Show("You must enter the number of tickets sold this day as a whole number.");
                txtTicketsSold.Focus();
                return;
            }

            int ticketsSold;
            try
            {
                ticketsSold = int.Parse(txtTicketsSold.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must enter the number for tickets sold this day as a whole number.");
                txtTicketsSold.Focus();
                return;
            }

            if(ticketsSold < 0)
            {
                MessageBox.Show("Tickets sold can not be negative.");
                txtTicketsSold.Focus();
                return;
            }

            if (txtRevenue.Text == null || txtRevenue.Text == "")
            {
                MessageBox.Show("You must enter the day's revenue as a number.");
                txtRevenue.Focus();
                return;
            }

            decimal revenue;
            try
            {
                string revenustr = txtRevenue.Text.Replace("$", "");
                revenue = decimal.Parse(revenustr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must enter the day's revenue as a number.");
                txtRevenue.Focus();
                return;
            }

            if(revenue < 0)
            {
                MessageBox.Show("Revenue can not be negative.");
                txtTicketsSold.Focus();
                return;
            }

            var result = MessageBox.Show("Verify the information you entered is correct?", "Confirm Add Use Day", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                if (venueUseManager.AddUseDay((VenueUseVM)cmbSelectUse.SelectedItem, (DateTime)dateDate.SelectedDate, ticketsSold, revenue, _user))
                {
                    MessageBox.Show("The use day has been added", "Success");
                }
                else
                {
                    MessageBox.Show("Addition Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }
            updateLists();
            clearDatGridsAndComboBoxs();
        }

        private void tabAdCompanyInfo_GotFocus(object sender, RoutedEventArgs e)
        {
            if(datAdCompanyInfo.Items.Count == 0)
            {
                datAdCompanyInfo.ItemsSource = _adCompanies;
                datAdCompanyInfo.Columns.RemoveAt(3); // removing company id
                // Reordering columns so they appear in the order CompanyName,StreatAdress,City,State,ZipCode,PhoneNumber
                DataGridColumn temp = datAdCompanyInfo.Columns[0];
                datAdCompanyInfo.Columns.RemoveAt(0);
                datAdCompanyInfo.Columns.Insert(3, temp);
                temp = datAdCompanyInfo.Columns[0];
                datAdCompanyInfo.Columns.RemoveAt(0);
                datAdCompanyInfo.Columns.Insert(3, temp);
            }
        }

        // variables that apply only to the add ad campaign tab
        List<string> acts = new List<string>();
        List<string> adTypes = new List<string>();
        private decimal totalCost = 0;
        private int numberOfAdItems = 0;
        List<AdItem> CampaignItems = new List<AdItem>();

        private void tabAddAdCampaign_GotFocus(object sender, RoutedEventArgs e)
        {
            if(acts.Count == 0)
            {
                acts = adCampaignManager.RetreiveActs();
            }

            if (adTypes.Count == 0)
            {
                adTypes = adCampaignManager.RetreiveAdTypes();
            }

            foreach (AdCompanyVM adCompany in _adCompanies)
            {
                if (!cmbSelectAdCompany.Items.Contains(adCompany))
                    cmbSelectAdCompany.Items.Add(adCompany);
            }

            foreach (string adType in adTypes)
            {
                if (!cmbSelectAdType.Items.Contains(adType))
                    cmbSelectAdType.Items.Add(adType);
            }

            foreach (string act in acts)
            {
                if (!cmbSelectAct.Items.Contains(act))
                    cmbSelectAct.Items.Add(act);
            }

            lblCurentCostNumber.Content = totalCost;
            lblCurentItemsNumber.Content = numberOfAdItems;
            lblItemNumber.Content = "Item #" + (numberOfAdItems + 1);
        }

        private void btnAnotherItem_Click(object sender, RoutedEventArgs e)
        {
            if(cmbSelectAdCompany.SelectedItem == null || cmbSelectAdCompany.SelectedItem.ToString().Contains("Select an ad company to use."))
            {
                MessageBox.Show("You must choose an ad company.");
                return;
            }

            if (cmbSelectAdType.SelectedItem == null || cmbSelectAdType.SelectedItem.ToString().Contains("Select an ad type."))
            {
                MessageBox.Show("You must choose an ad type for the ad Item.");
                return;
            }

            if (cmbSelectAct.SelectedItem == null || cmbSelectAct.SelectedItem.ToString().Contains("Select the focus act of the ad."))
            {
                MessageBox.Show("You must choose a act for the ad to focus on or \"None\".");
                return;
            }

            if(txtCost.Text == null || txtCost.Text == "")
            {
                MessageBox.Show("You must enter the cost of this ad item as a number.");
                txtCost.Focus();
                return;
            }

            decimal cost;
            try
            {
                string costStr = txtCost.Text.Replace("$", "");
                cost = decimal.Parse(costStr);
            }
            catch (Exception)
            {
                MessageBox.Show("You must enter the cost of this ad item as a number.");
                txtCost.Focus();
                return;
            }

            if(cost < 0)
            {
                MessageBox.Show("The cost of the ad Item can not be less than zero");
                txtCost.Focus();
                return;
            }

            cmbSelectAdCompany.IsEnabled = false;

            AdItem adItem = new AdItem();
            adItem.AdType = cmbSelectAdType.SelectedItem.ToString();
            if (cmbSelectAct.SelectedItem.ToString().Contains("None"))
            {
                adItem.FocusAct = null;
            }
            else
            {
                adItem.FocusAct = cmbSelectAct.SelectedItem.ToString();
            }
            
            adItem.Cost = cost;
            totalCost += cost;
            numberOfAdItems += 1;
            CampaignItems.Add(adItem);

            cmbSelectAct.SelectedIndex = 0;
            cmbSelectAdType.SelectedIndex = 0;
            txtCost.Clear();
            lblCurentCostNumber.Content = totalCost;
            lblCurentItemsNumber.Content = numberOfAdItems;
            lblItemNumber.Content = "Item #" + (numberOfAdItems + 1);
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if(CampaignItems.Count == 0)
            {
                MessageBox.Show("There are no ad items to remove from this ad campaign");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to remove the last ad item you added to this campaign?", "Remove Last Item?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                totalCost -= CampaignItems.Last().Cost;
                numberOfAdItems -= 1;
                CampaignItems.RemoveAt(numberOfAdItems);
                lblCurentCostNumber.Content = totalCost;
                lblCurentItemsNumber.Content = numberOfAdItems;
                lblItemNumber.Content = "Item #" + (numberOfAdItems + 1);
                if(numberOfAdItems == 0)
                {
                    cmbSelectAdCompany.IsEnabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to give up adding this ad campaign? All ad items will be removed.", "Cancel adding ad campaign?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            totalCost = 0;
            numberOfAdItems = 0;
            CampaignItems = new List<AdItem>();
            lblCurentCostNumber.Content = totalCost;
            lblCurentItemsNumber.Content = numberOfAdItems;
            lblItemNumber.Content = "Item #" + (numberOfAdItems + 1);
            cmbSelectAdCompany.IsEnabled = true;
        }

        private void btnAdCampaignSubmit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you are ready to submit the ad campaign with the current ad items?", "Submit Campaign?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            result = MessageBox.Show("Should the item currently displayed be included?", "Include Current Item?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                int oldNumberOfAdItems = numberOfAdItems;
                btnAnotherItem_Click(sender, e);
                if(oldNumberOfAdItems == numberOfAdItems)
                {
                    return;
                }
            }

            if(numberOfAdItems == 0)
            {
                MessageBox.Show("There must be al least one ad item included in the ad campaign.");
                return;
            }

            try
            {
                if(adCampaignManager.AddAdCampaign((AdCompany)cmbSelectAdCompany.SelectedItem, totalCost, _user, CampaignItems))
                {
                    MessageBox.Show("The ad campaign was added.");
                }
                else
                {
                    MessageBox.Show("Failed to add ad campaign.");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }

            totalCost = 0;
            numberOfAdItems = 0;
            CampaignItems = new List<AdItem>();
            lblCurentCostNumber.Content = totalCost;
            lblCurentItemsNumber.Content = numberOfAdItems;
            lblItemNumber.Content = "Item #" + (numberOfAdItems + 1);
            cmbSelectAdCompany.IsEnabled = true;

            updateLists();
            clearDatGridsAndComboBoxs();
        }

        private void tabViewUses_GotFocus(object sender, RoutedEventArgs e)
        {
            
            if (datUses.Items.Count == 0)
            {
                datUses.ItemsSource = _venueUses;
                    try { 
                        datUses.Columns.RemoveAt(7);
                        datUses.Columns.RemoveAt(0);
                        datUses.Columns.RemoveAt(9);
                        datUses.Columns.RemoveAt(9);
                        datUses.Columns[5].DisplayIndex = 0;
                    }
                catch(ArgumentOutOfRangeException) { }
                
            }
            if(!_user.Roles.Contains("Ad Planner"))
            {
                lblAssignAdCampaign.Visibility = Visibility.Hidden;
                cmbAdCampaignViewUse.Visibility = Visibility.Hidden;
                lblToVenueUse.Visibility = Visibility.Hidden;
                cmbToVenueUse.Visibility = Visibility.Hidden;
                btnAssignAdToUse.Visibility = Visibility.Hidden;
            }
            else
            {
                foreach (VenueUse use in _venueUses)
                {
                    if (!cmbToVenueUse.Items.Contains(use.UseId))
                        cmbToVenueUse.Items.Add(use.UseId);
                }
                foreach (AdCampaign adCampaign in _adCampaigns)
                {
                    if (!cmbAdCampaignViewUse.Items.Contains(adCampaign))
                        cmbAdCampaignViewUse.Items.Add(adCampaign);
                }
            }
            
        }

        private void btnAssignAdToUse_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAdCampaignViewUse.SelectedItem == null)
            {
                MessageBox.Show("You must choose an ad campaign.");
                return;
            }
            if (cmbToVenueUse.SelectedItem == null)
            {
                MessageBox.Show("You must choose a venue use.");
                return;
            }
            var result = MessageBox.Show("Verify the information you entered is correct?", "Confirm update use ad campaign", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            int useId = (int)cmbToVenueUse.SelectedItem;
            int campaignId = ((AdCampaign)cmbAdCampaignViewUse.SelectedItem).CampaignId;
            try
            {
                if (venueUseManager.UpdateVenueUseAdCampaign(useId,campaignId))
                {
                    MessageBox.Show("The update was successful", "Success");
                }
                else
                {
                    MessageBox.Show("Update Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException);
            }
            updateLists();
            clearDatGridsAndComboBoxs();
        }

        private void tabAdCampaignStats_GotFocus(object sender, RoutedEventArgs e)
        {
            if(datAdCampaignStats.Items.Count == 0)
            {
                datAdCampaignStats.ItemsSource = _adCampaigns;
                try { 
                    datAdCampaignStats.Columns.RemoveAt(0);
                    datAdCampaignStats.Columns.RemoveAt(4);
                    datAdCampaignStats.Columns.RemoveAt(5);
                    datAdCampaignStats.Columns[3].DisplayIndex = 0;
                } 
                catch(ArgumentOutOfRangeException)
                {

                }
                
            }
        }

        private void datAdCampaignStats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AdCampaignVM selectedAdCampaign = (AdCampaignVM)datAdCampaignStats.SelectedItem;
            AdCampaignItems details = new AdCampaignItems(selectedAdCampaign);
            details.ShowDialog();
        }
    }
}
