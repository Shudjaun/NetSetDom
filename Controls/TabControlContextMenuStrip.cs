using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSetDom.Controls
{
    public class TabControlContextMenuStrip : ContextMenuStrip
    {
        private readonly MainTabControl _tabControl;
        private const string NAME = "TabControlContextMenuStrip";
        public TabControlContextMenuStrip(MainTabControl tabControl)
        {
            Name = NAME;
            _tabControl = tabControl;

            //var renameTab = new ToolStripMenuItem("Renommer");
            var resetTab = new ToolStripMenuItem("Tout Effacer");

            /*renameTab.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show();
               _tabControl.SelectedTab.Text 

            };*/

            resetTab.Click += (s, e) => 
            {
                DialogResult dialogResult = MessageBox.Show("Etes vous sûr ?", "Effacer le formulaire", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //reset all components
                    var tabPage = _tabControl.SelectedTab;
                    TabPanel tab = (TabPanel) tabPage.Controls[0];
                    tab.Reset();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            };

            Items.AddRange(new ToolStripItem[] { resetTab });
        }
    }
}
