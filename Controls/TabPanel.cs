using NetSetDom.Model;
using NetSetDom.utils;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
        private MaskedTextBox[] _mtbs;
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
            foreach (var mtb in _mtbs)
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
                Console.WriteLine("Sélectionné: "+interfacesBox.Text);
                ResetLabels();
                PopulateLabels(IONetwork.GetInterfaceByName(interfacesBox.Text));
            };
            
            TextBox AliasBox = new TextBox();
            CheckBox chkDHCP = new CheckBox();
            MaskedTextBox mtbIp = new MaskedTextBox();
            MaskedTextBox mtbSubnet = new MaskedTextBox();
            MaskedTextBox mtbGateway = new MaskedTextBox();
            CheckBox chkDNS = new CheckBox();
            MaskedTextBox mtbDNS1 = new MaskedTextBox();
            MaskedTextBox mtbDNS2 = new MaskedTextBox();
            _mtbs = new MaskedTextBox[] { mtbIp, mtbSubnet, mtbGateway, mtbDNS1, mtbDNS2 };
            foreach (var tbs in _mtbs)
            {
                ToolTip toolTip = new ToolTip();
                tbs.Culture = new CultureInfo("en-EN"); // To display correctly (.) and not (,) for non english users
                tbs.Mask = "###.###.###.###"; // Accept only numeric caracters

                tbs.ValidatingType = typeof(System.Net.IPAddress);

                tbs.MaskInputRejected += new MaskInputRejectedEventHandler(RejectedHandler);
                tbs.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox1_TypeValidationCompleted);
                tbs.KeyDown += new KeyEventHandler(maskedTextBox1_KeyDown);

                tbs.Size = new Size(120, 30);
                tbs.Click += (s, e) =>
                {
                    //tbs.SelectAll();
                    tbs.Select(0, 0); // Put the cursor at the beginning ot the MaskedTextBox
                };
                
                void RejectedHandler(object sender, MaskInputRejectedEventArgs e)
                {
                    
                    if (tbs.MaskFull)
                    {
                        toolTip.ToolTipTitle = "Champ plein";
                        toolTip.Show("Vous ne pouvez pas insérer plus de données.", tbs, 0, 20, 2000);
                    }
                    else if (e.Position == tbs.Mask.Length)
                    {
                        toolTip.ToolTipTitle = "Fin du champ";
                        toolTip.Show("Vous ne pouvez pas insérer plus de données à cette position", tbs, 0, 20, 2000);
                    }
                    else
                    {
                        toolTip.ToolTipTitle = "Entrée incorrecte";
                        toolTip.Show("le caractère inséré est incorrect", tbs, 0, 20, 2000);
                    }
                }
                void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
                {
                    if (!e.IsValidInput)
                    {
                        toolTip.ToolTipTitle = "non valide";
                        toolTip.Show("Doit être au format ###.###.###.###", tbs, 0, 20, 2000);
                        tbs.BackColor = Color.Crimson;
                    }
                    else
                    {
                        // TODO timer pour repasser blanc
                        tbs.BackColor = Color.Green;
                    }
                    if (tbs.Equals(mtbSubnet))
                    {
                        //Subnet specific rules
                        string userInput = e.ReturnValue.ToString();

                        if (!Subnets.GetSubnets().Contains(userInput))
                        {
                            toolTip.ToolTipTitle = "Masque invalide";
                            toolTip.Show("Vérifiez la saisie du masque de sous-réseau", tbs, 0, 20, 2000);
                            tbs.BackColor = Color.MediumVioletRed;
                            e.Cancel = true;
                        }
                    }
                }

                // Hide the tooltip if the user starts typing again before the five-second display limit on the tooltip expires.
                void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
                {
                    toolTip.Hide(tbs);
                }
            }
            int y = _topAnchor;
            interfacesBox.Location = new Point(130, y);
            y += 30;
            chkDHCP.Location = new Point(130, y);
            interfacesBox.Size = new Size(120, 30);
            AliasBox.Size = new Size(120, 30);
            for (int i = 0; i < _mtbs.Length; i++)
            {
                y += 30;
                _mtbs[i].Location = new Point(130, y);
            }
            
            chkDNS.Location = new Point(130, y + 30);
            AliasBox.Location = new Point(130, y + 60);

            Controls.Add(interfacesBox);
            Controls.Add(chkDHCP);
            Controls.AddRange(_mtbs);
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
