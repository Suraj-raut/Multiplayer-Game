using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RegainHealth : NetworkBehaviour
{
   
    public  static int RegainHealthAmt = 50;
	
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.attachedRigidbody == null)
			return;
		
		if(other.attachedRigidbody.gameObject.tag.Equals("Player"))   //--If medikit found increase the health by 50--//
		{
			Debug.Log("Found MediKit");
				
		}
		
		
	}
	
	void OnDisable()                             
	{
		Destroy(this.gameObject);                               
		
		GameManager.GetTheMediKit(RegainHealthAmt);
	}
	
	
	

	
	
}
