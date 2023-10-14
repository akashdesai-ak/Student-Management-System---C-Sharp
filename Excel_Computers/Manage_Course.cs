using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class Manage_Course : Form
    {

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");
        OleDbCommand cmd = new OleDbCommand();

        public Manage_Course()
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

            dataGridView_ManageCourse.DataSource = ds.Tables[0];


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                if (txt_CourseID.Text == "" || txt_CourseName.Text == "" || txt_CourseDuration.Text == "" || txt_Fees.Text == "")
                {
                    MessageBox.Show("Please Search Record First", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    conn.Open();

                    cmd.CommandText = "UPDATE Courses SET CourseName='" + txt_CourseName.Text + "',Duration='" + txt_CourseDuration.Text + "',Fees=" + txt_Fees.Text + " WHERE CourseID=" + txt_CourseID.Text + " ";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record Successfully Update..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    ShowData();
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

        private void btn_SearchStudent_Click(object sender, EventArgs e)
        {
            conn.Open();

            OleDbCommand command = new OleDbCommand();
            command.CommandText = "SELECT * FROM Courses WHERE CourseID =" + txt_SearchCourse.Text + " ";
            command.Connection = conn;

            try
            {
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    txt_CourseID.Text = Convert.ToString(reader[0]);
                    txt_CourseName.Text = Convert.ToString(reader[1]);
                    txt_CourseDuration.Text = Convert.ToString(reader[2]);
                    txt_Fees.Text = Convert.ToString(reader[3]);

                }
                else
                {
                    MessageBox.Show("Course Not Found..", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            conn.Close();
        }

        private void Manage_Course_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'courseDataSet.Courses' table. You can move, or remove it, as needed.
            this.coursesTableAdapter.Fill(this.courseDataSet.Courses);
           





            conn.Open();

            ShowData();

            conn.Close();


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                if (txt_CourseID.Text == "" || txt_CourseName.Text == "" || txt_CourseDuration.Text == "" || txt_Fees.Text == "")
                {
                    MessageBox.Show("Please Search Record First", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    DialogResult res = MessageBox.Show("Are you sure you want to Delete ?", "Excel Computers", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.No)
                    {
                        txt_CourseID.Focus();
                    }
                    else if (res == DialogResult.Yes)
                    {
                        conn.Open();

                        cmd.CommandText = "DELETE * FROM Courses WHERE CourseID=" + txt_CourseID.Text + " ";
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record Successfully Delete..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    ClearText();
                    ShowData();
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

        private void dataGridView_ManageCourse_Click(object sender, EventArgs e)
        {
            txt_CourseID.Text = dataGridView_ManageCourse.CurrentRow.Cells[0].Value.ToString();
            txt_CourseName.Text = dataGridView_ManageCourse.CurrentRow.Cells[1].Value.ToString();
            txt_CourseDuration.Text = dataGridView_ManageCourse.CurrentRow.Cells[2].Value.ToString();
            txt_Fees.Text = dataGridView_ManageCourse.CurrentRow.Cells[3].Value.ToString();
        }
    }
}
