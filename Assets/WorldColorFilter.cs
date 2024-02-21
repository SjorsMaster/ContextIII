using PortalsVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldColorFilter : MonoBehaviour
{
	public List<WorldFilterSettings> worldFilterSettings = new List<WorldFilterSettings>();

	public PortalTraveller relativeTraveller;
	string currentWorld = "";

	public Material m;

	[System.Serializable]
	public struct WorldFilterSettings
	{
		public string name;
		public Color color;
	}

	private void Awake()
	{
		if (relativeTraveller)
		{
			relativeTraveller.onWorldChanged += ActiveWorldChanged;
		}
	}

	// Update is called once per frame
	public void ActiveWorldChanged(string newWorld)
	{
		WorldFilterSettings settings = FindSettings(newWorld);
		if (string.IsNullOrEmpty(currentWorld))
		{
			// immediately set the new values
			HardSet(settings);
		}
		else
		{
			// get the new values, and animate to them over a second (based on current status)
			StartCoroutine(ApplySettings(settings));
		}

		currentWorld = newWorld;
	}

	WorldFilterSettings FindSettings(string world)
	{
		for (int i = 0; i < worldFilterSettings.Count; ++i)
		{
			if (worldFilterSettings[i].name == world)
			{
				return worldFilterSettings[i];
			}
		}

		Debug.LogError($"NO WORLD SETTINGS FOUND FOR: {world}");
		return worldFilterSettings[0];
	}

	IEnumerator ApplySettings(WorldFilterSettings newSettings)
	{
		float t = 0;

		// Get the old color
		Color oldColor, newColor;
		oldColor = m.GetColor("_Color");

		while (t < 1f)
		{
			newColor = Color.Lerp(oldColor, newSettings.color, Mathf.Clamp01(t));
			m.SetColor("_Color", newColor);

			t += 0.01f;// Time.deltaTime;
			yield return null;
		}

		// Apply one final time, and enable/disable accordingly
		HardSet(newSettings);

		yield return null;
	}

	void HardSet(WorldFilterSettings settings)
	{
		m.SetColor("_Color", settings.color);
	}
}
