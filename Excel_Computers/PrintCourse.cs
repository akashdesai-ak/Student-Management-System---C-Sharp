using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Excel_Computers
{
    public partial class PrintCourse : Form
    {
        public PrintCourse()
        {
            InitializeComponent();
        }

        private void PrintCourse_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'courseDataSet.Courses' table. You can move, or remove it, as needed.
            this.coursesTableAdapter.Fill(this.courseDataSet.Courses);

            this.reportViewer1.RefreshReport();
        }
    }
}
