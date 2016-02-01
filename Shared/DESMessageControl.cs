using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MineService_Shared
{
    public class DESMessageControl : IMessageControl
    {
        public string getMessage(Stream stream)
        {
            MemoryStream memstrm = new MemoryStream();

            byte[] Key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

            byte[] IV = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream csw = new CryptoStream(memstrm, tdes.CreateDecryptor(Key, IV),
                CryptoStreamMode.Write);

            byte[] data = new byte[4];
            int recv = stream.Read(data, 0, 4);
            int size = BitConverter.ToInt32(data, 0);
            data = new byte[size];
            int offset = 0;
            while (size > 0)
            {
                recv = stream.Read(data, 0, size);
                csw.Write(data, offset, recv);
                offset += recv;
                size -= recv;
            }
            csw.FlushFinalBlock();
            memstrm.Position = 0;
            byte[] info = memstrm.GetBuffer();
            int infosize = (int)memstrm.Length;
            csw.Close();
            memstrm.Close();
            return Encoding.ASCII.GetString(info, 0, infosize);
        }

        public void sendMessage(Stream stream, string message)
        {
            MemoryStream memstrm = new MemoryStream();

            byte[] Key = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

            byte[] IV = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                   0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream csw = new CryptoStream(memstrm, tdes.CreateEncryptor(Key, IV),
                      CryptoStreamMode.Write);

            csw.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            csw.FlushFinalBlock();

            byte[] cryptdata = memstrm.GetBuffer();
            int size = (int)memstrm.Length;
            byte[] bytesize = BitConverter.GetBytes(size);
            stream.Write(bytesize, 0, 4);
            stream.Write(cryptdata, 0, size);
            stream.Flush();
            csw.Close();
            memstrm.Close();
        }
    }
}
