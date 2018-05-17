using UnityEngine;
using System.Collections;

namespace WNet
{
    public interface IConnection
    { 
        void SendMessage(byte[] inMsgContents);

        //down_msg GetMsg();
        byte[] GetLuaMsg();

		void Tick(float deltaTime);

        bool IsConnected();
    }
}