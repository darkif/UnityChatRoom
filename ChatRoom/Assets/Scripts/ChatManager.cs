using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System.Text;

public class ChatManager : MonoBehaviour {
    private Socket clientSocket;

    private Button btn;
    private InputField inputField;
    private Text showMessage;

    private byte[] data = new byte[1024];

    private string msg = "";

    void Start()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6688));

        btn = transform.Find("Button").GetComponent<Button>();
        btn.onClick.AddListener(OnSendClick);

        inputField = transform.Find("InputField").GetComponent<InputField>();
        showMessage = transform.Find("BG/Message").GetComponent<Text>();

        ClientStart();
    }

    void Update()
    {
        if(msg!=null && msg != "")
        {
            showMessage.text += msg+"\n";
            msg = "";
        }
    }


    private void OnSendClick()
    {
        if (inputField.text != "")
        {
            byte[] databytes = Encoding.UTF8.GetBytes(inputField.text);
            clientSocket.Send(databytes);
            inputField.text = "";
        }   
    }


    private void ClientStart()
    {
        clientSocket.BeginReceive(data, 0, 1024,SocketFlags.None, ReceiveCallBack, null);
    }

    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket.Connected == false)
            {
                clientSocket.Close();
                return;
            }
            int len = clientSocket.EndReceive(ar);
            string message = Encoding.UTF8.GetString(data, 0, len);
            //Debug.Log(message);
            msg = message;
            ClientStart();
        }
        catch(Exception e)
        {
            Debug.Log("ReceiveCallBack:" + e);
        }
        
    }

    void OnDestroy()
    {
        clientSocket.Close();
    }

}
