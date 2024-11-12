namespace Bpjs_Decrypt_App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void pCareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new PCareForm
            {
                MdiParent = this
            }.Show();
        }

        private void vClaimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new VClaimForm
            {
                MdiParent = this
            }.Show();
        }
    }
}