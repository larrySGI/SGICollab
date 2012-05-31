    var catchRange = 50.0;
    var holdDistance = 4.0;
    var minForce = 10;
    var maxForce = 10;
    var forceChargePerSec = 30;
    var layerMask : LayerMask = -1;
     
    enum GravityGunState { Free, Catch, Occupied, Charge, Release};
    private var gravityGunState : GravityGunState = 0;
    private var rigid : Rigidbody = null;
    private var currentForce = minForce;

    function FixedUpdate () {
        if(gravityGunState == GravityGunState.Free && Screen.lockCursor == true) {
            if(Input.GetButton("Fire1")) {
                var hit : RaycastHit;
                if(Physics.Raycast(transform.position, transform.forward, hit, catchRange, layerMask)) {
                    if(hit.rigidbody) {
                        rigid = hit.rigidbody;
                        rigid.isKinematic = true;
                        gravityGunState = GravityGunState.Catch;
                       
                    }
                }
            }
        }
        else if(gravityGunState == GravityGunState.Catch && Screen.lockCursor == true) {
            rigid.transform.position = transform.position + transform.forward * holdDistance;
            
            if(!Input.GetButton("Fire1"))
                gravityGunState = GravityGunState.Occupied;     
        }
        else if(gravityGunState == GravityGunState.Occupied && Screen.lockCursor == true) {            
            rigid.transform.position = transform.position + transform.forward * holdDistance;
            if(Input.GetButton("Fire1"))
                gravityGunState = GravityGunState.Charge;
                
                
                
        }
        else if(gravityGunState == GravityGunState.Charge && Screen.lockCursor == true) {
			rigid.transform.position = transform.position + transform.forward * holdDistance;
            if(!Input.GetButton("Fire1") && Screen.lockCursor == true)
            {
            	rigid.isKinematic = false;
                gravityGunState = GravityGunState.Release;
               
                
                }
               

        }
        else if(gravityGunState == GravityGunState.Release && Screen.lockCursor == true) {
            
            gravityGunState = GravityGunState.Free;
        }
    }
    
    @RPC
    function updatePos(pos:Vector3, rot:Quaternion){
     if(rigid != null){
     		
     	rigid.transform.position = pos;
    	rigid.transform.rotation = rot;
     	}
    } 
     
    @script ExecuteInEditMode()