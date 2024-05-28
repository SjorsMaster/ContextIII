using Meta.XR.MRUtilityKit;
using Mirror;
using Oculus.Interaction;
using SharedSpaces;
using SharedSpaces.Data;
using System;
using System.Collections;
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

        if (CommentSaveData.CommentSaveDataDict.ContainsKey(Guid.Parse(anchoredObject.ObjectID)))
        {
            CommentSaveData.CommentSaveDataDict[Guid.Parse(anchoredObject.ObjectID)] = new()
            {
                ObjectID = anchoredObject.ObjectID,
                CommentText = newText,
            };
        }
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
        StartCoroutine(OnObjectIDSet());
    }

    [ClientCallback]
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator OnObjectIDSet()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(anchoredObject.ObjectID));


        if (CommentSaveData.CommentSaveDataDict.TryGetValue(Guid.Parse(anchoredObject.ObjectID), out CommentData commentData))
        {
            CmdSetCommentText(commentData.CommentText);
        }
        else
        {
            CommentSaveData.CommentSaveDataDict.Add(Guid.Parse(anchoredObject.ObjectID), new()
            {
                ObjectID = anchoredObject.ObjectID,
                CommentText = commentText,
            });
        }
    }
    #endregion

    #region Server
    [Command(requiresAuthority = false)]
    public void CmdSetCommentText(string newText)
    {
        commentText = newText;
        commentTMP.text = newText;
    }
    #endregion
}
