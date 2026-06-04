namespace mintaZh_Valasztas_WinForms.View
{
    partial class GameView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Table = new TableLayoutPanel();
            SuspendLayout();
            // 
            // Table
            // 
            Table.ColumnCount = 6;
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666641F));
            Table.Location = new Point(102, 50);
            Table.Name = "Table";
            Table.RowCount = 3;
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            Table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            Table.Size = new Size(610, 362);
            Table.TabIndex = 0;
            // 
            // GameView
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Table);
            Name = "GameView";
            Text = "GameView";
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel Table;
    }
}