using System;
using UnityEngine;

public class ButtonPrefab : MonoBehaviour
{
    public Action<MiniGamePlayer> OnButtonPressed;

    public void PressButton(MiniGamePlayer player)
    {
        OnButtonPressed?.Invoke(player);
    }
}
