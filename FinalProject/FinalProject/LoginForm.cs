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
using System.Configuration;
using System.Data.Sql;

namespace FinalProject
{
    public partial class LoginForm : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM [dbo].[Users] where UserName='"+txtUsername.Text+"' and Password='"+txtPassword.Text+"'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Import imp = new Import();
                imp.ShowDialog();
            }
            else
            {
                MessageBox.Show("UserName or Password is incorrect.","Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DialogResult exit = MessageBox.Show("Are you sure you want to exit ?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (exit == DialogResult.OK)
                Application.Exit();
        }
    }
}
