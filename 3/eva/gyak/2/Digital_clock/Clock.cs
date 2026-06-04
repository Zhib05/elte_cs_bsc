using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_clock
{
    public partial class Clock : UserControl
    {
        private System.Windows.Forms.Timer timer;
        private int _timeZone;
        public Clock()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += RefreshTime; // Event handler for Tick event
            timer.Start(); // Start the timer
        }

        [Category("Appearance")]
        [Description("The name of the city.")]
        public string City
        {
            get => cityLabel.Text;
            set => cityLabel.Text = value;
        }

        [Category("Appearance")]
        [Description("The hour offset of the timezone.")]
        public int TimeZone
        {
            get { return _timeZone; }
            set
            {
                _timeZone = value;
                RefreshTime(this, EventArgs.Empty);
            }
        }

        private void RefreshTime(object? sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            timeLabel.Text = time
                .AddHours(_timeZone)
                .ToString(time.Second % 2 == 0 ? "HH:mm" : "HH mm");
        }

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }

        private void cityLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
