using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PortalsVR;


public enum PortalAction
{
    ACTIVATE,
    DEACTIVATE,
    SWITCH_TO
}

[RequireComponent(typeof(Portal))]
public class ButtonPortalAction : ButtonAction
{

    private Portal portal;
    public PortalAction action;
    public string targetWorld = "";
    private World sourceWorld;

	private void Awake()
    {
		portal = GetComponent<Portal>();
	}

    public override void Do()
    {
        switch(action)
        {
            case PortalAction.ACTIVATE:
                portal.gameObject.SetActive(true);
                break;
            case PortalAction.DEACTIVATE:
                portal.gameObject.SetActive(false);
                break;
            case PortalAction.SWITCH_TO:
                if (World.worlds.ContainsKey(targetWorld)) {
                    sourceWorld = portal.linkedPortal.parentWorld;
					portal.linkedPortal.parentWorld.Migrate(portal.linkedPortal.gameObject, World.worlds[targetWorld], true);
					portal.linkedPortal.parentWorld = World.worlds[targetWorld];
				}                
                break;
        }
        base.Do();
    }

    public override void Undo()
    {
		switch (action)
		{
			case PortalAction.ACTIVATE:
				portal.gameObject.SetActive(false);
				break;
			case PortalAction.DEACTIVATE:
				portal.gameObject.SetActive(true);
				break;
			case PortalAction.SWITCH_TO:
				if (World.worlds.ContainsKey(targetWorld))
				{
					portal.linkedPortal.parentWorld.Migrate(portal.linkedPortal.gameObject, sourceWorld, true);
					portal.linkedPortal.parentWorld = sourceWorld;
				}
				break;
		}
		base.Undo();
    }
}
