using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	[SerializeField]
	private NicknameInputPopup _nicknameInputPopup = null;

	[SerializeField]
	private List<Text> _nametags = null;

	// TODO(sorae): impl..
	public bool IsJoiningMsgEnabled { get; set; }

	public bool IsNicknameInputEnabled
	{
		get
		{
			return _nicknameInputPopup.IsEnabled;
		}
		set
		{
			_nicknameInputPopup.IsEnabled = value;
		}
	}

	public void Awake()
	{
		if (_nicknameInputPopup == null)
			Debug.LogError($"[{nameof(GameUI)}] {nameof(_nicknameInputPopup)} is null");
	}

	public enum ReadyButtonMode
	{
		kReady = 0,
		kCancel = 1,
	}

	public ReadyButtonMode ReadyButtonStatus { get; set; }

	public void EnableSetReadyButton(Action<bool> setReady)
	{
		// TODO(sorae): bind setReady to ready button
	}

	public void ShowGameStart()
	{
		// TODO(sorae): impl..
	}

	public void DisableReadyButton()
	{
		// TODO(sorae): impl..
	}

	public IEnumerator WaitForNicknameInput(Func<string, bool> nicknameValidator)
	{
		var nickname = string.Empty;
		var isOkButtonClicked = false;

		_nicknameInputPopup.OnOKButtonClicked
			+= input =>
			{
				nickname = input;
				isOkButtonClicked = true;
			};

		while(true)
		{
			if (isOkButtonClicked && nicknameValidator(nickname))
				yield break;

			isOkButtonClicked = false;
			yield return null;
		}
	}
}
