using UnityEngine;
using System.Collections;

public class ApplicationBehaviour : MonoBehaviour {

	public GameManager manager;
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			manager.Pause(true);
		}
	}


	public void QuitApplication()
	{
		Application.Quit();
	}


	public void ToggleFullscreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void ReloadScene()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
