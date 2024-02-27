using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContextIII
{
    public class SceneParent : MonoBehaviour
    {
        //Dummy to call with FindObjectsOfType<T>(). Keep this in case we need to add code to SceneParents.
        //public Material skyboxMaterial;
        //public Color realtimeShadowColor;

        //public AmbientMode ambientMode;

        //public Color ambientColor;

        //public float skyboxIntensity;

        public bool passthrough;

        public void Apply()
        {
            if (passthrough)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0)); //Use main scene for passthrough. Other scenes with skybox appear to be non functional.
            }
            else
            {
                SceneManager.SetActiveScene(gameObject.scene);
                RenderSettings.skybox.SetFloat("_Rotation", transform.eulerAngles.y); //Use transform eulers here, because the transform has already been set to the calibration point in the SceneController.
            }

            /*RenderSettings.skybox = skyboxMaterial;
            RenderSettings.subtractiveShadowColor = realtimeShadowColor;

            switch (ambientMode)
            {
                case AmbientMode.Skybox:
                    RenderSettings.ambientIntensity = skyboxIntensity;
                    break;
                case AmbientMode.Flat:
                    RenderSettings.ambientLight = ambientColor;
                    break;
                default:
                    goto case AmbientMode.Skybox;
            }*/


            DynamicGI.UpdateEnvironment();

            FindObjectOfType<OVRManager>().isInsightPassthroughEnabled = passthrough;

            // TODO: Abstract this away from the LocalPlayerController, there is no need to connect this rendering feature to it
            // LocalPlayerController.self.postProcessing = !passthrough;
            // Debug.Log(LocalPlayerController.self.postProcessing);

            Destroy(this);  //We destroy this script after applying. No data needs to be read after this function and it will otherwise interfere with other FindObjectOfType<SceneParent>() calls.
        }
    }
}