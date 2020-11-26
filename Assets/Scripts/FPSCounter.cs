using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	private int fps = 0;
	private int lastFps = 0;
	private float deltaTime = 0.016667f;

	private void Start()
	{
		StartCoroutine(CalculateFPS());
	}

	private void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	IEnumerator CalculateFPS()
	{
		while (true)
		{
			fps = Mathf.RoundToInt(Mathf.Lerp(lastFps, 1 / deltaTime, 0.5f));
			lastFps = fps;
			
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(4, 4, 150, 40), "Ping : " + PhotonNetwork.GetPing() + "ms\n" + fps + " FPS");
	}
}
