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
    public partial class FormDatTiec : Form
    {
        chucnang cn = new chucnang();
        SqlConnection conn = new SqlConnection();
        DataTable tblPDT, tblTD;
        public FormDatTiec()
        {
            InitializeComponent();
        }

        private void FormDatTiec_Load(object sender, EventArgs e)
        {
            cn.ketnoi(conn);
            string sql_kh = "SELECT * FROM KHACHHANG";
            string sql_nv = "SELECT * FROM NHANVIEN";
            string sql_td = "SELECT * FROM THUCDON";
            cn.ShowInComboBox(comboBoxKH, sql_kh, conn, "KH_TEN", "KH_MA");
            cn.ShowInComboBox(comboBoxNV, sql_nv, conn, "NV_TEN", "NV_MA");
            cn.ShowInComboBox(comboBoxTD, sql_td, conn, "TD_TEN", "TD_MA");
            if(txtMaphieu.Text != "")
            {
                LoadInfoPhieu();
            }
            LoadDataGridView();
            LoadDataGridViewTD();
        }

        private void LoadInfoPhieu()
        {
            string str;
            str = "SELECT PDT_TONGTIEN FROM PHIEU_DAT_TIEC WHERE PDT_STT = N'" + txtMaphieu.Text + "'";
            txttongtienPD.Text = chucnang.GetFieldValues(str, conn);

            str = "SELECT PDT_TIENCOC FROM PHIEU_DAT_TIEC WHERE PDT_STT = N'"+txtMaphieu.Text+"'";
            txttiencoc.Text = chucnang.GetFieldValues(str, conn);
        }

        private void LoadDataGridView()
        {
            string sql;
            //sql = "SELECT b.MaSach, b.TenSach, a.Soluong, a.DonGia, a.ThanhTien FROM CHITIET_HD a, Sach b WHERE MaHD = N'" + txtmahd.Text + "' and a.MaSach=b.MaSach";
            sql = "SELECT PDT_STT, PDT_NGAYDIENRA, NV_MA, KH_MA, PDT_TONGTIEN FROM PHIEU_DAT_TIEC";
            tblPDT = chucnang.GetDataToTable(sql, conn);
            dataGridView1.DataSource = tblPDT;
            dataGridView1.Columns[0].HeaderText = "Mã Phiếu Đặt";
            dataGridView1.Columns[1].HeaderText = "Ngày Diễn Ra Tiệc";
            dataGridView1.Columns[2].HeaderText = "Nhân Viên";
            dataGridView1.Columns[3].HeaderText = "Khách Hàng";
            dataGridView1.Columns[4].HeaderText = "Thành Tiền";
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 130;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void LoadDataGridViewTD()
        {
            string sql;
            sql = "SELECT * FROM CHITIET_PDT";
            tblTD = chucnang.GetDataToTable(sql, conn);
            dataGridViewTD.DataSource = tblTD;
            dataGridViewTD.Columns[0].HeaderText = "Mã Thực Đơn";
            dataGridViewTD.Columns[1].HeaderText = "Mã Phiếu Đặt Tiệc";
            dataGridViewTD.Columns[2].HeaderText = "Số Lượng";
            dataGridViewTD.Columns[3].HeaderText = "Thành Tiền Thực Đơn";
            dataGridViewTD.Columns[0].Width = 80;
            dataGridViewTD.Columns[1].Width = 130;
            dataGridViewTD.Columns[2].Width = 80;
            dataGridViewTD.Columns[3].Width = 90;
            dataGridViewTD.AllowUserToAddRows = false;
            dataGridViewTD.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaphieu.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtngaydienra.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            comboBoxNV.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            comboBoxKH.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            LoadInfoPhieu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_add_td_Click(object sender, EventArgs e)
        {
            string sql_add_td = "insert into CHITIET_PDT values('" + comboBoxTD.SelectedValue + "', '" + txtMaphieu.Text + "', '" + txtsl.Text + "', '" + txttongtientd.Text + "')";
            try
            {
                cn.capnhat(sql_add_td, conn);
                MessageBox.Show("Thêm vào phiếu "+ txtMaphieu.Text +" Thành công!");
                double tongPDT, tiencoc;
                tongPDT = Convert.ToDouble(chucnang.GetFieldValues("SELECT SUM(CTPDT_THANHTIEN) FROM CHITIET_PDT WHERE PDT_STT = '" + txtMaphieu.Text + "'", conn));
                tiencoc = (tongPDT * 10) / 100;
                string sql_update_tong_coc = "UPDATE PHIEU_DAT_TIEC SET PDT_TIENCOC = '" + tiencoc + "', PDT_TONGTIEN = '" + tongPDT + "' WHERE PDT_STT = '" + txtMaphieu.Text + "'";
                cn.capnhat(sql_update_tong_coc, conn);
                MessageBox.Show("Cập nhật lại tiền cọc và giá tiền thành công");
                LoadDataGridView();
            }
            catch (Exception er)
            {
                MessageBox.Show("Loi:" + er);
            }
            LoadDataGridViewTD();
            LoadInfoPhieu();
            comboBoxTD.Text = "";
            txtsl.Text = "0";
            txttongtientd.Text = "0";
            
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            comboBoxTD.Text = "";
            txtsl.Text = "0";
            txttongtientd.Text = "0";
        }

        private void ResetValue()
        {
            txtMaphieu.Text = "";
            //txtsdt.Text = "";
            txtsl.Text = "0";
            txtngaydienra.Text = DateTime.Today.ToString();
            txttiencoc.Text = "0";
            txttongtientd.Text = "0";
            txttongtienPD.Text = "0";
            comboBoxKH.Text = "";
            comboBoxNV.Text = "";
            comboBoxTD.Text = "";

        }

        private void btn_add_pdt_Click(object sender, EventArgs e)
        {
            ResetValue();
            txtMaphieu.Text =  chucnang.CreateKey("PDT");
            LoadDataGridView();
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            string sql_test = "select TD_DONGIA from THUCDON where TD_MA = '" + comboBoxTD.SelectedValue.ToString() + "'";
            double dongia = Convert.ToDouble(chucnang.GetFieldValues(sql_test, conn));
            txttongtientd.Text = Convert.ToString(dongia * Convert.ToDouble(txtsl.Text));
        }

        private void btn_datlai_tt_Click(object sender, EventArgs e)
        {
            txtngaydienra.Text = DateTime.Today.ToString();
            comboBoxKH.Text = "";
            comboBoxNV.Text = "";
        }

        private void btn_xn_tt_Click(object sender, EventArgs e)
        {
            string sql_add_tt = "insert into PHIEU_DAT_TIEC values('"+txtMaphieu.Text+"', '"+txtngaydienra.Text+"', '0', '0', '"+comboBoxKH.SelectedValue+"', '"+comboBoxNV.SelectedValue+"')";
            cn.capnhat(sql_add_tt, conn);
            MessageBox.Show("Đã thêm phiếu đặt tạm thời!");
            groupBox1.Enabled = false;
            LoadDataGridView();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            double tongPDT, tiencoc;
            tongPDT = Convert.ToDouble(chucnang.GetFieldValues("SELECT SUM(CTPDT_THANHTIEN) FROM CHITIET_PDT WHERE PDT_STT = '"+txtMaphieu.Text+"'", conn));
            tiencoc = (tongPDT * 10) / 100;
            string sql_update_tong_coc = "UPDATE PHIEU_DAT_TIEC SET PDT_TIENCOC = '"+tiencoc+"', PDT_TONGTIEN = '"+tongPDT+"' WHERE PDT_STT = '"+txtMaphieu.Text+"'";
            cn.capnhat(sql_update_tong_coc, conn);
            MessageBox.Show("Lưu thông tin phiếu đặt tiệc " + txtMaphieu.Text + " thành công!");
            //THEM NHANVIEN VAO BANG THAMGIA TIEC
            double hsl = Convert.ToDouble(chucnang.GetFieldValues("select a.BP_HESOLUONG from BOPHAN a, NHANVIEN b where a.BP_MA=b.BP_MA and b.NV_MA = '"+comboBoxNV.SelectedValue+"'", conn));
            double tongtienluong = hsl*100000;
            string sql_thamgia = "INSERT INTO THAMGIA VALUES('"+comboBoxNV.SelectedValue+"', '"+txtMaphieu.Text+"', '"+ tongtienluong + "')";
            cn.capnhat(sql_thamgia, conn);
            MessageBox.Show("Cập nhật bảng tham gia thành công!");
            LoadDataGridView();
            LoadDataGridViewTD();
            txttongtienPD.Text = Convert.ToString(tongPDT);
            txttiencoc.Text = Convert.ToString(tiencoc);
        }

        private void btn_del_td_Click(object sender, EventArgs e)
        {
            string sql_del = "DELETE FROM CHITIET_PDT WHERE TD_MA = '"+ comboBoxTD.SelectedValue+"'";
            cn.capnhat(sql_del, conn);
            MessageBox.Show("Xóa thực đơn "+comboBoxTD.SelectedValue.ToString()+" thành công!");
            LoadDataGridViewTD();
        }

        private void btn_tao_phieu_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "insert into PHIEU_DAT_TIEC values('" + txtMaphieu.Text + "', '" + txtngaydienra.Text + "', '0', '0', '" + comboBoxKH.SelectedValue + "', '" + comboBoxNV.SelectedValue + "')";
            cn.capnhat(sql, conn);
            MessageBox.Show("Đã thêm phiếu đặt tiệc với Mã phiếu "+ txtMaphieu.Text);
            LoadDataGridView();
            txtngaydienra.Text = "";
            comboBoxKH.Text = "";
            comboBoxNV.Text = "";
        }

        private void dataGridViewTD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBoxTD.Text = dataGridViewTD.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtsl.Text = dataGridViewTD.Rows[e.RowIndex].Cells[2].Value.ToString();
            txttongtientd.Text = dataGridViewTD.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void btn_del_pdt_Click(object sender, EventArgs e)
        {
            string sql_del = "DELETE FROM PHIEU_DAT_TIEC WHERE PDT_STT = '"+txtMaphieu.Text+"'";
            string sql_del_td = "DELETE FROM CHITIET_PDT WHERE PDT_STT = '" + txtMaphieu.Text + "'";
            cn.capnhat(sql_del_td, conn);
            cn.capnhat(sql_del, conn);
            MessageBox.Show("Xóa phiếu " + txtMaphieu.Text + " thành công tại phiếu đặt và chi tiết phiếu đặt!");
            LoadDataGridView();
            LoadDataGridViewTD();
        }

        private void btn_eedit_pdt_Click(object sender, EventArgs e)
        {
            string sql_update_pdt, sql_update_chitietpdt;
            sql_update_pdt = "UPDATE PHIEU_DAT_TIEC SET PDT_NGAYDIENRA = '"+txtngaydienra.Text+"', KH_MA = '"+comboBoxKH.SelectedValue+"', NV_MA = '"+comboBoxNV.SelectedValue+"' WHERE PDT_STT = '"+txtMaphieu.Text+"'";
            sql_update_chitietpdt = "UPDATE CHITIET_PDT SET CTPDT_SOLUONG = '"+txtsl.Text+"', CTPDT_THANHTIEN = '"+txttongtientd.Text+"' WHERE TD_MA = '"+comboBoxTD.SelectedValue+"'";
            cn.capnhat(sql_update_pdt, conn);
            MessageBox.Show("Cập nhật thành công phiếu đặt tiệc " + txtMaphieu.Text);
            cn.capnhat(sql_update_chitietpdt, conn);
            MessageBox.Show("Cập nhật thành công chi tiết phiếu đặt tiệc " + txtMaphieu.Text);
            double tongPDT, tiencoc;
            tongPDT = Convert.ToDouble(chucnang.GetFieldValues("SELECT SUM(CTPDT_THANHTIEN) FROM CHITIET_PDT WHERE PDT_STT = '" + txtMaphieu.Text + "'", conn));
            tiencoc = (tongPDT * 10) / 100;
            string sql_update_tong_coc = "UPDATE PHIEU_DAT_TIEC SET PDT_TIENCOC = '" + tiencoc + "', PDT_TONGTIEN = '" + tongPDT + "' WHERE PDT_STT = '" + txtMaphieu.Text + "'";
            cn.capnhat(sql_update_tong_coc, conn);
            MessageBox.Show("Cập nhật lại giá tiền cọc và tổng tiền của phiếu: " + txtMaphieu.Text);
            LoadDataGridView();
            LoadDataGridViewTD();
            LoadInfoPhieu();
        }

        private void btn_xuat_Click(object sender, EventArgs e)
        {
            FormXuatPDT fx = new FormXuatPDT(txtMaphieu.Text);
            fx.ShowDialog();
        }

        private void btn_reset_phieu_Click(object sender, EventArgs e)
        {
            txtngaydienra.Text = "";
            comboBoxKH.Text = "";
            comboBoxNV.Text = "";
        }
    }
}
