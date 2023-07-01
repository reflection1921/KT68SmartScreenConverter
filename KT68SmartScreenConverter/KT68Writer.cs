using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KT68SmartScreenConverter
{
    internal class KT68Writer
    {
        JObject kt68JObj;
        Bitmap m_bitmap;

        public KT68Writer()
        {
            GenerateKT68Json();
        }

        private void GenerateKT68Json()
        {
            kt68JObj = new JObject();
            kt68JObj["MACHENIKE"] = "KT 68 LED";


            JObject effectItem = new JObject();
            effectItem["brightness"] = 255;
            effectItem["speed"] = 90;
            effectItem["frames"] = new JArray();

            JArray effects = new JArray
            {
                effectItem
            };

            kt68JObj["led effects"] = effects;
        }

        public string ConvertFromBitmap(Bitmap bitmap)
        {
            if (bitmap.Width < 65)
            {
                return "";
            }

            if (bitmap.Height < 5)
            {
                return "";
            }

            m_bitmap = bitmap;

            JArray newFrames = new JArray();
            int i = 0;

            for (int y = 0; y < m_bitmap.Height - 5 + 1; y++)
            {
                if (y % 2 == 0) //even
                {
                    for (int x = 0; x < m_bitmap.Width - 65 + 1; x++)
                    {
                        i++;
                        JObject frameObj = new JObject();
                        frameObj["frameIndex"] = i + 1;
                        frameObj["frameRGB"] = GenerateFrame(x, y);
                        newFrames.Add(frameObj);
                    }
                }
                else //odd
                {
                    for (int x = m_bitmap.Width - 65; x >= 0; x--)
                    {
                        i++;
                        JObject frameObj = new JObject();
                        frameObj["frameIndex"] = i + 1;
                        frameObj["frameRGB"] = GenerateFrame(x, y);
                        newFrames.Add(frameObj);
                    }
                }
                
            }

            kt68JObj["led effects"][0]["frames"] = newFrames;

            return kt68JObj.ToString();
        }

        private JArray GenerateFrame(int srcx, int srcy)
        {
            JArray frame = new JArray();
            for (int y = srcy; y < srcy + 5; y++)
            {
                for (int x = srcx; x < srcx + 65; x++)
                {
                    frame.Add(m_bitmap.GetHexColor(x, y));
                }
            }

            return frame;
        }
    }
}
