using System.Windows.Forms;
using System;
using ELTE.DocuStat.Model;

namespace DocStat
{
    public partial class TextBox : Form
    {
        private IDocumentStatistics? _documentStatistics;
        public TextBox()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //f¨¢jl tall¨®z¨¢sa
            var filePath = "Input/TheBoyWhoLived.txt";

            _documentStatistics = new DocumentStatistics(filePath);
            _documentStatistics.FileContentReady += Model_FileContentReady;

            _documentStatistics.Load();
        }

        private void Model_FileContentReady(object? sender, EventArgs e)
        {
            FileContent.Text = _documentStatistics?.FileContent;
        }

        private void OpenDialog(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "txt files(*.txt)|*.txt|Allfiles(*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _documentStatistics = new DocumentStatistics(openFileDialog.FileName);
                        _documentStatistics.FileContentReady += UpdateFileContent;
                        _documentStatistics.TextStatisticsReady += UpdateTextStatistics;
                        _documentStatistics.Load();
                    }
                    catch (System.IO.IOException ex)
                    {
                        MessageBox.Show("Filereadingisunsuccessful!\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}
