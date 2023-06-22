using NetSetDom.Model;
using System.Windows.Forms;

namespace NetSetDom.Controls
{
    public class MainMenuStrip : MenuStrip
    {
        private const string NAME = "MainMenuStrip";
        public MainMenuStrip()
        {
            Name = NAME;
            Dock = DockStyle.Top;

            FileDropDownMenu();
            HelpDropDownMenu();
        }

        public void FileDropDownMenu()
        {
            var fileDropDownMenu = new ToolStripMenuItem("Fichier");
            var exitMenu = new ToolStripMenuItem("Quitter", null, null, Keys.Control | Keys.Q);

            exitMenu.Click += (s, e) => 
            {
                Application.Exit();
            };

            fileDropDownMenu.DropDownItems.Add(exitMenu);

            Items.Add(fileDropDownMenu);
        }
        public void HelpDropDownMenu()
        {
            var helpDropDownMenu = new ToolStripMenuItem("Aide");
            var aboutMenu = new ToolStripMenuItem("A propos");
            var resetMenu = new ToolStripMenuItem("Reset Winsock");

            aboutMenu.Click += (s, e) =>
            {
                MessageBox.Show("\t\tNetSetDom v1.0\n\nRequiert:\n\t-Windows 10 ou 11 en Français.\n\t-.NET framework 4.8");
            };

            resetMenu.Click += (s, e) =>
            {
                IONetwork.ResetWinsock();
            };

            helpDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] {aboutMenu, resetMenu});

            Items.Add(helpDropDownMenu);
        }
    }
}
