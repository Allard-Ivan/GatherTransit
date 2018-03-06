using GatherTransit.Server.Model;
using MDL;
using socket.core.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GatherTransit.Server
{
    internal class Pack
    {
        internal TcpPackServer server;

        private const string AZ = "abcdefghigklmnopqrstuvwxyz";

        /// <summary>
        /// 设置基本配置
        /// </summary>   
        /// <param name="numConnections">同时处理的最大连接数</param>
        /// <param name="receiveBufferSize">用于每个套接字I/O操作的缓冲区大小(接收端)</param>
        /// <param name="overtime">超时时长,单位秒.(每10秒检查一次)，当值为0时，不设置超时</param>
        /// <param name="port">端口</param>
        /// <param name="headerFlag">包头</param>
        public Pack(int numConnections, int receiveBufferSize, int overtime, int port, uint headerFlag)
        {
            server = new TcpPackServer(numConnections, receiveBufferSize, overtime, headerFlag);
            server.OnAccept += Server_OnAccept;
            server.OnReceive += Server_OnReceive;
            server.OnSend += Server_OnSend;
            server.OnClose += Server_OnClose;
            server.Start(port);
        }

        private void Server_OnAccept(int obj)
        {
            Console.WriteLine($"Pack已连接{obj}");
        }

        private void Server_OnSend(int arg1, int arg2)
        {
            //Console.WriteLine($"Time:{DateTime.Now.ToString("mm:ss.fff")} 长度:{arg2}");
        }

        private void Server_OnReceive(int arg1, byte[] arg2)
        {
            string result = Encoding.UTF8.GetString(arg2);
            string[] receiveData = result.Split('_');
            if (receiveData.Length == 2)
            {
                if (receiveData[0] == "heart")
                {
                    byte[] data = Encoding.UTF8.GetBytes("heart_abcdefghigklmnopqrstuvwxyz");
                    server.Send(arg1, data, 0, data.Length);
                    return;
                }
            }
            using (var stream = new MemoryStream())
            {
                stream.Write(arg2, 0, arg2.Length);
                stream.Position = 0;
                var binFormatters = new BinaryFormatter();
                Dictionary<string, MDL.RealData> myObject = binFormatters.Deserialize(stream) as Dictionary<string, MDL.RealData>;
                stream.Flush();
                stream.Close();
                try
                {
                    RedisHelper.SetAll(myObject);
                }
                catch (Exception ex)
                {
                    //LogHelper.WriteLog("Redis 写值失败", ex, new StackTrace());
                    Console.WriteLine($"异常 {ex.Message}");
                }
            }
        }

        private void Server_OnClose(int obj)
        {
            //int aaa = server.GetAttached<int>(obj);
            Console.WriteLine($"Pack断开{obj}");
        }
    }
}
