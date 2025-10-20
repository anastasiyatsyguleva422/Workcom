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
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        public CategoriesPage(ProductInventoryEntities productService)
        {
            InitializeComponent();
            _productService = productService;
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = _productService.GetAllCategories();
            CategoriesGrid.ItemsSource = categories;
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите название новой категории:", "Новая категория", "");

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show($"Категория '{categoryName}' добавлена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
            }
        }

        private void BtnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesGrid.SelectedItem != null)
            {
                dynamic selectedCategory = CategoriesGrid.SelectedItem;
                var result = MessageBox.Show($"Удалить категорию '{selectedCategory.CategoryName}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
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
    }
}