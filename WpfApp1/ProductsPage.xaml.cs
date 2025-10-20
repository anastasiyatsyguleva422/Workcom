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
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private readonly ProductInventoryEntities _productService;

        public ProductsPage(ProductInventoryEntities productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = _productService.GetAllProducts();
            ProductsGrid.ItemsSource = products;
            StatusText.Text = $"Найдено товаров: {products.Count}";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductEditWindow();
            if (dialog.ShowDialog() == true)
            {
                _productService.AddProduct(dialog.Product);
                LoadProducts();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                var dialog = new ProductEditWindow(selectedProduct);
                if (dialog.ShowDialog() == true)
                {
                    _productService.UpdateProduct(dialog.Product);
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show($"Удалить товар '{selectedProduct.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _productService.DeleteProduct(selectedProduct.ProductID);
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }
    }
}
