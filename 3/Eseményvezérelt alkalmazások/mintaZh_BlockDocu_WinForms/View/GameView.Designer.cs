namespace mintaZh_BlockDocu_WinForms
{
    partial class GameView
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
            BoardPanel = new TableLayoutPanel();
            NextShapePanel = new TableLayoutPanel();
            SuspendLayout();
            // 
            // BoardPanel
            // 
            BoardPanel.ColumnCount = 4;
            BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            BoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            BoardPanel.Location = new Point(20, 20);
            BoardPanel.Name = "BoardPanel";
            BoardPanel.RowCount = 4;
            BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            BoardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            BoardPanel.Size = new Size(300, 300);
            BoardPanel.TabIndex = 0;
            // 
            // NextShapePanel
            // 
            NextShapePanel.ColumnCount = 2;
            NextShapePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            NextShapePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            NextShapePanel.Location = new Point(350, 20);
            NextShapePanel.Name = "NextShapePanel";
            NextShapePanel.RowCount = 2;
            NextShapePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            NextShapePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            NextShapePanel.Size = new Size(100, 100);
            NextShapePanel.TabIndex = 1;
            // 
            // GameView
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(NextShapePanel);
            Controls.Add(BoardPanel);
            Name = "GameView";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel BoardPanel;
        private TableLayoutPanel NextShapePanel;
    }
}
