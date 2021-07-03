using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SerializeField]
	private int maxHealth = 100;
	
	private float fallLimit = 10f;
	
	[SyncVar]
	private int currentHealth;
	
	private PlayerController controller;
	
	public int GetHeathValue()              //----Send health value to ObjectPickup script for UI---//
	{
		return currentHealth;
	}
	
	
	[SyncVar]
	private bool _isDead = false;
	
	[SerializeField]
	private GameObject MediKit;
	
	[SerializeField]
	private GameObject CurrentWeapon;
	
	[SerializeField]
	private GameObject SecondaryWeapon;
	
	
	void Start()
	{
		CurrentWeapon.SetActive(true);
		CurrentWeapon.SetActive(false);
		controller = GetComponent<PlayerController>();
		SetDefaults();
		currentHealth = maxHealth;

		Instantiate(MediKit, MediKit.transform.position, MediKit.transform.rotation);   
			
	}
	
	void Update()
	{
		if(this.transform.position.y <= -fallLimit)     //---If it falls of the ground with no fuel it will die--//
		{
			RpcDie();
		}
		
		if(Input.GetKeyDown("1"))                      //---Weapon Swichting---//
		{
			CurrentWeapon.SetActive(true);
		    SecondaryWeapon.SetActive(false);
		}
		if(Input.GetKeyDown("2"))
		{
			SecondaryWeapon.SetActive(true);
		    CurrentWeapon.SetActive(false);
			
		}
		
	}

	
	[ClientRpc]
	public void RpcTakeDamage(int _amount)                                    //---Send the damage amount over network---//
	{
		
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();    //--Get Player netID--//
		
		string _playerID = "Player" + _netID;
		currentHealth -= _amount;                                            //--Damage the player's health by damage amount--//
		
		 Debug.Log(_playerID + " now has " + currentHealth +  "health");     //--Display the health after damage--//
		
		if(currentHealth <= 0)                                               //--Death if health is less than 0--//
		{
			_isDead = true;
			currentHealth = 0;
			RpcDie();
			
		}
		
	}
	
	[ClientRpc]
	private void RpcDie()                                                   
	{
		//Respawn Player
		
			StartCoroutine(ReSpawn());
	}
	
	IEnumerator ReSpawn()
	{
		yield return new WaitForSeconds(3f);
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();   //--Spawn the player randomly in any spawn point--//
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
		
		SetDefaults();
		
		
		Debug.Log( transform.name + "respawned");
	
	}
	
	
	public void SetDefaults()
	{
		_isDead = false;
			
		currentHealth = maxHealth;		
		
	}
	
	public void GetIncreasedHealth(int increasedHealthAmt)     //---Increase the health if medikit found--//
	{
		currentHealth += increasedHealthAmt;
	}
	
	
}
