using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QL_HD_NHAHANG
{
    public partial class FormXuatPDT : Form
    {
        chucnang cn = new chucnang();
        SqlConnection conn = new SqlConnection();
        DataTable tblTD;
        string ma_x;
        public FormXuatPDT()
        {
            InitializeComponent();
        }

        public FormXuatPDT(string ma)
        {
            ma_x = ma;
            InitializeComponent();
        }

        private void FormXuatPDT_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadDataGridViewTD();
            LoadInfoPhieuXuat();
        }

        private void LoadDataGridViewTD()
        {
            string sql;
            sql = "SELECT * FROM CHITIET_PDT WHERE PDT_STT = '"+ma_x+"'";
            tblTD = chucnang.GetDataToTable(sql, conn);
            dataGridView1.DataSource = tblTD;
            dataGridView1.Columns[0].HeaderText = "Mã Thực Đơn";
            dataGridView1.Columns[1].HeaderText = "Mã Phiếu Đặt Tiệc";
            dataGridView1.Columns[2].HeaderText = "Số Lượng";
            dataGridView1.Columns[3].HeaderText = "Thành Tiền Thực Đơn";
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 130;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadInfoPhieuXuat()
        {
            txtMa.Text = ma_x;
            string str;
            str = "SELECT b.KH_TEN FROM PHIEU_DAT_TIEC a, KHACHHANG b WHERE PDT_STT = '"+ma_x+"' and a.KH_MA=b.KH_MA";
            txtkh.Text = chucnang.GetFieldValues(str, conn);
            str = "SELECT b.NV_TEN FROM PHIEU_DAT_TIEC a, NHANVIEN b WHERE PDT_STT = '" + ma_x + "' and a.NV_MA=b.NV_MA";
            txtnv.Text = chucnang.GetFieldValues(str, conn);
            str = "SELECT PDT_NGAYDIENRA FROM PHIEU_DAT_TIEC WHERE PDT_STT = '" + ma_x + "'";
            txtngay.Text = chucnang.GetFieldValues(str, conn);
            str = "SELECT PDT_TIENCOC FROM PHIEU_DAT_TIEC WHERE PDT_STT = '" + ma_x + "'";
            txttiencoc.Text = chucnang.GetFieldValues(str, conn);
            str = "SELECT PDT_TONGTIEN FROM PHIEU_DAT_TIEC WHERE PDT_STT = '" + ma_x + "'";
            txttongtien.Text = chucnang.GetFieldValues(str, conn);
        }
    }
}
