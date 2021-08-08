using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
public class CameraManager : MonoBehaviour
{
    Camera cam;
    public CarController m_CarController;
    public Transform target;
    public Vector3 target_Offset;
    public float XRotOffSet = 34.801f;
    private void Start()
    {
        target_Offset = transform.position - target.position;
        XRotOffSet = transform.rotation.x - target.rotation.x;
    }
    void Update()
    {
        if (target)
        {
            transform.position =  target.position + target_Offset;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x+ XRotOffSet, target.eulerAngles.y, transform.eulerAngles.z);
        }
    }   
}
