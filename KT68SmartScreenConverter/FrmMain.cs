using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KT68SmartScreenConverter.KT68Converter;

namespace KT68SmartScreenConverter
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            cbAlign.SelectedIndex = 1; //default: center
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Cyberboard R1 JSON File...";
                ofd.Filter = "JSON File (*.json)|*.json";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtJSONPath.Text = ofd.FileName;
                }
            }

            
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color _color = colorDialog.Color;
                    picColorPreview.BackColor = _color;
                    txtColor.Text = "#" + _color.R.ToString("X2") + _color.G.ToString("X2") + _color.B.ToString("X2");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbAlign.SelectedIndex == -1)
            {
                MessageBox.Show("Align type is not selected!");
                return;
            }

            if (!File.Exists(txtJSONPath.Text))
            {
                MessageBox.Show("JSON File is not selected!");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save KT68 Pro JSON File...";
                sfd.Filter = "JSON File (*.json)|*.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    KT68Converter kt68Converter = new KT68Converter(File.ReadAllText(txtJSONPath.Text));
                    string converted = kt68Converter.Convert((AlignType)cbAlign.SelectedIndex, txtColor.Text);
                    File.WriteAllText(sfd.FileName, converted);
                }
            }

            MessageBox.Show("File is successfully converted.", "KT68 SmartScreen Converter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
