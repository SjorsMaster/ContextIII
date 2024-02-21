using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(NetworkIdentity))]
public class LocalPlayerController : NetworkBehaviour
{
    public static LocalPlayerController self;

    public float handGlowIntensity { get; set; }
    public float handGlowSpeed { get; set; }

    public Transform trackedHeadTransform, trackedLeftHandTransform, trackedRightHandTransform;

    [SerializeField]
    private Renderer fader;

    [SerializeField]
    private Renderer[] hands;

    private bool glowHands;

    private float power;

    private void Awake()
    {
        if (!self)
        {
            self = this;
        }

        if ( hands.Length > 0 ) 
            power = hands[0].material.GetFloat("_Fresnel_Power");
    }

    private void Update()
    {
        if (glowHands && hands.Length > 0 )
        {
            power = Mathf.MoveTowards(power, handGlowIntensity, Time.deltaTime * handGlowSpeed);
            hands[0].material.SetFloat("_Fresnel_Power", power);
            hands[1].material.SetFloat("_Fresnel_Power", power);
        }
    }

    [ServerCallback]
    public void FadeWhite(float toAlpha, float fadeSpeed)
    {
        StartCoroutine(DoFadeWhite(toAlpha, fadeSpeed));
        RpcFadeWhite(toAlpha, fadeSpeed);
    }

    [ClientRpc]
    public void RpcFadeWhite(float toAlpha, float fadeSpeed)
    {
        StartCoroutine(DoFadeWhite(toAlpha, fadeSpeed));
    }

    // TODO: This is part of scene management, not local player control, so move it there or rename this class.
    // DISCUSS: In general, the project-specific RPCs should be less attached to synchronization code (this applies across the entire project).
    private IEnumerator DoFadeWhite(float toAlpha, float fadeSpeed)
    {
        if (!fader) yield break;    // Valid case with no scene changes in project

        while (fader.material.GetColor("_BaseColor").a != toAlpha)
        {
            fader.material.SetColor("_BaseColor", new Color(1, 1, 1, Mathf.MoveTowards(fader.material.GetColor("_BaseColor").a, toAlpha, Time.deltaTime * fadeSpeed)));
            yield return null;
        }
    }

    [ServerCallback]
    public void SetHandIntensity(float intensity, float speed)
    {
        glowHands = true;
        handGlowIntensity = intensity;
        handGlowSpeed = speed;
        //StartCoroutine(DoSetHandIntensity(intensity, speed));
        RpcSetHandIntensity(intensity, speed);
    }

    [ClientRpc]
    public void RpcSetHandIntensity(float intensity, float speed)
    {
        glowHands = true;
        handGlowIntensity = intensity;
        handGlowSpeed = speed;
        //StartCoroutine(DoSetHandIntensity(intensity, speed));
    }

    /*private IEnumerator DoSetHandIntensity(float intensity, float speed)
    {
        float power = hands[0].material.GetFloat("_Fresnel_Power");
        while (power != intensity)
        {
            power = Mathf.MoveTowards(power, intensity, Time.deltaTime * speed);
            hands[0].material.SetFloat("_Fresnel_Power", power);
            hands[1].material.SetFloat("_Fresnel_Power", power);
            Debug.Log(power);
            yield return null;
        }
        yield break;
    }*/
}
