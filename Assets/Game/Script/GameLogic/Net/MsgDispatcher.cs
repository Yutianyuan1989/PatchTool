using UnityEngine;
using System.Collections.Generic;

namespace WNet
{
	public class MsgDispatcher
	{

	    private IConnection _connection;

        public delegate void MsgCallback(down_msg downmsg);

        private Dictionary<msg_down_op_code, MsgCallback> _msgDispatch = new Dictionary<msg_down_op_code, MsgCallback>();

        public MsgDispatcher(IConnection f_connection)
	    {
	        _connection = f_connection;
	    }
	
        public void ResetConnection(IConnection f_connection)
        {
            _connection = f_connection;
        }

	    public void process()
	    {
            _connection.Tick(Time.fixedDeltaTime);

            byte[] luaMsgDatas = _connection.GetLuaMsg();
            while (luaMsgDatas != null)
            {
                //EntityAdminUtil.MyEntityAdmin.luaMgr.HandleMsg(luaMsgDatas);
                luaMsgDatas = _connection.GetLuaMsg();
            }

    //        down_msg msg = _connection.GetMsg();
    //        while (msg != null)
	   //     {
				//if (_msgDispatch.ContainsKey(msg.down_op_code))
				//{
				//	MsgCallback callback = _msgDispatch[msg.down_op_code];
				//	callback(msg);
				//}
    //            msg = _connection.GetMsg();
    //        }
        }

		public void RegistMsg(msg_down_op_code f_id, MsgCallback f_func)
		{
			if (_msgDispatch.ContainsKey(f_id))
				_msgDispatch[f_id] += f_func;
			else
				_msgDispatch.Add(f_id, f_func);
			_msgDispatch[f_id] = f_func;
		}

		public void UnregistMsg(msg_down_op_code f_id, MsgCallback f_func)
		{
			if (_msgDispatch.ContainsKey(f_id))
			{
				if (f_func == null)
				{
					_msgDispatch.Remove(f_id);
				}
				else
				{
					_msgDispatch[f_id] -= f_func;
				}
			}
			else
				Debug.LogWarning("Unregist unknow Message : " + f_id.ToString());
			if (_msgDispatch.ContainsKey(f_id))
			{
				_msgDispatch.Remove(f_id);
			}
		}
	}
}