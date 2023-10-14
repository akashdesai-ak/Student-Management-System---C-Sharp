using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class RegistrationForm : Form
    {

        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader reader;

        string Gender;

        public RegistrationForm()
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


            txt_RegID.Focus();
        }

        private void ShowData()
        {
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Students", conn);
            StudentsDataSet ds = new StudentsDataSet();
            da.Fill(ds, "Students");
            dataGridView_NewStudent.RowTemplate.Height = 75;
            dataGridView_NewStudent.DataSource = ds.Tables[0];
            DataGridViewImageColumn pic1 = new DataGridViewImageColumn();
            pic1 = (DataGridViewImageColumn)dataGridView_NewStudent.Columns[10];
            pic1.ImageLayout = DataGridViewImageCellLayout.Zoom;

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_RegID.Text == "" || dt_RegDate.Text == "" || txt_FirstName.Text == "" ||
                    txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || txt_Fees.Text == "" || comboBox_BatchTime.Text =="")
                {
                    MessageBox.Show("Please Fill All Fields ", "Excel Commputers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {

                    if (radioButton_Male.Checked == true)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }



                    cmd.CommandText = "INSERT INTO Students(RegID,RegDate,Fname,Lname,MobileNo,Gender,Email,Course,Fees,Batch,StudentPhoto) VALUES(" + txt_RegID.Text + ",'" + dt_RegDate.Text + "','" + txt_FirstName.Text + "','" + txt_LastName.Text + "'," + txt_MobileNo.Text + ",'" + Gender + "','" + txt_Email.Text + "','" + comboBox_Course.Text + "'," + txt_Fees.Text + ",'" + comboBox_BatchTime.Text + "',@studentPhoto)";
                    cmd.Connection = conn;

                    //Convert_photo();

                    MemoryStream memstr = new MemoryStream();
                    pictureBox_StuPhoto.Image.Save(memstr, pictureBox_StuPhoto.Image.RawFormat);
                    cmd.Parameters.AddWithValue("studentPhoto", memstr.ToArray());


                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Successfully Save..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
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


        private void IDAutoIncreament()
        {
            int a;


            // course id auto increament
            cmd = new OleDbCommand("select count(RegID) from students", conn);

            a = Convert.ToInt32(cmd.ExecuteScalar());
            a = a + 1;
            txt_RegID.Text = a.ToString();



        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'studentsDataSet.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.studentsDataSet.Students);
         


            ViewCourse();

            conn.Open();
            ShowData();
            IDAutoIncreament();
            conn.Close();
        }

        private void btn_UploadPhoto_Click(object sender, EventArgs e)
        {
            openFileDialog_SudentPhoto.FileName = "";
            openFileDialog_SudentPhoto.Filter = "jpeg, jpg|*.jpeg ,*.jpg|bmp|*.bmp|all files|*.*";
            DialogResult res = openFileDialog_SudentPhoto.ShowDialog();

            if (res == DialogResult.OK)
            {
                pictureBox_StuPhoto.Image = Image.FromFile(openFileDialog_SudentPhoto.FileName);
            }

        }

        private void Convert_photo()
        {
            // converting photo to binary data
            if (pictureBox_StuPhoto.Image != null)
            {
                // using memoryStream :
                MemoryStream ms = new MemoryStream();
                pictureBox_StuPhoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
                cmd.Parameters.AddWithValue("@studentPhoto", photo_aray);
            }
        }

        
    }
}
