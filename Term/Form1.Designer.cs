    namespace UniversityManager
    {
        partial class Form1
        {
            private System.Windows.Forms.DataGridView dataGridView;
            private System.Windows.Forms.Button btnLoad;
            private System.Windows.Forms.Button btnAdd;
            private System.Windows.Forms.ComboBox comboTables; // ComboBox для вибору таблиці
            private System.Windows.Forms.Panel panelInputs; // Панель для динамічних полів
            private System.Windows.Forms.ComboBox comboPrimaryKey; // ComboBox для вибору значення первинного ключа
            private System.Windows.Forms.Button btnDelete; // Кнопка для видалення рядка

        private void InitializeComponent()
            {
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.comboTables = new System.Windows.Forms.ComboBox();
            this.panelInputs = new System.Windows.Forms.Panel();
            this.comboPrimaryKey = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(657, 200);
            this.dataGridView.TabIndex = 0;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 216);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Завантажити";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Lime;
            this.btnAdd.Location = new System.Drawing.Point(338, 216);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Додати";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // comboTables
            // 
            this.comboTables.Location = new System.Drawing.Point(118, 217);
            this.comboTables.Name = "comboTables";
            this.comboTables.Size = new System.Drawing.Size(120, 21);
            this.comboTables.TabIndex = 3;
            // 
            // panelInputs
            // 
            this.panelInputs.Location = new System.Drawing.Point(338, 249);
            this.panelInputs.Name = "panelInputs";
            this.panelInputs.Size = new System.Drawing.Size(331, 198);
            this.panelInputs.TabIndex = 4;
            // 
            // comboPrimaryKey
            // 
            this.comboPrimaryKey.Location = new System.Drawing.Point(549, 217);
            this.comboPrimaryKey.Name = "comboPrimaryKey";
            this.comboPrimaryKey.Size = new System.Drawing.Size(120, 21);
            this.comboPrimaryKey.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDelete.Location = new System.Drawing.Point(444, 216);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Видалити";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(681, 459);
            this.Controls.Add(this.comboPrimaryKey);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panelInputs);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.comboTables);
            this.Name = "Form1";
            this.Text = "University Manager by Roman Solonetskyi";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

            }
        }
    }
