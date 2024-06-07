using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointTracker : MonoBehaviour
{

    Vector2 _score = Vector2.zero;
    [SerializeField] TextMeshPro text;

    public void ChangeScore(int score1, int score2) {Vector2 tmp = new Vector2(score1, score2); _score += tmp; text.text = $"{_score.x}-{_score.y}"; }
}
