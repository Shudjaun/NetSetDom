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
            //var panel = new TabPanel();

            var onglet1 = new TabPage("onglet1");
            var onglet2 = new TabPage("onglet2");
            var onglet3 = new TabPage("onglet3");
            var onglet4 = new TabPage("onglet4");
            var onglet5 = new TabPage("onglet5");
            var onglet6 = new TabPage("onglet6");
            var onglet7 = new TabPage("onglet7");
            var onglet8 = new TabPage("onglet8");
            var onglet9 = new TabPage("onglet9");
            var onglet10 = new TabPage("onglet10");
            var onglets = new TabPage[] { onglet1, onglet2, onglet3, onglet4, onglet5, onglet6, onglet7, onglet8, onglet9, onglet10 };
            foreach ( var onglet in onglets )
            {
                onglet.Controls.Add(new TabPanel());
            }

            mainTabControl.TabPages.AddRange(onglets);



            Controls.AddRange(new Control[] { mainTabControl, menuStrip });
        }
    }
}
