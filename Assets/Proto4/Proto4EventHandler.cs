using SharedSpaces;
using UnityEngine;

public class Proto4EventHandler : MonoBehaviour
{
    [SerializeField] private TrackingDataMsgHandler handler;

    #region Event Handlers
    private void Proto3EventHandler_CommentWasSpawned()
    {
        ToggleComments(true);
    }
    #endregion

    private void OnEnable()
    {
        Proto3EventHandler.CommentWasSpawned += Proto3EventHandler_CommentWasSpawned;
    }

    private void OnDisable()
    {
        Proto3EventHandler.CommentWasSpawned -= Proto3EventHandler_CommentWasSpawned;
    }

    public void ToggleComments(bool value)
    {
        foreach (var comment in FindObjectsOfType<Comment>())
        {
            DynamicAnchoredObject obj = comment.gameObject.GetComponent<DynamicAnchoredObject>();
            if (obj != null)
            {
                obj.Visuals.SetActive(value);
            }
        }

        handler.ToggleLines(value);
    }
}
