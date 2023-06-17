using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace NetSetDom.Controls
{
    /// <summary>
    /// Conteneur principal de l'onglet
    /// </summary>
    public class TabPanel : Panel
    {
        private const string NAME = "TAB_PANEL";
        /// <summary>
        /// Instancie un nouveau conteneur principal pour l'onglet
        /// </summary>
        public TabPanel()
        {
            Name = NAME;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            Font = new Font("Arial", 12, FontStyle.Bold);
            InitComponents();
            //Controls.Add(new Button());
        }

        private void InitComponents()
        {
            
            MaskedTextBox mtbIp = new MaskedTextBox();
            MaskedTextBox mtbSubnet = new MaskedTextBox();
            MaskedTextBox mtbGateway = new MaskedTextBox();
            MaskedTextBox mtbDNS1 = new MaskedTextBox();
            MaskedTextBox mtbDNS2 = new MaskedTextBox();
            MaskedTextBox[] mtbs = new MaskedTextBox[] { mtbIp, mtbSubnet, mtbGateway, mtbDNS1, mtbDNS2 };
            foreach (var tbs in mtbs)
            {
                tbs.Culture = new CultureInfo("en-EN"); // To display correctly . and not , for non english users
                tbs.Mask = "###.###.###.###";
                tbs.ValidatingType = typeof(System.Net.IPAddress);
                
                tbs.Size = new Size(120, 30);
                tbs.Click += (s, e) =>
                {
                    tbs.SelectAll();
                };
            }

            mtbIp.Location = new Point(80, 50);
            mtbSubnet.Location = new Point(80, 80);
            mtbGateway.Location = new Point(80, 110);
            mtbDNS1.Location = new Point(80, 140);
            mtbDNS2.Location = new Point(80, 170);

            Controls.AddRange(mtbs);
        }

        public TabPanel GetPanel()
        { 
            return this;
        }    
    }
}
