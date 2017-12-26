using System;
using Fun;

public class DummyNetwork : NetworkManager
{
	public override bool IsConnected
	{
		get { return _targetServer != null; }
	}

	private DummyServer _targetServer = null;

	public override void Connect(string host)
	{
		_targetServer = DummyServer.Factory.Create();
		_targetServer.OnDummyServerMessage += base.notifyServerMessage;
	}

	public override void SendMessage<TPacket>(MessageType packetType, TPacket packet)
	{
		_targetServer?.OnMessageReceived(packet);
	}
}