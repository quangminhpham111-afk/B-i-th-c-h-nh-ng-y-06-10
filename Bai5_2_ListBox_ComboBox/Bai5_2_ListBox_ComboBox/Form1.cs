using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai5_2_ListBox_ComboBox
{
    public class Form1 : Form
    {
        private ComboBox cboCategory;
        private ListBox lstAvailableServices, lstSelectedServices;
        private Button btnSelect, btnRemove, btnClearAll, btnApplyDiscount;
        private Label lblTotal, lblDiscount, lblPayment, lblCode;
        private TextBox txtDiscountCode;
        private NumericUpDown nudDiscount;
        private Dictionary<string, List<Service>> services;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            cboCategory.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            Text = "Bài 5.2 - Bảng tính tiền dịch vụ";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 620);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var title = new Label {
                Text = "BẢNG TÍNH TIỀN DỊCH VỤ VÀ CHIẾT KHẤU ĐƠN HÀNG",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true, Location = new Point(150, 20)
            };

            var l1 = new Label { Text = "Loại dịch vụ:", AutoSize = true, Location = new Point(50, 80) };
            cboCategory = new ComboBox { Name = "cboCategory", DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(150, 75), Size = new Size(250, 30) };
            cboCategory.SelectedIndexChanged += cboCategory_SelectedIndexChanged;

            var l2 = new Label { Text = "Dịch vụ có sẵn", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(50, 125) };
            lstAvailableServices = new ListBox { Name = "lstAvailableServices", Location = new Point(50, 155), Size = new Size(330, 230) };
            lstAvailableServices.DoubleClick += lstAvailableServices_DoubleClick;

            var l3 = new Label { Text = "Dịch vụ đã chọn", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(520, 125) };
            lstSelectedServices = new ListBox { Name = "lstSelectedServices", Location = new Point(520, 155), Size = new Size(330, 230) };

            btnSelect = new Button { Name = "btnSelect", Text = ">", Location = new Point(405, 190), Size = new Size(80, 40) };
            btnRemove = new Button { Name = "btnRemove", Text = "<", Location = new Point(405, 245), Size = new Size(80, 40) };
            btnClearAll = new Button { Name = "btnClearAll", Text = "<<", Location = new Point(405, 300), Size = new Size(80, 40) };
            btnSelect.Click += btnSelect_Click;
            btnRemove.Click += btnRemove_Click;
            btnClearAll.Click += btnClearAll_Click;

            var box = new GroupBox { Text = "Tính tiền", Location = new Point(50, 410), Size = new Size(800, 170) };

            lblTotal = new Label { Text = "Tổng tiền chưa giảm: 0 VNĐ", AutoSize = true, Location = new Point(25, 30) };
            lblDiscount = new Label { Text = "Tỷ lệ chiết khấu (%):", AutoSize = true, Location = new Point(25, 65) };
            nudDiscount = new NumericUpDown { Name = "nudDiscount", Minimum = 0, Maximum = 100, Location = new Point(180, 60), Size = new Size(80, 25) };
            nudDiscount.ValueChanged += (s, e) => UpdateTotal();

            lblPayment = new Label { Text = "Thành tiền thanh toán: 0 VNĐ", AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold), Location = new Point(300, 65) };

            lblCode = new Label { Text = "Mã giảm giá:", AutoSize = true, Location = new Point(25, 105) };
            txtDiscountCode = new TextBox { Name = "txtDiscountCode", Location = new Point(110, 100), Size = new Size(120, 25) };
            btnApplyDiscount = new Button { Text = "Áp dụng", Location = new Point(240, 98), Size = new Size(80, 30) };
            btnApplyDiscount.Click += btnApplyDiscount_Click;

            var note = new Label { Text = "Mã mẫu: GIAM10 = 10%, GIAM20 = 20%", AutoSize = true, Location = new Point(350, 105) };

            box.Controls.AddRange(new Control[] { lblTotal, lblDiscount, nudDiscount, lblPayment, lblCode, txtDiscountCode, btnApplyDiscount, note });

            Controls.AddRange(new Control[] { title, l1, cboCategory, l2, lstAvailableServices, l3, lstSelectedServices,
                btnSelect, btnRemove, btnClearAll, box });
        }

        private void LoadData()
        {
            services = new Dictionary<string, List<Service>>
            {
                ["Khám bệnh"] = new List<Service> {
                    new Service("Khám tổng quát", 200000),
                    new Service("Khám chuyên khoa", 300000),
                    new Service("Khám sức khỏe định kỳ", 500000)
                },
                ["Xét nghiệm"] = new List<Service> {
                    new Service("Xét nghiệm máu", 150000),
                    new Service("Xét nghiệm nước tiểu", 100000),
                    new Service("Xét nghiệm đường huyết", 120000)
                },
                ["Chụp X-Quang"] = new List<Service> {
                    new Service("X-Quang ngực", 200000),
                    new Service("X-Quang xương", 250000),
                    new Service("X-Quang cột sống", 350000)
                },
                ["Vắc-xin"] = new List<Service> {
                    new Service("Vắc-xin cúm", 250000),
                    new Service("Vắc-xin viêm gan B", 300000),
                    new Service("Vắc-xin HPV", 1800000)
                }
            };

            cboCategory.Items.AddRange(services.Keys.ToArray());
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstAvailableServices.Items.Clear();
            if (cboCategory.SelectedItem == null) return;

            foreach (var s in services[cboCategory.SelectedItem.ToString()])
                lstAvailableServices.Items.Add(s);
        }

        private void AddSelectedService()
        {
            if (lstAvailableServices.SelectedItem == null) return;
            var service = (Service)lstAvailableServices.SelectedItem;
            lstSelectedServices.Items.Add(service);
            UpdateTotal();
        }

        private void btnSelect_Click(object sender, EventArgs e) => AddSelectedService();
        private void lstAvailableServices_DoubleClick(object sender, EventArgs e) => AddSelectedService();

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstSelectedServices.SelectedItem != null)
            {
                lstSelectedServices.Items.RemoveAt(lstSelectedServices.SelectedIndex);
                UpdateTotal();
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            lstSelectedServices.Items.Clear();
            UpdateTotal();
        }

        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            string code = txtDiscountCode.Text.Trim().ToUpper();
            if (code == "GIAM10") nudDiscount.Value = 10;
            else if (code == "GIAM20") nudDiscount.Value = 20;
            else
            {
                nudDiscount.Value = 0;
                MessageBox.Show("Mã giảm giá không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (Service s in lstSelectedServices.Items)
                total += s.Price;

            decimal payment = total * (100 - nudDiscount.Value) / 100;
            lblTotal.Text = $"Tổng tiền chưa giảm: {total:N0} VNĐ";
            lblPayment.Text = $"Thành tiền thanh toán: {payment:N0} VNĐ";
        }

        private class Service
        {
            public string Name { get; set; }
            public decimal Price { get; set; }

            public Service(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            public override string ToString()
            {
                return $"{Name} - {Price:N0} VNĐ";
            }
        }
    }
}
