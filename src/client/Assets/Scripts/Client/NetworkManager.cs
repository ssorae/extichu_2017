using extichu_messages;
using Fun;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkManager : MonoBehaviour
{
	protected NetworkManager() { }

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

	public abstract void SendMessage<TPacket, TResPacket>
		(MessageType packetType, TPacket packet,
		MessageType replyType, Action<TResPacket> onReply)
		where TPacket : class where TResPacket : class;

	public abstract bool IsConnected { get; }

	public Dictionary<MessageType, Action<object>> OnMessageReceived { get; }
		= new Dictionary<MessageType, Action<object>>();
}