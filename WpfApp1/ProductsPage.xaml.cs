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
        private bool _isEditing = false;

        public ProductsPage()
        {
            InitializeComponent();
            _context = new ProductInventoryEntities();
            LoadProducts();
            LoadCategories();
            HideEditForm();
        }

        private void LoadProducts()
        {
            try
            {
                _context = new ProductInventoryEntities(); 
                _context.Product.Load();
                var products = _context.Product.Include(p => p.Category).ToList();
                ProductsGrid.ItemsSource = products;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                _context.Category.Load();
                var categories = _context.Category.ToList();
                cmbCategory.ItemsSource = categories;
                cmbCategory.DisplayMemberPath = "CategoryName";
                cmbCategory.SelectedValuePath = "CategoryID";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowEditForm()
        {
            EditForm.Visibility = Visibility.Visible;
        }

        private void HideEditForm()
        {
            EditForm.Visibility = Visibility.Collapsed;
            _currentProduct = null;
            _isEditing = false;
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
            _isEditing = false;
            ClearForm();
            ShowEditForm();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selectedProduct)
            {
                _currentProduct = _context.Product.Find(selectedProduct.ProductID);
                _isEditing = true;

                txtName.Text = _currentProduct.Name;
                cmbCategory.SelectedValue = _currentProduct.CategoryID;
                txtPrice.Text = _currentProduct.Price.ToString();
                txtQuantity.Text = _currentProduct.Quantity.ToString();

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
                try
                {
                    _currentProduct.Name = txtName.Text.Trim();
                    _currentProduct.CategoryID = (int)cmbCategory.SelectedValue;
                    _currentProduct.Price = decimal.Parse(txtPrice.Text);
                    _currentProduct.Quantity = int.Parse(txtQuantity.Text);

                    if (!_isEditing)
                    {
                        _context.Product.Add(_currentProduct);
                    }

                    _context.SaveChanges();
                    LoadProducts();
                    HideEditForm();
                    MessageBox.Show("Данные сохранены", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                    try
                    {
                        var productToDelete = _context.Product.Find(selectedProduct.ProductID);
                        _context.Product.Remove(productToDelete);
                        _context.SaveChanges();
                        LoadProducts();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
            LoadCategories();
        }

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditForm.Visibility == Visibility.Visible)
            {
                HideEditForm();
            }
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