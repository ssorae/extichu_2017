﻿using extichu_messages;
using Fun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private GameUI _ui = null;

	[SerializeField]
	private string _hostAddress = "127.0.0.1";

	private string _nickname = default(string);

	private bool _isGameStarted = false;

	private string _currentGameID = default(string);

	public void Awake()
	{
		installHandlers();
	}

	private void installHandlers()
	{
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_game_start] 
			+= this.onSCGameStart;
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_player_joined] 
			+= this.onSCPlayerJoined;
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_player_ready]
			+= this.onSCPlayerReady;
		NetworkManager.Instance.OnMessageReceived[MessageType.sc_handout]
			+= this.onSCHandout;
	}

	private void setNameTags(params string[] names)
	{
		_ui.NameTags[0].Nickname = names[0];
		_ui.NameTags[1].Nickname = names[1];
		_ui.NameTags[2].Nickname = names[2];
		_ui.NameTags[3].Nickname = names[3];

		_ui.NameTags[0].IsEnabled = true;
		_ui.NameTags[1].IsEnabled = true;
		_ui.NameTags[2].IsEnabled = true;
		_ui.NameTags[3].IsEnabled = true;
	}

	private void onSCPlayerJoined(object source)
	{
		var packet = source as SCPlayerJoined;

		_ui.NameTags[packet.player_id].Nickname = packet.nickname;
	}

	private void onSCPlayerReady(object source)
	{
		var packet = source as SCPlayerReady;

		_ui.NameTags[packet.client_index].IsReadyTagEnabled = packet.is_ready;
	}

	private void onSCGameStart(object source)
	{
		var packet = source as SCGameStart;

		this._currentGameID = packet.game_id;

		_ui.ShowGameStart();
		this._currentGameID = packet.game_id;
		this._isGameStarted = true;
	}

	private void onSCHandout(object source)
	{
		var packet = source as SCHandout;

		Action<int, IEnumerable<PbCard>> addCardViewToHand
			= (playerIndex, cardInfos) =>
				_ui.HandViews[playerIndex].AddCards(
					cardInfos.Select(cardInfo => CardView.Factory.Create(cardInfo.ToCardInfo())));

		addCardViewToHand(0, packet.cards_1);
		addCardViewToHand(1, packet.cards_2);
		addCardViewToHand(2, packet.cards_3);
		addCardViewToHand(3, packet.cards_4);
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

		yield return firstHandoutRoutine();

	}

	private IEnumerator firstHandoutRoutine()
	{
		SCHandout response = null;
		

		yield break;
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

		var nickname = new CoroutineResult<string>();
		yield return _ui.WaitForNicknameInput(isValidNickname, nickname);

		this._nickname = nickname;

		_ui.IsNicknameInputEnabled = false;
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
		this.setNameTags(
			response.room_state.nickname_1, 
			response.room_state.nickname_2,
			response.room_state.nickname_3,
			response.room_state.nickname_4);
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