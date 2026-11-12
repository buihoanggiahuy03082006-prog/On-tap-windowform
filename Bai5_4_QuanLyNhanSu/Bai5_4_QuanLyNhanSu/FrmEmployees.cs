using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bai5_4_QuanLyNhanSu
{
    public partial class FrmEmployees : Form
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public FrmEmployees()
        {
            InitializeComponent();
            BuildImageLists();
        }

        private void FrmEmployees_Load(object sender, EventArgs e)
        {
            // Chế độ xem của ListView
            cboView.Items.AddRange(new object[] { "Details", "LargeIcon", "SmallIcon", "List", "Tile" });

            BuildSampleTree();
            BuildSampleEmployees();

            tvDepartments.ExpandAll();
            tvDepartments.SelectedNode = tvDepartments.Nodes[0]; // kích hoạt AfterSelect -> nạp ListView

            cboView.SelectedIndex = 0; // Details
        }

        // ==================== 1. Dữ liệu mẫu cho TreeView ====================
        // Node.Name dùng làm khóa (key) để lọc nhân viên.
        private void BuildSampleTree()
        {
            tvDepartments.Nodes.Clear();

            TreeNode root = tvDepartments.Nodes.Add("CTY", "Công ty ABC", "company", "company");

            TreeNode kt = root.Nodes.Add("KT", "Phòng Kỹ thuật", "department", "department");
            kt.Nodes.Add("KT_BE", "Nhóm Backend", "group", "group");
            kt.Nodes.Add("KT_FE", "Nhóm Frontend", "group", "group");
            kt.Nodes.Add("KT_QA", "Nhóm QA", "group", "group");

            TreeNode ns = root.Nodes.Add("NS", "Phòng Nhân sự", "department", "department");
            ns.Nodes.Add("NS_TD", "Nhóm Tuyển dụng", "group", "group");
            ns.Nodes.Add("NS_DT", "Nhóm Đào tạo", "group", "group");

            TreeNode kd = root.Nodes.Add("KD", "Phòng Kinh doanh", "department", "department");
            kd.Nodes.Add("KD_MB", "Nhóm Miền Bắc", "group", "group");
            kd.Nodes.Add("KD_MN", "Nhóm Miền Nam", "group", "group");
        }

        private void BuildSampleEmployees()
        {
            _employees.Clear();

            // Cấp công ty
            Add("NV001", "Nguyễn Văn An", "Giám đốc", 2015, 3, 1, "CTY");

            // Phòng Kỹ thuật
            Add("NV002", "Trần Minh Quân", "Trưởng phòng Kỹ thuật", 2017, 6, 15, "KT");
            Add("NV003", "Lê Hoàng Nam", "Lập trình viên Backend", 2020, 2, 10, "KT_BE");
            Add("NV004", "Phạm Thu Hà", "Lập trình viên Backend", 2021, 9, 5, "KT_BE");
            Add("NV005", "Vũ Đức Huy", "Lập trình viên Frontend", 2022, 1, 17, "KT_FE");
            Add("NV006", "Đặng Mai Linh", "Lập trình viên Frontend", 2023, 4, 3, "KT_FE");
            Add("NV007", "Bùi Quốc Bảo", "Kiểm thử viên", 2022, 11, 21, "KT_QA");

            // Phòng Nhân sự
            Add("NV008", "Hoàng Thị Lan", "Trưởng phòng Nhân sự", 2016, 8, 8, "NS");
            Add("NV009", "Ngô Thanh Tùng", "Chuyên viên tuyển dụng", 2021, 5, 12, "NS_TD");
            Add("NV010", "Đỗ Khánh Vy", "Chuyên viên đào tạo", 2020, 10, 26, "NS_DT");

            // Phòng Kinh doanh
            Add("NV011", "Lý Gia Bảo", "Trưởng phòng Kinh doanh", 2016, 1, 4, "KD");
            Add("NV012", "Phan Ngọc Ánh", "Nhân viên kinh doanh", 2021, 3, 29, "KD_MB");
            Add("NV013", "Trịnh Văn Khoa", "Nhân viên kinh doanh", 2023, 7, 14, "KD_MB");
            Add("NV014", "Mai Thị Hương", "Nhân viên kinh doanh", 2022, 12, 1, "KD_MN");
        }

        private void Add(string id, string name, string position, int y, int m, int d, string unitKey)
        {
            _employees.Add(new Employee(id, name, position, new DateTime(y, m, d), unitKey));
        }

        // ==================== 2. Click Node -> lọc & nạp ListView ====================
        private void tvDepartments_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadEmployees(e.Node);
        }

        // Lấy khóa của node đang chọn và toàn bộ node con cháu
        private void CollectKeys(TreeNode node, HashSet<string> keys)
        {
            keys.Add(node.Name);
            foreach (TreeNode child in node.Nodes)
                CollectKeys(child, keys);
        }

        private void LoadEmployees(TreeNode node)
        {
            var keys = new HashSet<string>();
            CollectKeys(node, keys);

            int count = 0;
            lsvEmployees.BeginUpdate();
            lsvEmployees.Items.Clear();

            foreach (Employee emp in _employees)
            {
                if (!keys.Contains(emp.UnitKey)) continue;

                // Details: cột 1 = Mã NV, tiếp theo Họ Tên, Chức vụ, Ngày vào làm
                var item = new ListViewItem(emp.EmployeeId, "employee");
                item.SubItems.Add(emp.FullName);
                item.SubItems.Add(emp.Position);
                item.SubItems.Add(emp.StartDate.ToString("dd/MM/yyyy"));
                item.ToolTipText = emp.FullName + " - " + emp.Position;
                item.Tag = emp;

                lsvEmployees.Items.Add(item);
                count++;
            }

            lsvEmployees.EndUpdate();
            lblStatus.Text = node.Text + ": " + count + " nhân viên";
        }

        // ==================== Chuyển chế độ xem ====================
        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboView.SelectedItem == null) return;
            lsvEmployees.View = (View)Enum.Parse(typeof(View), cboView.SelectedItem.ToString());
        }

        // ==================== ImageList: vẽ icon bằng code ====================
        private void BuildImageLists()
        {
            string[] keys = { "company", "department", "group", "employee" };
            foreach (string key in keys)
            {
                imgSmall.Images.Add(key, DrawIcon(key, 16));
                imgLarge.Images.Add(key, DrawIcon(key, 32));
            }
        }

        private static Bitmap DrawIcon(string kind, int size)
        {
            var bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                float s = size / 16f; // tỉ lệ theo lưới 16x16

                switch (kind)
                {
                    case "company": // tòa nhà
                        using (var body = new SolidBrush(Color.FromArgb(52, 101, 164)))
                        using (var win = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(body, 3 * s, 1 * s, 10 * s, 14 * s);
                            for (int r = 0; r < 4; r++)
                                for (int c = 0; c < 2; c++)
                                    g.FillRectangle(win, (5 + c * 4) * s, (3 + r * 3) * s, 2 * s, 2 * s);
                        }
                        break;

                    case "department": // thư mục
                        using (var tab = new SolidBrush(Color.FromArgb(230, 170, 40)))
                        using (var body = new SolidBrush(Color.FromArgb(250, 200, 70)))
                        {
                            g.FillRectangle(tab, 1 * s, 3 * s, 6 * s, 3 * s);
                            g.FillRectangle(body, 1 * s, 5 * s, 14 * s, 9 * s);
                        }
                        break;

                    case "group": // hai người
                        using (var b1 = new SolidBrush(Color.FromArgb(78, 154, 6)))
                        using (var b2 = new SolidBrush(Color.FromArgb(115, 190, 40)))
                        {
                            g.FillEllipse(b1, 2 * s, 3 * s, 5 * s, 5 * s);
                            g.FillEllipse(b1, 0.5f * s, 9 * s, 8 * s, 6 * s);
                            g.FillEllipse(b2, 9 * s, 3 * s, 5 * s, 5 * s);
                            g.FillEllipse(b2, 7.5f * s, 9 * s, 8 * s, 6 * s);
                        }
                        break;

                    default: // employee: một người
                        using (var head = new SolidBrush(Color.FromArgb(92, 53, 102)))
                        using (var body = new SolidBrush(Color.FromArgb(117, 80, 123)))
                        {
                            g.FillEllipse(head, 4.5f * s, 1.5f * s, 7 * s, 7 * s);
                            g.FillEllipse(body, 2 * s, 9 * s, 12 * s, 8 * s);
                        }
                        break;
                }
            }
            return bmp;
        }
    }
}
