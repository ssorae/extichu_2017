using extichu_messages;
using Fun;
using System;
using UnityEngine;

public class DummyServer : MonoBehaviour
{
	public void OnMessageReceived(object msg)
	{
		handleMessage(msg as CSJoinMatch);
	}

	private void handleMessage(CSJoinMatch msg)
	{
		if (msg == null)
			return;

		OnDummyServerMessage.Invoke(MessageType.sc_join_match,
			new SCJoinMatch
			{
				result = extichu_messages.ErrorCode.EC_OK,
				room_state = new PbRoomState
				{
					nickname_1 = "�̸�1",
					nickname_2 = "�̸�2",
					nickname_3 = msg.nickname,
					nickname_4 = "�̸��̸�4���ڴ�"
				}
			});
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