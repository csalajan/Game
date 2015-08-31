using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	void OnTriggernEnter (Collision col)
	{
		Destroy(this.gameObject);
	}

	void Update () {
		transform.Rotate(new Vector3(0, 0, 25*Time.deltaTime));
	}
}