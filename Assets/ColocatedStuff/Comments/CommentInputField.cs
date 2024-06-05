using System;
using TMPro;
using UnityEngine;

public class CommentInputField : MonoBehaviour
{
    public Action<string> OnEndEdit;

    public delegate string SomeDelegate(string text);

    [SerializeField] private TMP_InputField commentInputField;

    private void Awake()
    {
        commentInputField.onEndEdit.AddListener(EndEdit);

        commentInputField.Select();
    }

    private void EndEdit(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        OnEndEdit?.Invoke(text);

        Destroy(gameObject);
    }
}
