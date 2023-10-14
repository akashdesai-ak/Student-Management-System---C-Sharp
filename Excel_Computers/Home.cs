using System;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Excel_Computers
{
    public partial class Home : Form
    {
        OleDbConnection conn= new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\C#\Excel_Computers\Excel_Computers\ExcelComputer.mdb");

        OleDbCommand cmd;


        public Home()
        {
            InitializeComponent();
            CustomizeDesign();
        }

        private void CustomizeDesign()
        {
            panel_StudentSubMenu.Visible = false;
            panel_CourseSubMenu.Visible = false;
            panel_ReportsSubMenu.Visible = false;
            panel_EnquirySubMenu.Visible = false;
        }

        private void HideSubMenu()
        {
            if (panel_StudentSubMenu.Visible == true)
            {
                panel_StudentSubMenu.Visible = false;
            }
            if (panel_CourseSubMenu.Visible == true)
            {
                panel_CourseSubMenu.Visible = false;
            }
            if (panel_EnquirySubMenu.Visible == true)
            {
                panel_EnquirySubMenu.Visible = false;
            }
            if (panel_ReportsSubMenu.Visible == true)
            {
                panel_ReportsSubMenu.Visible = false;
            }
        }

        private void ShowSubMenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                HideSubMenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }

        }
        private void btn_Student_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panel_StudentSubMenu);
        }

        #region StudentSubMenu
        private void btn_AddStudent_Click(object sender, EventArgs e)
        {
            OpenChildForm(new RegistrationForm());
            HideSubMenu();
        }

        private void btn_ManageStudent_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Manage_Student());
            HideSubMenu();
        }

        private void btn_PrintStudent_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PrintStudent());
            HideSubMenu();
        }
        #endregion

        private void btn_Course_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panel_CourseSubMenu);
        }

        #region CourseSubMenu
        private void btn_AddCourse_Click(object sender, EventArgs e)
        {
            OpenChildForm(new New_Course());
            HideSubMenu();
        }

        private void btn_ManageCourse_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Manage_Course());
            HideSubMenu();
        }

        private void btn_PrintCourse_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PrintCourse());
            HideSubMenu();
        }
        #endregion

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            if (activeForm!=null)
                    activeForm.Close();

            panel_Main.Controls.Add(panel_Cover);

            label_TotalStudent.Text = totalStudent();
            label_TotalCourses.Text = totalCourse();

        }

        private void Home_Load(object sender, EventArgs e)
        {

            label_TotalStudent.Text = totalStudent();
            label_TotalCourses.Text = totalCourse();    

        }

        //to show Child form in home(mainForm)
        private Form activeForm = null;
        private void OpenChildForm(Form ChildForm)
        {
            if (activeForm !=null)
            
                activeForm.Close();
                activeForm = ChildForm;
                ChildForm.TopLevel = false;
                ChildForm.FormBorderStyle=FormBorderStyle.None;
                ChildForm.Dock = DockStyle.Fill;
                panel_Main.Controls.Add(ChildForm);
                panel_Main.Tag = ChildForm;
                ChildForm.BringToFront();
                ChildForm.Show();
            
        }


        public string Count(string query)
        {
          cmd= new OleDbCommand(query, conn);
            conn.Open();
            string count = cmd.ExecuteScalar().ToString();
            conn.Close();
            return count;
            
        }

        // to get the total students
        public string totalStudent()
        {
            return Count("SELECT COUNT(*) FROM Students");
        }

        // to get the total courses
        public string totalCourse()
        {
            return Count("SELECT COUNT(*) FROM Courses");
        }

        private void btn_EnquiryMenu_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panel_EnquirySubMenu);
           
        }
        private void btnNewEnquiry_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NewEnquiry());
            HideSubMenu();
        }

        private void btnReportsMenu_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panel_ReportsSubMenu);
            
        }

        private void btnStudentReports_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

        private void btn_EnquiryReports_Click(object sender, EventArgs e)
        {
            HideSubMenu();
        }

       
    }
}
