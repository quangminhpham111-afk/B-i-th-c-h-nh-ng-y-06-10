using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai5_3_DataGridView
{
    public class Form1 : Form
    {
        private TextBox txtProductId, txtProductName, txtUnitPrice, txtQuantity, txtSearch;
        private ComboBox cboCategory;
        private Button btnAdd, btnEdit, btnDelete, btnSearch, btnClear;
        private DataGridView dgvProducts;
        private BindingSource bindingSource;
        private BindingList<Product> products;
        private Label lblSearch;

        public Form1()
        {
            InitializeComponent();
            products = new BindingList<Product>();
            bindingSource = new BindingSource();
            bindingSource.DataSource = products;
            dgvProducts.DataSource = bindingSource;
        }

        private void InitializeComponent()
        {
            Text = "Bài 5.3 - Quản lý sản phẩm";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1050, 650);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var title = new Label {
                Text = "QUẢN LÝ DANH SÁCH SẢN PHẨM",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true, Location = new Point(350, 20)
            };

            var info = new GroupBox { Text = "Thông tin sản phẩm", Location = new Point(30, 70), Size = new Size(480, 300) };
            var l1 = new Label { Text = "Mã SP:", AutoSize = true, Location = new Point(25, 35) };
            txtProductId = new TextBox { Name = "txtProductId", Location = new Point(130, 30), Size = new Size(300, 25) };

            var l2 = new Label { Text = "Tên SP:", AutoSize = true, Location = new Point(25, 80) };
            txtProductName = new TextBox { Name = "txtProductName", Location = new Point(130, 75), Size = new Size(300, 25) };

            var l3 = new Label { Text = "Đơn giá:", AutoSize = true, Location = new Point(25, 125) };
            txtUnitPrice = new TextBox { Name = "txtUnitPrice", Location = new Point(130, 120), Size = new Size(300, 25) };

            var l4 = new Label { Text = "Số lượng:", AutoSize = true, Location = new Point(25, 170) };
            txtQuantity = new TextBox { Name = "txtQuantity", Location = new Point(130, 165), Size = new Size(300, 25) };

            var l5 = new Label { Text = "Danh mục:", AutoSize = true, Location = new Point(25, 215) };
            cboCategory = new ComboBox { Name = "cboCategory", DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(130, 210), Size = new Size(300, 25) };
            cboCategory.Items.AddRange(new object[] { "Điện thoại", "Laptop", "Phụ kiện", "Máy tính bảng" });

            btnClear = new Button { Text = "Làm mới", Location = new Point(130, 250), Size = new Size(100, 32) };
            btnClear.Click += (s, e) => ClearInputs();
            info.Controls.AddRange(new Control[] { l1, txtProductId, l2, txtProductName, l3, txtUnitPrice, l4, txtQuantity, l5, cboCategory, btnClear });

            var func = new GroupBox { Text = "Chức năng", Location = new Point(530, 70), Size = new Size(490, 150) };
            btnAdd = new Button { Text = "Thêm", Location = new Point(25, 35), Size = new Size(90, 35) };
            btnEdit = new Button { Text = "Sửa", Location = new Point(125, 35), Size = new Size(90, 35) };
            btnDelete = new Button { Text = "Xóa", Location = new Point(225, 35), Size = new Size(90, 35) };
            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;

            lblSearch = new Label { Text = "Tìm tên:", AutoSize = true, Location = new Point(25, 90) };
            txtSearch = new TextBox { Name = "txtSearch", Location = new Point(85, 85), Size = new Size(250, 25) };
            btnSearch = new Button { Text = "Tìm kiếm", Location = new Point(345, 83), Size = new Size(100, 30) };
            btnSearch.Click += btnSearch_Click;
            func.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete, lblSearch, txtSearch, btnSearch });

            dgvProducts = new DataGridView {
                Name = "dgvProducts", Location = new Point(30, 390), Size = new Size(990, 230),
                AutoGenerateColumns = true, ReadOnly = true, AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;

            Controls.AddRange(new Control[] { title, info, func, dgvProducts });
        }

        private bool ReadInput(out Product p)
        {
            p = null;
            if (string.IsNullOrWhiteSpace(txtProductId.Text) ||
                string.IsNullOrWhiteSpace(txtProductName.Text) ||
                !decimal.TryParse(txtUnitPrice.Text, out decimal price) ||
                !int.TryParse(txtQuantity.Text, out int quantity) ||
                cboCategory.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng dữ liệu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            p = new Product {
                ProductId = txtProductId.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                UnitPrice = price,
                Quantity = quantity,
                Category = cboCategory.SelectedItem.ToString()
            };
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ReadInput(out Product p)) return;

            if (products.Any(x => x.ProductId.Equals(p.ProductId, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!");
                return;
            }

            products.Add(p);
            bindingSource.ResetBindings(false);
            ClearInputs();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !ReadInput(out Product p)) return;

            int index = dgvProducts.CurrentRow.Index;
            if (index < 0 || index >= products.Count) return;

            products[index] = p;
            bindingSource.ResetBindings(false);
            ClearInputs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            int index = dgvProducts.CurrentRow.Index;
            if (index < 0 || index >= products.Count) return;

            var result = MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                products.RemoveAt(index);
                bindingSource.ResetBindings(false);
                ClearInputs();
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || dgvProducts.CurrentRow.DataBoundItem is not Product p) return;

            txtProductId.Text = p.ProductId;
            txtProductName.Text = p.ProductName;
            txtUnitPrice.Text = p.UnitPrice.ToString();
            txtQuantity.Text = p.Quantity.ToString();
            cboCategory.SelectedItem = p.Category;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string key = txtSearch.Text.Trim().ToLower();

            var result = products
                .Where(p => p.ProductName.ToLower().Contains(key))
                .ToList();

            bindingSource.DataSource = result;
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = -1;
            dgvProducts.ClearSelection();
        }
    }
}
