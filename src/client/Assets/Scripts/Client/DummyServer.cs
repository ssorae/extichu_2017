using extichu_messages;
using Fun;
using System;
using UnityEngine;

public class DummyServer : MonoBehaviour
{
	public void OnMessageReceived(object msg)
	{
		handleMessage(msg as CSJoinMatch);
		handleMessage(msg as CSSetReady);
	}

	private void handleMessage(CSJoinMatch msg)
	{
		if (msg == null)
			return;

		var roomState = new SCJoinMatch
		{
			result = extichu_messages.ErrorCode.EC_OK,
			room_state = new PbRoomState
			{
				nickname_1 = "name1",
				nickname_2 = "name2",
				nickname_3 = msg.nickname,
			}
		};

		OnDummyServerMessage.Invoke(MessageType.sc_join_match, roomState);

		var player1Ready = new SCPlayerReady
		{
			client_index = 1,
			is_ready = true
		};

		OnDummyServerMessage.Invoke(MessageType.sc_player_ready, player1Ready);
	}

	private void handleMessage(CSSetReady msg)
	{
		if (msg == null)
			return;

		OnDummyServerMessage.Invoke(MessageType.sc_set_ready,
			new SCSetReady { result = extichu_messages.ErrorCode.EC_OK });
		OnDummyServerMessage.Invoke(MessageType.sc_player_ready, 
			new SCPlayerReady { client_index = 2, is_ready = msg.is_ready });
	}

	public event Action<MessageType, object> OnDummyServerMessage = delegate { };

	public static class Factory
	{
		public static DummyServer Create()
		{
			return new GameObject(nameof(DummyServer)).AddComponent<DummyServer>();
		}
	}
}