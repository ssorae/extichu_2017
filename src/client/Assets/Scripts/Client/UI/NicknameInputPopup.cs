using System;
using UnityEngine;
using UnityEngine.UI;

public class NicknameInputPopup : MonoBehaviour
{
	[SerializeField]
	private InputField _inputField = null;
	[SerializeField]
	private Button _okButton = null;

	public event Action<string> OnOKButtonClicked = delegate { };

	public bool IsEnabled
	{
		get { return this.gameObject.activeSelf; }
		set { this.gameObject.SetActive(value); }
	}

	public void Awake()
	{
		if (_inputField == null || _okButton == null)
			Debug.LogWarning("Null field detected");

		_okButton.onClick.AddListener(() => OnOKButtonClicked.Invoke(_inputField.text));
	}


}