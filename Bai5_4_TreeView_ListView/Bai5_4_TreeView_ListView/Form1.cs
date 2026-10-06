using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai5_4_TreeView_ListView
{
    public class Form1 : Form
    {
        private SplitContainer splitContainer;
        private TreeView tvDepartments;
        private ListView lsvEmployees;
        private ComboBox cboView;
        private ImageList imageList;
        private List<Employee> employees;

        public Form1()
        {
            InitializeComponent();
            LoadEmployees();
            BuildTree();
        }

        private void InitializeComponent()
        {
            Text = "Bài 5.4 - Quản lý nhân viên";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 680);

            var top = new Panel { Dock = DockStyle.Top, Height = 55 };
            var label = new Label { Text = "Chế độ xem:", AutoSize = true, Location = new Point(20, 18) };
            cboView = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(100, 13), Size = new Size(180, 28) };
            cboView.Items.AddRange(new object[] { "Details", "SmallIcon", "LargeIcon", "Tile" });
            cboView.SelectedIndex = 0;
            cboView.SelectedIndexChanged += cboView_SelectedIndexChanged;
            top.Controls.AddRange(new Control[] { label, cboView });

            splitContainer = new SplitContainer {
                Dock = DockStyle.Fill, SplitterDistance = 330, IsSplitterFixed = false
            };

            tvDepartments = new TreeView {
                Name = "tvDepartments", Dock = DockStyle.Fill,
                Font = new Font("Arial", 10)
            };
            tvDepartments.AfterSelect += tvDepartments_AfterSelect;

            lsvEmployees = new ListView {
                Name = "lsvEmployees", Dock = DockStyle.Fill,
                View = View.Details, FullRowSelect = true,
                GridLines = true, MultiSelect = false
            };
            lsvEmployees.Columns.Add("Mã NV", 100);
            lsvEmployees.Columns.Add("Họ Tên", 220);
            lsvEmployees.Columns.Add("Chức vụ", 180);
            lsvEmployees.Columns.Add("Ngày vào làm", 130);

            imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.Images.Add(SystemIcons.Application);
            imageList.Images.Add(SystemIcons.Information);
            tvDepartments.ImageList = imageList;
            lsvEmployees.SmallImageList = imageList;
            lsvEmployees.LargeImageList = imageList;

            splitContainer.Panel1.Controls.Add(tvDepartments);
            splitContainer.Panel2.Controls.Add(lsvEmployees);

            Controls.Add(splitContainer);
            Controls.Add(top);
        }

        private void LoadEmployees()
        {
            employees = new List<Employee>
            {
                new Employee("NV001", "Nguyễn Văn An", "Trưởng phòng", new DateTime(2020, 3, 10), "Phòng Kinh doanh", "Nhóm 1"),
                new Employee("NV002", "Trần Thị Bình", "Nhân viên kinh doanh", new DateTime(2022, 6, 15), "Phòng Kinh doanh", "Nhóm 1"),
                new Employee("NV003", "Lê Văn Cường", "Nhân viên kinh doanh", new DateTime(2023, 1, 20), "Phòng Kinh doanh", "Nhóm 2"),
                new Employee("NV004", "Phạm Thị Dung", "Trưởng phòng", new DateTime(2019, 8, 5), "Phòng Kỹ thuật", "Nhóm 1"),
                new Employee("NV005", "Hoàng Văn Em", "Lập trình viên", new DateTime(2021, 4, 12), "Phòng Kỹ thuật", "Nhóm 1"),
                new Employee("NV006", "Đỗ Thị Hoa", "Lập trình viên", new DateTime(2024, 2, 1), "Phòng Kỹ thuật", "Nhóm 2"),
                new Employee("NV007", "Vũ Văn Khánh", "Trưởng phòng", new DateTime(2020, 9, 18), "Phòng Nhân sự", "Nhóm 1"),
                new Employee("NV008", "Ngô Thị Lan", "Chuyên viên nhân sự", new DateTime(2022, 11, 7), "Phòng Nhân sự", "Nhóm 1")
            };
        }

        private void BuildTree()
        {
            tvDepartments.Nodes.Clear();

            TreeNode company = new TreeNode("Công ty") { ImageIndex = 0, SelectedImageIndex = 0 };

            var departmentNames = employees.Select(e => e.Department).Distinct();
            foreach (string deptName in departmentNames)
            {
                TreeNode dept = new TreeNode(deptName) { ImageIndex = 1, SelectedImageIndex = 1 };

                foreach (string groupName in employees.Where(e => e.Department == deptName)
                    .Select(e => e.Group).Distinct())
                {
                    TreeNode group = new TreeNode(groupName) { ImageIndex = 1, SelectedImageIndex = 1 };
                    dept.Nodes.Add(group);
                }

                company.Nodes.Add(dept);
            }

            tvDepartments.Nodes.Add(company);
            company.Expand();
            foreach (TreeNode n in company.Nodes) n.Expand();
            tvDepartments.SelectedNode = company;
        }

        private void tvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string selected = e.Node.Text;
            IEnumerable<Employee> result;

            if (selected == "Công ty")
            {
                result = employees;
            }
            else if (e.Node.Parent != null && e.Node.Parent.Text == "Công ty")
            {
                result = employees.Where(x => x.Department == selected);
            }
            else
            {
                result = employees.Where(x => x.Group == selected &&
                                               x.Department == e.Node.Parent.Text);
            }

            ShowEmployees(result);
        }

        private void ShowEmployees(IEnumerable<Employee> list)
        {
            lsvEmployees.Items.Clear();

            foreach (var e in list)
            {
                var item = new ListViewItem(e.Id, 0);
                item.SubItems.Add(e.FullName);
                item.SubItems.Add(e.Position);
                item.SubItems.Add(e.StartDate.ToString("dd/MM/yyyy"));
                lsvEmployees.Items.Add(item);
            }
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboView.SelectedItem.ToString())
            {
                case "Details":
                    lsvEmployees.View = View.Details;
                    break;
                case "SmallIcon":
                    lsvEmployees.View = View.SmallIcon;
                    break;
                case "LargeIcon":
                    lsvEmployees.View = View.LargeIcon;
                    break;
                case "Tile":
                    lsvEmployees.View = View.Tile;
                    break;
            }
        }
    }
}
