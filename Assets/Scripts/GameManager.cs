using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

public class GameManager : NetworkBehaviour
{

 public static int currentHealth;
	
private const string PLAYER_ID_PREFIX = "Player";
	
 private static Dictionary<string, Player> players = new Dictionary<string, Player>();   //--Keep track of player in game--//
	
	public static void RegisterPlayer(string _netID, Player _player)   //---Get the Player from PlayerSetup script --//
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);                              //--Add the player to dictionary--//
		_player.transform.name = _playerID;
		
	}
	
	public static void UnRegisterPlayer(string _playerID)        //--get the player which is dead from PlayerSetup Script--//
	{
		players.Remove(_playerID);                               //--Remove the player from dictionary --//
	}
	
	public static Player GetPlayer(string _playerID)             //--Return the player from the dictionary--//
	{
	return players[_playerID];
	}
	
	public static void GetTheMediKit(int increasedHealthAmt)    //--Save the increased amount when medikit found--//
	{
		currentHealth = increasedHealthAmt;
	}
	
}
