using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Forms.Properties;

namespace Forms
{
    public partial class Form : System.Windows.Forms.Form
    {
        KeyboardHook hook = new KeyboardHook();

        public Form()
        {
            InitializeComponent();
            hook.KeyPressed += hook_KeyPressed;
            hook.RegisterHotKey(Forms.ModifierKeys.Control | Forms.ModifierKeys.Alt, (Keys)Enum.Parse(typeof(Keys), Settings.Default.Key));
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            Thread.Sleep(Settings.Default.WaitTimeout);
            SendKeys.SendWait("^c");
            Thread.Sleep(Settings.Default.WaitTimeout);
            var text = Clipboard.GetText();
            textBox1.AppendText(Environment.NewLine + text);
            
            Uri uri;
            if (!Uri.TryCreate(text, UriKind.Absolute, out uri))
            {
                text = "https://www.google.ru/search?q="+ text.Replace(" ", "+");
            }
            
            Process.Start(text);
        }

        private void Form_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }
    }
}
