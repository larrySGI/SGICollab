	using UnityEngine;
using System.Collections;

public class BuilderPickup : Photon.MonoBehaviour {
	public int pickupAmount;
	 public enum powerUpType {block, plank};
	public powerUpType type;
	
//	[RPC] 
//	void DestroyObject ()
//	{
//		Destroy(gameObject);
//	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody.name.Contains("Builder")){
//			if(other.transform.GetComponent<ThirdPersonNetworkVik>().photonView.isMine)//if user is current user
//			{
				if(type==powerUpType.block){
				ThirdPersonControllerNET.currentMaxBlocks = ThirdPersonControllerNET.currentMaxBlocks+pickupAmount;
				ThirdPersonControllerNET.blockammo = ThirdPersonControllerNET.blockammo+pickupAmount;
				
				}
				if(type==powerUpType.plank){
				ThirdPersonControllerNET.currentMaxPlanks = ThirdPersonControllerNET.currentMaxPlanks+pickupAmount;
				ThirdPersonControllerNET.plankammo = ThirdPersonControllerNET.plankammo+pickupAmount;
				}
//				photonView.RPC("DestroyObject", PhotonTargets.OthersBuffered);
				Destroy(gameObject);	
				//photonView.RPC("DestroyBlock",PhotonTargets.All);
//			}
		
		}
	}

}
