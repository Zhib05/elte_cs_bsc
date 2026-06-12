namespace Digital_clock
{
    partial class Clock
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cityLabel = new Label();
            timeLabel = new Label();
            SuspendLayout();
            // 
            // cityLabel
            // 
            cityLabel.AutoSize = true;
            cityLabel.Location = new Point(35, 31);
            cityLabel.Name = "cityLabel";
            cityLabel.Size = new Size(97, 24);
            cityLabel.TabIndex = 0;
            cityLabel.Text = "City name";
            cityLabel.Click += cityLabel_Click;
            // 
            // timeLabel
            // 
            timeLabel.BorderStyle = BorderStyle.Fixed3D;
            timeLabel.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            timeLabel.Location = new Point(35, 69);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(123, 38);
            timeLabel.TabIndex = 1;
            timeLabel.Text = "HH:MM";
            timeLabel.TextAlign = ContentAlignment.MiddleCenter;
            timeLabel.Click += timeLabel_Click;
            // 
            // Clock
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(timeLabel);
            Controls.Add(cityLabel);
            Name = "Clock";
            Size = new Size(224, 142);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label cityLabel;
        private Label timeLabel;
    }
}
