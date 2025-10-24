using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            LoadStatistics();
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }
        private void LoadStatistics()
        {
            try
            {
                using (var context = new ProductInventoryEntities())
                {
                    var products = context.Product.ToList();
                    var totalProducts = products.Count;
                    var totalValue = products.Sum(p => p.Price * p.Quantity);
                    var averagePrice = products.Any() ? products.Average(p => p.Price) : 0;

                    txtTotalProducts.Text = totalProducts.ToString();
                    txtTotalValue.Text = $"{totalValue:N2} руб";
                    txtAveragePrice.Text = $"{averagePrice:N2} руб";

                    var categoryStats = (from product in context.Product
                                         join category in context.Category on product.CategoryID equals category.CategoryID
                                         group product by new { category.CategoryID, category.CategoryName } into g
                                         select new CategoryStatistics
                                         {
                                             Category = g.Key.CategoryName,
                                             TotalProducts = g.Count(),
                                             AveragePrice = g.Average(p => p.Price),
                                             TotalValue = g.Sum(p => p.Price * p.Quantity)
                                         }).ToList();

                    StatisticsGrid.ItemsSource = categoryStats;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class CategoryStatistics
        {
            public string Category { get; set; }
            public int TotalProducts { get; set; }
            public decimal AveragePrice { get; set; }
            public decimal TotalValue { get; set; }
        }
    }
}