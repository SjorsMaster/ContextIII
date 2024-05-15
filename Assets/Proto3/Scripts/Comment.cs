using Mirror;
using SharedSpaces;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(InstantiableAnchorObject))]
public class Comment : NetworkBehaviour
{
    [SerializeField] private TextMeshPro commentTMP;
    [SerializeField] private CommentInputField commentInputFieldPrefab;

    [SyncVar(hook = nameof(SetCommentText))] private string commentText;

    private InstantiableAnchorObject instantiableAnchorObject;

    private CommentData commentData;

    #region Event Handlers
    public void SetCommentText(string oldText, string newText)
    {
        commentTMP.text = newText;
    }

    [ClientCallback]
    private void InstantiableAnchorObject_OnObjectIDUpdated(long newObjectID)
    {
        // At this point in time, the object is spawned in by the server. Thus we can load the comment data.
        if (CommentSaveData.CommentSaveDataDict.TryGetValue(newObjectID, out CommentData commentData))
        {
            CmdSetCommentText(commentData.CommentText);
        }
    }

    [Client]
    private void CommentInputFIeld_OnEndEdit(string newText)
    {
        CmdSetCommentText(newText);
    }
    #endregion

    #region Client
    [ClientCallback]
    private void OnEnable()
    {
        instantiableAnchorObject = GetComponent<InstantiableAnchorObject>();

        instantiableAnchorObject.OnObjectIDUpdated += InstantiableAnchorObject_OnObjectIDUpdated;
    }

    [ClientCallback]
    private void OnDisable()
    {
        instantiableAnchorObject.OnObjectIDUpdated -= InstantiableAnchorObject_OnObjectIDUpdated;
    }

    [ClientCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.GetComponent<TrackedAnchorObject>() == null)
        {
            return;
        }

        string name = other.GetComponent<TrackedAnchorObject>().name;
        if (name != "RightHand" && name != "LeftHand")
        {
            return;
        }

        CommentInputField temp = Instantiate(commentInputFieldPrefab);
        temp.OnEndEdit += CommentInputFIeld_OnEndEdit;
    }
    #endregion

    #region Server
    [Command(requiresAuthority = false)]
    private void CmdSetCommentText(string newText)
    {
        commentText = newText;
    }
    #endregion
}
