using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    NetClient client;
    Queue<KeyValuePair<int,string>> MessageQueue=new Queue<KeyValuePair<int, string>>();
    XLua.LuaFunction ReceiveMessage;
    public void Init()
    {
        client = new NetClient();
        ReceiveMessage = Manager.Lua.LuaEnv.Global.Get<XLua.LuaFunction>("ReceiveMessage");
    }
    public void SendMessage(int messageId, string message)
    {
        client.SendMessages(messageId, message);
    }
    public void ConnectedServer(string ip,int port)
    {
        client.OnConnectServer(ip, port);
    }
    internal void OnNetConnected()
    {
        
    }
    internal void OnNetDisConnected()
    {

    }
    internal void Receive(int msgId, string message)
    {
        MessageQueue.Enqueue(new KeyValuePair<int, string>(msgId, message));
    }
    private void Update()
    {
        if(MessageQueue.Count > 0)
        {
            KeyValuePair<int,string> msg = MessageQueue.Dequeue();
            ReceiveMessage?.Call(msg.Key, msg.Value);
        }
    }
}
