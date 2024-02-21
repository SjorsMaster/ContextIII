using PortalsVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldUpdateText : MonoBehaviour
{
    public string prefix, postfix;
    public PortalTraveller referenceTraveller;
    public TextMeshPro displayTarget;

    private string refWorld = null;

    // Start is called before the first frame update
    void Start()
    {
        if (!displayTarget || !referenceTraveller) enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (refWorld != referenceTraveller.activeWorld) UpdateText();
    }

    void UpdateText()
    {
        refWorld = referenceTraveller.activeWorld;
        displayTarget.text = prefix + refWorld + postfix;
    }
}
