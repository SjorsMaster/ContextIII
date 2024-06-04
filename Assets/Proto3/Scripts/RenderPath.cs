using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RenderPath : MonoBehaviour
{
    [SerializeField] private float lineStartWidth = 0.05f;
    [SerializeField] private float lineEndWidth = 0.2f;

    private LineRenderer lineRenderer;

    public void RenderLine(List<Vector3> positions)
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.material = Resources.Load("LineMat", typeof(Material)) as Material;

        // Make line pretty.
        Gradient gradient = new();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 0.0f),
                new(Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 1.0f)
            },
            new GradientAlphaKey[]
            {
                new(0, 0f),
                new(1, .25f),
                new(1, .95f),
                new(0, 1f)
            }
        );

        lineRenderer.colorGradient = gradient;

        lineRenderer.startWidth = lineStartWidth;
        lineRenderer.endWidth = lineEndWidth;

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
