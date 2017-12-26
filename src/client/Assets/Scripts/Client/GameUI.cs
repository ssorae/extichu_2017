using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	[SerializeField]
	private Text _nicknameComponent = default(Text);

	// TODO(sorae): impl..
	public bool IsJoiningMsgEnabled { get; set; }

	public void Awake()
	{
		if (_nicknameComponent == null)
			Debug.LogError($"[{nameof(GameUI)}] {nameof(_nicknameComponent)} is null");
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

	public void DisableReadyButton()
	{
		// TODO(sorae): impl..
	}

	public IEnumerator WaitForNicknameInput(Func<string, bool> nicknameValidator)
	{
		while (false == nicknameValidator(_nicknameComponent.text))
			yield return null;
	}
}
