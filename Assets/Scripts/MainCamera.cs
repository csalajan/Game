using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject Avatar;
    private Vector3 offset;
    public float damping = 1;

    void Start()
    {
        offset = Avatar.transform.position - transform.position;
    }
    
    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = Avatar.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = Avatar.transform.position - (rotation*offset);

        transform.LookAt(Avatar.transform);
    }
}
