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

namespace WpfApp1
{


    public partial class MainWindow : Window
    {
        private readonly ProductInventoryEntities _productService;

        public MainWindow()
        {
            InitializeComponent();
            _productService = new ProductInventoryEntities();
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            NavigateToProducts(null, null);
            UpdateStatus("Система учёта товаров - выберите раздел");
        }

        private void NavigateToProducts(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new ProductsPage(_productService);
            UpdateButtonStates(btnProducts);
            UpdateStatus("Управление товарами");
        }

        private void NavigateToCategories(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Content = new CategoriesPage(_productService);
            UpdateButtonStates(btnCategories);
            UpdateStatus("Управление категориями");
        }

        private void NavigateToStatistics(object sender, RoutedEventArgs e)
        {
            //  MainContentFrame.Content = new StatisticsPage(_productService);
            UpdateButtonStates(btnStatistics);
            UpdateStatus("Просмотр статистики");
        }

        private void NavigateToSearch(object sender, RoutedEventArgs e)
        {
            // MainContentFrame.Content = new SearchPage(_productService);
            UpdateButtonStates(btnSearch);
            UpdateStatus("Поиск товаров");
        }

        private void UpdateButtonStates(Button activeButton)
        {
            btnProducts.Background = Brushes.Transparent;
            btnCategories.Background = Brushes.Transparent;
            btnStatistics.Background = Brushes.Transparent;
            btnSearch.Background = Brushes.Transparent;

            if (activeButton != null)
            {
                activeButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                activeButton.Foreground = Brushes.White;
            }
        }

        private void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }
    }
}