using NetSetDom.Controls;
using NetSetDom.Model;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace NetSetDom
{
    public partial class MainForm : Form
    {
        public TabControl mainTabControl ;
        public MainForm()
        {
            InitializeComponent();

            NetworkInterface[] ifs =  IONetwork.GetInterfaces();

            var menuStrip = new MainMenuStrip();
            mainTabControl = new MainTabControl();
            //var panel = new TabPanel();

            var onglet1 = new TabPage("configuration 1");
            var onglet2 = new TabPage("configuration 2");
            var onglet3 = new TabPage("configuration 3");
            var onglet4 = new TabPage("configuration 4");
            var onglet5 = new TabPage("configuration 5");
            var onglet6 = new TabPage("configuration 6");
            var onglets = new TabPage[] { onglet1, onglet2, onglet3, onglet4, onglet5, onglet6 };
            foreach ( var onglet in onglets )
            {
                onglet.Controls.Add(new TabPanel(ifs));
            }

            mainTabControl.Alignment = TabAlignment.Top;
            mainTabControl.TabPages.AddRange(onglets);



            Controls.AddRange(new Control[] { mainTabControl, menuStrip });
        }
        
    }
}
