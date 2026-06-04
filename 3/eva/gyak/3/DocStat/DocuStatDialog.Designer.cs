namespace DocStat
{
    partial class TextBox
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            FileContent = new System.Windows.Forms.TextBox();
            fileMenu = new ToolStripMenuItem();
            openFileDialogMenuItem = new ToolStripMenuItem();
            countWordsMenuItem = new ToolStripMenuItem();
            listBoxCounter = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            labelProperNouns = new Label();
            labelColemanLieuIndex = new Label();
            labelFleschReadingEase = new Label();
            labelCharacters = new Label();
            labelNonWhitespaceCharacters = new Label();
            labelSentences = new Label();
            spinBoxMinLength = new NumericUpDown();
            spinBoxMinOccurrence = new NumericUpDown();
            textBoxIgnoredWords = new System.Windows.Forms.TextBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1052, 32);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileContent
            // 
            FileContent.Location = new Point(0, 35);
            FileContent.Multiline = true;
            FileContent.Name = "FileContent";
            FileContent.ReadOnly = true;
            FileContent.ScrollBars = ScrollBars.Vertical;
            FileContent.Size = new Size(485, 412);
            FileContent.TabIndex = 1;
            // 
            // fileMenu
            // 
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openFileDialogMenuItem, countWordsMenuItem });
            fileMenu.Name = "fileMenu";
            fileMenu.Size = new Size(56, 28);
            fileMenu.Text = "File";
            fileMenu.Click += fileToolStripMenuItem_Click;
            // 
            // openFileDialogMenuItem
            // 
            openFileDialogMenuItem.Name = "openFileDialogMenuItem";
            openFileDialogMenuItem.Size = new Size(270, 34);
            openFileDialogMenuItem.Text = "Open file dialog";
            // 
            // countWordsMenuItem
            // 
            countWordsMenuItem.Name = "countWordsMenuItem";
            countWordsMenuItem.Size = new Size(270, 34);
            countWordsMenuItem.Text = " Count Words";
            // 
            // listBoxCounter
            // 
            listBoxCounter.FormattingEnabled = true;
            listBoxCounter.ItemHeight = 24;
            listBoxCounter.Location = new Point(542, 35);
            listBoxCounter.Name = "listBoxCounter";
            listBoxCounter.Size = new Size(505, 412);
            listBoxCounter.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(669, 464);
            label1.Name = "label1";
            label1.Size = new Size(208, 24);
            label1.TabIndex = 3;
            label1.Text = "Minimum word length:";
            label1.Click += this.label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(669, 528);
            label2.Name = "label2";
            label2.Size = new Size(246, 24);
            label2.TabIndex = 4;
            label2.Text = "Minimum word occurrence:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(669, 597);
            label3.Name = "label3";
            label3.Size = new Size(139, 24);
            label3.TabIndex = 5;
            label3.Text = "Ignored words:";
            // 
            // labelProperNouns
            // 
            labelProperNouns.AutoSize = true;
            labelProperNouns.Location = new Point(342, 464);
            labelProperNouns.Name = "labelProperNouns";
            labelProperNouns.Size = new Size(175, 24);
            labelProperNouns.TabIndex = 6;
            labelProperNouns.Text = "Proper noun count:";
            // 
            // labelColemanLieuIndex
            // 
            labelColemanLieuIndex.AutoSize = true;
            labelColemanLieuIndex.Location = new Point(342, 528);
            labelColemanLieuIndex.Name = "labelColemanLieuIndex";
            labelColemanLieuIndex.Size = new Size(182, 24);
            labelColemanLieuIndex.TabIndex = 7;
            labelColemanLieuIndex.Text = "Coleman Lieu Index:";
            // 
            // labelFleschReadingEase
            // 
            labelFleschReadingEase.AutoSize = true;
            labelFleschReadingEase.Location = new Point(342, 597);
            labelFleschReadingEase.Name = "labelFleschReadingEase";
            labelFleschReadingEase.Size = new Size(187, 24);
            labelFleschReadingEase.TabIndex = 8;
            labelFleschReadingEase.Text = "Flesch Reading Ease:";
            // 
            // labelCharacters
            // 
            labelCharacters.AutoSize = true;
            labelCharacters.Location = new Point(0, 464);
            labelCharacters.Name = "labelCharacters";
            labelCharacters.Size = new Size(151, 24);
            labelCharacters.TabIndex = 9;
            labelCharacters.Text = "Character count:";
            // 
            // labelNonWhitespaceCharacters
            // 
            labelNonWhitespaceCharacters.AutoSize = true;
            labelNonWhitespaceCharacters.Location = new Point(0, 528);
            labelNonWhitespaceCharacters.Name = "labelNonWhitespaceCharacters";
            labelNonWhitespaceCharacters.Size = new Size(248, 24);
            labelNonWhitespaceCharacters.TabIndex = 10;
            labelNonWhitespaceCharacters.Text = "Non-whitespace characters:";
            // 
            // labelSentences
            // 
            labelSentences.AutoSize = true;
            labelSentences.Location = new Point(0, 591);
            labelSentences.Name = "labelSentences";
            labelSentences.Size = new Size(146, 24);
            labelSentences.TabIndex = 11;
            labelSentences.Text = "Sentence count:";
            // 
            // spinBoxMinLength
            // 
            spinBoxMinLength.Location = new Point(921, 458);
            spinBoxMinLength.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            spinBoxMinLength.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinLength.Name = "spinBoxMinLength";
            spinBoxMinLength.Size = new Size(104, 30);
            spinBoxMinLength.TabIndex = 12;
            spinBoxMinLength.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // spinBoxMinOccurrence
            // 
            spinBoxMinOccurrence.Location = new Point(921, 526);
            spinBoxMinOccurrence.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            spinBoxMinOccurrence.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            spinBoxMinOccurrence.Name = "spinBoxMinOccurrence";
            spinBoxMinOccurrence.Size = new Size(104, 30);
            spinBoxMinOccurrence.TabIndex = 13;
            spinBoxMinOccurrence.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // textBoxIgnoredWords
            // 
            textBoxIgnoredWords.Location = new Point(814, 594);
            textBoxIgnoredWords.Name = "textBoxIgnoredWords";
            textBoxIgnoredWords.Size = new Size(211, 30);
            textBoxIgnoredWords.TabIndex = 14;
            // 
            // TextBox
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1052, 630);
            Controls.Add(textBoxIgnoredWords);
            Controls.Add(spinBoxMinOccurrence);
            Controls.Add(spinBoxMinLength);
            Controls.Add(labelSentences);
            Controls.Add(labelNonWhitespaceCharacters);
            Controls.Add(labelCharacters);
            Controls.Add(labelFleschReadingEase);
            Controls.Add(labelColemanLieuIndex);
            Controls.Add(labelProperNouns);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxCounter);
            Controls.Add(FileContent);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "TextBox";
            Text = "Document statistics";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)spinBoxMinOccurrence).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox FileContent;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openFileDialogMenuItem;
        private ToolStripMenuItem countWordsMenuItem;
        private ListBox listBoxCounter;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelProperNouns;
        private Label labelColemanLieuIndex;
        private Label labelFleschReadingEase;
        private Label labelCharacters;
        private Label labelNonWhitespaceCharacters;
        private Label labelSentences;
        private NumericUpDown spinBoxMinLength;
        private NumericUpDown spinBoxMinOccurrence;
        private System.Windows.Forms.TextBox textBoxIgnoredWords;
    }
}
