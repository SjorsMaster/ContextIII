using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.Oculus;
using static Unity.XR.Oculus.Utils;
using UnityEngine.XR;

/// <summary>
/// TODO: See if we can blit the depth texture per eye directly to the cameras using a CommandBuffer at the beginning of the render pass
///     https://forum.unity.com/threads/blit-rendertexture-into-cameras-depth-texture.701450/
/// </summary>
public class EnvironmentDepthManager : MonoBehaviour
{
    bool isRendering = false;
    uint textureID = 0;
    XRDisplaySubsystem displaySubsystem;

	// Start is called before the first frame update
	void Awake()
    {
        EnvironmentDepthCreateParams createParams = new EnvironmentDepthCreateParams();
        createParams.removeHands = false;

        Utils.SetupEnvironmentDepth(createParams);
	}

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        Utils.SetEnvironmentDepthRendering(true);
        isRendering = true;

		displaySubsystem = OVRManager.GetCurrentDisplaySubsystem();
	}

    private void LateUpdate()
    {
        if (isRendering )
        {
            Utils.GetEnvironmentDepthTextureId(ref textureID);
			RenderTexture rt = displaySubsystem.GetRenderTexture(textureID);

            Shader.SetGlobalTexture("_SceneDepth", rt);
		}
    }

    // Update is called once per frame
    void OnDestroy()
    {
        Utils.ShutdownEnvironmentDepth();
        isRendering = false;
	}
}
