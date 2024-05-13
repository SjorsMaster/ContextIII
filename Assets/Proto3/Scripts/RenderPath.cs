using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RenderPath : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] Vector2 sizeLine = new Vector2(.05f, .2f);
    public void RenderLine(List<Vector3> input){
        //fetch renderer
        lineRenderer = GetComponent<LineRenderer>();

        //Make line pretty
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 0.0f), new GradientColorKey(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0, 0f), new GradientAlphaKey(1, .25f), new GradientAlphaKey(1, .95f), new GradientAlphaKey(0, 1f) }
        );
        lineRenderer.colorGradient = gradient;

        lineRenderer.SetWidth(sizeLine.x, sizeLine.y);
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        
        //Fetch input and apply those positions
        List<Vector3> poscol = input;
 
        lineRenderer.positionCount = poscol.Count;
        lineRenderer.SetPositions(poscol.ToArray()); 
    }
}
