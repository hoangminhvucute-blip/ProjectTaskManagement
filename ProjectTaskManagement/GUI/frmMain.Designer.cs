using System.Drawing;
using System.Windows.Forms;

namespace ProjectTaskManagement.GUI
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer components = null;

        // Project Controls
        private GroupBox grpDuAn;
        private TextBox txtTenDuAn;
        private DateTimePicker dtpNgayBatDau;
        private DateTimePicker dtpNgayKetThuc;
        private ComboBox cboTrangThaiDuAn;
        private Button btnThemDuAn;
        private Button btnSuaDuAn;
        private Button btnXoaDuAn;
        private DataGridView dgvDuAn;

        // Task Controls
        private GroupBox grpTask;
        private TextBox txtTenTask;
        private TextBox txtMoTaTask;
        private DateTimePicker dtpHanNop;
        private ComboBox cboDoUuTien;
        private Button btnThemTask;

        // Filter & Action Controls
        private ComboBox cboDuAnFilter;
        private ComboBox cboTrangThaiFilter;
        private ComboBox cboTrangThaiMoi;
        private Button btnCapNhatTrangThai;
        private Button btnPhanCong;

        // Views: List & Kanban
        private TabControl tabViewMode;
        private TabPage tabListView;
        private TabPage tabKanbanView;
        private DataGridView dgvTasks;
        private FlowLayoutPanel flpTodo;
        private FlowLayoutPanel flpInProgress;
        private FlowLayoutPanel flpReview;
        private FlowLayoutPanel flpDone;

        // Overdue & Stats
        private GroupBox grpBottom;
        private Label lblCanhBaoTreHan;
        private DataGridView dgvOverdue;
        private DataGridView dgvStats;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text = "Hệ thống Quản lý Công việc & Tiến độ Dự án (3-Tier ADO.NET)";
            this.Size = new Size(1340, 820);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);

            // --- LEFT PANEL: DỰ ÁN ---
            grpDuAn = new GroupBox { Text = "1. Quản lý Dự án", Location = new Point(12, 10), Size = new Size(410, 480) };

            var lblTenDA = new Label { Text = "Tên dự án:", Location = new Point(12, 26), AutoSize = true };
            txtTenDuAn = new TextBox { Location = new Point(110, 23), Width = 285 };

            var lblBD = new Label { Text = "Ngày bắt đầu:", Location = new Point(12, 56), AutoSize = true };
            dtpNgayBatDau = new DateTimePicker { Location = new Point(110, 53), Width = 130, Format = DateTimePickerFormat.Short };

            var lblKT = new Label { Text = "Kết thúc:", Location = new Point(12, 86), AutoSize = true };
            dtpNgayKetThuc = new DateTimePicker { Location = new Point(110, 83), Width = 130, Format = DateTimePickerFormat.Short };

            var lblTTDA = new Label { Text = "Trạng thái:", Location = new Point(12, 116), AutoSize = true };
            cboTrangThaiDuAn = new ComboBox { Location = new Point(110, 113), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThaiDuAn.Items.AddRange(new object[] { "Đang chạy", "Hoàn thành", "Tạm dừng" });
            cboTrangThaiDuAn.SelectedIndex = 0;

            btnThemDuAn = new Button { Text = "Thêm DA", Location = new Point(12, 148), Size = new Size(90, 28) };
            btnSuaDuAn = new Button { Text = "Sửa DA", Location = new Point(110, 148), Size = new Size(90, 28) };
            btnXoaDuAn = new Button { Text = "Xóa DA", Location = new Point(208, 148), Size = new Size(90, 28) };

            btnThemDuAn.Click += btnThemDuAn_Click;
            btnSuaDuAn.Click += btnSuaDuAn_Click;
            btnXoaDuAn.Click += btnXoaDuAn_Click;

            dgvDuAn = new DataGridView
            {
                Location = new Point(12, 185),
                Size = new Size(385, 282),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
            dgvDuAn.SelectionChanged += dgvDuAn_SelectionChanged;

            grpDuAn.Controls.AddRange(new Control[] {
                lblTenDA, txtTenDuAn, lblBD, dtpNgayBatDau, lblKT, dtpNgayKetThuc,
                lblTTDA, cboTrangThaiDuAn, btnThemDuAn, btnSuaDuAn, btnXoaDuAn, dgvDuAn
            });

            // --- RIGHT TOP: TẠO TASK & LỌC ---
            grpTask = new GroupBox { Text = "2. Tạo Công việc (Task) & Điều phối", Location = new Point(432, 10), Size = new Size(880, 130) };

            var lblTenTask = new Label { Text = "Tên Task:", Location = new Point(12, 26), AutoSize = true };
            txtTenTask = new TextBox { Location = new Point(80, 23), Width = 240 };

            var lblMoTa = new Label { Text = "Mô tả:", Location = new Point(12, 56), AutoSize = true };
            txtMoTaTask = new TextBox { Location = new Point(80, 53), Width = 240 };

            var lblHan = new Label { Text = "Hạn nộp:", Location = new Point(335, 26), AutoSize = true };
            dtpHanNop = new DateTimePicker { Location = new Point(400, 23), Width = 120, Format = DateTimePickerFormat.Short };

            var lblUuTien = new Label { Text = "Ưu tiên:", Location = new Point(335, 56), AutoSize = true };
            cboDoUuTien = new ComboBox { Location = new Point(400, 53), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };

            btnThemTask = new Button
            {
                Text = "+ Tạo Task cho Dự án đang chọn",
                Location = new Point(535, 22),
                Size = new Size(210, 58),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThemTask.Click += btnThemTask_Click;

            // Filter & Status Update bar
            var lblLocDA = new Label { Text = "Lọc Dự án:", Location = new Point(12, 92), AutoSize = true };
            cboDuAnFilter = new ComboBox { Location = new Point(80, 89), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cboDuAnFilter.SelectedIndexChanged += cboDuAnFilter_SelectedIndexChanged;

            var lblLocTT = new Label { Text = "Trạng thái:", Location = new Point(290, 92), AutoSize = true };
            cboTrangThaiFilter = new ComboBox { Location = new Point(358, 89), Width = 115, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTrangThaiFilter.SelectedIndexChanged += cboTrangThaiFilter_SelectedIndexChanged;

            cboTrangThaiMoi = new ComboBox { Location = new Point(490, 89), Width = 115, DropDownStyle = ComboBoxStyle.DropDownList };
            btnCapNhatTrangThai = new Button { Text = "Đổi trạng thái", Location = new Point(612, 88), Size = new Size(105, 26) };
            btnCapNhatTrangThai.Click += btnCapNhatTrangThai_Click;

            btnPhanCong = new Button { Text = "Phân công NV", Location = new Point(725, 88), Size = new Size(115, 26) };
            btnPhanCong.Click += btnPhanCong_Click;

            grpTask.Controls.AddRange(new Control[] {
                lblTenTask, txtTenTask, lblMoTa, txtMoTaTask, lblHan, dtpHanNop,
                lblUuTien, cboDoUuTien, btnThemTask, lblLocDA, cboDuAnFilter,
                lblLocTT, cboTrangThaiFilter, cboTrangThaiMoi, btnCapNhatTrangThai, btnPhanCong
            });

            // --- RIGHT MIDDLE: TAB LIST VIEW / KANBAN BOARD ---
            tabViewMode = new TabControl { Location = new Point(432, 146), Size = new Size(880, 344) };
            tabListView = new TabPage("List View (Danh sách Task)");
            tabKanbanView = new TabPage("Kanban Board (Bảng tiến độ)");

            dgvTasks = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
            tabListView.Controls.Add(dgvTasks);

            // Kanban 4 columns
            var tblKanban = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 2 };
            for (int i = 0; i < 4; i++) tblKanban.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblKanban.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tblKanban.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tblKanban.Controls.Add(new Label { Text = "TO DO", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.LightGray, Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 0, 0);
            tblKanban.Controls.Add(new Label { Text = "IN PROGRESS", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.LightSkyBlue, Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 1, 0);
            tblKanban.Controls.Add(new Label { Text = "REVIEW", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.Khaki, Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 2, 0);
            tblKanban.Controls.Add(new Label { Text = "DONE", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.LightGreen, Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 3, 0);

            flpTodo = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.WhiteSmoke };
            flpInProgress = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.WhiteSmoke };
            flpReview = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.WhiteSmoke };
            flpDone = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, BackColor = Color.WhiteSmoke };

            tblKanban.Controls.Add(flpTodo, 0, 1);
            tblKanban.Controls.Add(flpInProgress, 1, 1);
            tblKanban.Controls.Add(flpReview, 2, 1);
            tblKanban.Controls.Add(flpDone, 3, 1);
            tabKanbanView.Controls.Add(tblKanban);

            tabViewMode.TabPages.Add(tabListView);
            tabViewMode.TabPages.Add(tabKanbanView);

            // --- BOTTOM: CẢNH BÁO TRỄ HẠN & THỐNG KÊ ---
            grpBottom = new GroupBox { Text = "3. Cảnh báo Trễ hạn (Overdue) & Thống kê % Hoàn thành theo Thành viên", Location = new Point(12, 496), Size = new Size(1300, 275) };

            lblCanhBaoTreHan = new Label
            {
                Text = "Cảnh báo trễ hạn:",
                Location = new Point(12, 22),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            dgvOverdue = new DataGridView
            {
                Location = new Point(12, 46),
                Size = new Size(630, 218),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            var lblStatsTitle = new Label
            {
                Text = "Thống kê tỷ lệ % hoàn thành của từng thành viên trong dự án đang chọn:",
                Location = new Point(658, 22),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            dgvStats = new DataGridView
            {
                Location = new Point(658, 46),
                Size = new Size(630, 218),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            grpBottom.Controls.AddRange(new Control[] { lblCanhBaoTreHan, dgvOverdue, lblStatsTitle, dgvStats });

            this.Controls.AddRange(new Control[] { grpDuAn, grpTask, tabViewMode, grpBottom });
            this.Load += frmMain_Load;
        }
    }
}
