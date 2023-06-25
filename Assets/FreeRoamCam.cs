using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamCam : MonoService<FreeRoamCam>
{



    public float Speed = 1;
    public float RotSpeed = 20;
    public bool FreeRoam = false;
    // Start is called before the first frame update

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void FreeRoamToggle(bool Toggle)
    {
        FreeRoam = Toggle;
    }

    private void FixedUpdate()
    {
        if (FreeRoam)
        {
            Rot();
            Movement();
        }
    }


    void Movement()
    {
        Vector3 Rot = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            Rot = Rot + transform.forward;//new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Rot = Rot + transform.forward * -1;//new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Rot = Rot + new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Rot = Rot + new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Rot = Rot + new Vector3(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Rot = Rot + new Vector3(0, -1, 0);
        }

        this.transform.position += ((Rot * Speed) * Time.deltaTime);
    }
    void Rot()
    {
        Vector3 Rot = Vector3.zero;
        if(Input.GetKey(KeyCode.RightArrow))
        {
            Rot = Rot + new Vector3(0,1,0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Rot = Rot + new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Rot = Rot + new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Rot = Rot + new Vector3(-1, 0, 0);
        }

        this.transform.Rotate((Rot * RotSpeed) * Time.deltaTime);
    }

}
