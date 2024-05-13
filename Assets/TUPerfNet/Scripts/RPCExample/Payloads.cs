using UnityEngine;
using TU.PerfNet;

public class ActivateSiblings : RpcPayload
{
    public override void HandleRpc()
	{
        // FindObjectOfType<HologramDistorter>().PlayDirector();
    }
}

public class HologramGoesWrong : RpcPayload
{
    float time;
	public override void Parse(object[] data)
	{
        time = (float)data[0];
	}

	public override void HandleRpc()
    {
        // FindObjectOfType<HologramDistorter>().SetDirectorTime(time);
        Debug.Log(time);
    }
}

public class ResumeHolograms : RpcPayload
{
    public override void HandleRpc()
    {
        //  FindObjectOfType<UnityEngine.Playables.PlayableDirector>().Play();
    }
}

public class RemoveHolograms : RpcPayload
{
    public override void HandleRpc()
    {
        // Destroy(FindObjectOfType<UnityEngine.Playables.PlayableDirector>().gameObject);
    }
}

/*
// TODO: These functions are project specific, the core mirror implementation should not be dependent on it
//          I've mocked up a payload example above
[ClientRpc]
public void RpcActivateSiblings()
{
    Debug.Log("RpcActivateSiblings called from server.");
    // TODO: Abstract this away from the network implementation
    // FindObjectOfType<HologramDistorter>().PlayDirector();
}


[ClientRpc]
public void RpcHologramGoesWrong(float time)
{
    Debug.Log("RpcHologramGoesWrong called from server.");
    // TODO: Abstract this away from the network implementation
    // FindObjectOfType<HologramDistorter>().SetDirectorTime(time);
}

[ClientRpc]
public void RpcResumeHolograms()
{
    Debug.Log("RpcResumeHolograms called from server.");
    // TODO: Abstract this away from the network implementation
    // FindObjectOfType<UnityEngine.Playables.PlayableDirector>().Play();
}

[ClientRpc]
public void RpcRemoveHolograms()
{
    Debug.Log("RpcRemoveHolograms called from server.");
    // TODO: Abstract this away from the network implementation
    // Destroy(FindObjectOfType<UnityEngine.Playables.PlayableDirector>().gameObject);
}
*/