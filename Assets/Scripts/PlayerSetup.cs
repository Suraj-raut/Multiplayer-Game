using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//[RequiredComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
	[SerializeField]
	private Behaviour[] componentsToDisabled;
	
	private Camera sceneCamera;
	
	[SerializeField]
	private string remoteLayerName = "RemotePlayer";
	private const string PLAYER_TAG = "Enemy";
	
	public Player _player;
	
	[SerializeField]
	private GameObject CanvasPrefab;
	
	private GameObject playerUIInstance;
	
	
    // Start is called before the first frame update
    void Start()
    {
         if(!isLocalPlayer)                            //--If it is not local player i.e if it is not you--//
		{
		    AssigningTheLayer(); 
			DisableComponents(); 
		}
		
		else
		{
			sceneCamera = Camera.main;         //---Set the player's camera to main camera to avoid multiple audio listener--//
			if(sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(false);
			}
			
			SetTheGameElements();
				
		}
		    
		
    }
	
		public void SetTheGameElements()
	{
		 playerUIInstance = Instantiate(CanvasPrefab); //---Instantiate the Canvas for UI in runtime and set an instance of it--//
		 playerUIInstance.name = CanvasPrefab.name;
			
		
			//config player UI
			ObjectPickup ui = playerUIInstance.GetComponent<ObjectPickup>();   //--Get the ObjectPickup script from canvas--//
			if(ui == null)
			Debug.LogError("No ObjectPickup script available");
			
			ui.SetPlayer(GetComponent<Player>());                //--Set the player in SetPlayer method of ObjectPickup script--//
		
	
	}
	
	
	
	public override void OnStartClient()           //---On Start client i.e at start of game--//
	{
		base.OnStartClient();
		
		
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		_player = GetComponent<Player>();
		
		GameManager.RegisterPlayer(_netID, _player);    //--Set the Player with PlayerID and Player script to gameManager--//
	}
	

	
	void AssigningTheLayer()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);   //--Set the layer to Remote Player--//
		gameObject.tag =  PLAYER_TAG;                                //--Set the Tag to Enemy--//
	}
	
	
	void DisableComponents()                                   //--Disabled the component because both player move after inputs--// 
	{
			for(int i=0; i<= componentsToDisabled.Length; i++ )
			{
				componentsToDisabled[i].enabled = false;
			}
	}
	

	void OnDisable()                             
	{
		Destroy(playerUIInstance);                                //--If it is disabled i.e death destroy the UI also--//
		
		if(sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(true);
			}
		
		
		GameManager.UnRegisterPlayer(transform.name);           //--UnRegister Player with the name if it is dead--//
	}
	
	

  
}
