using UnityEngine;
using System.Collections;
using GoMap;

using GoShared;
using System;
using UnityEngine.Events;


public class MoveAvatar : MonoBehaviour {
	public double ch_longitude, ch_latitude;
	public GOMap goMap;
	private GameObject avatarFigure;
	public GameObject[] characters;
	int selectedCharater;
	int playerID;
	private Animator animator;
	public LocationManager location_manager;

	
	public AvatarAnimationState animationState = AvatarAnimationState.Idle;
	[HideInInspector] public float dist;
	public enum AvatarAnimationState{
		Idle, 
		Walk,
		Run
	};
	public GOAvatarAnimationStateEvent OnAnimationStateChanged;

	// Use this for initialization
	void Start () {
		selectedCharater = PlayerPrefs.GetInt("selectedCharacter");
		animator = characters[selectedCharater].GetComponent<Animator>();
		Debug.Log("animator=" + animator.gameObject.name);
		playerID = PlayerPrefs.GetInt("playerID");
		if(playerID != 0)
        {
			Debug.Log("Registered player character");
			//selectedCharater = PlayerPrefs.GetInt("selectedCharacter");
		}
		else
        {
			Debug.Log("Guest Character");
			selectedCharater = PlayerPrefs.GetInt("GuestCharacter");
        }
		
		Debug.Log($"Selected Character index is {selectedCharater}");
		avatarFigure = characters[selectedCharater];
		avatarFigure.SetActive(true);
		goMap.locationManager.onOriginSet.AddListener((Coordinates) => {OnOriginSet(Coordinates);});
		goMap.locationManager.onLocationChanged.AddListener((Coordinates) => {OnLocationChanged(Coordinates);});
		if (goMap.useElevation)
			goMap.OnTileLoad.AddListener((GOTile) => {OnTileLoad(GOTile);});

		LocationManager locationManager = (LocationManager)FindObjectOfType(typeof(LocationManager));
		locationManager.SetAvatar(characters[selectedCharater]);
    }

    public void Update()
    {
    //      //AvatarAnimationState State = AvatarAnimationState.Idle;
    //      /*float h = Input.GetAxis("Horizontal");
    //float v = Input.GetAxis("Vertical");
    //bool fire = Input.GetButtonDown("Fire1");

    //animator.SetFloat("Forward", v);
    //animator.SetFloat("Strafe", h);
    //animator.SetBool("Fire", fire);*/
    //      //Debug.Log("Animation State: " + animationState);

    if (animationState.ToString() == "Idle")
	{
			animator.SetBool("Idle", true);
			animator.SetBool("Walk", false);
			animator.SetBool("Run", false);
		}
	else if(animationState.ToString() == "Walk")
        {
			animator.SetBool("Walk", true);
			animator.SetBool("Run", false);
			animator.SetBool("Idle", false);
		}
    else if (animationState.ToString() == "Run")
    {
			animator.SetBool("Walk", true);
			animator.SetBool("Run", false);
			animator.SetBool("Idle", false);
		}

}



#region GoMap events

public void OnTileLoad (GOTile tile) {

		Vector3 currentLocation = goMap.locationManager.currentLocation.convertCoordinateToVector ();

		if (tile.goTile.vectorIsInTile(currentLocation)) {

			if (goMap.useElevation)
				currentLocation = GOMap.AltitudeToPoint (currentLocation);
			
			transform.position = currentLocation;
		} 
	}

	#endregion

	#region Location manager events

	void OnOriginSet (Coordinates currentLocation) {
		ch_longitude = currentLocation.longitude;
		ch_latitude = currentLocation.latitude;
		Debug.Log($"Long: {currentLocation.longitude} and Lat: {currentLocation.latitude}");
		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector (0);
		if(goMap.useElevation)
			currentPosition = GOMap.AltitudeToPoint (currentPosition);

		transform.position = currentPosition;

	}

	void OnLocationChanged (Coordinates currentLocation) {

		Vector3 lastPosition = transform.position;

		//Position
		Vector3 currentPosition = currentLocation.convertCoordinateToVector (0);

		if(goMap.useElevation)
			currentPosition = GOMap.AltitudeToPoint (currentPosition);

		if (lastPosition == Vector3.zero) {
			lastPosition = currentPosition;
		}
			
		moveAvatar (lastPosition,currentPosition);

	}

	#endregion

	#region Move Avatar

	void moveAvatar (Vector3 lastPosition, Vector3 currentPosition) {
		StartCoroutine (move (lastPosition,currentPosition,0.5f));
	}

	private IEnumerator move(Vector3 lastPosition, Vector3 currentPosition, float time) {

		float elapsedTime = 0;
		Vector3 targetDir = currentPosition-lastPosition;
		Quaternion finalRotation = Quaternion.LookRotation (targetDir);

		while (elapsedTime < time)
		{
			transform.position = Vector3.Lerp(lastPosition, currentPosition, (elapsedTime / time));
			avatarFigure.transform.rotation = Quaternion.Lerp(avatarFigure.transform.rotation, finalRotation,(elapsedTime / time));

			elapsedTime += Time.deltaTime;

			dist = Vector3.Distance (lastPosition, currentPosition);
			//Debug.Log("Distance: " + dist);
			AvatarAnimationState state = AvatarAnimationState.Idle;

			if (dist > 4)
			{		
				state = AvatarAnimationState.Run;	
				/*animator.SetBool("Run", true);
				animator.SetBool("Idle", false);*/
			}
		
			//state = AvatarAnimationState.Walk;
			
          /*  else if(animationState.ToString() == "Idle")
            {
				animator.SetBool("Idle", true);
				animator.SetBool("Run", false);
			}*/
			
			if (state != animationState) {
				
				animationState = state;
				OnAnimationStateChanged.Invoke(animationState);
			}

			yield return new WaitForEndOfFrame();
		}
		

		
		animationState = AvatarAnimationState.Idle;
		Debug.Log(animationState);
		OnAnimationStateChanged.Invoke(animationState);


	}
		
	void rotateAvatar(Vector3 lastPosition) {

		Vector3 targetDir = transform.position-lastPosition;

		if (targetDir != Vector3.zero) {
			avatarFigure.transform.rotation = Quaternion.Slerp(
				avatarFigure.transform.rotation,
				Quaternion.LookRotation(targetDir),
				Time.deltaTime * 10.0f
			);
		}
	}

	#endregion
}

[Serializable]
public class GOAvatarAnimationStateEvent : UnityEvent <MoveAvatar.AvatarAnimationState> {


}
