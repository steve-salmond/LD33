using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour 
{
    public Weapon Weapon { get; set; }
	public abstract void Target(Transform target);

}
