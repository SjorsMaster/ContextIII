using SharedSpaces.SaveSystem;
using System.Collections;
using UnityEngine;

public class PersistentDataManagerProto3 : PersistentDataManager<PersistentDataProto3>
{
    [SerializeField] private bool SaveEveryInterval = true;
    [SerializeField] private float interval = 60.0f;

    private IEnumerator Start()
    {
        while (SaveEveryInterval)
        {
            yield return new WaitForSeconds(interval);
            SavePersistentData();
        }
    }
}
