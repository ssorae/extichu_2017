using Fun;
using System;
using UnityEngine;

public class DummyServer : MonoBehaviour
{
	public void OnMessageReceived(object msg)
	{

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