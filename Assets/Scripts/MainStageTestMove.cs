using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageTestMove : MonoBehaviour
{
    public float Speed;
    float HAxis;
    float VAxis;

    void Start()
    {
        
    }
    
    void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        HAxis = Input.GetAxis("Horizontal");
        VAxis = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector3 moveVec = new Vector3(HAxis, 0, VAxis).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);

    }
}
