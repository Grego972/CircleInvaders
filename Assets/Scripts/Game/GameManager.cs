using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum GameState
	{
		NotReady,
		Ready,
		Starting,
		Running,
		Ending,
		End,
		Pause
	}


	public GameBoard board;


	public GameObject playerPrefab;

	public UiCountDown uiCountDown;

	public float newGameDelay = 6f;

	public List<WeaponSettings> weaponList = new List<WeaponSettings>();

	[SerializeField]
	private float gameTime = 90f;

	private GameState _state = GameState.NotReady;

	private float currentPlayTime = 0;

	private List<AbstractPlayer> playerList = new List<AbstractPlayer>();

	public delegate void GameStateEventHandler(GameState s);

	public event GameStateEventHandler OnGameStateChange;


	void Start () {
		//Init
		State = GameState.Ready;
	}

	void OnDestroy()
	{
		//Restoring time scale juste in case
		Time.timeScale = 1f;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		GuizmoCircle(board.minLimit);
		GuizmoCircle(board.maxLimit);

	}

	private void GuizmoCircle(float radius, int slice = 100)
	{
		float inc = 360f/(float)slice;
		Vector3 p0 = new Vector3(1f,0f,0f);
		for(int i = 1 ; i<=slice; i++)
		{
			float a =Mathf.Deg2Rad*inc * (float) i;
			Vector3 p1 = new Vector3(Mathf.Cos(a),0f,Mathf.Sin(a));
			Gizmos.DrawLine(p0*radius, p1*radius);
			p0 = p1;
		}
	}
	
	// Update is called once per frame
	void Update () {


		if(State == GameState.Running)
		{
			UpdateRunning();
			return;
		}

		/*
		if(State == GameState.Starting)
		{
			InitNewGame();
			State = GameState.Running;
			return;
		}
		*/

		if(State == GameState.Ending)
		{
			State = GameState.End;
			return;
		}

	}

	void UpdateRunning()
	{
		currentPlayTime += Time.deltaTime;

		if(currentPlayTime > gameTime)
		{
			State = GameState.Ending;
		}
	}

	public GameState State
	{
		get
		{
			return _state;
		}
		private set
		{
			if(value != _state)
			{
				_state = value;

				if(OnGameStateChange != null)
					OnGameStateChange.Invoke(_state);
			}
		}
	}


	private void InitNewGame()
	{
		currentPlayTime = 0;
		ClearPlayerList();

		GameObject go = Instantiate(playerPrefab);
		Vector2 p = Random.insideUnitCircle* Random.Range(board.minLimit, board.maxLimit);
		go.transform.position = new Vector3(p.x, 0, p.y);
		AbstractPlayer player = go.GetComponent<AbstractPlayer>();
		player.Init(this);
		playerList.Add(player);

	}


	private void ClearPlayerList()
	{
		for(int i = 0; i < playerList.Count ; i++)
		{
			Destroy(playerList[i].gameObject);
		}
		playerList.Clear();
	}


	public void StartNewGameDelayed()
	{
		if(State == GameState.Ready || State == GameState.End)
		{
			State = GameState.Starting;
			ClearPlayerList();
			uiCountDown.StartCountDown(newGameDelay,"START !!!",StartNewGameNow,0.1f, 1f);
		}
	}

	public void Restart()
	{
		if(State == GameState.Pause)
		{
			Time.timeScale = 1;
			State = GameState.Starting;
			ClearPlayerList();
			uiCountDown.StartCountDown(newGameDelay,"START !!!",StartNewGameNow,0.1f, 1f);
		}
	}

	private void StartNewGameNow()
	{
		InitNewGame();
		State = GameState.Running;
	}

	public void Pause(bool s)
	{
		if( s  && State == GameState.Running)
		{
			Time.timeScale = 0;
			State = GameState.Pause;
		}
		else if(!s && State == GameState.Pause)
		{
			Time.timeScale = 1;
			State = GameState.Running;
		}
	}
		

	public float CurrentPlayTime
	{
		get
		{
			return currentPlayTime;
		}
	}

	public float RemainingPlayTime
	{
		get
		{
			return gameTime -currentPlayTime;
		}
	}

	public WeaponSettings GetWeapon(int idx)
	{
		return weaponList[ Mathf.Clamp(idx,0,weaponList.Count)];
	}
}
