using System.Collections.Generic;
using UnityEngine;

public enum ButtonState
{
	UNPRESSED,
	PRESSED
}

public class Button : MonoBehaviour
{
    public float requiredWeight = 1f;

    public List<Button> group = new List<Button>();
    public List<ButtonAction> buttonActions;

    private ButtonAnimation buttonAnim;

    private float pressure = 0;
    private bool performed = false;

    ButtonState oldState = ButtonState.UNPRESSED;

	public ButtonState GetState()
	{
		return pressure >= requiredWeight ? ButtonState.PRESSED : ButtonState.UNPRESSED;
	}

	private void Start()
    {
		buttonAnim = GetComponentInChildren<ButtonAnimation>();
        if (!group.Contains(this)) group.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if ( rb ) pressure += rb.mass;

        //Debug.Log(pressure);

        CheckState();
    }

    private void OnTriggerExit(Collider other)
    {
		Rigidbody rb = other.GetComponent<Rigidbody>();
		if (rb) pressure -= rb.mass;

		//Debug.Log(pressure);

		CheckState();
	}

    private void CheckState()
    {
        ButtonState newState = GetState();
        if ( newState != oldState )
        {
            if (newState == ButtonState.PRESSED) buttonAnim.Do();
            else buttonAnim.Undo();
            oldState = newState;
        }
    }

    private void Update()
    {
        if (buttonActions.Count > 0 && this == group[0] && Time.frameCount % 10 == 0 )
        {
            bool performAction = true;
            foreach( Button b in group )
            {
                if (b.GetState() != ButtonState.PRESSED)
                {
                    performAction = false;
                    break;
                }
            }
            if (performAction && !performed)
            {
                foreach( ButtonAction ba in buttonActions ) ba.Do();
                performed = true;

			}
            else if ( !performAction && performed)
            {
				foreach (ButtonAction ba in buttonActions) ba.Undo();
                performed = false;
			}
        }
    }
}
