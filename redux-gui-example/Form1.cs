using redux_gui_example.redux;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace redux_gui_example
{
    public partial class Form1 : Form
    {
        delegate void Callback();

        God god;

        public Form1()
        {
            InitializeComponent();
            god = new God(Notify);
            Notify(); // first time run
        }

        public void Notify()
        {
            if (InvokeRequired)
            {
                Invoke(new Callback(Notify), new object[] { }); 
            }
            var s = god.store.GetState();

            if (god.store.IsChange("a"))
            {
                a.Text = "" + s.a;
            }

            if (god.store.IsChange("b"))
            {
                b.Text = "" + s.b;
            }

            if (god.store.IsChange(new string[] { "result", "has_result" }))
            {
                if (s.has_result)
                {
                    result.Text = "" + s.result;
                }
                else
                {
                    result.Text = "Nothing";
                }
            }
        }

        private void plus_Click(object sender, EventArgs e)
        {
            god.store.Plus();
        }

        private void minus_Click(object sender, EventArgs e)
        {
            god.store.Minus();
        }

        private void clear_Click(object sender, EventArgs e)
        {
            god.store.Clear();
        }

        private void a_TextChanged(object sender, EventArgs e)
        {
            god.store.SetA(Int32.Parse(a.Text));
        }

        private void b_TextChanged(object sender, EventArgs e)
        {
            god.store.SetB(Int32.Parse(b.Text));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            god.Dispose();
        }
    }
}
