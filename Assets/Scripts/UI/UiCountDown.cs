using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiCountDown : MonoBehaviour {
	public Text text;


	public void StartCountDown(float t, string endMessage = "", System.Action onFinish = null, float fadeIn = 0.1f, float fadeOut = 0.1f)
	{
		
		StartCoroutine(StartCountDownCoroutine(t, endMessage , onFinish ,  fadeIn ,  fadeOut) );
	}

	private IEnumerator StartCountDownCoroutine(float t, string endMessage = "", System.Action onFinish = null, float fadeIn = 0.1f, float fadeOut = 0.1f)
	{

		text.gameObject.SetActive(true);
		text.text = Mathf.FloorToInt(t).ToString();
		if(fadeIn != 0)
		{
			text.canvasRenderer.SetAlpha(0);
			text.CrossFadeAlpha(1f,fadeIn,false);
			//yield return new WaitForSeconds(fadeIn);
		}
		else
		{
			text.canvasRenderer.SetAlpha(1f);
		}


		while(t > 0)
		{
			text.text = Mathf.FloorToInt(t).ToString();
			yield return null;
			t -= Time.deltaTime;
		}

		text.text = "0";

		if(onFinish != null)
		{
			onFinish.Invoke();
		}

		if(!string.IsNullOrEmpty(endMessage))
		{
			text.text = endMessage;
			text.CrossFadeAlpha(0,fadeOut, false);
			yield return new WaitForSeconds(fadeOut);
		}
		text.gameObject.SetActive(false);


	}
}
