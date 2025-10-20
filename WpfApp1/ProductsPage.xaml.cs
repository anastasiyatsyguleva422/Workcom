using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ProductsPage : Page
    {
        private ProductInventoryEntities _context;
        private Product _currentProduct;

        public ProductsPage(ProductInventoryEntities context)
        {
            InitializeComponent();
            _context = context;
            LoadProducts();
            LoadCategories();
            HideEditForm();
        }

        private void LoadProducts()
        {
            _context = new ProductInventoryEntities(); // Пересоздаем контекст для обновления данных
            var products = _context.Product.Include(p => p.Category).ToList();
            ProductsGrid.ItemsSource = products;
        }

        private void LoadCategories()
        {
            var categories = _context.Category.ToList();
            cmbCategory.ItemsSource = categories;
            cmbCategory.DisplayMemberPath = "CategoryName";
            cmbCategory.SelectedValuePath = "CategoryID";
        }

        private void ShowEditForm()
        {
            EditForm.Visibility = Visibility.Visible;
        }

        private void HideEditForm()
        {
            EditForm.Visibility = Visibility.Collapsed;
            _currentProduct = null;
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            _currentProduct = new Product();
            ClearForm();
            ShowEditForm();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                _currentProduct = selectedProduct;
                txtName.Text = selectedProduct.Name;
                cmbCategory.SelectedValue = selectedProduct.CategoryID;
                txtPrice.Text = selectedProduct.Price.ToString();
                txtQuantity.Text = selectedProduct.Quantity.ToString();
                ShowEditForm();
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                _currentProduct.Name = txtName.Text.Trim();
                _currentProduct.CategoryID = (int)cmbCategory.SelectedValue;
                _currentProduct.Price = decimal.Parse(txtPrice.Text);
                _currentProduct.Quantity = int.Parse(txtQuantity.Text);

                if (_currentProduct.ProductID == 0)
                {
                    _context.Product.Add(_currentProduct);
                }

                _context.SaveChanges();
                LoadProducts();
                HideEditForm();
                MessageBox.Show("Данные сохранены", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            HideEditForm();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show($"Удалить товар '{selectedProduct.Name}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Product.Remove(selectedProduct);
                    _context.SaveChanges();
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

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HideEditForm(); // Скрываем форму при выборе другого товара
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}