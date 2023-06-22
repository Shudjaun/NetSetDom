using NetSetDom.Model;
using NetSetDom.utils;
using System.Drawing;
using System.Net.NetworkInformation;
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
        private TextBox _aliasBox;
        private TextBox _mtbIp;
        private TextBox _mtbSubnet;
        private TextBox _mtbGateway;
        private TextBox _mtbDNS1;
        private TextBox _mtbDNS2;
        private TextBox[] _TextBoxs;
        private Label[] _labelsD;
        private CheckBox _chkDHCP;
        private CheckBox _chkDNS;
        private ComboBox _interfacesBox;

        /// <summary>
        /// Instancie un nouveau conteneur principal pour l'onglet
        /// </summary>
        private TabPanel()
        {
        }
        /// <summary>
        /// Instancie un nouveau conteneur pricipal pour l'onglet<br>
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
            InitValiderButton();
        }
        /// <summary>
        /// Save Configurations in a xml file
        /// </summary>
        private void SaveConfigurations()
        {
            // TODO
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
            _interfacesBox = new ComboBox();
            foreach (NetworkInterface iface in _networkInterface)
            {
                _interfacesBox.Items.Add(iface.Name);
            }
            _interfacesBox.SelectedValueChanged += (s, e) =>
            {
                //Console.WriteLine("Sélectionné: "+interfacesBox.Text);
                ResetLabels();
                PopulateLabels(IONetwork.GetInterfaceByName(_interfacesBox.Text));
            };
            
            _aliasBox = new TextBox();
            _chkDHCP = new CheckBox();
            _mtbIp = new TextBox();
            _mtbSubnet = new TextBox();
            _mtbGateway = new TextBox();
            _chkDNS = new CheckBox();
            _mtbDNS1 = new TextBox();
            _mtbDNS2 = new TextBox();
            _TextBoxs = new TextBox[] { _mtbIp, _mtbSubnet, _mtbGateway, _mtbDNS1, _mtbDNS2 };
            foreach (var textBox in _TextBoxs)
            {
                textBox.BackColor = Constantes.VALIDATION_DEFAULT;

                ToolTip toolTip = new ToolTip();

                textBox.Size = new Size(120, 30);
                /*textBox.Click += (s, e) =>
                {
                    //tbs.SelectAll();
                    textBox.Select(0, 0); // Put the cursor at the beginning ot the TextBox
                };*/
                textBox.GotFocus += (s, e) =>
                {
                    if (textBox.Equals(_mtbSubnet) && _mtbIp.Text.Length >= 7)
                    {
                        textBox.Text = "255.255.255.0";
                    }
                };
                textBox.LostFocus += (s, e) =>
                {
                    if (textBox.Text.Length == 0) return;
                    ValidateTestBox(textBox);
                };

            }

            _chkDHCP.CheckedChanged += (s, e) =>
            {
                _mtbIp.BackColor = Constantes.VALIDATION_DEFAULT;
                _mtbSubnet.BackColor = Constantes.VALIDATION_DEFAULT;
                _mtbGateway.BackColor = Constantes.VALIDATION_DEFAULT;
                if (_chkDHCP.Checked)
                {
                    _mtbIp.Text = "";
                    _mtbSubnet.Text = "";
                    _mtbGateway.Text = "";
                    _mtbIp.Enabled = false;
                    _mtbSubnet.Enabled = false;
                    _mtbGateway.Enabled = false;
                } else
                {
                    _mtbIp.Enabled = true;
                    _mtbSubnet.Enabled = true;
                    _mtbGateway.Enabled = true;
                }
            };
            _chkDNS.CheckedChanged += (s, e) =>
            {
                _mtbDNS1.BackColor = Constantes.VALIDATION_DEFAULT;
                _mtbDNS2.BackColor = Constantes.VALIDATION_DEFAULT;
                if ( _chkDNS.Checked)
                {
                    _mtbDNS1.Text = "";
                    _mtbDNS2.Text = "";
                    _mtbDNS1.Enabled = false;
                    _mtbDNS2.Enabled = false;
                } else
                {
                    _mtbDNS1.Enabled = true;
                    _mtbDNS2.Enabled = true;
                }
                
            };

            int y = _topAnchor;
            _interfacesBox.Location = new Point(130, y);
            y += 30;
            _chkDHCP.Location = new Point(130, y);
            _interfacesBox.Size = new Size(120, 30);
            _aliasBox.Size = new Size(120, 30);
            for (int i = 0; i < _TextBoxs.Length; i++)
            {
                y += 30;
                _TextBoxs[i].Location = new Point(130, y);
            }
            
            _chkDNS.Location = new Point(130, y + 30);
            _aliasBox.Location = new Point(130, y + 60);

            Controls.Add(_interfacesBox);
            Controls.Add(_chkDHCP);
            Controls.AddRange(_TextBoxs);
            Controls.Add(_chkDNS);
            Controls.Add(_aliasBox);
        }

        /// <summary>
        /// Mets à jour les informations relatives à l'interface réseau sélectionnée
        /// </summary>
        /// <param name="networkInterface"> l'interface réseau sélectionnée</param>

        private void InitValiderButton()
        {
            Button btnValider = new Button
            {
                Text = "Valider",
                Location = new Point(200, 320)
            };
            btnValider.Click += (s, e) =>
            {
                if (InputValidation())
                {
                    SaveConfigurations();
                    if ( !_chkDHCP.Checked ) // IP static
                    {
                        IONetwork.SetStaticIP(_interfacesBox.Text, _TextBoxs[0].Text, _TextBoxs[1].Text, _TextBoxs[2].Text);
                    }
                    else // IP auto
                    {
                        IONetwork.SetDynamicIP(_interfacesBox.Text);
                    }
                    if ( !_chkDNS.Checked ) // DNS static
                    {
                        if (_TextBoxs[4].Text.Length > 6) // IF a second server is defined
                        {
                            IONetwork.SetStaticDNS(_interfacesBox.Text, _TextBoxs[3].Text + ';' + _TextBoxs[4].Text);
                        } else // Only primary server
                        {
                            IONetwork.SetStaticDNS(_interfacesBox.Text, _TextBoxs[3].Text);
                        }
                        
                    }
                    else // Dns auto
                    {
                        IONetwork.SetDynamicDNS(_interfacesBox.Text);
                    }
                } else
                {
                    MessageBox.Show("Veuillez corriger les erreurs avant de valider.\n\nN'oubliez pas de choisir une interface (carte réseau)");
                }
                
            };

            Controls.Add(btnValider);

        }

        /// <summary>
        /// Validation of all user inputs
        /// </summary>
        /// <returns></returns>
        private bool InputValidation()
        {
            if (_interfacesBox.Text.Length < 2) // No interface selected
            {
                return false;
            }
            if (_chkDHCP.Checked && _chkDNS.Checked) // IP and DNS auto
            {
                return true;
            } else if (_chkDHCP.Checked && !_chkDNS.Checked) // IP auto and DNS static
            {
                if (ValidateTestBox(_mtbDNS1) && ValidateTestBox(_mtbDNS2)) return true;
                else return false;

            } else if (!_chkDHCP.Checked) // IP static
            {
                if (ValidateTestBox(_mtbIp)
                    && ValidateTestBox(_mtbSubnet)
                    && ValidateTestBox(_mtbGateway)
                    && ValidateTestBox(_mtbDNS1)
                    && ValidateTestBox(_mtbDNS2))
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }
        /// <summary>
        /// Change BackColor depending on user Entry
        /// </summary>
        /// <param name="textBox"></param>
        private bool ValidateTestBox(TextBox textBox)
        {
            if (!textBox.Equals(_mtbSubnet) && Constantes.IPV4REGEX.IsMatch(textBox.Text))
            {
                textBox.BackColor = Constantes.VALIDATION_PASS;
                return true;
            }
            else if (Subnets.GetSubnets().Contains(textBox.Text))
            {
                textBox.BackColor = Constantes.VALIDATION_PASS;
                return true;
            } 
            else if (textBox.Equals(_mtbGateway) || textBox.Equals(_mtbDNS1) || textBox.Equals(_mtbDNS2)) // GW and DNSs can be left blank in static
            {
                if (textBox.Text.Length == 0)
                {
                    textBox.BackColor = Constantes.VALIDATION_PASS;
                    return true;
                }
                else
                {
                    textBox.BackColor = Constantes.VALIDATION_FAILED;
                    return false;
                }
            }
            else
            {
                textBox.BackColor = Constantes.VALIDATION_FAILED;
                return false;
            }
        }

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
