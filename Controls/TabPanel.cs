using NetSetDom.utils;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace NetSetDom.Controls
{
    /// <summary>
    /// Conteneur principal de l'onglet
    /// </summary>
    public class TabPanel : Panel
    {
        private const string NAME = "TAB_PANEL";
        private readonly int _topAnchor = 50;
        private MaskedTextBox[] mtbs;

        /// <summary>
        /// Instancie un nouveau conteneur principal pour l'onglet
        /// </summary>
        public TabPanel()
        {
            Name = NAME;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            Font = new Font("Arial", 10, FontStyle.Bold);
            InitComponents();
        }
        /// <summary>
        /// Efface tous les champs de saisie
        /// </summary>
        public void Reset()
        {
            foreach (var mtb in mtbs)
            {
                mtb.Clear();
            }
        }

        private void InitComponents()
        {
            InitTextBox();
            InitStaticLabels();
        }

        private void InitStaticLabels()
        {
            Label lblIp = new Label { Text = "Ip:"};
            Label lblSubnet = new Label { Text = "Masque:" };
            Label lblGateway = new Label { Text = "Passerelle:" };
            Label lblDNS1 = new Label { Text = "DNS1:" };
            Label lblDNS2 = new Label { Text = "DNS2:" };
            Label[] labels = new Label[] { lblIp, lblSubnet, lblGateway, lblDNS1, lblDNS2 };
            int y = _topAnchor;
            for (int i=0; i<labels.Length; i++)
            {
                labels[i].Location = new Point(20, y);
                y += 30;
            }
            Controls.AddRange(labels);
        }

        private void InitTextBox()
        {
            MaskedTextBox mtbIp = new MaskedTextBox();
            MaskedTextBox mtbSubnet = new MaskedTextBox();
            MaskedTextBox mtbGateway = new MaskedTextBox();
            MaskedTextBox mtbDNS1 = new MaskedTextBox();
            MaskedTextBox mtbDNS2 = new MaskedTextBox();
            mtbs = new MaskedTextBox[] { mtbIp, mtbSubnet, mtbGateway, mtbDNS1, mtbDNS2 };
            foreach (var tbs in mtbs)
            {
                ToolTip toolTip = new ToolTip();
                tbs.Culture = new CultureInfo("en-EN"); // To display correctly (.) and not (,) for non english users
                tbs.Mask = "###.###.###.###"; // Accept only numerci caracters

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
                        tbs.BackColor = Color.MediumVioletRed;
                    }
                    else if (tbs.Equals(mtbSubnet))
                    {
                        //Subnet specific rules
                        string userInput = e.ReturnValue.ToString();

                        if (! Subnets.GetSubnets().Contains(userInput))
                        {
                            toolTip.ToolTipTitle = "Masque invalide";
                            toolTip.Show("Vérifiez la saisie du masque de sous-réseau", tbs, 0, 20, 2000);
                            tbs.BackColor = Color.MediumVioletRed;
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        // TODO timer pour repasser blanc
                        tbs.BackColor = Color.Green;
                    }
                }

                // Hide the tooltip if the user starts typing again before the five-second display limit on the tooltip expires.
                void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
                {
                    toolTip.Hide(tbs);
                }
            }
            int y = _topAnchor;
            for (int i = 0; i < mtbs.Length; i++)
            {
                mtbs[i].Location = new Point(110, y);
                y += 30;
            }
            Controls.AddRange(mtbs);
        }

        public TabPanel GetPanel()
        { 
            return this;
        }    
    }
}
