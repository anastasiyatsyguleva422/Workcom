using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1;

namespace WpfApp1
{

    public partial class SearchPage : Page
    {

        private List<Product> _allProducts;

        public SearchPage()
        {
            InitializeComponent();

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            _allProducts = new List<Product>
            {
                //new Product { ProductID = 1, Name = "Ноутбук Dell", Category = "Электроника", Price = 55000.00m, Quantity = 10 },
                //new Product { ProductID = 2, Name = "Монитор LG", Category = "Электроника", Price = 15500.50m, Quantity = 25 },
                //new Product { ProductID = 3, Name = "Кофеварка Philips", Category = "Бытовая техника", Price = 8000.00m, Quantity = 5 },
                // new Product { ProductID = 4, Name = "Клавиатура Logitech", Category = "Периферия", Price = 3500.00m, Quantity = 40 }
            };

            // Отображаем все товары при загрузке страницы
            SearchResultsGrid.ItemsSource = _allProducts;
            UpdateResultsCount(_allProducts.Count);
        }


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // Если строка поиска пуста, показываем все товары
                SearchResultsGrid.ItemsSource = _allProducts;
                UpdateResultsCount(_allProducts.Count);
                return;
            }

            // Логика поиска: ищем совпадения в Названии, Категории или ID
            var filteredList = _allProducts
                .Where(p => p.Name.ToLower().Contains(searchText) ||
                            ///  p.Category.ToLower().Contains(searchText) ||
                            p.ProductID.ToString().Equals(searchText))
                .ToList();

            SearchResultsGrid.ItemsSource = filteredList;
            UpdateResultsCount(filteredList.Count);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
            // Сбрасываем фильтр, показываем все товары
            SearchResultsGrid.ItemsSource = _allProducts;
            UpdateResultsCount(_allProducts.Count);
        }

        private void UpdateResultsCount(int count)
        {
            txtResultsCount.Text = $"Найдено: {count} товаров";
        }
    }
}