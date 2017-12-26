using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	[SerializeField]
	private InputField _nicknameInput = default(InputField);

	[SerializeField]
	private List<Text> _nametags = null;

	// TODO(sorae): impl..
	public bool IsJoiningMsgEnabled { get; set; }

	public bool IsNicknameInputEnabled
	{
		get
		{
			return _nicknameInput.gameObject.activeSelf;
		}
		set
		{
			_nicknameInput.gameObject.SetActive(value);
		}
	}

	public void Awake()
	{
		if (_nicknameInput == null)
			Debug.LogError($"[{nameof(GameUI)}] {nameof(_nicknameInput)} is null");
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
		while (false == nicknameValidator(_nicknameInput.text))
			yield return null;
	}
}
