using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaptorTCP3.Forms
{
    public partial class frmShowUsers : Form
    {
        public frmShowUsers()
        {
            InitializeComponent();
        }

        private void frmShowUsers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'damoclesDataSet.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.damoclesDataSet.Users);

        }
    }
}
