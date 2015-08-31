using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour {

	Vector3 offset;
	Vector3 pos1;
	Vector3 pos2; 
	float moveSpeed = 0.8F;
	Vector3 moveTo;

	float delay = 5.0F;
	bool collected = false;
	
	void Start () {
		offset = Vector3.down;
		pos1 = transform.position;
		pos2 = transform.position + offset; 
	}
	
	void OnTriggerEnter (Collider col)
	{
        if (col.gameObject.tag == "Player")
	    {
	        col.gameObject.SendMessage("CollectItem", "Fruit");
	    }
		transform.Translate (1000, 1000, 1000);
		//collected = true;
	}
	
	void Update () {
		if (collected)
			delay -= Time.deltaTime;

		if (delay <= 0.0F)
			//Application.LoadLevel (0);

		if(transform.position == pos1)
		{
			moveTo = pos2;
		}
		if(transform.position == pos2)
		{
			moveTo = pos1;
		}
		
		transform.position = Vector3.MoveTowards(transform.position, moveTo, moveSpeed*Time.deltaTime);
		transform.Rotate(new Vector3(0, 25*Time.deltaTime, 0));
	}
}
