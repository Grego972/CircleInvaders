using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiGameState : MonoBehaviour {

	[System.Serializable]
	public class UiGameStateItem
	{
		public List<GameManager.GameState> states = new List<GameManager.GameState>();
		public Transform pages;


		public void Process(GameManager.GameState s)
		{
			bool res = states.Exists((obj) => obj == s);
			pages.gameObject.SetActive(res);
		}
	}


	public GameManager manager;

	public List<UiGameStateItem> pages = new List<UiGameStateItem>();

	public void OnEnable()
	{
		manager.OnGameStateChange += Manager_OnGameStateChange;

		//Sync with current state
		Manager_OnGameStateChange(manager.State);
	}

	public void OnDisable()
	{
		manager.OnGameStateChange -= Manager_OnGameStateChange;
	}

	void Manager_OnGameStateChange (GameManager.GameState s)
	{
		pages.ForEach((obj) => obj.Process(s));
	}

}
