using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KT68SmartScreenConverter
{
    internal class Bitmap
    {
        System.Drawing.Bitmap m_bitmap;
        BitmapData m_bitmapData;
        byte[] m_bitmapBytes;
        int m_imageSize;
        int m_bpp;

        public int Width { get { return m_bitmap.Width; } }
        public int Height { get { return m_bitmap.Height; } }
        public Bitmap(string filename)
        {
            m_bitmap = new System.Drawing.Bitmap(filename);
            Initialize();
        }

        private void Initialize()
        {
            m_bpp = System.Drawing.Bitmap.GetPixelFormatSize(m_bitmap.PixelFormat);
            _LockBits();
        }

        private void _LockBits()
        {
            m_bitmapData = m_bitmap.LockBits(new Rectangle(0, 0, m_bitmap.Width, m_bitmap.Height), ImageLockMode.ReadWrite, m_bitmap.PixelFormat);
            //isTopDown = (bitData.Stride > 0);  //TopDown: byte[0,0] = (0,0) BottomUp: byte[0,h] = (0,0)
            m_imageSize = Math.Abs(m_bitmapData.Stride) * m_bitmapData.Height;
            m_bitmapBytes = new byte[m_imageSize];
            System.Runtime.InteropServices.Marshal.Copy(m_bitmapData.Scan0, m_bitmapBytes, 0, m_imageSize);
        }

        private void _UnlockBits()
        {
            System.Runtime.InteropServices.Marshal.Copy(m_bitmapBytes, 0, m_bitmapData.Scan0, m_bitmapBytes.Length);
            m_bitmap.UnlockBits(m_bitmapData);
        }

        public Color GetPixel(int x, int y)
        {
            Color color;
            int pixelPos = m_bitmapData.Stride * y + (m_bpp / 8) * x;
            if (m_bpp == 8)
            {
                int data = m_bitmapBytes[pixelPos];
                color = Color.FromArgb(data, data, data);
            }
            else if (m_bpp == 24)
            {
                int b = m_bitmapBytes[pixelPos + 0];
                int g = m_bitmapBytes[pixelPos + 1];
                int r = m_bitmapBytes[pixelPos + 2];

                color = Color.FromArgb(r, g, b);
            }
            else if (m_bpp == 32)
            {
                int b = m_bitmapBytes[pixelPos + 0];
                int g = m_bitmapBytes[pixelPos + 1];
                int r = m_bitmapBytes[pixelPos + 2];

                color = Color.FromArgb(r, g, b);
            }
            else
            {
                throw new NotSupportedException();
            }

            return color;
        }

        public string GetHexColor(int x, int y)
        {
            Color _color = GetPixel(x, y);
            return "#" + _color.R.ToString("X2") + _color.G.ToString("X2") + _color.B.ToString("X2");
        }
    }
}
