using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System;

public class NetClient
{
    private TcpClient client;
    private NetworkStream tcpStream;
    private const int BUFFER_SIZE = 1024 * 64;
    private byte[] buffer = new byte[BUFFER_SIZE];
    private MemoryStream memoryStream;
    private BinaryReader binaryReader;
    public NetClient()
    {
        memoryStream = new MemoryStream();
        binaryReader = new BinaryReader(memoryStream);
    }
    public void OnConnectServer(string host,int port)
    {
        try
        {
            IPAddress[] addresses = Dns.GetHostAddresses(host);
            if (addresses.Length == 0)
            {
                Debug.LogError("host invalid");
                return;
            }
            if (addresses[0].AddressFamily == AddressFamily.InterNetworkV6)
                client = new TcpClient(AddressFamily.InterNetworkV6);
            else
                client = new TcpClient(AddressFamily.InterNetwork);
            client.SendTimeout = 1000;
            client.ReceiveTimeout = 1000;
            client.NoDelay = true;
            client.BeginConnect(host, port, OnConnent, null);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnConnent(IAsyncResult ar)
    {
        if(client==null || !client.Connected)
        {
            Debug.LogError("connect server error");
            return;
        }
        Manager.Net.OnNetConnected();
        tcpStream = client.GetStream();
        tcpStream.BeginRead(buffer, 0, BUFFER_SIZE, OnRead, null);
    }

    private void OnRead(IAsyncResult ar)
    {
        try
        {
            if(client==null || tcpStream == null)
            {
                return;
            }
            //收到的消息长度
            int length = tcpStream.EndRead(ar);
            if (length < 1)
            {
                OnDisConnected();
                return;
            }
            ReceiveData(length);
            lock (tcpStream)
            {
                Array.Clear(buffer, 0, buffer.Length);
                tcpStream.BeginRead(buffer, 0, BUFFER_SIZE, OnRead, null);
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            OnDisConnected();
        }
    }

    private void OnDisConnected()
    {
        if(client!=null && client.Connected)
        {
            client.Close();
            client = null;

            tcpStream.Close();
            tcpStream = null;
        }
        Manager.Net.OnNetConnected();
    }
    private void ReceiveData(int length)
    {
        memoryStream.Seek(0, SeekOrigin.End);
        memoryStream.Write(buffer, 0, length);
        memoryStream.Seek(0, SeekOrigin.Begin);
        while (RemainingBytesLength() > 8)
        {
            int msgId = binaryReader.ReadInt32();
            int msgLen = binaryReader.ReadInt32();
            if (RemainingBytesLength() >= msgLen)
            {
                byte[] data = binaryReader.ReadBytes(msgLen);
                string message = System.Text.Encoding.UTF8.GetString(data);
                //转到lua
                Manager.Net.Receive(msgId, message);
            }
            else
            {
                memoryStream.Position = memoryStream.Position - 8;
                break;
            }
        }
        //剩余字节
        byte[] leftover = binaryReader.ReadBytes(RemainingBytesLength());
        memoryStream.SetLength(0);
        memoryStream.Write(leftover, 0, leftover.Length);
    }

    private int RemainingBytesLength()
    {
        return (int)(memoryStream.Length - memoryStream.Position);
    }
    public void SendMessages(int msgId,string message)
    {
        using(MemoryStream ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter bw = new BinaryWriter(ms);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
            //协议Id
            bw.Write(msgId);
            bw.Write((int)data.Length);
            bw.Write(data);
            bw.Flush();

            if(client!=null && client.Connected)
            {
                byte[] sendData = ms.ToArray();
                tcpStream.BeginWrite(sendData, 0, sendData.Length, OnEndSend, null);
            }
            else
            {
                Debug.LogError("服务器未连接");
            }
        }
    }

    private void OnEndSend(IAsyncResult ar)
    {
        try
        {
            tcpStream.EndWrite(ar);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
