using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour {

	// 播放完倒计时后调用
	public void CountEnd()
	{
		GameControl.Instance.StartGame();
	}
}
