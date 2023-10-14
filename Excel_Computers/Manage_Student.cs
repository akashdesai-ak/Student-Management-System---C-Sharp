using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class Manage_Student : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");

        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader reader;


        public Manage_Student()
        {
            InitializeComponent();
        }
        private void ClearText()
        {
            txt_FirstName.Text = string.Empty;
            txt_LastName.Text = string.Empty;
            txt_MobileNo.Text = string.Empty;
            txt_Fees.Text = string.Empty;
            txt_Email.Text = string.Empty;
            dt_RegDate.Text = string.Empty;
            radioButton_Female.Checked = false;
            radioButton_Male.Checked = false;
            comboBox_Course.Text = "Select Course";
            comboBox_BatchTime.Text = "Select Batch Time";


            txt_SearchStudent.Focus();
        }

        private void ShowData()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Students", conn);
            StudentDataSet ds = new StudentDataSet();
            da.Fill(ds, "Students");
            dataGridView_ManageStudent.RowTemplate.Height = 75;
            dataGridView_ManageStudent.DataSource = ds.Tables[0];
            DataGridViewImageColumn pic1 = new DataGridViewImageColumn();
            pic1 = (DataGridViewImageColumn)dataGridView_ManageStudent.Columns[10];
            pic1.ImageLayout = DataGridViewImageCellLayout.Zoom;

        }
        private void Manage_Student_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'studentDataSet.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.studentDataSet.Students);
            // TODO: This line of code loads data into the 'manageStudentDataSet.Students' table. You can move, or remove it, as needed.
            //this.studentsTableAdapter.Fill(this.manageStudentDataSet.Students);


            ViewCourse();

            conn.Open();

            ShowData();

            conn.Close();


        }

        private void btn_SearchStudent_Click(object sender, EventArgs e)
        {
            conn.Open();

            cmd.CommandText = "SELECT * FROM Students WHERE RegID = " + txt_SearchStudent.Text + "";
            cmd.Connection = conn;

            try
            {
                string Gender;
                if (radioButton_Male.Checked == true)
                {
                    Gender = "Male";
                }
                else
                {
                    Gender = "Female";
                }

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txt_RegID.Text = Convert.ToString(reader[0]);
                    dt_RegDate.Text = Convert.ToString(reader[1]);
                    txt_FirstName.Text = Convert.ToString(reader[2]);
                    txt_LastName.Text = Convert.ToString(reader[3]);
                    txt_MobileNo.Text = Convert.ToString(reader[4]);
                    Gender = Convert.ToString(reader[5]);
                    txt_Email.Text = Convert.ToString(reader[6]);
                    comboBox_Course.Text = Convert.ToString(reader[7]);
                    txt_Fees.Text = Convert.ToString(reader[8]);
                    comboBox_BatchTime.Text = Convert.ToString(reader[9]);

                    Byte[] img = (Byte[])dataGridView_ManageStudent.CurrentRow.Cells[10].Value;
                    MemoryStream ms = new MemoryStream(img);

                    pictureBox_Photo.Image = Image.FromStream(ms);




                }
                else
                {
                    MessageBox.Show("Record Not Found..", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void ViewCourse()
        {
            conn.Open();

            cmd.CommandText = "SELECT CourseName FROM Courses";
            cmd.Connection = conn;

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox_Course.Items.Add(reader[0]);
            }

            conn.Close();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_RegID.Text == "" || dt_RegDate.Text == "" || txt_FirstName.Text == "" ||
                    txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || txt_Fees.Text == "" || comboBox_BatchTime.Text == "")
                {
                    MessageBox.Show("Please Select Record First ", "Excel Commputers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {


                    string Gender;
                    if (radioButton_Male.Checked == true)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }

                    cmd.CommandText = "UPDATE Students SET RegDate='" + dt_RegDate.Text + "',Fname='" + txt_FirstName.Text + "',Lname='" + txt_LastName.Text + "',MobileNo=" + txt_MobileNo.Text + ",Gender='" + Gender + "',Email='" + txt_Email.Text + "',Course='" + comboBox_Course.Text + "',Fees=" + txt_Fees.Text + ",Batch='" + comboBox_BatchTime.Text + "',StudentPhoto=@studentPhoto WHERE RegID=" + txt_RegID.Text + " ";
                    cmd.Connection = conn;



                    MemoryStream memstr = new MemoryStream();
                    pictureBox_Photo.Image.Save(memstr, pictureBox_Photo.Image.RawFormat);
                    cmd.Parameters.AddWithValue("studentPhoto", memstr.ToArray());


                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Successfully Update..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btn_ChangePhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpeg, jpg|*.jpeg ,*.jpg|bmp|*.bmp|all files|*.*";

            openFileDialog.Title = "SELECT File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox_Photo.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void dataGridView_ManageStudent_Click(object sender, EventArgs e)
        {
            Byte[] img = (Byte[])dataGridView_ManageStudent.CurrentRow.Cells[10].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox_Photo.Image = Image.FromStream(ms);

            string Gender;
            if (radioButton_Male.Checked == true)
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }

            txt_RegID.Text = dataGridView_ManageStudent.CurrentRow.Cells[0].Value.ToString();
            dt_RegDate.Text = dataGridView_ManageStudent.CurrentRow.Cells[1].Value.ToString();
            txt_FirstName.Text = dataGridView_ManageStudent.CurrentRow.Cells[2].Value.ToString();
            txt_LastName.Text = dataGridView_ManageStudent.CurrentRow.Cells[3].Value.ToString();
            txt_MobileNo.Text = dataGridView_ManageStudent.CurrentRow.Cells[4].Value.ToString();
            Gender = dataGridView_ManageStudent.CurrentRow.Cells[5].Value.ToString();
            txt_Email.Text = dataGridView_ManageStudent.CurrentRow.Cells[6].Value.ToString();
            comboBox_Course.Text = dataGridView_ManageStudent.CurrentRow.Cells[7].Value.ToString();
            txt_Fees.Text = dataGridView_ManageStudent.CurrentRow.Cells[8].Value.ToString();
            comboBox_BatchTime.Text = dataGridView_ManageStudent.CurrentRow.Cells[9].Value.ToString();

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_RegID.Text == "" || dt_RegDate.Text == "" || txt_FirstName.Text == "" ||
                    txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || txt_Fees.Text == "" || comboBox_BatchTime.Text == "")
                {
                    MessageBox.Show("Please Select Record First ", "Excel Commputers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {


                    cmd.CommandText = "DELETE * FROM Students WHERE RegID=" + txt_RegID.Text + " ";


                    DialogResult res = MessageBox.Show("Are you sure you want to Delete ?", "Excel Computers", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (res == DialogResult.No)
                    {
                        txt_SearchStudent.Focus();
                    }
                    else if (res == DialogResult.Yes)
                    {

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student Successfully Delete..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearText();
                        ShowData();
                        ShowData();

                    }
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


    }
}
