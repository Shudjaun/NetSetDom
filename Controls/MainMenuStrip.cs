using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            fileDropDownMenu.DropDownItems.Add(exitMenu);

            Items.Add(fileDropDownMenu);
        }
        public void HelpDropDownMenu()
        {
            var  helpDropDownMenu = new ToolStripMenuItem("Aide");
            var aboutMenu = new ToolStripMenuItem("A propos");
            var resetMenu = new ToolStripMenuItem("Reset Winsock");

            helpDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] {aboutMenu, resetMenu});

            Items.Add(helpDropDownMenu);
        }
    }
}
