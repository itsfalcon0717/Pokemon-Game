﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	public BattleMenu currentMenu;
	public Player player;
	public GameObject PlayerThrowingBall;

	private bool DefencePokemonAttacked;




	[Header("Cameras")]
	public GameObject battleCamera;
	public GameObject mainCamera;

	[Header("Selection")]
	public GameObject SelectionMenu;
	public GameObject SelectionInfo;
	public Text selectionInfoText;
	public Text fight;
	private string fightT;
	public Text bag;
	private string bagT;
	public Text pokemon;
	private string pokemonT;
	public Text run;
	private string runT;



	[Header("Moves")]
	public GameObject movesMenu;
	public GameObject movesDetails;
	public Text PP;
	public Text PType;
	public Text moveO;
	private string moveOT;
	public Text moveT;
	private  string moveTT;
	public Text moveTH;
	private string moveTHT;
	public Text moveF;
	private string moveFT;

	[Header("Info")]
	public GameObject InfoMenu;
	public Text infoText;

	[Header("Misc")]
	public int currentSelection;

	[Header("Health Bars")]
	public Image opponentHealthBar;
	public Image playerHealthBar;
	public Text playerHealth;


	[Header("PP")]
	public Text PPMax;
	public Text currentPP;


	[Header("BattleUpdate")]
	public Text battleMessageText;
	public GameObject battleMessagePanel;

	[Header("Audio")]
	public AudioClip routeMusic;


	void Awake()
	{
		player = FindObjectOfType<Player>();
		mainCamera = player.transform.Find ("Main Camera").gameObject;

	}


	// Use this for initialization
	void Start () {
		fightT = fight.text;
		bagT = bag.text;
		pokemonT = pokemon.text;
		runT = run.text;
		moveOT = moveO.text;
		moveTT = moveT.text;
		moveTHT = moveTH.text;
		moveFT = moveF.text;
	}

	void OnLevelWasLoaded()
	{
		DefencePokemonAttacked = false;
	}
	
	// Update is called once per frame
	void Update () {


		playerHealth.text = player.ownedPokemon [0].ownedPokemon.HP + "/" + player.ownedPokemon [0].ownedPokemon.maxHP;



		if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			if (currentSelection < 4){
				currentSelection++;
			}
		}

		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			
			if (currentSelection > 0) {
				currentSelection--;
			}
		}

		if (currentSelection == 0) {
			currentSelection = 1; 
		}

		if (currentSelection == 1 && Input.GetKeyDown (KeyCode.Return) && currentMenu == BattleMenu.Selection)     //if fight option is selected
		{
			if (PlayerThrowingBall.GetComponent<PlayerThrowingBall> ().move == false && PlayerThrowingBall.GetComponent<RectTransform> ().offsetMax.x >= -3) {
				PlayerThrowingBall.GetComponent<PlayerThrowingBall> ().move = true;
			} else {
				ChangeMenu (BattleMenu.Fight);
			}
		}

		if (currentSelection == 1 && Input.GetKeyDown (KeyCode.Return) && currentMenu == BattleMenu.Fight) 
		{
			BattleMove (player.ownedPokemon[0].moves [0], "AttackPokemon");

		}



		if (currentSelection == 4 && Input.GetKeyDown (KeyCode.Return) && currentMenu == BattleMenu.Selection)              // if run option is selected
		{
			Debug.Log ("RUN SELECTED");
			Run ();
		}

		if (currentMenu == BattleMenu.Fight && Input.GetKeyDown (KeyCode.Backspace)) 
		{
			ChangeMenu (BattleMenu.Selection);
		}

		if (currentMenu == BattleMenu.BattleInfo && Input.GetKeyDown (KeyCode.Return)) 
		{
			if (DefencePokemonAttacked == false) {
				BattleMove (GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<WildPokemonMoves> ().wildMoves.moves [0], "DefencePokemon");
				DefencePokemonAttacked = true;
			} else {
				ChangeMenu (BattleMenu.Fight);
			}
		}


		switch (currentMenu) 
		{
		case BattleMenu.Fight:
			switch (currentSelection) 
			{
			case 1:
				moveO.text = "> " + player.ownedPokemon [0].moves [0].Name;
				moveT.text = moveTT;
				moveTH.text = moveTHT;
				moveF.text = moveFT;
				currentPP.text = player.ownedPokemon [0].moves [0].currentPP.ToString();   //displays current PP of move
				break;

			case 2:
				moveO.text =  player.ownedPokemon[0].moves[0].Name;
				moveT.text = "> " +moveTT;
				moveTH.text =  moveTHT;
				moveF.text = moveFT;
				break;

			case 3:
				moveO.text =  player.ownedPokemon[0].moves[0].Name;
				moveT.text =  moveTT;
				moveTH.text = "> " + moveTHT;
				moveF.text = moveFT;
				break;

			case 4:
				moveO.text =  player.ownedPokemon[0].moves[0].Name;
				moveT.text =  moveTT;
				moveTH.text =  moveTHT;
				moveF.text = "> " +moveFT;
				break;

			}
			break;


		case BattleMenu.Selection:
			
			switch (currentSelection) 
			{
			case 1:
				fight.text = "> " + fightT;
				bag.text = bagT;
				pokemon.text = pokemonT;
				run.text = runT;
				break;

			case 2:
				fight.text = fightT;
				bag.text ="> " + bagT;
				pokemon.text = pokemonT;
				run.text = runT;
				break;

			case 3:
				fight.text = fightT;
				bag.text = bagT;
				pokemon.text = "> " + pokemonT;
				run.text = runT;
				break;

			case 4:
				fight.text =  fightT;
				bag.text = bagT;
				pokemon.text = pokemonT;
				run.text = "> " +runT;
				break;

			}
			break;

		}

	
	}


	public void Run()                                                           //run function called when player wants to escape battle/ battle ends!
	{

		battleCamera.SetActive (false);
		mainCamera.SetActive (true);
		player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
		Destroy (GameObject.Find ("DefencePodium/emptyPoke(Clone)"));
		Destroy (GameObject.Find ("AttackPodium/emptyPoke(Clone)"));
		PlayMusic (routeMusic);
		PlayerThrowingBall.GetComponent<RectTransform> ().localPosition = new Vector2(-1,1.256f);


	}


	void PlayMusic(AudioClip name)
	{
		GameObject.Find ("BgAudio").GetComponent<AudioSource> ().clip = name;
		GameObject.Find ("BgAudio").GetComponent<AudioSource> ().Play ();
	}


	public void BattleMove (PokemonMoves moveSelected, string side)                       // the move selected gets processed and action occurs
	{

		if (side == "AttackPokemon") {

			int damageAttackPokemon = 0;
			int defencePokemonMaxHp = GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().maxHP;
			int attackPokemonLevel = player.ownedPokemon [0].level;

			float power = moveSelected.power;
			int attackStatAttackPokemon = player.ownedPokemon [0].ownedPokemon.pokemonStats.attackStat;
			int defenceStatDefencePokemon = GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().pokemonStats.defenceStat;
	
			damageAttackPokemon = Mathf.FloorToInt (((((((2 * attackPokemonLevel) / 5) + 2) * power * ((float)attackStatAttackPokemon / defenceStatDefencePokemon)) / 50) + 2) * 1);

			GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().HP -= damageAttackPokemon;   //reduces opponent damage
			float percentDamage = (((float)damageAttackPokemon / defencePokemonMaxHp) * 100);
			float opponentBarValue = -opponentHealthBar.GetComponent<RectTransform> ().offsetMax.x;    //offest sets value to negative!!
			float newValue = (((((100 + percentDamage) / 100) * (203)) - 203) + opponentBarValue);      
			opponentHealthBar.GetComponent<RectTransform> ().offsetMax = new Vector2 (-newValue, opponentHealthBar.GetComponent<RectTransform> ().offsetMax.y);

			player.ownedPokemon [0].moves [0].currentPP = player.ownedPokemon [0].moves [0].currentPP - 1;
			currentPP.text = player.ownedPokemon [0].moves [0].currentPP.ToString ();

			battleMessageText.text = player.ownedPokemon [0].ownedPokemon.PName + " used " + moveSelected.Name;
			StartCoroutine (Wait (0.01f));

			DefencePokemonAttacked = false;

		} else if (side == "DefencePokemon") {
			
			int damageDefencePokemon = 0;
			int defencePokemonLevel = GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().level;
			int attackPokemonHP = player.ownedPokemon [0].ownedPokemon.HP;
			int attackPokemonMaxHp = player.ownedPokemon [0].ownedPokemon.maxHP;

			float power = moveSelected.power;
			int attackStatDefencePokemon = GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().pokemonStats.attackStat;
			int defenceStatAttackPokemon = player.ownedPokemon [0].ownedPokemon.pokemonStats.defenceStat;

			damageDefencePokemon = Mathf.FloorToInt (((((((2 * defencePokemonLevel) / 5) + 2) * power * ((float)attackStatDefencePokemon / defenceStatAttackPokemon)) / 50) + 2) * 1);

			player.ownedPokemon [0].ownedPokemon.HP -= damageDefencePokemon;

			float percentDamage = (((float)damageDefencePokemon / attackPokemonMaxHp) * 100);
			float playerBarValue = -playerHealthBar.GetComponent<RectTransform> ().offsetMax.x;
			float newValue = (((((100 + percentDamage) / 100) * (203)) - 203) + playerBarValue); 
			playerHealthBar.GetComponent<RectTransform> ().offsetMax = new Vector2 (-newValue, playerHealthBar.GetComponent<RectTransform> ().offsetMax.y);

			battleMessageText.text = GameObject.Find ("emptyPoke(Clone)").gameObject.GetComponent<BasePokemon> ().PName + " used "+ moveSelected.Name;
			StartCoroutine (Wait (0.01f));
		}

	}




	IEnumerator Wait(float time)
	{
		yield return new  WaitForSeconds(time);
		ChangeMenu (BattleMenu.BattleInfo);
	}


	public void ChangeMenu(BattleMenu m)
	{	
		currentMenu = m;
		currentSelection = 0;
		switch (m) 
		{
		case BattleMenu.Selection:
			SelectionMenu.gameObject.SetActive (true);
			SelectionInfo.gameObject.SetActive (true);
			movesMenu.gameObject.SetActive (false);
			movesDetails.gameObject.SetActive (false);
			InfoMenu.gameObject.SetActive (false);
			break;

		case BattleMenu.Fight: 
			SelectionMenu.gameObject.SetActive (false);
			SelectionInfo.gameObject.SetActive (false);
			movesMenu.gameObject.SetActive (true);
			movesDetails.gameObject.SetActive (true);
			InfoMenu.gameObject.SetActive (false);
			battleMessagePanel.SetActive (false);
			break;

		case BattleMenu.Info: 
			SelectionMenu.gameObject.SetActive (false);
			SelectionInfo.gameObject.SetActive (false);
			movesMenu.gameObject.SetActive (false);
			movesDetails.gameObject.SetActive (false);
			InfoMenu.gameObject.SetActive (true);
			break;

		case BattleMenu.BattleInfo:
			movesMenu.SetActive (false);
			battleMessagePanel.SetActive (true);
			break;

		/*case BattleMenu.Selection: 

			break;

		case BattleMenu.Selection: 

			break;*/

		}

	}

	public enum BattleMenu
	{
		Selection,
		Pokemon,
		Bag,
		Fight,
		Info,
		BattleInfo


	}
}
