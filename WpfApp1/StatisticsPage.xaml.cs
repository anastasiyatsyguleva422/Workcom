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
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private ProductInventoryEntities _context;
        public StatisticsPage(ProductInventoryEntities context)
        {
            InitializeComponent();
            _context = context;
            LoadStatistics();
        }
        private void LoadStatistics()
        {
            try
            {
                var generalStats = GetGeneralStatistics();
                txtTotalProducts.Text = generalStats.TotalProducts.ToString();
                txtTotalValue.Text = $"{generalStats.TotalValue:N2} руб";
                txtAveragePrice.Text = $"{generalStats.AveragePrice:N2} руб";

                var categoryStats = GetCategoryStatistics();
                StatisticsGrid.ItemsSource = categoryStats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статистики: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private GeneralStatistics GetGeneralStatistics()
        {
            var products = _context.Products.ToList();

            var stats = new GeneralStatistics
            {
                TotalProducts = products.Count,
                TotalValue = products.Sum(p => p.Price * p.Quantity),
                AveragePrice = products.Any() ? products.Average(p => p.Price) : 0
            };

            return stats;
        }
        private List<CategoryStatistics> GetCategoryStatistics()
        {
            var categoryStats = (from product in _context.Products
                                 join category in _context.Categories on product.CategoryId equals category.Id
                                 group product by new { category.Id, category.Name } into g
                                 select new CategoryStatistics
                                 {
                                     Category = g.Key.Name,
                                     TotalProducts = g.Count(),
                                     AveragePrice = g.Average(p => p.Price),
                                     TotalValue = g.Sum(p => p.Price * p.Quantity)
                                 }).ToList();

            return categoryStats;
        }
    }

    public class GeneralStatistics
    {
        public int TotalProducts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class CategoryStatistics
    {
        public string Category { get; set; }
        public int TotalProducts { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalValue { get; set; }
    }
}

