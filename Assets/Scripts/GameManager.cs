using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class GameManager : MonoBehaviour {


	public GameObject battleCamera;

	public List<BasePokemon> allPokemon = new List<BasePokemon>();
	public List<PokemonMoves> allMoves = new List<PokemonMoves>();

	public Transform defencePodium;
	public Transform attackPodium;
	public GameObject emptyPoke;
	public Image opponentHealthbar;

	public BattleManager bm;

	public Text type;
	public Text PP;
	public Text pokemonName;
	public Text levelTextOppenent;
	public Text levelTextPlayer;
	public Text updateText;
	public Text OpponentPokemonName;


	[Header("Sprites")]
	public Sprite southSprite;
	public Sprite eastSprite;
	public Sprite westSprite;
	public Sprite northSprite;

	string LastScene;

	public GameObject player;
	public GameObject playerCamera;

	[Header("Audio")]
	public AudioClip battleMusic;
	public AudioClip townMusic;

	[Header("Menu")]
	public GameObject menu;


	private GameObject imageBetweenScenes;

	public GameObject playerThrowingPokeball;



	//loading phase
	public OwnedPokemon loadedPokemon;
	public BasePokemon basePokemon = new BasePokemon ();
	public GameObject pokemon;



	void Awake()
	{

		player = GameObject.FindGameObjectWithTag ("Player");
		playerCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		LastScene = PlayerPrefs.GetString ("LastScene", "");
		menu = player.transform.Find ("Main Camera/Menu").gameObject;

		switch (LastScene) 
		{
		case "PlayerHouseBottom":
			player.transform.position = new Vector2 (-9.5f, -22.5f);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			break;

		case "GarysHouse":
			player.transform.position = new Vector2 (5.5f, -22.5f);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			break;

		case "OakLab":
			player.transform.position = new Vector2 (4.5f, -32.5f);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			break;

		case "MainMenu":
			Load ();
			break;

		default:
		break;
			



		}

	}


	void OnLevelWasLoaded()
	{
		StartCoroutine (Wait (1.5f));
		Debug.Log (LastScene);
		imageBetweenScenes = player.transform.Find ("Main Camera/ImageBetweenScenes").gameObject;
	}

	IEnumerator Wait(float n)
	{

		switch (LastScene) {
		case "PlayerHouseBottom":
			yield return new WaitForSeconds (n);
			player.transform.position = new Vector3 (-9.5f, -22.5f, 0);
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			imageBetweenScenes.SetActive (false);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			break;

		case "GarysHouse":
			yield return new WaitForSeconds (n);
			player.transform.position = new Vector3 (5.5f, -22.5f, 0);
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			imageBetweenScenes.SetActive (false);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			break;

		case "OakLab":
			yield return new WaitForSeconds (n);
			player.transform.position = new Vector3 (4.5f, -32.5f, 0);
			player.GetComponent<PlayerMovement> ().isAllowedToMove = true;
			imageBetweenScenes.SetActive (false);
			player.GetComponent<SpriteRenderer> ().sprite = southSprite;
			break;


		default:
			break;

		}


	}
		


	// Use this for initialization
	void Start () {
		playerCamera.SetActive (true);
		battleCamera.SetActive (false);
	}
	
	// Update is called once per frame


	// TODO: create an in-game menu so the player can pause the game. This menu also should allow the player to save game state!
	void Update () {


		//Debug.Log (player.transform.position.x);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			menu.SetActive (true);
			player.GetComponent<PlayerMovement> ().isAllowedToMove = false;
		}


		
	}



	public void EnterBattle(Rarity rarity)
	{
		playerCamera.SetActive (false);
		battleCamera.SetActive (true);

		PlayMusic (battleMusic);

		BasePokemon battlePokemon = GetRandomPokemonFromList (GetPokemonByRarity (rarity));

		player.GetComponent<PlayerMovement> ().isAllowedToMove = false;



		GameObject dPoke = Instantiate (emptyPoke, defencePodium.transform.position, Quaternion.identity) as GameObject;
		dPoke.AddComponent<WildPokemonMoves> ();             //add WildPokemonMoves script that will automatically add moves to the the wild pokemon

		Vector3 pokeLocalPos = new Vector3 (0, 1, 0);

		dPoke.transform.parent = defencePodium;
		dPoke.transform.localPosition = pokeLocalPos;
		BasePokemon tempPoke = dPoke.AddComponent<BasePokemon> () as BasePokemon;
		tempPoke.AddMember (battlePokemon);

		dPoke.GetComponent<SpriteRenderer> ().sprite = battlePokemon.image;
		updateText.text ="A wild "+ dPoke.GetComponent<BasePokemon>().PName+ " appeared!";


		GameObject ownedPoke = Instantiate (emptyPoke, attackPodium.transform.position, Quaternion.identity) as GameObject;
		Vector3 OwnedPokeLocalPos = new Vector3 (0, 1, 0);

	


		ownedPoke.transform.parent = attackPodium;
		ownedPoke.transform.localPosition = OwnedPokeLocalPos;
		ownedPoke.transform.Rotate (0, 180, 0);
		BasePokemon ownedPokemon = ownedPoke.AddComponent<BasePokemon> () as BasePokemon;
		ownedPokemon.AddMember (player.GetComponent<Player> ().ownedPokemon [0].ownedPokemon);

		ownedPoke.GetComponent<SpriteRenderer> ().sprite = ownedPokemon.image;

	
		type.text = player.GetComponent<Player> ().ownedPokemon [0].moves [0].Category.ToString ();
		PP.text = player.GetComponent<Player> ().ownedPokemon [0].moves [0].PP.ToString();          //eventually move this all to BattleManager?
		pokemonName.text = player.GetComponent<Player>().ownedPokemon[0].NickName;
		levelTextPlayer.text = player.GetComponent<Player> ().ownedPokemon [0].level.ToString();

		ownedPoke.SetActive (false);
		//playerThrowingPokeball.GetComponent<PlayerThrowingBall> ().move = true;



		int WildPokemonLevel = Random.Range (2, 4);       //here we try to set a random level for our pokemon!    not maximally inclusive hence 4!!!!
		GameObject.Find ("DefencePodium/emptyPoke(Clone)").GetComponent<BasePokemon>().level = WildPokemonLevel;   // Here we try to set it in the script
		levelTextOppenent.text = GameObject.Find ("DefencePodium/emptyPoke(Clone)").GetComponent<BasePokemon>().level.ToString();
		OpponentPokemonName.text = GameObject.Find ("DefencePodium/emptyPoke(Clone)").GetComponent<BasePokemon> ().PName;

		opponentHealthbar.GetComponent<RectTransform> ().offsetMax = new Vector2 (-13, opponentHealthbar.GetComponent<RectTransform> ().offsetMax.y);

		bm.ChangeMenu (BattleManager.BattleMenu.Selection);
	}

	public List<BasePokemon> GetPokemonByRarity(Rarity rarity)
	{
		List<BasePokemon> returnPokemon = new List<BasePokemon> ();
		foreach (BasePokemon Pokemon in allPokemon) 
		{
			if (Pokemon.rarity == rarity)
				returnPokemon.Add (Pokemon);
		}

		return returnPokemon;
	}

	public BasePokemon GetRandomPokemonFromList (List<BasePokemon> pokeList)
	{
		BasePokemon poke = new BasePokemon ();
		int pokeIndex = Random.Range (0, pokeList.Count);        // it was originally (0,pokeList.Count - 1)
		poke = pokeList [pokeIndex];
		return poke;

	}

	void PlayMusic(AudioClip name)
	{
		GameObject.Find ("BgAudio").GetComponent<AudioSource> ().clip = name;
		GameObject.Find ("BgAudio").GetComponent<AudioSource> ().Play ();
	}


	public void Load()
	{
		
		if (File.Exists (Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			player.transform.position = new Vector2 (data.positionX, data.positionY);

			if (data.audio == "TownMusic") {
				PlayMusic (townMusic);
			}

			PlayerPrefs.SetString ("PlayerName", data.name);

			pokemon = Instantiate(Resources.Load ("Prefabs/Pokemon/Squirtle", typeof(GameObject))) as GameObject;
			Debug.Log(pokemon.name);
			pokemon.transform.parent = player.transform;
			pokemon.SetActive (false);


			loadedPokemon.level = data.level;
			loadedPokemon.NickName = data.nickName;
			loadedPokemon.moves.Add (data.moves);


		/*	basePokemon.canEvolve = data.basePokemonSerialized.canEvolve;
			basePokemon.attackStat = data.basePokemonSerialized.attackStat;
			basePokemon.biomeFound = data.basePokemonSerialized.biomeFound;
			basePokemon.defenceStat = data.basePokemonSerialized.defenceStat;
			basePokemon.HP = data.basePokemonSerialized.HP;
			basePokemon.level = data.basePokemonSerialized.level;
			basePokemon.maxHP = data.basePokemonSerialized.maxHP;
			basePokemon.PName = data.basePokemonSerialized.PName;
			basePokemon.pokemonStats = data.basePokemonSerialized.pokemonStats;
			basePokemon.rarity = data.basePokemonSerialized.rarity;
			basePokemon.speed = data.basePokemonSerialized.speed;
			basePokemon.type = data.basePokemonSerialized.type;
			basePokemon.image = Resources.Load (data.basePokemonSerialized.image) as Sprite;*/


			loadedPokemon.ownedPokemon = pokemon.GetComponent<BasePokemon> ();






			player.GetComponent<Player> ().ownedPokemon.Add (loadedPokemon);






			switch (data.sprite) {

			case "South_0":
				player.GetComponent<SpriteRenderer> ().sprite = southSprite;
				break;

			case "North_0":
				player.GetComponent<SpriteRenderer> ().sprite = northSprite;
				break;

			case "east_0":
				player.GetComponent<SpriteRenderer> ().sprite = eastSprite;
				break;

			case "West_0":
				player.GetComponent<SpriteRenderer> ().sprite = westSprite;
				break;

			default:
				break;


			}

		}
	}
     
}


[System.Serializable]
public class PlayerData
{
	public float positionX,positionY;
	public string audio;
	public string sprite;
	public string name;
	public List<OwnedPokemon> ownedPokemon;
	public string nickName;
	public int level;
	public PokemonMoves moves;
	public BasePokemonSerialized basePokemonSerialized;
}





[System.Serializable]
public class PokemonMoves
{

	public string Name;
	public MoveType Category;
	public Stat moveStat;
	public PokemonType moveType;
	public int currentPP;
	public int PP;
	public float power;
	public float accuracy;
}

[System.Serializable]
public class Stat
{

	public float minimum;
	public float maximum;

}

public enum MoveType
{
	Physical,
	Special,
	Status


}
