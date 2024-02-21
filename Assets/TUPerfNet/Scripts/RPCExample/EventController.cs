using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine;
using TU.PerfNet;

public class EventController : NetworkBehaviour
{
    public float timelineTimeGoesWrong = 29.0f;

	private void Update()
	{
        if (Input.GetKeyUp(KeyCode.Alpha1)) ActivateSiblings();
        if (Input.GetKeyUp(KeyCode.Alpha2)) ActivateSiblings(0);

        if (Input.GetKeyUp(KeyCode.Alpha3)) HologramGoesWrong();
        if (Input.GetKeyUp(KeyCode.Alpha4)) HologramGoesWrong(0);

        if (Input.GetKeyUp(KeyCode.Alpha5)) ResumeHologram();
        if (Input.GetKeyUp(KeyCode.Alpha6)) ResumeHologram(0);

        if (Input.GetKeyUp(KeyCode.Alpha7)) CompleteRitualTwo();
        if (Input.GetKeyUp(KeyCode.Alpha8)) CompleteRitualTwo(0);
    }

	//For unity UX
	public void ActivateSiblings()
    {
        print("Activate Siblings");
        ClientRpcController.PerformRpc("ActivateSiblings", true, null);
    }

    //For OSC connection with chataigne
    public void ActivateSiblings(int value)
    {
        if(value != 0)
        {
            print("Activate Siblings");
            ClientRpcController.PerformRpc("ActivateSiblings", true, null);
        }
    }


    //For unity UX
    public void HologramGoesWrong()
    {
        print("HologramGoesWrong");
        ClientRpcController.PerformRpc("HologramGoesWrong", true, timelineTimeGoesWrong);
    }

    //For OSC connection with chataigne
    public void HologramGoesWrong(int value)
    {
        if(value != 0)
        {
            print("HologramGoesWrong");
            ClientRpcController.PerformRpc("HologramGoesWrong", true, timelineTimeGoesWrong);
        }
    }

    //For unity UX
    public void ResumeHologram()
    {
        print("Resume Hologram");
        ClientRpcController.PerformRpc("ResumeHolograms", true, null);
    }

    //For OSC connection with chataigne
    public void ResumeHologram(int value)
    {
        if(value != 0)
        {
            print("Resume Hologram");
            ClientRpcController.PerformRpc("ResumeHolograms", true, null);
        }
    }

    //For unity UX
    public void CompleteRitualTwo()
    {
        print("Complete Ritual 02");
        ClientRpcController.PerformRpc("RemoveHolograms", true, null);
    }

    //For OSC connection with chataigne
    public void CompleteRitualTwo(int value)
    {
        if(value != 0)
        {
            print("Complete Ritual 02");
            ClientRpcController.PerformRpc("RemoveHolograms", true, null);
        }
    }
}
