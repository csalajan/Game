using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject Avatar;
    Transform target;
    float distance = 12.0F;

    void Start()
    {
        target = Avatar.transform;
    }

    void Update()
    {
        transform.position = target.position - target.forward * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y + 4.0f, transform.position.z);
        transform.LookAt(target.position);
    }
}
