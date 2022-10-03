using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RPGSystem{
public class PlayerNavLink : MonoBehaviour {

	public PlayerNavLink end;
	public NavMeshLink link;
	public BoxCollider col;
	public bool reverseDir;
	void OnTriggerStay(Collider other)
	{
		if(link.enabled){
			PlayerController p=other.GetComponent<PlayerController>();
			if(p!=null)
			{
				if(!p.OffMesh&&p.rig.isKinematic)
				{
					Vector3 dir=end.transform.position-transform.position;
					if(reverseDir)
					{
						dir.x*=-1;
						dir.z*=-1;
					}
					if(Vector3.Angle(new Vector3(p.dir.x,0,p.dir.z),new Vector3(dir.x,0,dir.z))<30)
					{	
						NavMeshHit hit;
						Vector3 offT = end.transform.position;
						if(!link.ignoreOffset){
							offT+=(p.self.position-transform.position);
						}
						if (NavMesh.SamplePosition(offT, out hit, 1.0f, NavMesh.AllAreas)) {
							p.HandleOffMesh(link);
						}
					}
				}
			}
		}
	}
}
}