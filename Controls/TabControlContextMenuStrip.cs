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
        private TabControl _tabControl;
        private const string NAME = "TabControlContextMenuStrip";
        public TabControlContextMenuStrip(TabControl tabControl)
        {
            Name = NAME;
            _tabControl = tabControl;

            var renameTab = new ToolStripMenuItem("Renommer");
            var resetTab = new ToolStripMenuItem("Tout Effacer");

            renameTab.Click += (s, e) =>
            {
                _tabControl.SelectedTab.BorderStyle = BorderStyle.Fixed3D;
               

            };

            resetTab.Click += (s, e) => 
            {
                DialogResult dialogResult = MessageBox.Show("Etes vous sûr ?", "Effacer le formulaire", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //reset all components
                    //_tabControl.SelectedTab.
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            };

            Items.AddRange(new ToolStripItem[] { renameTab, resetTab });
        }
    }
}
