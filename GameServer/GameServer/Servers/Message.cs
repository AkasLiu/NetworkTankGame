using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameServer.Servers
{
    class Message
    {
        private byte[] data = new byte[1024];
        private int startIndex = 0;

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        public void ReadMessage(int newDataAmount,Action<int,byte[]> processDataCallback)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);  //总长度
                if (startIndex >= count)
                {
                    int protoId = BitConverter.ToInt32(data, 4);
                    byte[] stream = new byte[count];
                    Array.Copy(data, stream, count);
                    //对分析的结果进行处理 转发
                    processDataCallback(protoId, stream);
                    Array.Copy(data, count, data, 0, startIndex-count);
                    startIndex -= count ;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
