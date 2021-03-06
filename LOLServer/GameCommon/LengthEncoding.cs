﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameCommon
{
    public class LengthEncoding
    {
        /// <summary>
        /// 长度编包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] encode(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            //写入长度
            bw.Write(buffer.Length);
            //写入body
            bw.Write(buffer);
            //Copy
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, result.Length);
            //Close
            ms.Close();
            bw.Close();

            return result;
        }

        /// <summary>
        /// 长度解包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] decode(ref List<byte> buffer)
        {
            if (buffer.Count < 4)
                return null;

            MemoryStream ms = new MemoryStream(buffer.ToArray());
            BinaryReader br = new BinaryReader(ms);
            int pack_length = br.ReadInt32();

            //长度不足
            if (ms.Length - ms.Position < pack_length)
                return null;

            //读取包体
            byte[] result = br.ReadBytes(pack_length);

            //Clear
            buffer.Clear();
            //写入剩余的部分
            buffer.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            //Close
            ms.Close();
            br.Close();

            return result;
        }
    }
}
