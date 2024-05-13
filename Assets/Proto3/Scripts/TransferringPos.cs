using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferringPos : MonoBehaviour
{
    //Subscribe to items to see if we get new data
    void Start (){
        PositionTracker.newPositionData += processData;
    }

    //Process newfound data
    public void processData(List<Vector3> input){
        GameObject obj = new GameObject("PathRenderer");
        obj.AddComponent<RenderPath>();
        obj.GetComponent<RenderPath>().RenderLine(input);
    }

    
}
