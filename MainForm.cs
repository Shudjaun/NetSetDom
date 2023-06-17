using NetSetDom.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSetDom
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            var menuStrip = new MainMenuStrip();
            var mainTabControl = new MainTabControl();
            var panel = new TabPanel();

            var onglet1 = new TabPage("onglet1");
            onglet1.Controls.Add(panel);

            mainTabControl.TabPages.AddRange(new TabPage[] { onglet1 });



            Controls.AddRange(new Control[] { mainTabControl, menuStrip });
        }
    }
}
