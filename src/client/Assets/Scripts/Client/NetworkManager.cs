using extichu_messages;
using Fun;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkManager : MonoBehaviour
{
	public NetworkManager()
	{
		this.OnMessageReceived.Clear();
		this._temporalHandlers.Clear();

		foreach (MessageType eachMsgType in Enum.GetValues(typeof(MessageType)))
		{
			if (false == eachMsgType.ToString().StartsWith("sc_"))
				continue;
			this.OnMessageReceived.Add(eachMsgType, delegate { });
			this._temporalHandlers.Add(eachMsgType, delegate { });
		}
	}

	public static NetworkManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var existingInstance = GameObject.FindObjectOfType<NetworkManager>();
				if (existingInstance != null)
					_instance = existingInstance;
				else
					_instance = null; // error
			}
			return _instance;
		}
	}
	private static NetworkManager _instance = null;

	public abstract void Connect(string host);

	public abstract void SendMessage<TPacket>(MessageType packetType, TPacket packet);

	public void SendMessage<TPacket, TResPacket>
		(MessageType packetType, TPacket packet,
		MessageType replyType, Action<TResPacket> onReply)
		where TPacket : class where TResPacket : class
	{
		_temporalHandlers[replyType] 
			+= reply =>
			{
				var converted = reply as TResPacket;
				if (converted != null)
					onReply.Invoke(converted);
			};

		this.SendMessage(packetType, packet);
	}

	public abstract bool IsConnected { get; }

	public Dictionary<MessageType, Action<object>> OnMessageReceived { get; }
		= new Dictionary<MessageType, Action<object>>();

	protected Dictionary<MessageType, Action<object>> _temporalHandlers
		= new Dictionary<MessageType, Action<object>>();

	protected void notifyServerMessage(MessageType msgType, object msg)
	{
		this.OnMessageReceived[msgType].Invoke(msg);
		this._temporalHandlers[msgType].Invoke(msg);
		this._temporalHandlers[msgType] = delegate { };
	}
}