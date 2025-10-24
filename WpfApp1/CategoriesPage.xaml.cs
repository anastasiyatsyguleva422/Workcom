using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CategoriesPage : Page
    {
        private ProductInventoryEntities _context;

        public CategoriesPage()
        {
            InitializeComponent();
            _context = new ProductInventoryEntities();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                _context = new ProductInventoryEntities();
                var categories = _context.Category.ToList();
                CategoriesGrid.ItemsSource = categories;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = txtNewCategory.Text.Trim();

            if (!string.IsNullOrWhiteSpace(categoryName) && categoryName != "Новая категория")
            {
                try
                {
                    if (_context.Category.Any(c => c.CategoryName == categoryName))
                    {
                        MessageBox.Show("Категория с таким названием уже существует", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var newCategory = new Category { CategoryName = categoryName };
                    _context.Category.Add(newCategory);
                    _context.SaveChanges();

                    MessageBox.Show($"Категория '{categoryName}' добавлена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories();
                    txtNewCategory.Text = "Новая категория";
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка добавления категории: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите название категории", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesGrid.SelectedItem is Category selectedCategory)
            {
                try
                {
                    if (_context.Product.Any(p => p.CategoryID == selectedCategory.CategoryID))
                    {
                        MessageBox.Show("Нельзя удалить категорию, в которой есть товары", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var result = MessageBox.Show($"Удалить категорию '{selectedCategory.CategoryName}'?",
                        "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        var categoryToDelete = _context.Category.Find(selectedCategory.CategoryID);
                        _context.Category.Remove(categoryToDelete);
                        _context.SaveChanges();

                        MessageBox.Show("Категория удалена", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCategories();
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления категории: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите категорию для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }
    }
}