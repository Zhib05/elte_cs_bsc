namespace applications
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //quitButton.Click += quitButton_Click;
        }

        private void quitButton_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
