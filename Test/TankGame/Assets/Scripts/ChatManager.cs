using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{


    public string ipaddress = "192.168.137.1";//服务器ip
    public int port = 7799;
    public InputField MessageInput;
    public Text MessageText;

    private Socket clientSocket;
    private Thread thread;
    private byte[] data = new byte[1024];// 数据容器
    private string message = "";

    void Start()
    {
        
        ConnectToServer();
    }

    void Update()
    {
        //只有在主线程才能更新UI
        if (message != "" && message != null)
        {
            MessageText.text += "\n" + message;
            message = "";
        }
        Debug.Log(getLocalIP());
    }
    /**
     * 连接服务器端函数
     * */
    void ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //跟服务器连接
       // clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));
         IPAddress ipAddress = IPAddress.Parse("192.168.137.1");

         //Debug.Log(Dns.GetHostEntry(ipaddress).AddressList[4].ToString());
         IPEndPoint serverEndPoint = new IPEndPoint(ipAddress,port);
         clientSocket.Connect(serverEndPoint);
        //客户端开启线程接收数据
        thread = new Thread(ReceiveMessage);
        thread.Start();

    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)//客户端掉线了
            {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
            print(message);
        }

    }

    void SendMessage(string _message)
    {
        byte[] data = Encoding.UTF8.GetBytes(_message);
        clientSocket.Send(data);
    }

    public void OnSendButtonClick()
    {
  
        string value = System.DateTime.Now.Hour.ToString()+":"+System.DateTime.Now.Minute +" "+ MessageInput.text;
        SendMessage(value);
        MessageInput.text = " ";//更新inputField
    }
    /**
     * unity自带方法
     * 停止运行时会执行
     * */
    void OnDestroy()
    {
        //关闭连接，分接收功能和发送功能，both为两者均关闭
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }

    public string getLocalIP()
    {
        string strHostName = Dns.GetHostName(); //得到本机的主机名
        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP，
        string strAddr = ipEntry.AddressList[4].ToString();//取出ipv4地址  list的所有ip，包括ipv6、ipv4
        return (strAddr);
    }

}
