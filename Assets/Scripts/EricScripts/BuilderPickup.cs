using UnityEngine;
using System.Collections;

public class BuilderPickup : Photon.MonoBehaviour {
	 public enum powerUpType {block, plank};
	public powerUpType type;
	[RPC] 
	void DestroyObject ()
	{
		Destroy(gameObject);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody.name.Contains("Builder")){
			if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine)//if user is current user
			{
				if(type==powerUpType.block){
				ThirdPersonControllerNET.currentMaxBlocks++;
				ThirdPersonControllerNET.blockammo++;
					photonView.RPC("DestroyObject", PhotonTargets.All);
				}
				if(type==powerUpType.plank){
				ThirdPersonControllerNET.currentMaxPlanks++;
				ThirdPersonControllerNET.plankammo++;
				photonView.RPC("DestroyObject", PhotonTargets.All);
				}
				//photonView.RPC("DestroyBlock",PhotonTargets.All);
			}
		
		}
	}

}
