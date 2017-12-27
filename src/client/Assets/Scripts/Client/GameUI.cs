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
	private List<NameTag> _nametags = null;

	public IReadOnlyList<NameTag> NameTags { get { return _nametags; } }

	[SerializeField]
	private GameObject _joiningMessageObject = null;
	
	public bool IsJoiningMsgEnabled
	{
		get { return _joiningMessageObject.activeSelf; }
		set { _joiningMessageObject.SetActive(value); }
	}

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

		this._nametags.ForEach(eachTag => eachTag.IsEnabled = false);
		this.IsJoiningMsgEnabled = false;
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
