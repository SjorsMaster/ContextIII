using SharedSpaces;
using SharedSpaces.Data;
using SharedSpaces.Managers;
using SharedSpaces.SaveSystem;
using System;
using System.Collections;
using UnityEngine;

public class Proto3EventHandler : MonoBehaviour
{
    [SerializeField] private CommentInputField CommentInputFieldPrefab;

    [SerializeField] private float cooldown = 10f;

    private float timeSinceLastComment = 0f;

    #region Comment Creation
    public void CreateEmptyComment()
    {
        if (Time.time - timeSinceLastComment < cooldown)
        {
            return;
        }
        timeSinceLastComment = Time.time;

        AnchoredObject head = LocalPlayerManager.Instance.Head;

        CommentInputField newComment = Instantiate(CommentInputFieldPrefab, head.transform.position + head.transform.forward, head.transform.rotation);
        newComment.OnEndEdit += (text) => CreateComment(text, head.AnchorUUID, head.transform.localPosition + head.transform.forward, head.transform.localRotation);
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

        while (!AnchoredObjectsManager.Instance.AnchoredObjects.ContainsKey(objectID))
        {
            if (time >= timeout)
            {
                yield break;
            }

            time += Time.deltaTime;

            yield return null;
        }

        var anchoredObject = AnchoredObjectsManager.Instance.AnchoredObjects[objectID];
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

