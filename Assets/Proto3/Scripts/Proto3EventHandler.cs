using SharedSpaces;
using SharedSpaces.Managers;
using System;
using System.Collections;
using UnityEngine;

public class Proto3EventHandler : MonoBehaviour
{
    [SerializeField] private CommentInputField CommentInputFieldPrefab;

    [SerializeField] private float cooldown = 10f;

    private float timeSinceLastComment = 0f;


    #region Comment Creation
    // When the player opens the keyboard, their controllers are disabled.
    // We save these values so we can create the comment at the correct position.
    private string targetSaveUUID;
    private Vector3 targetSavePosition;
    private Quaternion targetSaveRotation;
    public void CreateEmptyComment()
    {
        if (Time.time - timeSinceLastComment < cooldown)
        {
            return;
        }
        timeSinceLastComment = Time.time;

        AnchoredObject target = LocalPlayerManager.LeftHand;

        targetSaveUUID = target.AnchorUUID;
        targetSavePosition = target.transform.localPosition;
        targetSaveRotation = target.transform.localRotation;

        CommentInputField newComment = Instantiate(CommentInputFieldPrefab, target.transform.position + target.transform.forward, target.transform.rotation);
        newComment.OnEndEdit += (text) => CreateComment(text, targetSaveUUID, targetSavePosition, targetSaveRotation);
    }

    private void CreateComment(
        string text,
        string uuid,
        Vector3 localPosition,
        Quaternion localRotation)
    {
        Guid objectID = Guid.NewGuid();
        AnchoredObjectsManager.Instance.CmdSpawnAnchoredObject(
            "ChatBubble",
            uuid,
            objectID.ToString(),
            localPosition,
            localRotation);

        StartCoroutine(SetCommentText(objectID, text));
    }

    private IEnumerator SetCommentText(Guid objectID, string text)
    {
        float timeout = 5f;
        float time = 0f;

        while (!AnchoredObjectsManager.AnchoredObjects.ContainsKey(objectID))
        {
            if (time >= timeout)
            {
                yield break;
            }

            time += Time.deltaTime;

            yield return null;
        }

        var anchoredObject = AnchoredObjectsManager.AnchoredObjects[objectID];
        Comment comment = anchoredObject.GetComponent<Comment>();
        comment.CmdSetCommentText(text);
    }

    //public void CorrectCommentData()
    //{
    //    foreach (var data in PersistentDataManagerProto3.PersistentData.CommentData)
    //    {
    //        if (AnchoredObjectsManager.Instance.AnchoredObjects.TryGetValue(data.ObjectID, out AnchoredObject anchoredObject))
    //        {
    //            Comment comment = anchoredObject.GetComponent<Comment>();
    //            comment.CmdSetCommentText(data.CommentText);
    //        }
    //    }
    //}
    #endregion
}

