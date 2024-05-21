using Mirror;
using Oculus.Interaction;
using SharedSpaces;
using SharedSpaces.Data;
using TMPro;
using UnityEngine;

public class Comment : NetworkBehaviour
{
    [SerializeField] private TextMeshPro commentTMP;
    [SerializeField] private AnchoredObject anchoredObject;

    [SyncVar(hook = nameof(OnCommentTextUpdated))] private string commentText;

    private CommentData commentData;

    #region Event Handlers
    [Client]
    private void OnCommentTextUpdated(string oldText, string newText)
    {
        commentTMP.text = newText;

        if (anchoredObject == null)
        {
            return;
        }

        if (CommentSaveData.CommentSaveDataDict.ContainsKey(anchoredObject.ObjectID))
        {
            CommentSaveData.CommentSaveDataDict[anchoredObject.ObjectID] = new()
            {
                ObjectID = anchoredObject.ObjectID,
                CommentText = newText,
            };
        }
    }

    [Client]
    private void AnchoredObject_OnDataSet(AnchoredObjectData data)
    {
        if (CommentSaveData.CommentSaveDataDict.TryGetValue(data.ObjectID, out CommentData commentData))
        {
            CmdSetCommentText(commentData.CommentText);
        }
        else
        {
            CommentSaveData.CommentSaveDataDict.Add(data.ObjectID, new()
            {
                ObjectID = data.ObjectID,
                CommentText = commentText,
            });
        }
    }

    [Client]
    public void CommentInputFIeld_OnEndEdit(string newText)
    {
        if (string.IsNullOrEmpty(newText))
        {
            Destroy(gameObject);
            return;
        }

        CmdSetCommentText(newText);
    }
    #endregion

    private void Awake()
    {
        anchoredObject = GetComponent<AnchoredObject>();

        syncDirection = SyncDirection.ServerToClient;
    }

    #region Client
    [ClientCallback]
    private void OnEnable()
    {
        anchoredObject.OnDataSet += AnchoredObject_OnDataSet;
    }

    [ClientCallback]
    private void OnDisable()
    {
        anchoredObject.OnDataSet -= AnchoredObject_OnDataSet;
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
