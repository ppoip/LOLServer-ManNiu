using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class ByteArray
    {
        private MemoryStream memoryStream;
        private BinaryWriter bw;
        private BinaryReader br;

        public ByteArray()
        {
            memoryStream = new MemoryStream();
            bw = new BinaryWriter(memoryStream);
            br = new BinaryReader(memoryStream);
        }

        public ByteArray(byte[] data)
        {
            memoryStream = new MemoryStream(data);
            bw = new BinaryWriter(memoryStream);
            br = new BinaryReader(memoryStream);
        }

        public int Length
        {
            get
            {
                return (int)memoryStream.Length;
            }
        }

        public int Position
        {
            get
            {
                return (int)memoryStream.Position;
            }
        }

        public bool Readnable
        {
            get
            {
                return memoryStream.Position < memoryStream.Length;
            }
        }


        public void Write(byte value)
        {
            bw.Write(value);
        }

        public void Write(int value)
        {
            bw.Write(value);
        }

        public void Write(long value)
        {
            bw.Write(value);
        }

        public void Write(bool value)
        {
            bw.Write(value);
        }

        public void Write(float value)
        {
            bw.Write(value);
        }

        public void Write(double value)
        {
            bw.Write(value);
        }

        public void Write(string value)
        {
            bw.Write(value);
        }

        public void Write(byte[] value)
        {
            bw.Write(value);
        }



        public void Read(out byte value)
        {
            value = br.ReadByte();
        }

        public void Read(out int value)
        {
            value = br.ReadInt32();
        }

        public void Read(out long value)
        {
            value = br.ReadInt64();
        }

        public void Read(out bool value)
        {
            value = br.ReadBoolean();
        }

        public void Read(out float value)
        {
            value = br.ReadSingle();
        }

        public void Read(out double value)
        {
            value = br.ReadDouble();
        }

        public void Read(out string value)
        {
            value = br.ReadString();
        }

        public void Read(out byte[] value,int length)
        {
            value = br.ReadBytes(length);
        }

        public void RePosition()
        {
            memoryStream.Position = 0;
        }

        public void Close()
        {
            memoryStream.Close();
            bw.Close();
            br.Close();
        }

        public byte[] GetBuffer()
        {
            byte[] result = new byte[memoryStream.GetBuffer().Length];
            Buffer.BlockCopy(memoryStream.GetBuffer(), 0, result, 0, memoryStream.GetBuffer().Length);
            return result;
        }

    }
}
