
namespace ELTE.ButtonHunt
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
            statusStrip = new System.Windows.Forms.StatusStrip();
            statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            pushButton = new System.Windows.Forms.Button();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 513);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            statusStrip.Size = new System.Drawing.Size(1075, 31);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(308, 24);
            statusLabel.Text = "Click the button to start the game!";
            // 
            // pushButton
            // 
            pushButton.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            pushButton.Location = new System.Drawing.Point(440, 187);
            pushButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            pushButton.Name = "pushButton";
            pushButton.Size = new System.Drawing.Size(206, 84);
            pushButton.TabIndex = 0;
            pushButton.Text = "PUSH ME";
            pushButton.UseVisualStyleBackColor = true;
            pushButton.Click += ButtonClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1075, 544);
            Controls.Add(pushButton);
            Controls.Add(statusStrip);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            MinimumSize = new System.Drawing.Size(1092, 589);
            Name = "Form1";
            Text = "Button Hunter";
            FormClosing += GameClosing;
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button pushButton;
    }
}

