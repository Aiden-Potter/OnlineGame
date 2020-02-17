using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Sever_Unity
{
    class Client
    {
        private Socket clientSocket;
        private byte[] data = new byte[1024];//存储数据

        public Client(Socket s)
        {
            clientSocket = s;
            //开启一个线程 处理客户端的数据接收
            Thread thread = new Thread(ReceiveMessage);
            thread.Start();
        }

        private void ReceiveMessage()
        {
            //服务器一直接收客户端数据
            while (true)
            {
                //如果客户端掉线，直接出循环
                if (clientSocket.Poll(10, SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    break;
                }
                //接收信息
                int length = clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);//utf-8转码
                //广播信息
                Program.BroadcastMessage(message);
                Console.WriteLine(message);
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
        }

        public bool Connected()
        {
            return clientSocket.Connected;
        }
    }
}
