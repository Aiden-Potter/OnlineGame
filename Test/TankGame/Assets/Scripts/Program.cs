using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Sever_Unity
{
    class Program
    {
        static List<Client> clientlists = new List<Client>();
        /// <summary>
        /// 广播信息
        /// </summary>
        /// <param name="message"></param>
        public static void BroadcastMessage(string message)
        {
            var NotConnectClient = new List<Client>();//掉线线客户端集合
            foreach (var client in clientlists)
            {
                if (client.Connected())//判断是否在线
                {
                    client.SendMessage(message);
                }
                else
                {
                    NotConnectClient.Add(client);
                }
            }
            //将掉线的客户端从集合里移除
            foreach (var temp in NotConnectClient)
            {
                clientlists.Remove(temp);
            }
        }

        static void Main(string[] args)
        {
            Socket tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("192.168.0.102");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 7799);
            tcpServer.Bind(ipEndPoint);

            Console.WriteLine("服务器开启....");
            tcpServer.Listen(100);
            //循环，每连接一个客户端建立一个Client对象
            while (true)
            {
                Socket clientSocket = tcpServer.Accept();//暂停等待客户端连接，连接后执行后面的代码
                Client client = new Client(clientSocket);//连接后，客户端与服务器的操作封装到Client类中
                Console.WriteLine("一个客户端连接....");
                clientlists.Add(client);//放入集合
            }
        }
    }
}
