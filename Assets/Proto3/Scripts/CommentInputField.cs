using System;
using TMPro;
using UnityEngine;

public class CommentInputField : MonoBehaviour
{
    public Action<string> OnEndEdit;

    public delegate string SomeDelegate(string text);
    
    SomeDelegate mDelegate;

    [SerializeField] private TMP_InputField commentInputField;

    private void Awake()
    {
        commentInputField.onEndEdit.AddListener(EndEdit);

        commentInputField.Select();

        mDelegate += (s) =>
        {
            Debug.Log(s);
            return s;
        };
    }

    private void EndEdit(string text)
    {
        OnEndEdit?.Invoke(text);

        Destroy(gameObject);
    }
}
