using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class CameraManager : MonoService<CameraManager> 
{
    public List<GameObject> locations = new List<GameObject>();
    public GameObject Camera;
    public float speed;

    bool InProgress;

    public Action CameraMoved;

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    // Start is called before the first frame update
    void Start()
    {
        //MoveToLocation(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveCamera(GameObject Target)
    {
        InProgress = true;
        GameObject L = Target;
        Vector3 Damp= new Vector3();
        while(Camera.transform.position != L.transform.position)
        {

            Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, L.transform.position, ref Damp, speed);
            //Camera.transform.position = Vector3.Lerp(Camera.transform.position, L.transform.position, speed * Time.deltaTime);
            Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, L.transform.rotation, speed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
            if(Vector3.Distance(Camera.transform.position,L.transform.position) < 0.2f)
            {
                Camera.transform.position = L.transform.position;            
            }
        }
        InProgress = false;
        Debug.Log("Finshed Move");
    }

    public void MoveToLocation(int L)
    {
        if(InProgress)
        {
            StopAllCoroutines();
        }
        StartCoroutine(MoveCamera(locations[L]) );
    }
}
