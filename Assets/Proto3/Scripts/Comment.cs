using Mirror;
using SharedSpaces;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(InstantiableAnchorObject))]
public class Comment : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI commentText;

    private InstantiableAnchorObject instantiableAnchorObject;

    private CommentData commentData;

    private void OnEnable()
    {
        instantiableAnchorObject = GetComponent<InstantiableAnchorObject>();

        // At this point in time, the object is spawned in by the server. Thus we can load the comment data.
        commentData = CommentSaveData.CommentSaveDataDict[instantiableAnchorObject.ObjectID];

        SetCommentText(commentData.CommentText);
    }

    public void SetCommentText(string text)
    {
        commentText.text = text;
    }
}
