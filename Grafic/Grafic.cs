using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace Grafic
{
    public partial class Grafic : Form
    {
        public Grafic(List<Product> productList)
        {
            InitializeComponent();
            List<Product> products = productList;
            BuildChart(products);
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.FormClosed += (s, args) => Close();
            form1.Show();
        }

        private void BuildChart(List<Product> products)
        {
            chart1.Series.Clear();

            Series series = new Series("Ценовые категории");
            series.ChartType = SeriesChartType.Column;
            series["PointWidth"] = "1"; // Ширина колонок
            chart1.ChartAreas[0].AxisX.Interval = 1;
            Dictionary<string, int> priceCategories = new Dictionary<string, int>();

            foreach (Product product in products)
            {
                string category = GetPriceCategory(product.Price);
                if (priceCategories.ContainsKey(category))
                    priceCategories[category]++;
                else
                    priceCategories[category] = 1;
            }
            foreach (var kvp in priceCategories)
            {
                series.Points.AddXY(kvp.Key, kvp.Value);
            }
            chart1.Series.Add(series);
        }

        private string GetPriceCategory(decimal price)
        {
            if (price >= 100 && price <= 500)
                return "Категория 1: 100-500 руб.";
            else if (price > 500 && price <= 1000)
                return "Категория 2: 500-1000 руб.";
            else
                return "Другая категория";
        }
    }
}

