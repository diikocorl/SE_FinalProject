using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WinForms;

namespace FinalProject
{
    public partial class Import : Form
    {
        SqlConnection conn;

        public Import()
        {
            InitializeComponent();
        }

        private void Import_Load(object sender, EventArgs e)
        {
            
            string conStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString.ToString();
            conn = new SqlConnection(conStr);
            conn.Open();
            LoadData();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            String sql = "insert into [dbo].[Products] values (@Brand, @Size, @Price, @Quantity, @Item, @Day, @Status)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Brand", txtBrand.Text);
            cmd.Parameters.AddWithValue("Size", txtSize.Text);
            cmd.Parameters.AddWithValue("Price", txtPrice.Text);
            cmd.Parameters.AddWithValue("Quantity", txtQuantity.Text);
            cmd.Parameters.AddWithValue("Item", cbbItem.Text);
            cmd.Parameters.AddWithValue("Day", txtDay.Text);
            cmd.Parameters.AddWithValue("Status", "In Stored");

            cmd.ExecuteNonQuery();
            LoadData();
            
            String sql1 = "select * from [dbo].[Products] where Brand = '" + txtBrand.Text + "' and Size = '" + txtSize.Text + "' and Price = '" + txtPrice.Text + "' and Quantity = '" + txtQuantity.Text + "' and Item = '" + cbbItem.Text + "' and Day = '" + txtDay.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql1, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Product");

            this.reportImport.LocalReport.ReportEmbeddedResource = "FinalProject.ImportReceipt.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dtImportRec";
            rds.Value = ds.Tables["Product"];

            this.reportImport.LocalReport.DataSources.Add(rds);
            adapter.Dispose();

            this.reportImport.RefreshReport();
        }
        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = dataGridViewProducts.CurrentRow.Index;
            txtID.Text = dataGridViewProducts.Rows[i].Cells[0].Value.ToString();
            txtBrand.Text = dataGridViewProducts.Rows[i].Cells[1].Value.ToString();
            txtSize.Text = dataGridViewProducts.Rows[i].Cells[2].Value.ToString();
            txtPrice.Text = dataGridViewProducts.Rows[i].Cells[3].Value.ToString();
            txtQuantity.Text = dataGridViewProducts.Rows[i].Cells[4].Value.ToString();
            cbbItem.Text = dataGridViewProducts.Rows[i].Cells[5].Value.ToString();
            txtDay.Text = dataGridViewProducts.Rows[i].Cells[6].Value.ToString();
        }
        private void LoadData()
        {
            string getDt = "SELECT * FROM [dbo].[Products]";
            SqlCommand cmd = new SqlCommand(getDt,conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable(); 
            dt.Load(dr);
            dataGridViewProducts.DataSource = dt;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            String sql1 = "update [dbo].[Products] set Status = 'Transferred' where ID= '" + txtID.Text + "'" +
                "select * from [dbo].[Products] where ID = '" + txtID.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql1, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Distribution");

            this.reportImport.LocalReport.ReportEmbeddedResource = "FinalProject.DistributionReceipt.rdlc";
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dtDistributionRec";
            rds.Value = ds.Tables["Distribution"];

            this.reportImport.LocalReport.DataSources.Add(rds);
            adapter.Dispose();
            this.reportImport.RefreshReport();

            String delete = "delete from [dbo].[Products] where ID = '" + txtID.Text + "'";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery();
            LoadData();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtBrand.Text = "";
            txtSize.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            cbbItem.Text = "";
            txtDay.Text = "";
        }
    }
}
