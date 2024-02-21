using UnityEngine.SceneManagement;
using UnityEngine;
using Mirror;
using System.Collections;

namespace TU.PerfNet
{
    public class SceneController : NetworkBehaviour
    {
        private static SceneController instance;

        public ServerOffsetter serverOffsetter;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        [Server]
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                LoadSceneOnAllClients(1, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(2, 3, 4, 5, 6);
            }
            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                LoadSceneOnAllClients(2, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(1, 3, 4, 5, 6);
            }
            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                LoadSceneOnAllClients(3, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(1, 2, 4, 5, 6);
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                LoadSceneOnAllClients(4, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(1, 2, 3, 5, 6);
            }
            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                LoadSceneOnAllClients(5, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(1, 2, 3, 4, 6);
            }
            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                LoadSceneOnAllClients(6, LoadSceneMode.Additive);
                UnloadSceneOnAllClients(1, 2, 3, 4, 5);
            }
        }

        public static void LoadSceneOnAllClients(int index, LoadSceneMode mode)
        {
            if (!SceneManager.GetSceneByBuildIndex(index).isLoaded)
            {
                instance.LoadOnClients(index, mode);
            }
        }

        public static void UnloadSceneOnAllClients(int index)
        {
            if (SceneManager.GetSceneByBuildIndex(index).isLoaded)
            {
                instance.UnloadOnClients(index);
            }
        }

        public static void UnloadSceneOnAllClients(params int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                Scene targetScene = SceneManager.GetSceneByBuildIndex(indices[i]);

                if (targetScene.buildIndex != -1) //Returns -1 if scene is invalid - it is not loaded and shouldn't be unloaded
                {
                    instance.UnloadOnClients(targetScene.buildIndex);
                }
            }
        }

        [ServerCallback]
        private void LoadOnClients(int index, LoadSceneMode mode)
        {
            ClientVRPositionSync[] clients = FindObjectsOfType<ClientVRPositionSync>();

            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].RpcSetLocalScene(index, mode);
            }

            StartCoroutine(LoadAndActivateScene(index, mode));
        }

        private IEnumerator LoadAndActivateScene(int index, LoadSceneMode mode)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(index, mode);

            while (!op.isDone)
            {
                yield return null;
            }

            Debug.Log("YOYOYO");

            yield return SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));

            DynamicGI.UpdateEnvironment();

        }

        [ServerCallback]
        private void UnloadOnClients(int index)
        {
            ClientVRPositionSync[] clients = FindObjectsOfType<ClientVRPositionSync>();

            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].RpcUnloadLocalScene(index);
            }

            AsyncOperation op = SceneManager.UnloadSceneAsync(index);
        }
    }
}