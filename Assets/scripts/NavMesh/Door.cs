using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RPGSystem{
public class Door : MonoBehaviour {

	public NavMeshLink link;
	public bool _unlocked;
	public Animator anim;
	public bool canAttack=false;
	public void SetLocked(bool _l)
	{
		_unlocked=_l;
		link.gameObject.SetActive(_unlocked);
	}
	void OnValidate()
	{
		if(link==null)
		{
			link=new GameObject().AddComponent<NavMeshLink>();
			link.transform.SetParent(transform);
			link.transform.localPosition=Vector3.zero;
			link.transform.localRotation=Quaternion.identity;
			link.name="NavMeshLink";
		}
	}
	void Start()
	{
		SetLocked(_unlocked);
		Toggle(false);
	}
	public void Toggle(bool _o)
	{
		if(anim!=null)
		{
			anim.SetBool("Open",_o);
		}
	}
	public float AttackTimer=1.25f;
	float attackTime;
	public AudioClip clip;
	List<MovementController> cols=new List<MovementController>();
	void OnTriggerEnter(Collider other)
	{
		MovementController c=other.GetComponent<MovementController>();
		if(c!=null&&!cols.Contains(c))
		{
			cols.Add(c);
		}
	}
	void OnTriggerStay(Collider other)
	{
		if(canAttack&&!_unlocked){
			if(Time.time>=attackTime)
			{
				bool found=false;
				for(int i=cols.Count-1;i>=0;i--){
					MovementController c=cols[i];
					if(c!=null)
					{
						if(c.health>0){
							found=true;
							c.GetHit(10);
						}
					}else{
						cols.RemoveAt(i);
					}
				}
				if(found){
						anim.SetTrigger("Attack");
						SoundEffects.PlaySound(clip,transform,clip.length);
						attackTime=Time.time+AttackTimer;
				}
			}
		}
	}
	void OnTriggerExit(Collider other)
	{		
		MovementController c=other.GetComponent<MovementController>();
		if(c!=null&&cols.Contains(c))
		{
			cols.Remove(c);
		}
	}
}
#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(Door))]
class DoorEditor : Editor
{
	
    public override void OnInspectorGUI()
    {
		serializedObject.Update();
		Door d=(Door)target;
        DrawDefaultInspector ();
		d.SetLocked(d._unlocked);
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
}