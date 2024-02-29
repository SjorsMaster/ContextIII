using MacUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenLog : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private Queue<string> stringQueue = new();

    private int current;

    public void DebugMessage(string message)
    {
        StartCoroutine(MessageRemoveDelay(message, 5));
    }

    /// <summary>
    /// Removes the message after some time in seconds.
    /// </summary>
    /// <param name="delay">The delay in seconds.</param>
    /// <returns></returns>
    private IEnumerator MessageRemoveDelay(string message, float delay)
    {
        if (string.IsNullOrEmpty(message))
            throw new Exception("Message was empty");

        stringQueue.Enqueue($"{current}: {message}");
        current++;

        UpdateText();

        yield return WaitForSecondsPool.GetWaitForSeconds(delay);
        stringQueue.Dequeue();

        UpdateText();

        if (stringQueue.Count == 0)
            current = 0;
    }

    private void UpdateText()
    {
        string result = string.Empty;
        foreach (string v in stringQueue)
            result += $"{v}\v";

        text.text = result;
    }
}
