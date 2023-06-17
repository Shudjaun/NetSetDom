using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetSetDom.Controls
{
    public class MainTabControl : TabControl
    {
        private const string NAME = "MainTabControl";
        private ContextMenuStrip _contextMenuStrip;
        public MainTabControl()
        {
            Name = NAME;
            Dock = DockStyle.Fill;
            _contextMenuStrip = new TabControlContextMenuStrip(this);
            ContextMenuStrip = _contextMenuStrip;
            
        }
    }
}
