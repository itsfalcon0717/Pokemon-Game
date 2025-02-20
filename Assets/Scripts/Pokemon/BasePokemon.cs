using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePokemon : MonoBehaviour
{

	public string PName;
	public Sprite image;
	public BiomeList biomeFound;
	public PokemonType type;
	public Rarity rarity;
	public int HP;
	public int maxHP; // was private
	public Stat attackStat;
	public Stat defenceStat;
	public float speed;

	public PokemonStats pokemonStats;

	public bool canEvolve;
	public PokemonEvolution evolveTo;


	public int level;  //was private


	// Use this for initialization
	void Start ()
	{
		maxHP = HP;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	public void AddMember (BasePokemon bp)
	{

		this.PName = bp.PName;
		this.image = bp.image;
		this.biomeFound = bp.biomeFound;
		this.type = bp.type;
		this.rarity = bp.rarity;
		this.HP = bp.HP;
		this.maxHP = bp.maxHP;
		this.attackStat = bp.attackStat;
		this.defenceStat = bp.defenceStat;
		this.pokemonStats = bp.pokemonStats;
		this.canEvolve = bp.canEvolve;
		this.level = bp.level;


	}

}


public enum Rarity
{
	veryCommon,
	Common,
	SemiRare,
	Rare,
	VeryRare
}

public enum PokemonType
{
	Flying,
	Ground,
	Rock,
	Steel,
	Fire,
	Water,
	Grass,
	Ice,
	Electric,
	Psychic,
	Dark,
	Dragon,
	Fighting,
	Normal


}


[System.Serializable]
public class PokemonEvolution
{

	public BasePokemon nextEvolution;
	public int levelUpLevel;


}

[System.Serializable]
public class PokemonStats
{
	public int attackStat;
	public int defenceStat;
	public int speedStat;
	public int spAttackStat;
	public int spDefenceStat;
	public int evasionStat;
}



//serialized base pokemon!!

[System.Serializable]
public class BasePokemonSerialized
{
	public string PName;
	public string image;
	public BiomeList biomeFound;
	public PokemonType type;
	public Rarity rarity;
	public int HP;
	public int maxHP; // was private
	public Stat attackStat;
	public Stat defenceStat;
	public float speed;

	public PokemonStats pokemonStats;

	public bool canEvolve;
	public PokemonEvolution evolveTo;


	public int level;  //was private

}


