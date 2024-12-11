using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UniversityManager
{
    public partial class Form1 : Form
    {
        private MySqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            string connectionString = "Server=localhost;Database=Universitet;Uid=root;Pwd=1234;";
            connection = new MySqlConnection(connectionString);

            // Заповнюємо ComboBox таблицями
            comboTables.Items.AddRange(new string[] { "students", "teachers", "subjects", "grades", "schedule" });
            comboTables.SelectedIndexChanged += ComboTables_SelectedIndexChanged;
            comboTables.SelectedIndex = 0; // Вибираємо першу таблицю за замовчуванням
        }

        private void ComboTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateInputFields();
        }

        private void GenerateInputFields()
        {
            panelInputs.Controls.Clear(); // Очищаємо панель

            try
            {
                connection.Open();
                string selectedTable = comboTables.SelectedItem.ToString();
                string query = $"DESCRIBE {selectedTable}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                int yPosition = 0;

                while (reader.Read())
                {
                    string columnName = reader["Field"].ToString();
                    string dataType = reader["Type"].ToString();

                    // Додаємо підпис
                    Label label = new Label
                    {
                        Text = columnName,
                        Location = new System.Drawing.Point(10, yPosition),
                        AutoSize = true
                    };
                    panelInputs.Controls.Add(label);

                    // Визначаємо тип поля
                    if (dataType.Contains("date") || dataType.Contains("timestamp") || dataType.Contains("datetime"))
                    {
                        // Поле для вибору дати
                        DateTimePicker datePicker = new DateTimePicker
                        {
                            Name = "dtp_" + columnName,
                            Location = new System.Drawing.Point(120, yPosition),
                            Size = new System.Drawing.Size(200, 20),
                            Format = DateTimePickerFormat.Custom,
                            CustomFormat = "yyyy-MM-dd" // Формат для MySQL
                        };
                        panelInputs.Controls.Add(datePicker);
                    }
                    else
                    {
                        // Текстове поле для інших типів
                        TextBox textBox = new TextBox
                        {
                            Name = "txt_" + columnName,
                            Location = new System.Drawing.Point(120, yPosition),
                            Size = new System.Drawing.Size(200, 20)
                        };
                        panelInputs.Controls.Add(textBox);
                    }

                    yPosition += 30; // Зміщуємо наступний рядок вниз
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка створення полів введення: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }



        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string selectedTable = comboTables.SelectedItem.ToString();
                string query = $"SELECT * FROM {selectedTable}";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView.DataSource = table;

                // Очищуємо та заповнюємо ComboBox значеннями з першої колонки
                comboPrimaryKey.Items.Clear();
                foreach (DataRow row in table.Rows)
                {
                    comboPrimaryKey.Items.Add(row[0].ToString()); // Додаємо значення першої колонки
                }

                if (comboPrimaryKey.Items.Count > 0)
                {
                    comboPrimaryKey.SelectedIndex = 0; // Вибираємо перше значення
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string selectedTable = comboTables.SelectedItem.ToString();

                // Формуємо SQL-запит
                string query = $"INSERT INTO {selectedTable} ({GetColumns()}) VALUES ({GetParameters()})";
                MySqlCommand command = new MySqlCommand(query, connection);

                foreach (Control control in panelInputs.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        string paramName = textBox.Name.Replace("txt_", "");
                        command.Parameters.AddWithValue("@" + paramName, textBox.Text);
                    }
                    else if (control is DateTimePicker datePicker)
                    {
                        string paramName = datePicker.Name.Replace("dtp_", "");
                        command.Parameters.AddWithValue("@" + paramName, datePicker.Value.ToString("yyyy-MM-dd"));
                    }
                }

                // Виконання команди
                command.ExecuteNonQuery();
                MessageBox.Show("Запис успішно додано.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
            finally
            {
                connection.Close();
                btnLoad_Click(sender, e); // Оновлюємо таблицю після додавання
            }
        }

        private string GetColumns()
        {
            string columns = "";
            foreach (Control control in panelInputs.Controls)
            {
                if (control is TextBox textBox)
                {
                    columns += textBox.Name.Replace("txt_", "") + ",";
                }
                else if (control is DateTimePicker datePicker)
                {
                    columns += datePicker.Name.Replace("dtp_", "") + ",";
                }
            }
            return columns.TrimEnd(','); // Видаляємо останню кому
        }

        private string GetParameters()
        {
            string parameters = "";
            foreach (Control control in panelInputs.Controls)
            {
                if (control is TextBox textBox)
                {
                    parameters += "@" + textBox.Name.Replace("txt_", "") + ",";
                }
                else if (control is DateTimePicker datePicker)
                {
                    parameters += "@" + datePicker.Name.Replace("dtp_", "") + ",";
                }
            }
            return parameters.TrimEnd(','); // Видаляємо останню кому
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (comboPrimaryKey.SelectedItem == null)
            {
                MessageBox.Show("Оберіть значення для видалення.");
                return;
            }

            try
            {
                connection.Open();
                string selectedTable = comboTables.SelectedItem.ToString();
                string primaryKeyValue = comboPrimaryKey.SelectedItem.ToString();

                // Отримуємо назву первинного ключа
                string primaryKeyColumn = GetPrimaryKey(selectedTable);
                if (string.IsNullOrEmpty(primaryKeyColumn))
                {
                    MessageBox.Show("Не вдалося визначити первинний ключ для таблиці.");
                    return;
                }

                // Формуємо SQL-запит на видалення
                string query = $"DELETE FROM {selectedTable} WHERE {primaryKeyColumn} = @primaryKeyValue";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@primaryKeyValue", primaryKeyValue);

                // Виконуємо запит
                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    MessageBox.Show("Рядок успішно видалено.");
                    btnLoad_Click(sender, e); // Оновлюємо таблицю
                }
                else
                {
                    MessageBox.Show("Помилка видалення рядка.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        // Метод для отримання первинного ключа таблиці (той самий, що раніше)
        private string GetPrimaryKey(string tableName)
        {
            try
            {
                string query = $"SHOW KEYS FROM {tableName} WHERE Key_name = 'PRIMARY'";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string primaryKey = reader["Column_name"].ToString();
                    reader.Close();
                    return primaryKey;
                }

                reader.Close();
                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}
