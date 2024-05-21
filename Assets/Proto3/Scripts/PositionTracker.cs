using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] bool track = true;
    [SerializeField] float trackSecInterval = .25f;
    public List<Vector3> posCol;

    public delegate void sendPositions(List<Vector3> dataList);
    public static event sendPositions newPositionData;

    //start tracking based on interval
    void Start()
    {
        InvokeRepeating ("TrackPos", 0, trackSecInterval); 
    }

    // Because of InvokeRepeating, tis is called every 5 seconds.
    void TrackPos()
    {
        if (!track)
        {
            //Done, send out data and stop tracking
            CancelInvoke("TrackPos");
            newPositionData.Invoke(posCol);
        }
        //Make sure we don't save unnecessary data
        if(posCol.LastOrDefault() != transform.position)
            posCol.Add (new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z));
    }

    public void ToggleTrack()
    {
        track = !track;

        if (track)
        {
            posCol.Clear();
            InvokeRepeating("TrackPos", 0, trackSecInterval);
        }
    }
}
