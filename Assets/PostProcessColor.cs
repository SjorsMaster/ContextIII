using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessColor : MonoBehaviour
{
    public Shader colorizeShader;
    private Material m;

    private void Start()
    {
        m = new Material(colorizeShader);
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m);
    }
}
