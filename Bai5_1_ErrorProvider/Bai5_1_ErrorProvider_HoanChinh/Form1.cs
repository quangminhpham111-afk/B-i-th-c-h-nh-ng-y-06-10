using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bai5_1_ErrorProvider_HoanChinh
{
    public class Form1 : Form
    {
        private Label lblTieuDe;
        private Label lblTenDangNhap;
        private Label lblMatKhau;
        private Label lblXacNhan;
        private Label lblNgaySinh;
        private Label lblGioiTinh;

        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private TextBox txtXacNhan;

        private DateTimePicker dtpNgaySinh;
        private RadioButton rdoNam;
        private RadioButton rdoNu;
        private CheckBox chkDieuKhoan;

        private Button btnDangKy;
        private Button btnLamMoi;

        private ErrorProvider epCheck;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 5.1 - Form đăng ký tài khoản";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(650, 430);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTieuDe = new Label();
            lblTieuDe.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTieuDe.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTieuDe.AutoSize = true;
            lblTieuDe.Location = new Point(195, 25);

            lblTenDangNhap = new Label();
            lblTenDangNhap.Text = "Tên đăng nhập:";
            lblTenDangNhap.AutoSize = true;
            lblTenDangNhap.Location = new Point(70, 85);

            txtTenDangNhap = new TextBox();
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(350, 25);
            txtTenDangNhap.Location = new Point(220, 80);

            lblMatKhau = new Label();
            lblMatKhau.Text = "Mật khẩu:";
            lblMatKhau.AutoSize = true;
            lblMatKhau.Location = new Point(70, 130);

            txtMatKhau = new TextBox();
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(350, 25);
            txtMatKhau.Location = new Point(220, 125);
            txtMatKhau.UseSystemPasswordChar = true;

            lblXacNhan = new Label();
            lblXacNhan.Text = "Xác nhận mật khẩu:";
            lblXacNhan.AutoSize = true;
            lblXacNhan.Location = new Point(70, 175);

            txtXacNhan = new TextBox();
            txtXacNhan.Name = "txtXacNhan";
            txtXacNhan.Size = new Size(350, 25);
            txtXacNhan.Location = new Point(220, 170);
            txtXacNhan.UseSystemPasswordChar = true;

            lblNgaySinh = new Label();
            lblNgaySinh.Text = "Ngày sinh:";
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(70, 220);

            dtpNgaySinh = new DateTimePicker();
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Size = new Size(200, 25);
            dtpNgaySinh.Location = new Point(220, 215);
            dtpNgaySinh.MaxDate = DateTime.Today;

            lblGioiTinh = new Label();
            lblGioiTinh.Text = "Giới tính:";
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Location = new Point(70, 265);

            rdoNam = new RadioButton();
            rdoNam.Name = "rdoNam";
            rdoNam.Text = "Nam";
            rdoNam.AutoSize = true;
            rdoNam.Location = new Point(220, 260);

            rdoNu = new RadioButton();
            rdoNu.Name = "rdoNu";
            rdoNu.Text = "Nữ";
            rdoNu.AutoSize = true;
            rdoNu.Location = new Point(300, 260);

            chkDieuKhoan = new CheckBox();
            chkDieuKhoan.Name = "chkDieuKhoan";
            chkDieuKhoan.Text = "Tôi đồng ý với điều khoản dịch vụ";
            chkDieuKhoan.AutoSize = true;
            chkDieuKhoan.Location = new Point(220, 305);

            btnDangKy = new Button();
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Text = "Đăng Ký";
            btnDangKy.Size = new Size(110, 40);
            btnDangKy.Location = new Point(220, 350);
            btnDangKy.Click += btnDangKy_Click;

            btnLamMoi = new Button();
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Text = "Làm Mới";
            btnLamMoi.Size = new Size(110, 40);
            btnLamMoi.Location = new Point(350, 350);
            btnLamMoi.Click += btnLamMoi_Click;

            epCheck = new ErrorProvider();
            epCheck.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            epCheck.ContainerControl = this;

            this.Controls.AddRange(new Control[]
            {
                lblTieuDe,
                lblTenDangNhap, txtTenDangNhap,
                lblMatKhau, txtMatKhau,
                lblXacNhan, txtXacNhan,
                lblNgaySinh, dtpNgaySinh,
                lblGioiTinh, rdoNam, rdoNu,
                chkDieuKhoan,
                btnDangKy, btnLamMoi
            });
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            epCheck.Clear();
            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                epCheck.SetError(txtTenDangNhap, "Tên đăng nhập không được để trống!");
                hopLe = false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                epCheck.SetError(txtMatKhau, "Mật khẩu không được để trống!");
                hopLe = false;
            }

            if (txtMatKhau.Text != txtXacNhan.Text)
            {
                epCheck.SetError(txtXacNhan, "Mật khẩu nhập lại không khớp!");
                hopLe = false;
            }

            int tuoi = DateTime.Today.Year - dtpNgaySinh.Value.Year;

            if (dtpNgaySinh.Value.Date > DateTime.Today.AddYears(-tuoi))
            {
                tuoi--;
            }

            if (tuoi < 18)
            {
                epCheck.SetError(dtpNgaySinh, "Người đăng ký phải đủ 18 tuổi!");
                hopLe = false;
            }

            if (!chkDieuKhoan.Checked)
            {
                epCheck.SetError(chkDieuKhoan, "Bạn phải đồng ý với điều khoản dịch vụ!");
                hopLe = false;
            }

            if (hopLe)
            {
                MessageBox.Show(
                    "Đăng ký tài khoản thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtXacNhan.Clear();

            dtpNgaySinh.Value = DateTime.Today;

            rdoNam.Checked = false;
            rdoNu.Checked = false;
            chkDieuKhoan.Checked = false;

            epCheck.Clear();
            txtTenDangNhap.Focus();
        }
    }
}
