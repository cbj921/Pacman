using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot : MonoBehaviour {

	public bool isSuperDot = false;
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player")
		{
			if(isSuperDot)
			{
				// TODO: 强化pacman
				// 吃掉超级豆子
				GameControl.Instance.OnEatSuperDot(gameObject);
			}
			else
			{
				// 吃掉普通豆子
				GameControl.Instance.OnEatPacDot(gameObject);
			}
			gameObject.SetActive(false);
		}
	}
}
