using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ObjectPickup : NetworkBehaviour
{
	[SerializeField]
	private GameObject WeaponName;
	
	private string LableOfWeapon;
	
	public static float DistancefromTarget;
	public float ToTarget;
	
	private WeaponInfo weapon;
	private Player player;
	private PlayerShooting shoot;
	private PlayerController controller;
	
	
	
	[SerializeField]
	private Slider HealthBar;
	private int HealthDamageAmt;
	
	[SerializeField]
	private Slider FuelBar;
	private float FuelAmount;
	
	[SerializeField]
	private GameObject BulletUI;
	private int BulletCount;
	private int MaxBullets;
	
	
	[SerializeField]
	private GameObject[] weaponsDisplay;
	

	
    // Start is called before the first frame update
    void Start()
    {   
       	HealthBar.minValue = 0;                                         //--HealthBar slider with value 0 - 100--//
		HealthBar.maxValue = 100;
		HealthBar.wholeNumbers = true;
		HealthBar.value = 100;
	
		
		FuelBar.minValue = 0;                                           //--FuleBar slider with value 0 - 1--//
		FuelBar.maxValue = 1;
		FuelBar.value = 1;
		
    }

	
	public void SetPlayer(Player _player)                                   //--Get the Player Object--//
	{
		player = _player;
	
		shoot = player.GetComponent<PlayerShooting>();                     //--Get the scripts attached to Player object--//
		weapon = player.GetComponent<WeaponInfo>();
		controller = player.GetComponent<PlayerController>();
	}
	
	
    // Update is called once per frame
    void Update()
    {
		GetName(PlayerShooting.GetTheObjectName());            //--Get the values of Static Methods from the scripts of player--//
		GetBullets(PlayerShooting.GetBulletsCount());
		GetMaxBullets(PlayerShooting.GetMaxBullets());
		
		SetHealthAmount(player.GetHeathValue());
		SetFuelAmount(PlayerController.GetFuelAmt());
		
	   //GetName(TempName);
		
        WeaponName.GetComponent<Text>().text = LableOfWeapon;                         //--Display the Object Name in UI--// 
		BulletUI.GetComponent<Text>().text = "" + BulletCount + "/" + MaxBullets;     //--Display the Bullets in UI--//
		
		FuelBar.value = FuelAmount;
		
		
		if(HealthBar.value <= 0)                               //--Set the Max Heath to 100 again in UI after respawning---//
		{
			ReBornWithMaxhealth();
			
		}

					
	}
	
	public void GetName(string _ObjectName)                  //---Set the value from player script to the local variables for UI--//
	{
		LableOfWeapon = _ObjectName; 
	}
	
	public void GetBullets(int _BulletsCount)
	{
		BulletCount = _BulletsCount;
	}
	
		
	public void GetMaxBullets(int _MaxBullets)
	{
		MaxBullets = _MaxBullets;
	}
	
	public void ReBornWithMaxhealth()
	{
		  
			HealthBar.value = 100;
	}
	
	public void SetHealthAmount(int _HealthDamageAmt)
	{
		HealthDamageAmt = _HealthDamageAmt;
		HealthBar.value = HealthDamageAmt;
		
		
	}
	
	public void SetFuelAmount(float _FuelAmt)
	{
		FuelAmount = _FuelAmt;
	    
	}
	
	public void GetNextWeapon()                         //---Show the equiped weapons in UI--//
	{
			weaponsDisplay[0].SetActive(false);
		    weaponsDisplay[1].SetActive(true);
		
		
	}
	
	public void GetPreviousWeapon()
	{
			weaponsDisplay[1].SetActive(false);
		    weaponsDisplay[0].SetActive(true);
	}
	
       
	 
    }
	
	

