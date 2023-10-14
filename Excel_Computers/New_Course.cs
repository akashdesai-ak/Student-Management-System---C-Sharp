using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class New_Course : Form
    {

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");
        OleDbCommand cmd;
        public New_Course()
        {
            InitializeComponent();
        }

        private void ClearText()
        {
            txt_CourseDuration.Text = string.Empty;
            txt_CourseName.Text = string.Empty;
            txt_Fees.Text = string.Empty;

            txt_CourseID.Focus();
        }

        private void ShowData()
        {


            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Courses", conn);
            CourseDataSet ds = new CourseDataSet();
            adapter.Fill(ds, "Courses");

            dataGridView_AddCourse.DataSource = ds.Tables[0];


        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_CourseID.Text == "" || txt_CourseName.Text == "" || txt_CourseDuration.Text == "" || txt_Fees.Text == "")
                {
                    MessageBox.Show("Please Enter All Fields", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_CourseID.Focus();
                }
                else
                {
                    OleDbCommand cmd = new OleDbCommand("INSERT INTO Courses VALUES(" + txt_CourseID.Text + ",'" + txt_CourseName.Text + "','" + txt_CourseDuration.Text + "'," + txt_Fees.Text + ")");
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record Successfully Saved..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    ShowData();
                    IDAutoIncreament();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                conn.Close();
            }
        }



        private void IDAutoIncreament()
        {

            // Course ID Auto Increament
            int a;
            cmd = new OleDbCommand("SELECT max(CourseID) FROM Courses", conn);

            a = Convert.ToInt32(cmd.ExecuteScalar());

            a = a + 1;
            txt_CourseID.Text = a.ToString();


        }

        private void New_Course_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'courseDataSet.Courses' table. You can move, or remove it, as needed.
            this.coursesTableAdapter.Fill(this.courseDataSet.Courses);



            conn.Open();
            ShowData();

            IDAutoIncreament();
            conn.Close();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearText();

        }
    }
}
