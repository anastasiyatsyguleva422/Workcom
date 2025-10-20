using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CategoriesPage : Page
    {
        private ProductInventoryEntities _context;

        public CategoriesPage(ProductInventoryEntities context)
        {
            InitializeComponent();
            _context = context;
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _context.Category.ToList();
            CategoriesGrid.ItemsSource = categories;
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = txtNewCategory.Text.Trim();

            if (!string.IsNullOrWhiteSpace(categoryName) && categoryName != "Новая категория")
            {
                // Проверяем, нет ли уже такой категории
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
                // Проверяем, нет ли товаров в этой категории
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
                    _context.Category.Remove(selectedCategory);
                    _context.SaveChanges();

                    MessageBox.Show("Категория удалена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories();
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