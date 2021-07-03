using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
public class PlayerController : MonoBehaviour
{
	private Player _player;
	
	[SerializeField]
	private float speed = 10f;
    
	[SerializeField]
	private float lookSensetivity = 3.0f;
	
	[SerializeField]
	private GameObject cam;
	[SerializeField]
	private float cameraRotationLimit = 85f;
	private float currentCamRotation = 0f;
	private float _cameraRotationX = 0f;
	
	
	private CharacterController controller;
	private Rigidbody rb;
	
	[SerializeField]
	private float thursterForce = 1f;
	
	
	public float gravity = 1f;
	
	[SerializeField]
	private float FuelBurnSpeed = 1f;
	[SerializeField]
	private float FuelRegainSpeed = 0.1f;
	[SerializeField]
	public static float FuelAmt = 1f;
	
    // Start is called before the first frame update
    void Start()
    {
     
		controller = GetComponent<CharacterController>();
		 rb = GetComponent<Rigidbody>();
		_player = GetComponent<Player>();
    }
	
	public static float GetFuelAmt()                                     //--Send the fuel amount to OjectPickup scrit for UI--//
	{
		return FuelAmt;
	}

    // Update is called once per frame
    void Update()
	 {  
		
		// Calculating movement Velocity in terms of Vector3
		
		float _xMove = Input.GetAxisRaw("Horizontal");
        float _zMove = Input.GetAxisRaw("Vertical");
		
		
		Vector3 _moveHorizontal = transform.right * _xMove;  //(1, 0, 0)
		Vector3 _moveVertical = transform.forward * _zMove;  //(0, 0, 1)
		
		// final movement vector3
		Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;
	
		
	
		//Jump Mechanics
		if(Input.GetButton("Jump") && FuelAmt >= 0)
		{
			FuelAmt -= FuelBurnSpeed * Time.deltaTime;    //--Decrease the fuel amout--//
			_velocity.y = thursterForce;
					
		}
		else 
		{
			if(controller.isGrounded)
			{
				FuelAmt += FuelRegainSpeed * Time.deltaTime / 2;    //--Increase the fuel amout--//
			}
			
			_velocity.y -=  gravity * thursterForce * Time.deltaTime;   //--If player is not grounded apply down force --//
		}
		

		
		//Apply movement
		controller.Move(_velocity * Time.deltaTime);
		
		//For Rotation or turning around
		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensetivity;
		
		_rotation = transform.TransformDirection(_rotation);
		
		
		rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotation));
		
		float _xRot = Input.GetAxisRaw("Mouse Y");
		_xRot = Mathf.Clamp(_xRot, -35, 60);
		
		 _cameraRotationX = _xRot * lookSensetivity;
		
		
		
		//Camera rotation 
		if(cam != null)
		{
		currentCamRotation -= _cameraRotationX;
		currentCamRotation = Mathf.Clamp(currentCamRotation, -cameraRotationLimit, cameraRotationLimit);
		
			
		cam.transform.localEulerAngles = new Vector3(currentCamRotation, 0f, 0f);
		}	
		
    }
	
	
}
