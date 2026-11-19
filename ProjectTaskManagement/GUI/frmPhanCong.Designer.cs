using System.Drawing;
using System.Windows.Forms;

namespace ProjectTaskManagement.GUI
{
    partial class frmPhanCong
    {
        private System.ComponentModel.IContainer components = null;
        private CheckedListBox clbThanhVien;
        private Button btnLuu;
        private Button btnHuy;
        private Label lblHeader;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new Label();
            this.clbThanhVien = new CheckedListBox();
            this.btnLuu = new Button();
            this.btnHuy = new Button();
            this.SuspendLayout();

            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblHeader.Location = new Point(16, 15);
            this.lblHeader.Text = "Chọn thành viên thực hiện công việc:";

            this.clbThanhVien.CheckOnClick = true;
            this.clbThanhVien.Font = new Font("Segoe UI", 10F);
            this.clbThanhVien.Location = new Point(18, 45);
            this.clbThanhVien.Size = new Size(360, 210);

            this.btnLuu.BackColor = Color.FromArgb(0, 120, 215);
            this.btnLuu.ForeColor = Color.White;
            this.btnLuu.FlatStyle = FlatStyle.Flat;
            this.btnLuu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            this.btnLuu.Location = new Point(192, 270);
            this.btnLuu.Size = new Size(90, 32);
            this.btnLuu.Text = "Lưu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            this.btnHuy.Font = new Font("Segoe UI", 9.5F);
            this.btnHuy.Location = new Point(288, 270);
            this.btnHuy.Size = new Size(90, 32);
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += (s, e) => this.Close();

            this.ClientSize = new Size(396, 320);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.clbThanhVien);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnHuy);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmPhanCong_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
