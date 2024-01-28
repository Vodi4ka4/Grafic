using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Grafic
{
    public partial class Form1 : Form
    {
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=159632;Database=Grafic";
        private List<Product> productList = new List<Product>();
        public Form1()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Product";
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(query, connection);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        Product product = new Product
                        {
                            Name = row["Name"].ToString(),
                            Price = Convert.ToDecimal(row["Price"]),
                            Article = row["Article"].ToString()
                        };

                        productList.Add(product);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
                
            }

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            DataRowView selectedRow = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;

            if (selectedRow != null)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string request = ("INSERT INTO product (name,price,article) values (@name,@price,@article)");
                    using (NpgsqlCommand command = new NpgsqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@name", selectedRow["name"]);
                        command.Parameters.AddWithValue("@price", selectedRow["price"]);
                        command.Parameters.AddWithValue("@article", selectedRow["article"]);
                        try
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Данные успешно добавлены в базу данных.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при вставке данных: " + ex.Message);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Выберите строку для вставки данных.");
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DataRowView selectedRow = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;

            if (selectedRow != null)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string request = ("DELETE FROM product where id = @id");
                    using (NpgsqlCommand command = new NpgsqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedRow["id"]);

                        try
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Данные успешно удалены из базы данных.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при удалении данных: " + ex.Message);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Выберите строку для вставки данных.");
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            DataRowView selectedRow = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;

            if (selectedRow != null)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string request = ("UPDATE product SET id  = @id, name = @name, price =@price , article = @article WHERE id = @id");
                    using (NpgsqlCommand command = new NpgsqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedRow["id"]);
                        command.Parameters.AddWithValue("@name", selectedRow["name"]);
                        command.Parameters.AddWithValue("@price", selectedRow["price"]);
                        command.Parameters.AddWithValue("@article", selectedRow["article"]);

                        try
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Данные успешно изменены в базе данных.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при изменении данных: " + ex.Message);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Выберите строку для вставки данных.");
            }
        }

        private void button_new_form_Click(object sender, EventArgs e)
        {
            Grafic grafic = new Grafic(productList);
            grafic.FormClosed += (s, args) => Close();
            grafic.Show();
        }
    }
    
}
