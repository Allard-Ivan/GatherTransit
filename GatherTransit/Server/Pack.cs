using GatherTransit.Server.Model;
using GatherTransit.Utils;
using Newtonsoft.Json;
using socket.core.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace GatherTransit.Server
{
    internal class Pack
    {
        internal TcpPackServer server;

        private const string AZ = "abcdefghigklmnopqrstuvwxyz";

        private const string HEART = "heart";

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
                if (receiveData[0] == HEART)
                {
                    byte[] data = Encoding.UTF8.GetBytes(HEART + "_" + AZ);
                    server.Send(arg1, data, 0, data.Length);
                    return;
                }
            }
            string[] receiveDataZmSpecial = result.Split("|||");
            if (receiveDataZmSpecial.Length == 2)
            {
                result = receiveDataZmSpecial[0];
            }
            try
            {
                Dictionary<string, RealData> myObject = JsonToDictionary<RealData>(result);
                RedisHelper.SetAll(myObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常 {ex.Message}");
            }
         }

        private void Server_OnClose(int obj)
        {
            //int aaa = server.GetAttached<int>(obj);
            Console.WriteLine($"Pack断开{obj}");
        }

        public Dictionary<string, T> JsonToDictionary<T>(string jsonData)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonData);
            }
            catch (Exception ex)
            {
                return new Dictionary<string, T>();
            }
        }
    }
}
