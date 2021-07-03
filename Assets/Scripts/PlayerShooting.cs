using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
	private const string PLAYER_TAG = "Enemy";
	private const string WEAPON_TAG = "Weapon";
	private const string MEDIKIT_TAG = "MediKit";
	
	[SerializeField]
	private WeaponInfo weapon;
	
	
	private ObjectPickup Pickup;
	
	[SerializeField]
	private Camera cam;
	
	[SerializeField]
	private LayerMask mask;
	
	[SerializeField]
	private LayerMask Environmentmask;
	
	[SerializeField]
	 private GameObject weaponGFX;
	
	[SerializeField]
	private string weaponLayerName = "Weapon";
	
	public Player _player;
	public static float DistancefromTarget;
	public float ToTarget;
	
	public static string ObjectName;
	
	[SerializeField]
	private GameObject WeaponHolderPoint;
	
	public static int HealthBarAmount;
	
	[SerializeField]
	private ParticleSystem MuzzleFlash;
	
	
	public static int BulletCount;
	public static int ClipSize;
	public static int MaxBullets;
	public AudioSource FireSound;
	public AudioSource BulletsEmpty;
	

	
	public static string GetTheObjectName()             //---Send the Name of the gameObject to ObjectPickup script for UI---//
	{
		return ObjectName;
	}
	
	public static int GetBulletsCount()               //--Send the bullet count to ObjectPickup script for UI--//
	{
		return BulletCount;
	}
	
	public static int GetMaxBullets()                //--Send the Max Bullet count to ObjectPickup script for UI--//
	{
		return MaxBullets;
	}
	
	
	public void SetObjectPickuping(ObjectPickup _Pickup)  //--Get the ObjectPickup Script--//

	{
		Pickup = _Pickup;
	}
	
    // Start is called before the first frame update
    void Start()
    {
		FireSound.enabled = false;
        BulletsEmpty.enabled = false;
		
        if(cam == null)
		{
			Debug.Log("PlayerShooting : Camera is missing");	
		}
		_player = GetComponent<Player>();
		
		weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);     //--Set the layer of weapon to Weapon--//
		
		BulletCount = weapon.bullets;                                 //--Get the variables from WeaponInfo class--//
		ClipSize = weapon.clipSize;
		MaxBullets = weapon.maxBullets;
    }
	
	[ClientRpc]
	void RpcDoShootingEffect()
	{
		MuzzleFlash.Play();
		
	}

    // Update is called once per frame
    void Update()
    {
		
		if(Input.GetButtonDown("Fire1"))                      //---Shoot when Left mouse click---//
		{

			BulletCount--;
			
			
			if(BulletCount > 0)
			{
				FireSound.enabled = true;
				FireSound.Play();
				MuzzleFlash.Play();	
				RpcDoShootingEffect();
			    Shoot();
			}
			else
			{
				BulletCount = 0;
				
				NoBullets();
				
			}
			
			if(MaxBullets < 20)                                  //--Increase bullets if it is less than 20--//
		{
			StartCoroutine(IncreaseBullets());
			
		}
				
		}
		
     
		RaycastHit Hit;
		if(Physics.Raycast(WeaponHolderPoint.transform.position, WeaponHolderPoint.transform.TransformDirection(Vector3.forward), out Hit, 10f, Environmentmask)) //--shoot the raycast from WeaponHolderPoint in forward dir within 10f if hits anything //
		{
			    ToTarget = Hit.distance;
			     DistancefromTarget = ToTarget;
				
				if(DistancefromTarget <= 2)
				{
					ObjectName = Hit.collider.name;                 //--Get the name of gameObject to diplay in UI--//
					if(Input.GetKeyDown("z")  && Hit.collider.tag == WEAPON_TAG)
					{
						 Hit.collider.gameObject.SetActive(false);
					}
					
					if(Input.GetKeyDown("z") && Hit.collider.tag == MEDIKIT_TAG )
					{
						Hit.collider.gameObject.SetActive(false);
						_player.GetIncreasedHealth(GameManager.currentHealth);
					}
					
				}
			else
			{
				ObjectName = " ";
			}
			
		}
		
			
    }
	
	[Client]
	public void Shoot()   //--Shoot a ray from cam position if hit in weapon range and hits a enemy tag--//
		
	{
	
		RaycastHit _hit;
			if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
			{                           
				
				if(_hit.collider.tag == PLAYER_TAG)
				{
					CmdPlayerShot(_hit.collider.name, weapon.damage, BulletCount);
					
				}
			}
	
		
	}
	
	[Command]
	public void CmdPlayerShot(string _playerID, int _damage, int _BulletCount)   //---Get Player name, damage amount and bullets--//
		
	{
		//isShot = true;
		Debug.Log(_playerID + "has been shot");
		Debug.Log(_playerID + "has bullet :" + _BulletCount);
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);                  //--Take the damage amount from the player health which is hit---//
	
		
	}
	
	public void NoBullets()                              //---If bullets is zero--//
	{
		BulletsEmpty.enabled = true;
		BulletsEmpty.Play();
		Debug.Log("No Bullets");
		
		MaxBullets -= ClipSize;                          //--Decrease the clipsize amount i.e 20 from Max bullets--//
		BulletCount += ClipSize;                         //--Add the same amount to main bullet count--//
		
		
	}
	
	IEnumerator IncreaseBullets()
	{
		yield return new WaitForSeconds(2);             //--Increase the maxbullets every 2s until it reaches 50--//
			
			do{ 
				MaxBullets++;
			  } while(MaxBullets == 50);
		
	}
	
	
}
