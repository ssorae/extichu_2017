using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
	private List<CardView> _cardViews = null;

	[SerializeField]
	// Enable mouse interaction
	private bool _isInteractable = false;
}
