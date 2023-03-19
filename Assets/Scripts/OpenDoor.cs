using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpenDoor : MonoBehaviour
{
    public GameObject LeftDoor;
    public GameObject RightDoor;
    
    public void Opening()
    {
        LeftDoor.transform.DOMove(LeftDoor.transform.position + Vector3.left * 2, 4);
        RightDoor.transform.DOMove(RightDoor.transform.position + Vector3.right * 2, 4);
        
    }


}
