using NetSetDom.Model;
using NetSetDom.utils;
using System;
using System.CodeDom;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NetSetDom.Controls
{
    /// <summary>
    /// Conteneur principal de l'onglet
    /// </summary>
    public class TabPanel : Panel
    {
        private const string NAME = "TAB_PANEL";
        private readonly int _topAnchor = 20;
        private readonly NetworkInterface[] _networkInterface;
        private TextBox[] _TextBoxs;
        private Label[] _labelsD;

        /// <summary>
        /// Instancie un nouveau conteneur principal pour l'onglet
        /// </summary>
        private TabPanel()
        {
        }
        /// <summary>
        /// Instancie un nouveau conteneur pricipal pour l'ongler<br>
        /// La liste des interfaces est passé en paramètre</br>
        /// </summary>
        /// <param name="interfaces"></param>
        public TabPanel(NetworkInterface[] interfaces)
        {
            _networkInterface = interfaces;
            Name = NAME;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;

            InitComponents();
        }

        /// <summary>
        /// Efface tous les champs de saisie
        /// </summary>
        public void Reset()
        {
            foreach (var mtb in _TextBoxs)
            {
                mtb.Clear();
            }
        }
        /// <summary>
        /// Efface les labels de droite
        /// </summary>
        private void ResetLabels()
        {
            foreach (var lbl in _labelsD)
            {
                lbl.Text = "...";
            }
            _labelsD[0].Text = "Configuration actuelle";
        }
        public TabPanel GetPanel()
        {
            return this;
        }

        private void InitComponents()
        {
            InitBoxs();
            InitLabels();
        }

        private void InitLabels()
        {
            Font fontElemsG = new Font("Arial", 10, FontStyle.Regular);
            Font fontElemsD = new Font("Arial", 10, FontStyle.Italic);
            Font fontTitre = new Font("Arial", 10, FontStyle.Bold);
            // Labels sur la partie Gauche
            Label lblInterface = new Label { Text = "Interface:" };
            Label lblIPAUTO = new Label { Text = "IP auto:"};
            Label lblIp = new Label { Text = "Adresse IpV4:"};
            Label lblSubnet = new Label { Text = "Masque:" };
            Label lblGateway = new Label { Text = "Passerelle:" };
            Label lblDNS1 = new Label { Text = "DNS1:" };
            Label lblDNS2 = new Label { Text = "DNS2:" };
            Label lblDNSAUTO = new Label { Text = "DNS auto:" };
            Label lblAlias = new Label { Text = "Alias:" };
            Label[] labelsG = new Label[] { lblInterface, lblIPAUTO, lblIp, lblSubnet, lblGateway, lblDNS1, lblDNS2, lblDNSAUTO, lblAlias };
            int y = _topAnchor;
            for (int i=0; i<labelsG.Length; i++)
            {
                labelsG[i].Size = new Size(150, 30);
                labelsG[i].Location = new Point(20, y);
                labelsG[i].Font = fontElemsG;
                y += 30;
            }
            // Labels sur la partie droite
            Label lblConfiguration = new Label { Text = "Configuration actuelle" };
            Label lblDHCPNow = new Label { Text = "..." };
            Label lblIpNow = new Label { Text = "..." };
            Label lblSubnetNow = new Label { Text = "..." };
            Label lblGatewayNow = new Label { Text = "..." };
            Label lblDNS1Now = new Label { Text = "..." };
            Label lblDNS2Now = new Label { Text = "..." };
            Label lblDNSAutoNow = new Label { Text = "..." };
            _labelsD = new Label[] { lblConfiguration, lblDHCPNow, lblIpNow, lblSubnetNow, lblGatewayNow, lblDNS1Now, lblDNS2Now, lblDNSAutoNow };
            int z = _topAnchor;
            for (int i = 0; i < _labelsD.Length; i++)
            {
                _labelsD[i].Size = new Size(200, 30);
                _labelsD[i].Location = new Point(300, z);
                _labelsD[i].Font = fontElemsD;
                z += 30;
            }
            lblConfiguration.Font = fontTitre;


            Controls.AddRange(labelsG);
            Controls.AddRange(_labelsD);
        }
        /// <summary>
        /// Initialise les champs de saisie
        /// Crée les évenements
        /// </summary>
        private void InitBoxs()
        {
            ComboBox interfacesBox = new ComboBox();
            foreach (NetworkInterface iface in _networkInterface)
            {
                interfacesBox.Items.Add(iface.Name);
            }
            interfacesBox.SelectedValueChanged += (s, e) =>
            {
                //Console.WriteLine("Sélectionné: "+interfacesBox.Text);
                ResetLabels();
                PopulateLabels(IONetwork.GetInterfaceByName(interfacesBox.Text));
            };
            
            TextBox AliasBox = new TextBox();
            CheckBox chkDHCP = new CheckBox();
            TextBox mtbIp = new TextBox();
            TextBox mtbSubnet = new TextBox();
            TextBox mtbGateway = new TextBox();
            CheckBox chkDNS = new CheckBox();
            TextBox mtbDNS1 = new TextBox();
            TextBox mtbDNS2 = new TextBox();
            _TextBoxs = new TextBox[] { mtbIp, mtbSubnet, mtbGateway, mtbDNS1, mtbDNS2 };
            foreach (var textBox in _TextBoxs)
            {
                Regex iPv4Regex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
                //Regex ipv6Regex = new Regex("^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$");

                ToolTip toolTip = new ToolTip();

                textBox.Size = new Size(120, 30);
                /*textBox.Click += (s, e) =>
                {
                    //tbs.SelectAll();
                    textBox.Select(0, 0); // Put the cursor at the beginning ot the TextBox
                };*/
                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Equals(mtbSubnet) && mtbIp.Text.Length >= 7)
                    {
                        textBox.Text = "255.255.255.0";
                    }
                };
                textBox.LostFocus += (s, e) =>
                {
                    if ( !textBox.Equals(mtbSubnet) && iPv4Regex.IsMatch(textBox.Text))
                    {
                        textBox.BackColor = Color.Green;
                    } else if ( Subnets.GetSubnets().Contains(textBox.Text) )
                    {
                        textBox.BackColor = Color.Green;
                    }
                    else
                    {
                        textBox.BackColor = Color.Red;
                    }
                };

            }

            chkDHCP.CheckedChanged += (s, e) =>
            {
                mtbIp.BackColor = Color.White;
                mtbSubnet.BackColor = Color.White;
                mtbGateway.BackColor = Color.White;
                if (chkDHCP.Checked)
                {
                    mtbIp.Text = "";
                    mtbSubnet.Text = "";
                    mtbGateway.Text = "";
                    mtbIp.Enabled = false;
                    mtbSubnet.Enabled = false;
                    mtbGateway.Enabled = false;
                } else
                {
                    mtbIp.Enabled = true;
                    mtbSubnet.Enabled = true;
                    mtbGateway.Enabled = true;
                }
            };
            chkDNS.CheckedChanged += (s, e) =>
            {
                mtbDNS1.BackColor = Color.White;
                mtbDNS2.BackColor = Color.White;
                if ( chkDNS.Checked)
                {
                    mtbDNS1.Text = "";
                    mtbDNS2.Text = "";
                    mtbDNS1.Enabled = false;
                    mtbDNS2.Enabled = false;
                } else
                {
                    mtbDNS1.Enabled = true;
                    mtbDNS2.Enabled = true;
                }
                
            };

            int y = _topAnchor;
            interfacesBox.Location = new Point(130, y);
            y += 30;
            chkDHCP.Location = new Point(130, y);
            interfacesBox.Size = new Size(120, 30);
            AliasBox.Size = new Size(120, 30);
            for (int i = 0; i < _TextBoxs.Length; i++)
            {
                y += 30;
                _TextBoxs[i].Location = new Point(130, y);
            }
            
            chkDNS.Location = new Point(130, y + 30);
            AliasBox.Location = new Point(130, y + 60);

            Controls.Add(interfacesBox);
            Controls.Add(chkDHCP);
            Controls.AddRange(_TextBoxs);
            Controls.Add(chkDNS);
            Controls.Add(AliasBox);
        }
        /// <summary>
        /// Mets à jour les informations relatives à l'interface réseau sélectionnée
        /// </summary>
        /// <param name="networkInterface"> l'interface réseau sélectionnée</param>
        private void PopulateLabels(NetworkInterface networkInterface)
        {
            if (networkInterface != null)
            {
                _labelsD[1].Text = networkInterface.GetIPProperties().GetIPv4Properties().IsDhcpEnabled.ToString(); // DHCP
                foreach (var addr in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (addr.Address.ToString().Contains(".")) _labelsD[2].Text = addr.Address.ToString();// IP
                    _labelsD[3].Text = addr.IPv4Mask.ToString(); // Subnet
     
                }
                foreach (var gw in networkInterface.GetIPProperties().GatewayAddresses)
                {
                    if (gw.Address.ToString().Contains(".")) _labelsD[4].Text = gw.Address.ToString(); // Gateway
                }

                var dnss = networkInterface.GetIPProperties().DnsAddresses;
                int v = 0;
                foreach (var addr in dnss)
                {
                    if (addr.ToString().Contains("."))
                    {
                        _labelsD[5 + v].Text = addr.ToString();
                        v++;
                    }
                }
                _labelsD[7].Text = networkInterface.GetIPProperties().IsDynamicDnsEnabled.ToString(); // DNS auto
            }
        }
    }
}
