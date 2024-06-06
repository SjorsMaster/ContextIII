using UnityEngine;

public class SimpleSceneSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDestroy;

    public void LoadScene(int sceneIndex)
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadScene(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            LoadScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadScene(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadScene(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadScene(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LoadScene(8);
        }
    }
}
