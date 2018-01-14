using extichu_messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkUtils
{
	public static CardInfo ToCardInfo(this PbCard me)
	{
		return new CardInfo
		{
			IsHidden = me.is_hidden,
			Number = me.numberSpecified ? me.number : null as int?,
			// NOTE(sorae): int값이 같아야 함
			Shape = me.shapeSpecified ? (CardInfo.ShapeType)me.shape : null as CardInfo.ShapeType?,
		};
	}
}
