using extichu_messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardView : MonoBehaviour
{
	private static IReadOnlyDictionary<Tuple<PbCard.Shape, PbCard.SpecialCardType>, Sprite> _sourceImages;

	public static void LoadResources()
	{
		if (_sourceImages != null)
		{
			Debug.Log("Source Images are already loaded");
			return;
		}
	}

	private PbCard _cardInfo;
	public PbCard CardInfo
	{
		get { return _cardInfo; }
		set
		{
			_cardInfo = value;
			this.refreshViewFromCardInfo();
		}
	}

	private void refreshViewFromCardInfo()
	{

	}

}