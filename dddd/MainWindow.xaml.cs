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

namespace dddd
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Update();
        }
        /// <summary>
        /// основной метод заполнения данных из БД в datagrid и другие элементы страницы
        /// </summary>
        private void Update()
        {
            var query = lastEntities.GetContext().Product.ToList();

            //фильтрация по убыванию и возрастанию
            switch (cmbSor.SelectedIndex)
            {
                case 0:
                    query = query.OrderBy(p => p.cost).ToList();
                    break;
                case 1:
                    query = query.OrderByDescending(p => p.cost).ToList();
                    break;
               default:
                    break;
            }

            //фильтрация по скидке
            switch (cmbFiltration.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    query = query.Where(p => p.maxDiscount < 10).ToList();
                    break;
                case 2:
                    query = query.Where(p => p.maxDiscount > 10 && p.maxDiscount < 15).ToList();
                    break;
                case 3:
                    query = query.Where(p => p.maxDiscount > 15).ToList();
                    break;
                default:
                    break;
            }

            dgProducts.ItemsSource = query;

            //присваюваю каждому товару данные о сниженной цене, для использования их в dataGrid
            foreach(var item in query)
            {
                item.discounedPrice = item.cost * (1 - (item.currentDiscount) / 100);
            }
            //данные о количестве вывденных товаров
            tbCurrentProductsNum.Text = (query.Count()).ToString();
            tbTotalProductsNum.Text = (lastEntities.GetContext().Product.ToList().Count()).ToString();
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            var item = e.Row.Item as Product;
            //окрашиваю строку, если максимальная скидка элемента в нем больше 15
            if (item != null && item.maxDiscount > 15)
            {
                e.Row.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7fff00"));
            }
            else
            {
                e.Row.Background = Brushes.White;
            }
        }

        private void cmbSor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void cmbFiltration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
    }
}
