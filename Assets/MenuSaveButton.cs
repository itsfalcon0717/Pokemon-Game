using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

public class MenuSaveButton : MonoBehaviour {

	public Button saveButton;
	public GameObject player;


	void Awake(){
		player = GameObject.Find ("Player");
		saveButton = gameObject.GetComponent<Button> ();

	}

	void Start()
	{
		player.transform.Find ("Main Camera/Menu").gameObject.SetActive (false);
		saveButton.onClick.AddListener (TaskOnClick);
	}

	void TaskOnClick () {
		
		Save ();
		Debug.Log ("GAME SAVED");
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd");
		PlayerData data = new PlayerData ();

		data.positionX = player.transform.position.x;
		data.positionY = player.transform.position.y;
		data.audio = GameObject.Find ("BgAudio").gameObject.GetComponent<AudioSource> ().clip.name;
		data.sprite = player.GetComponent<SpriteRenderer> ().sprite.name;
		data.name = PlayerPrefs.GetString ("PlayerName","");
		data.nickName = player.GetComponent<Player> ().ownedPokemon [0].NickName;
		data.level = player.GetComponent<Player> ().ownedPokemon [0].level;
		data.moves = player.GetComponent<Player> ().ownedPokemon [0].moves [0];

		data.basePokemonSerialized = new BasePokemonSerialized ();

		data.basePokemonSerialized.biomeFound = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().biomeFound;
		data.basePokemonSerialized.canEvolve = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().canEvolve;
		data.basePokemonSerialized.defenceStat = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().defenceStat;
		data.basePokemonSerialized.attackStat = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().attackStat;
		//data.basePokemonSerialized.evolveTo = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().evolveTo;
		data.basePokemonSerialized.HP = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().HP;
		data.basePokemonSerialized.level = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().level;
		data.basePokemonSerialized.maxHP = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().maxHP;
		data.basePokemonSerialized.PName = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().PName;
		data.basePokemonSerialized.pokemonStats = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().pokemonStats;
		data.basePokemonSerialized.rarity = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().rarity;
		data.basePokemonSerialized.speed = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().speed;
		data.basePokemonSerialized.type = player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().type;
		data.basePokemonSerialized.image = AssetDatabase.GetAssetPath(player.transform.Find ("Squirtle(Clone)").GetComponent<BasePokemon> ().image);
		Debug.Log (data.basePokemonSerialized.image);


		bf.Serialize (file, data);
		file.Close ();
	}




}
