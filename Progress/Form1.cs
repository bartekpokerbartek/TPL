using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;

            //constructor has UI context, so we can update UI
            var progressHandler = new Progress<int>(value =>
            {
                label1.Text = value + "%";
                progressBar1.Value = value;
            });

            await Task.Run(() => DoWork(progressHandler));

            label1.Text = "Done";
        }

        private void DoWork(IProgress<int> progressHandler)
        {
            for (int i = 0; i < 101; ++i)
            {
                Random rnd = new Random();
                int delay = rnd.Next(1, 10);
                Thread.Sleep(100 * delay);

                if (progressHandler != null)
                    progressHandler.Report(i);
            }
        }
    }
}
