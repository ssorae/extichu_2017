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
	private List<HandView> _handViews = null;
	public IReadOnlyList<HandView> HandViews { get { return _handViews; } }

	[SerializeField]
	private GameObject _joiningMessageObject = null;

	[SerializeField]
	private Button _readyButton = null;
	[SerializeField]
	private GameObject _readyButtonReadyObj = null;
	[SerializeField]
	private GameObject _readyButtonCancelObj = null;

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

		CardView.LoadResources();
	}

	public enum ReadyButtonMode
	{
		kReady = 0,
		kCancel = 1,
	}

	private ReadyButtonMode _readyButtonStatus = default(ReadyButtonMode);
	public ReadyButtonMode ReadyButtonStatus
	{
		get { return _readyButtonStatus; }
		set
		{
			_readyButtonStatus = value;
			_readyButtonReadyObj.SetActive(value == ReadyButtonMode.kReady);
			_readyButtonCancelObj.SetActive(value == ReadyButtonMode.kCancel);
		}
	}

	public void EnableSetReadyButton(Action<bool> setReady)
	{
		this.ReadyButtonStatus = ReadyButtonMode.kReady;

		_readyButton.onClick.AddListener(
			() => setReady(this.ReadyButtonStatus == ReadyButtonMode.kReady ? true : false));
	}

	public void ShowGameStart()
	{
		// TODO(sorae): impl..
	}

	public void DisableReadyButton()
	{
		// TODO(sorae): impl..
	}

	public IEnumerator WaitForNicknameInput(Func<string, bool> nicknameValidator, CoroutineResult<string> nickname)
	{
		var isOkButtonClicked = false;

		_nicknameInputPopup.OnOKButtonClicked
			+= input =>
			{
				nickname.Set(input);
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
