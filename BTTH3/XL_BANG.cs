using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTTH3
{
    class XL_BANG : DataTable
    {
        #region
        public static String Chuoi_lien_ket;
        private SqlDataAdapter mBo_doc_ghi = new SqlDataAdapter();
        private SqlConnection mKet_noi;
        public string chuoi_csdl;
        private String mChuoi_SQL;
        private String mTen_bang;
        #endregion

        #region
        public String MChuoi_SQL
        {
            get { return mChuoi_SQL; }
            set { mChuoi_SQL = value; }
        }
        public String MTen_bang
        {
            get { return mTen_bang; }
            set { mTen_bang = value; }
        }
        public int So_dong
        {
            get { return this.DefaultView.Count; }
        }
        #endregion

        #region
        public XL_BANG() : base() { }
        public XL_BANG(String pTen_bang)
        {
            mTen_bang = pTen_bang;
            Doc_bang();
        }
        public XL_BANG(String pTen_bang, String pChuoi_SQL)
        {
            mTen_bang = pTen_bang;
            mChuoi_SQL = pChuoi_SQL;
            Doc_bang();
        }
        #endregion
        public void Doc_bang()
        {
            if (mChuoi_SQL == null)
                mChuoi_SQL = "SELECT * FROM " + mTen_bang;
            if (mKet_noi == null)
                mKet_noi = new SqlConnection(Chuoi_lien_ket);
            try
            {
                mBo_doc_ghi = new SqlDataAdapter(mChuoi_SQL, mKet_noi);
                mBo_doc_ghi.FillSchema(this, SchemaType.Mapped);
                mBo_doc_ghi.Fill(this);
                mBo_doc_ghi.RowUpdated += new SqlRowUpdatedEventHandler(mBo_doc_ghi_RowUpdated);
                SqlCommandBuilder Bo_phat_sinh = new SqlCommandBuilder(mBo_doc_ghi);
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public Boolean Ghi()
        {
            Boolean ket_qua = true;
            try
            {
                mBo_doc_ghi.Update(this);
                this.AcceptChanges();
            }
            catch ( SqlException ex)
            {
                this.RejectChanges();
                ket_qua = false;
            }
            return ket_qua;
        }
        public void Loc_du_lieu(String pDieu_kien)
        {
            try
            {
                this.DefaultView.RowFilter = pDieu_kien;
            }
            catch ( Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public int Thuc_hien_lenh(String Lenh)
        {
            try
            {
                SqlCommand Cau_lenh = new SqlCommand(Lenh, mKet_noi);
                mKet_noi.Open();
                int ket_qua = Cau_lenh.ExecuteNonQuery();
                mKet_noi.Close();
                return ket_qua;
            }
            catch
            {
                return -1;
            }
        }
        public Object Thuc_hien_lenh_tinh_toan(String Lenh)
        {
            try
            {
                SqlCommand Cau_lenh = new SqlCommand(Lenh, mKet_noi);
                mKet_noi.Open();
                Object ket_qua = Cau_lenh.ExecuteScalar();
                mKet_noi.Close();
                return ket_qua;
            }
            catch
            {
                return null;
            }
        }

        private void mBo_doc_ghi_RowUpdated(Object sender, SqlRowUpdatedEventArgs e)
        {
            if(this.PrimaryKey[0].AutoIncrement)
            {
                if((e.Status == UpdateStatus.Continue) && (e.StatementType == StatementType.Insert))
                {
                    SqlCommand cmd = new SqlCommand("SELECT @@IDENTITY ", mKet_noi);
                    e.Row.AcceptChanges();
                }
            }
        }

    }
}
