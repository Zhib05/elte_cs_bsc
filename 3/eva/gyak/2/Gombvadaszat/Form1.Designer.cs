namespace Gombvadaszat
{
    partial class Form1
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
            pushButton = new Button();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pushButton
            // 
            pushButton.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            pushButton.Location = new Point(390, 191);
            pushButton.Name = "pushButton";
            pushButton.Size = new Size(169, 67);
            pushButton.TabIndex = 0;
            pushButton.Text = "PUSH ME";
            pushButton.UseVisualStyleBackColor = true;
            pushButton.Click += pushButton_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(24, 24);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 419);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 31);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(308, 24);
            statusLabel.Text = "Click the button to start the game!";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip);
            Controls.Add(pushButton);
            Name = "Form1";
            Text = "Button Hunter";
            FormClosing += GameClosing;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button pushButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
    }
}
