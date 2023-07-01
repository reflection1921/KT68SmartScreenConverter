using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KT68SmartScreenConverter.KT68Converter;

namespace KT68SmartScreenConverter
{
    public partial class FrmConvImage : Form
    {
        Bitmap m_bitmap;
        public FrmConvImage()
        {
            InitializeComponent();
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bitmap;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Image File...";
                ofd.Filter = "Image File (*.jpg, *.png, *.bmp)|*.jpg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    bitmap = new System.Drawing.Bitmap(ofd.FileName);
                    m_bitmap = new Bitmap(ofd.FileName);
                    picPreview.Image = bitmap;
                    txtPath.Text = ofd.FileName;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save KT68 Pro JSON File...";
                sfd.Filter = "JSON File (*.json)|*.json";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    KT68Writer kT68Writer = new KT68Writer();
                    string json = kT68Writer.ConvertFromBitmap(m_bitmap);
                    File.WriteAllText(sfd.FileName, json);
                }
            }

            MessageBox.Show("File is successfully converted.", "KT68 SmartScreen Converter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
