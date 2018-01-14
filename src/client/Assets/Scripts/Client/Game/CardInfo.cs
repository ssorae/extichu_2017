using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardInfo
{
	public enum ShapeType
	{
		k1 = 1,
		k2 = 2,
		k3 = 3,
		k4 = 4,
		kDragon = 5,
		kPhoenix = 6,
		kDoggy = 7,
		kMahjong = 8,
	}

	public bool IsHidden;
	public int? number;
	public ShapeType? Shape;

	public static readonly CardInfo Unknown 
		= new CardInfo { IsHidden = true, number = null, Shape = null };
	public static readonly CardInfo Dragon
		 = new CardInfo { IsHidden = false, number = null, Shape = ShapeType.kDragon };
	public static readonly CardInfo Phoenix
		 = new CardInfo { IsHidden = false, number = null, Shape = ShapeType.kPhoenix};
	public static readonly CardInfo Doggy
		 = new CardInfo { IsHidden = false, number = null, Shape = ShapeType.kDoggy };
	public static readonly CardInfo Mahjong
		 = new CardInfo { IsHidden = false, number = null, Shape = ShapeType.kMahjong };
}