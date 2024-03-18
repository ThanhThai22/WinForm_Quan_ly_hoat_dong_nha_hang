using System;
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
    public partial class FormTinhLuongNV : Form
    {
        chucnang cn = new chucnang();
        SqlConnection conn = new SqlConnection();
        DataTable tblNV;
        public FormTinhLuongNV()
        {
            InitializeComponent();
        }

        private void FormTinhLuongNV_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            LoadDataGridView();
            //LoadDataGridViewNV();
            //txttongtien.Text = chucnang.GetFieldValues("select sum(b.TG_TIENLUONG) from NHANVIEN a, THAMGIA b where a.NV_MA=b.NV_MA and a.NV_THANGBD = '"+txtthang.Text+"'", conn);
        }

        private void In_ds_Click(object sender, EventArgs e)
        {
            string datafind = txtthang.Text;

            LoadDataGridViewfind(datafind);

            txttongtien.Text = chucnang.GetFieldValues("select sum(b.TG_TIENLUONG) from NHANVIEN a, THAMGIA b where a.NV_MA=b.NV_MA and a.NV_THANGBD = '" + txtthang.Text + "'", conn);
        }

        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT a.NV_TEN, b.BP_TEN, b.BP_HESOLUONG, c.TG_TIENLUONG, a.NV_THANGBD, c.PDT_STT FROM NHANVIEN a, BOPHAN b, THAMGIA c WHERE a.BP_MA=b.BP_MA and a.NV_MA=c.NV_MA";
            tblNV = chucnang.GetDataToTable(sql, conn);
            dataGridView1.DataSource = tblNV;
            dataGridView1.Columns[0].HeaderText = "TÊN NHÂN VIÊN";
            dataGridView1.Columns[1].HeaderText = "BỘ PHẬN";
            dataGridView1.Columns[2].HeaderText = "HỆ SỐ LƯƠNG";
            dataGridView1.Columns[3].HeaderText = "TỔNG LƯƠNG";
            dataGridView1.Columns[4].HeaderText = "THÁNG";
            dataGridView1.Columns[5].HeaderText = "THAM GIA TIỆC";
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 90;
            dataGridView1.Columns[5].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridViewfind(string thang)
        {
            string sql;
            sql = "SELECT a.NV_TEN, b.BP_TEN, b.BP_HESOLUONG, c.TG_TIENLUONG, a.NV_THANGBD, c.PDT_STT FROM NHANVIEN a, BOPHAN b, THAMGIA c WHERE a.BP_MA=b.BP_MA and a.NV_MA=c.NV_MA and a.NV_THANGBD = '"+ thang + "'";
            tblNV = chucnang.GetDataToTable(sql, conn);
            dataGridView1.DataSource = tblNV;
            dataGridView1.Columns[0].HeaderText = "TÊN NHÂN VIÊN";
            dataGridView1.Columns[1].HeaderText = "BỘ PHẬN";
            dataGridView1.Columns[2].HeaderText = "HỆ SỐ LƯƠNG";
            dataGridView1.Columns[3].HeaderText = "TỔNG LƯƠNG";
            dataGridView1.Columns[4].HeaderText = "THÁNG";
            dataGridView1.Columns[5].HeaderText = "THAM GIA TIỆC";
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 90;
            dataGridView1.Columns[5].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridViewNV()
        {
            string sql = "SELECT a.NV_TEN, c.BP_TEN, b.SUM(TG_TIENLUONG) FROM NHANVIEN a, THAMGIA b, BOPHAN c WHERE NV_MA = '"+txtthang.Text+"' and a.NV_MA=b.NV_MA and a.BP_MA=c.BP_MA";
            tblNV = chucnang.GetDataToTable(sql, conn);
            dataGridView1.DataSource = tblNV;
            dataGridView1.Columns[0].HeaderText = "TÊN NHÂN VIÊN";
            dataGridView1.Columns[1].HeaderText = "BỘ PHẬN";
            dataGridView1.Columns[2].HeaderText = "TỔNG CHI TRẢ";
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
    }
}
