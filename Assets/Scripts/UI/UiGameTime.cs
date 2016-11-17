using UnityEngine;
using System.Collections;

public class UiGameTime : MonoBehaviour {
	public GameManager manager;

	public UnityEngine.UI.Text text;

	public float endingTime = 10f;
	public float fadeTime = .5f;
	public Color normalColor = Color.white;
	public Color endingColor = Color.red;



	private bool endState = false;

	private bool EndState
	{
		set
		{
			if(endState != value)
			{
				endState = value;
				text.CrossFadeColor(endState ? endingColor: normalColor,fadeTime,false, false);
			}
		}
	}


	void OnEnable()
	{
		text.canvasRenderer.SetColor(normalColor);
	}

	void Update()
	{
		if(manager.State == GameManager.GameState.Running)
		{
			float t = manager.RemainingPlayTime;
			EndState = t < endingTime;
			System.TimeSpan ts = System.TimeSpan.FromSeconds(t);

			if(endState)
			{
				text.text = ts.TotalSeconds.ToString("F2");
			}
			else
			{
				text.text = ts.Minutes.ToString("D2")+ ":"+ ts.Seconds.ToString("D2");
			}
		}
	}
}
