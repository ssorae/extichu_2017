using extichu_messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
	private const string RESOURCE_PATH = "Ingame/Cards/";
	private static IReadOnlyDictionary<CardInfo, Sprite> sourceImages;

	[SerializeField]
	private Image _cardImage = null;

	public static void LoadResources()
	{
		if (sourceImages != null)
		{
			Debug.Log("Source Images are already loaded. Keep going");
			return;
		}

		// load
		var cardImages = new Dictionary<CardInfo, Sprite>();

		Action<CardInfo, string> addResource = (cardInfo, filename) =>
		{
			var sprite = Resources.Load<Sprite>(RESOURCE_PATH + filename);
			if(sprite == null)
			{
				Debug.LogError("Cannot find asset - " + RESOURCE_PATH + filename);
				return;
			}
			cardImages.Add(cardInfo, Resources.Load<Sprite>(RESOURCE_PATH + filename));
		};
		addResource(CardInfo.Unknown, "Back");
		addResource(CardInfo.Dragon,  "Dragon");
		addResource(CardInfo.Phoenix, "Phoenix");
		addResource(CardInfo.Doggy,   "Doggy");
		addResource(CardInfo.Mahjong, "Mahjong");

		var allNumbers = Enumerable.Range(2, 13);
		var normalShapes = new CardInfo.ShapeType[]
		{
			CardInfo.ShapeType.k1,
			CardInfo.ShapeType.k2,
			CardInfo.ShapeType.k3,
			CardInfo.ShapeType.k4,
		};

		foreach (var eachNum in allNumbers)
		{
			foreach(var eachShape in normalShapes)
			{
				Func<int, string> numToChar = number =>
				{
					if (number <= 10)
						return number.ToString();
					else if (number == 11)
						return "J";
					else if (number == 12)
						return "Q";
					else if (number == 13)
						return "K";
					else
						return "A";
				};

				var cardInfo = new CardInfo {
					IsHidden = false, number = eachNum, Shape = eachShape };
				var filename = string.Format("{0}_{1}",
					(int)eachShape, numToChar(eachNum));

				addResource(cardInfo, filename);
			}
		}


		sourceImages = cardImages;
	}

	private CardInfo _info;
	public CardInfo CardData
	{
		get { return _info; }
		set
		{
			if (_info.Equals(value))
				return;
			_info = value;
			this.refreshViewFromCardInfo();
		}
	}

	private void refreshViewFromCardInfo()
		=> this._cardImage.sprite = sourceImages[this.CardData];

	public static class Factory
	{
		public static CardView Create(CardInfo cardInfo)
		{
			var comp = GameObject.Instantiate(
				Resources.Load<GameObject>("Ingame/Prefabs/CardView"))
				.GetComponent<CardView>();

			comp.CardData = cardInfo;

			return comp;
		}
	}
}