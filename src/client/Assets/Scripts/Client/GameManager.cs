using extichu_messages;
using Fun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameUI _ui = null;

	[SerializeField]
	private string _hostAddress = "127.0.0.1";

	private string _nickname = default(string);

	private bool _isGameStarted = false;

	public void Awake()
	{
		installHandlers();
	}

	private void installHandlers()
	{
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_game_start] += this.onSCGameStart;
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_player_joined] += this.onSCPlayerJoined;
	}

	private void onSCPlayerJoined(object source)
	{
		var packet = source as SCPlayerJoined;

		// TODO(sorae): set view
	}

	private void onSCGameStart(object source)
	{
		var packet = source as SCGameStart;

		_ui.ShowGameStart();
		this._isGameStarted = true;
	}

	public IEnumerator Start()
	{
		yield return this.mainRoutine();
	}

	private IEnumerator mainRoutine()
	{
		yield return waitForNicknameInput();

		yield return connectToServer();

		var isJoinSuccess = new CoroutineResult<bool>();
		yield return joinRoom(isJoinSuccess);
		if (false == isJoinSuccess)
		{
			// TODO(sorae): handle ex case..
		}

		_ui.EnableSetReadyButton(setReady);

		while (!!!_isGameStarted)
			yield return null;



	}

	private void setReady(bool isReady)
	{
		NetworkManager.Instance.SendMessage<CSSetReady, SCSetReady>(
			MessageType.cs_set_ready, new CSSetReady { is_ready = isReady },
			MessageType.sc_set_ready, reply =>
			{
				if (reply.result != extichu_messages.ErrorCode.EC_OK)
					Debug.LogError("SCSetReady.result != EC_OK");
				else
					_ui.ReadyButtonStatus = isReady ? GameUI.ReadyButtonMode.kCancel : GameUI.ReadyButtonMode.kReady;
			});
	}

	private IEnumerator waitForNicknameInput()
	{
		Func<string, bool> isValidNickname = source =>
			!!!string.IsNullOrEmpty(source) && source.Length >= 2 && source.Length <= 8;

		yield return _ui.WaitForNicknameInput(isValidNickname);
	}

	private IEnumerator joinRoom(CoroutineResult<bool> isSuccess)
	{
		SCJoinMatch response = null;

		_ui.IsJoiningMsgEnabled = true;

		NetworkManager.Instance.SendMessage<CSJoinMatch, SCJoinMatch>(
			MessageType.cs_join_match, new CSJoinMatch { nickname = _nickname },
			MessageType.sc_join_match, res => response = res);

		while (response == null)
			yield return null;

		if (response.result != extichu_messages.ErrorCode.EC_OK)
		{
			isSuccess.Set(false);
			yield break;
		}

		_ui.IsJoiningMsgEnabled = false;

		// TODO(sorae): set view from SCJoinMatch datas

	}

	private IEnumerator connectToServer()
	{
		NetworkManager.Instance.Connect(_hostAddress);

		while (!!!NetworkManager.Instance.IsConnected)
			yield return null;

		yield break;
	}

	private void onSessionEventReceived(SessionEventType type, string session_id)
	{
		// TODO(sorae): impl..
	}
}