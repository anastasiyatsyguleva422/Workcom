using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            NavigateToProducts(null, null);
            UpdateStatus("Система учёта товаров - выберите раздел");
        }

        private void NavigateToProducts(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ProductsPage();
            UpdateButtonStates(btnProducts);
            UpdateStatus("Управление товарами");
        }

        private void NavigateToCategories(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new CategoriesPage();
            UpdateButtonStates(btnCategories);
            UpdateStatus("Управление категориями");
        }

        private void NavigateToSearch(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new SearchPage();
            UpdateButtonStates(btnSearch);
            UpdateStatus("Поиск товаров");
        }
        private void NavigateToStatistics(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new StatisticsPage();
            UpdateButtonStates(btnStatistics);
            UpdateStatus("Просмотр статистики");
        }
        private void UpdateButtonStates(Button activeButton)
        {
            btnProducts.ClearValue(Button.BackgroundProperty);
            btnCategories.ClearValue(Button.BackgroundProperty);
            btnSearch.ClearValue(Button.BackgroundProperty);

            if (activeButton != null)
            {
                activeButton.Background = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(33, 150, 243));
                activeButton.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }
    }
}