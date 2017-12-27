using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour
{
	[SerializeField]
	private GameObject _readyTag = null;

	[SerializeField]
	private Text _nicknameLabel = null;
	
	public bool IsReadyTagEnabled
	{
		get { return _readyTag.activeSelf; }
		set { _readyTag.SetActive(value); }
	}

	public bool IsEnabled
	{
		get { return this.gameObject.activeSelf; }
		set { this.gameObject.SetActive(value); }
	}

	public string Nickname
	{
		get { return _nicknameLabel.text; }
		set { _nicknameLabel.text = value; }
	}

	public void Awake()
	{
		this.IsReadyTagEnabled = false;
	}
}
