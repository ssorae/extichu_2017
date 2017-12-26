using System;
using Fun;
using UnityEngine;
using System.Collections.Generic;

public class ServerNetwork : NetworkManager
{
	private FunapiSession _session;

	public override bool IsConnected
	{
		get
		{
			return _session != null ? _session.Connected : false;
		}
	}

	public override void Connect(string host)
	{
		_session = FunapiSession.Create(host);
		_session.ReceivedMessageCallback += this.onMessageReceived;
		_session.SessionEventCallback += this.onSessionEvent;
		_session.TransportEventCallback += this.onTransportEvent;

		_session.Connect(TransportProtocol.kTcp, FunEncoding.kProtobuf, 8012);
	}

	public override void SendMessage<TPacket>(MessageType packetType, TPacket packet)
		=> _session.SendMessage(packetType, packet);

	private void onTransportEvent(TransportProtocol protocol, TransportEventType type)
	{
		// TODO(sorae): impl..
	}

	private void onSessionEvent(SessionEventType type, string session_id)
	{
		Debug.LogFormat("[{0}] onSessionEvent - <color=blue>{1}</color>", 
			nameof(NetworkManager), type);
	}

	private void onMessageReceived(string msg_type, object message)
	{
		MessageType msgType;
		if (!!!Enum.TryParse(msg_type, out msgType))
		{
			Debug.LogWarning("Unregistered msg type - " + msg_type);
			return;
		}

		var msg = FunapiMessage.GetMessage(
			message as funapi.network.fun_message.FunMessage, msgType);
		if (msg == null)
		{
			Debug.LogError("Cannot parse msg - " + msg + "as " + msgType);
			return;
		}

#if DEBUG
		Debug.LogFormat("[MsgDump] <color=blue>{0}</color> : {1}",
			msg_type, MiniJSON.Json.Serialize(msg));
#endif

		base.notifyServerMessage(msgType, msg);
	}
}