using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using System.Xml.Serialization;

namespace KT68SmartScreenConverter
{
    internal class KT68Converter
    {
        public enum AlignType { Left, Center, Right };
        JObject cbJObj;
        JObject kt68JObj;
        public KT68Converter(string cyberJsonData)
        {
            cbJObj = JObject.Parse(cyberJsonData);
            GenerateKT68Json();
        }

        public string Convert(AlignType align, string backgroundColor)
        {
            kt68JObj["led effects"][0]["frames"] = GetFrames(backgroundColor, align);
            File.WriteAllText("sss.json", kt68JObj.ToString());

            return kt68JObj.ToString();

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

        private JArray GetFrames(string color, AlignType align)
        {
            JObject effectItem = (JObject)cbJObj["lightEffects"][4]; //led screen
            JArray frames = (JArray)effectItem["frames"];
            JArray newFrames = new JArray();

            for (int i = 0; i < frames.Count; i++)
            {
                JArray frameRGB = (JArray)frames[i]["frameRGB"];

                JObject frameObj = new JObject();
                frameObj["frameIndex"] = i + 1;
                frameObj["frameRGB"] = ConvertFrameData(frameRGB, color, align);
                newFrames.Add(frameObj);
            }

            return newFrames;
        }

        private JArray ConvertFrameData(JArray frameRGB, string color, AlignType align)
        {
            string[,] newFrame = new string[65, 5];

            JArray newFrameJArr = new JArray();

            int alignIdx = 0;
            switch (align)
            {
                case AlignType.Center:
                    alignIdx = 13;
                    break;
                case AlignType.Left:
                    alignIdx = 0;
                    break;
                case AlignType.Right:
                    alignIdx = 25;
                    break;
            }

            for (int i = 0; i < frameRGB.Count; i++)
            {
                double num = (double)i / 5;
                int truncated = (int)Math.Truncate(num);

                int mod = i % 5;

                newFrame[truncated + alignIdx, mod] = (string)frameRGB[i];
            }


            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 65; x++)
                {
                    if (newFrame[x, y] == null || newFrame[x, y].Length == 0)
                    {
                        newFrameJArr.Add(color);
                        continue;
                    }
                    newFrameJArr.Add(newFrame[x, y]);
                }
            }

            return newFrameJArr;
        }


    }
}
