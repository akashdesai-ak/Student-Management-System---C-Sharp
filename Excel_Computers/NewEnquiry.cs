using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class NewEnquiry : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");

        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader reader;

        public NewEnquiry()
        {
            InitializeComponent();
        }

        private void ClearText()
        {
            txt_FirstName.Text = string.Empty;
            txt_LastName.Text = string.Empty;
            txt_MobileNo.Text = string.Empty;
            dt_EnquiryDate.Text = string.Empty;
            radioButton_Female.Checked = false;
            radioButton_Male.Checked = false;
            comboBox_Course.Text = "--Select Course--";
            comboBox_BatchTime.Text = "--Select Batch Time--";
            txt_EnquiryID.Focus();


        }

        private void ShowData()
        {


            OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Enquiry", conn);
            EnquiryDataSet ds = new EnquiryDataSet();
            da.Fill(ds, "Enquiry");

            dataGridView_NewEnquiry.DataSource = ds.Tables[0];


        }
        private void ViewCourse()
        {


            cmd.CommandText = "SELECT CourseName FROM Courses";
            cmd.Connection = conn;

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox_Course.Items.Add(reader[0]);
            }



        }


        private void IDAutoIncreament()
        {

            int a;

            // course id auto increament
            cmd = new OleDbCommand("select count(EnquiryID) from Enquiry", conn);

            a = Convert.ToInt32(cmd.ExecuteScalar());
            a = a + 1;
            txt_EnquiryID.Text = a.ToString();



        }

        private void dataGridView_NewEnquiry_Click(object sender, EventArgs e)
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

            txt_EnquiryID.Text = dataGridView_NewEnquiry.CurrentRow.Cells[0].Value.ToString();
            dt_EnquiryDate.Text = dataGridView_NewEnquiry.CurrentRow.Cells[1].Value.ToString();
            txt_FirstName.Text = dataGridView_NewEnquiry.CurrentRow.Cells[2].Value.ToString();
            txt_LastName.Text = dataGridView_NewEnquiry.CurrentRow.Cells[3].Value.ToString();
            txt_MobileNo.Text = dataGridView_NewEnquiry.CurrentRow.Cells[4].Value.ToString();
            Gender = dataGridView_NewEnquiry.CurrentRow.Cells[5].Value.ToString();
            comboBox_Course.Text = dataGridView_NewEnquiry.CurrentRow.Cells[6].Value.ToString();
            comboBox_BatchTime.Text = dataGridView_NewEnquiry.CurrentRow.Cells[7].Value.ToString();

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_EnquiryID.Text == "" || dt_EnquiryDate.Text == "" || txt_FirstName.Text == "" || txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || comboBox_BatchTime.Text == "")
                {
                    MessageBox.Show("Please Fill All Fields ", "Excel Commputers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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



                    cmd.CommandText = "INSERT INTO Enquiry VALUES(" + txt_EnquiryID.Text + ",'" + dt_EnquiryDate.Text + "','" + txt_FirstName.Text + "','" + txt_LastName.Text + "'," + txt_MobileNo.Text + ",'" + Gender + "','" + comboBox_Course.Text + "','" + comboBox_BatchTime.Text + "')";

                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enquiry Successfully Save..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void NewEnquiry_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'enquiryDataSet.Enquiry' table. You can move, or remove it, as needed.
            this.enquiryTableAdapter.Fill(this.enquiryDataSet.Enquiry);

            conn.Open();

            IDAutoIncreament();
            ViewCourse();
            ShowData();

            conn.Close();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_EnquiryID.Text == "" || dt_EnquiryDate.Text == "" || txt_FirstName.Text == "" ||
                    txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || comboBox_BatchTime.Text == "")
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

                    cmd.CommandText = "UPDATE Enquiry SET EnquiryDate='" + dt_EnquiryDate.Text + "',Fname='" + txt_FirstName.Text + "',Lname='" + txt_LastName.Text + "',MobileNum=" + txt_MobileNo.Text + ",Gender='" + Gender + "',Course='" + comboBox_Course.Text + "',Batch='" + comboBox_BatchTime.Text + "' WHERE EnquiryID=" + txt_EnquiryID.Text + " ";


                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enquiry Successfully Update..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
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



        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_EnquiryID.Text == "" || dt_EnquiryDate.Text == "" || txt_FirstName.Text == "" ||
                    txt_LastName.Text == "" || txt_MobileNo.Text == "" || comboBox_Course.Text == "" || comboBox_BatchTime.Text == "")
                {
                    MessageBox.Show("Please Select Record First ", "Excel Commputers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {

                    cmd.CommandText = "DELETE * FROM Enquiry WHERE EnquiryID = " + txt_EnquiryID.Text + " ";

                    cmd.Connection = conn;

                    DialogResult res = MessageBox.Show("Are you sure you want to Delete?", "Excel Computers", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (res == DialogResult.Yes)
                    {

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Enquiry Successfully Delete..!", "Excel Computers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    ShowData();

                    }
                    else
                    {
                        ClearText();
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
