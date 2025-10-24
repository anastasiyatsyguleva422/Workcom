using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
            Loaded += SearchPage_Loaded;
        }

        private void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadAllProducts();
        }

        private void LoadCategories()
        {
            try
            {
                using (var context = new ProductInventoryEntities())
                {
                    var categories = context.Category.ToList();

                    if (cmbCategory != null)
                    {
                        cmbCategory.Items.Clear();
                        cmbCategory.Items.Add(new ComboBoxItem { Content = "Все категории", IsSelected = true });

                        foreach (var category in categories)
                        {
                            cmbCategory.Items.Add(category);
                        }
                        cmbCategory.DisplayMemberPath = "CategoryName";
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки категорий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAllProducts()
        {
            try
            {
                using (var context = new ProductInventoryEntities())
                {
                    var products = context.Product.Include("Category").ToList();
                    if (SearchResultsGrid != null)
                    {
                        SearchResultsGrid.ItemsSource = products;
                    }
                    UpdateResultsCount(products.Count);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchProducts();
        }

        private void CmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchProducts();
        }

        private void SearchProducts()
        {
            try
            {
                using (var context = new ProductInventoryEntities())
                {
                    var searchText = txtSearch?.Text?.Trim()?.ToLower() ?? "";
                    var products = context.Product.Include("Category").AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        products = products.Where(p => p.Name.ToLower().Contains(searchText));
                    }

                    if (cmbCategory?.SelectedItem is Category selectedCategory)
                    {
                        products = products.Where(p => p.CategoryID == selectedCategory.CategoryID);
                    }
                    else if (cmbCategory?.SelectedItem is ComboBoxItem comboItem && comboItem.Content.ToString() == "Все категории")
                    {
                    }

                    var results = products.ToList();
                    if (SearchResultsGrid != null)
                    {
                        SearchResultsGrid.ItemsSource = results;
                    }
                    UpdateResultsCount(results.Count);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch != null)
                txtSearch.Text = "";

            if (cmbCategory != null)
                cmbCategory.SelectedIndex = 0;

            LoadAllProducts();
        }

        private void UpdateResultsCount(int count)
        {
            if (txtResultsCount != null)
                txtResultsCount.Text = $"Найдено товаров: {count}";
        }
    }
}