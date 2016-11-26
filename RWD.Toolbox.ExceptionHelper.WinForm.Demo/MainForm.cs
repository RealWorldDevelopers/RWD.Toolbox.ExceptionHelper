using System;
using System.IO;
using System.Windows.Forms;

namespace RWD.Toolbox.ExceptionHelper.WinForm.Demo
{
    public partial class MainForm : Form
    {
        public ExceptionHelper Logger = new ExceptionHelper();

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //throw new Exception("Simple Test Message");
                MiddleMethod();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());

                Logger.AllowManualReporting = true;
                Logger.IncludeInnerExceptions = true;

                try
                {
                    var result = Logger.BuildExceptionString(ex);

                    Logger.DisplayErrorToUser(ex, result);
                }
                catch (Exception oops)
                {
                    MessageBox.Show(oops.ToString());
                }

            }
        }


        private void MiddleMethod()
        {
            ThrowError();
        }

        private void ThrowError()
        {
            var ex = new FileNotFoundException("File Not found when trying to write argument exception to the file", "file.txt",
                new NullReferenceException("First Inner Exception Message", new ArgumentNullException("param1", "Second Inner Exception Message")));
            ex.Data.Add("data1", "value1");
            throw ex;

        }
    }
}
