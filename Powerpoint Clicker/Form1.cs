using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace PptHotkey
{
    public partial class Form1 : Form
    {
        private PowerPoint.Application ppapp;

        private const int VK_LEFT = 0x25;
        private const int VK_UP = 0x26;
        private const int VK_RIGHT = 0x27;
        private const int VK_DOWN = 0x28;
        private const int WM_KEYDOWN = 0x0100;

        public Form1(KeyboardHooker hooker)
        {
            InitializeComponent();

            hooker.KeyIntercepted += new KeyboardHooker.KeyInterceptedHandler(hooker_KeyIntercepted);

            if (hooker.ErrorCode == 0)
            {
                this.lblKeyboardHookStatus.Text = "OK";
                this.lblKeyboardHookStatus.ForeColor = Color.Green;
            }
            else
            {
                this.lblKeyboardHookStatus.Text = string.Format("Fehler 0x{0:X}", hooker.ErrorCode);
                this.lblKeyboardHookStatus.ForeColor = Color.Red;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            this.UseWaitCursor = true;

            try
            {
                ppapp = new PowerPoint.Application();
            }
            catch (Exception x) 
            {
                MessageBox.Show(x.Message, "PPT Load Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            this.UseWaitCursor = false;
            
            base.OnShown(e);
        }

        bool hooker_KeyIntercepted(IntPtr wparam, int keycode)
        {
            if (keycode == VK_LEFT || keycode == VK_UP) 
            {
                if (wparam == (IntPtr)WM_KEYDOWN) 
                {
                    this.BeginInvoke(new Action(this.Prev));
                }
                return true;
            }

            if (keycode == VK_RIGHT || keycode == VK_DOWN)
            {
                if (wparam == (IntPtr)WM_KEYDOWN)
                {
                    this.BeginInvoke(new Action(this.Next));
                }
                return true;
            }

            return false;
        }

        private void SetSlideStatusOk()
        {
            lblSlideSwitchStatus.Text = "OK";
            lblSlideSwitchStatus.ForeColor = Color.Green;
        }

        private void SetSlideStatusFail(string text)
        {
            lblSlideSwitchStatus.Text = text;
            lblSlideSwitchStatus.ForeColor = Color.Red;
        }

        private void Prev()
        {
            if (ppapp == null) 
            {
                SetSlideStatusFail("PowerPoint nicht geladen");
            }
            else if (ppapp.SlideShowWindows.Count == 0)
            {
                SetSlideStatusFail("Keine Präsentation");
            }
            else
            {
                try
                {
                    ppapp.SlideShowWindows[1].View.Previous();
                    SetSlideStatusOk();
                }
                catch (Exception e)
                {
                    SetSlideStatusFail(e.Message);
                }
            }
        }

        private void Next()
        {
            if (ppapp == null) 
            {
                SetSlideStatusFail("PowerPoint nicht geladen");
            }
            else if (ppapp.SlideShowWindows.Count == 0)
            {
                SetSlideStatusFail("Keine Präsentation");
            }
            else
            {
                try
                {
                    ppapp.SlideShowWindows[1].View.Next();
                    SetSlideStatusOk();
                }
                catch (Exception e)
                {
                    SetSlideStatusFail(e.Message);
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Prev();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }
    }
}
