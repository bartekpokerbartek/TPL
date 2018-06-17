using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Invoke
{
    public partial class Form1 : Form
    {
        Task task = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var test = AddEvent();
            task = Task.Run(() => test.MyMethod());
        }

        Test AddEvent()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            
            var test = new Test();
            test.SomeEvent += (s, e) => dispatcher.Invoke(() =>
            {
                label1.Text = "Updated from event";
            });
            return test;
        }
    }
}
