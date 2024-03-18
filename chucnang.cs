using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QL_HD_NHAHANG
{
    class chucnang
    {
        public static SqlConnection conn = new SqlConnection();
        public void ketnoi(SqlConnection conn)
        {
            //tao chuoi ket noi
            //string ChuoiKetNoi = "SERVER = P219M04\\SQLEXPRESS; database = QL_BanHang ; Integrated Security = true";
            //string ChuoiKetNoi = "SERVER = DESKTOP-3SEBU1D\\SQLEXPRESS; database = QLSach ; Integrated Security = true";
            string ChuoiKetNoi = "SERVER = LAPTOP-64EM8U3V; database = QL_HDNHAHANG ; Integrated Security = true";
            conn.ConnectionString = ChuoiKetNoi;

            //mo ket noi
            conn.Open();
        }



        //Hien thi du lieu tren GridView
        public void ShowDataGV(DataGridView dgv, string sql, SqlConnection conn)
        {
            //khai bao class sql adapter su dung ket noi voi cau lenh string sql
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            //khai bao class dataset de loc du lieu
            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "new data");

            dgv.DataSource = dataset;
            dgv.DataMember = "new data";
        }

        //Hien thi du lieu tren comboBox loaihang
        public void ShowInComboBox(ComboBox cb, string sql, SqlConnection conn, string hienthi, string giatri)
        {
            //tao ket noi tren command
            SqlCommand comd = new SqlCommand(sql, conn);
            //tao dataReader va gan datareader = combobox
            SqlDataReader datareader = comd.ExecuteReader();
            //tao class datatable
            DataTable datatable = new DataTable();
            //load du lieu reader len table
            datatable.Load(datareader);

            //load du lieu len comboBox
            cb.DataSource = datatable;
            cb.DisplayMember = hienthi;
            cb.ValueMember = giatri;
        }

        //cap nhat du lieu
        public void capnhat(string sql, SqlConnection conn)
        {
            
            //MessageBox.Show(sql);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public static string CreateKey(string tiento)
        {
            string key = tiento;
            /*string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;*/
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }

        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

        public static DataTable GetDataToTable(string sql, SqlConnection conn)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, conn); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Tạo đối tượng thuộc lớp SqlCommand
            dap.SelectCommand = new SqlCommand(sql, conn);/*
            dap.SelectCommand.Connection = chucnang.conn; //Kết nối cơ sở dữ liệu
            dap.SelectCommand.CommandText = sql; //Lệnh SQL*/
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table);
            return table;
        }

        public static string GetFieldValues(string sql, SqlConnection conn)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }

        /*//Hàm thực hiện câu lệnh SQL
        public static void RunSQL(string sql)
        {
            SqlCommand cmd; //Đối tượng thuộc lớp SqlCommand
            cmd = new SqlCommand();
            cmd.Connection = conn; //Gán kết nối
            cmd.CommandText = sql; //Gán lệnh SQL
            try
            {
                cmd.ExecuteNonQuery(); //Thực hiện câu lệnh SQL
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();//Giải phóng bộ nhớ
            cmd = null;
        }*/

        //Hàm kiểm tra khoá trùng
        public static bool CheckKey(string sql)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, conn);
            DataTable table = new DataTable();
            dap.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }

        public static string ConvertDateTime(string date)
        {
            string[] elements = date.Split('/');
            string dt = string.Format("{0}/{1}/{2}", elements[0], elements[1], elements[2]);
            return dt;
        }
    }
}
