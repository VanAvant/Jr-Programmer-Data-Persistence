using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    [SerializeField] Text bestScoreText;

    private void Awake()
    {
        SetBestScore(StartMenuUI.highScoreTable);
    }

    public void SetBestScore(StartMenuUI.HighScoreTable highScoreTable)
    {
        string newString = "Best Score: \n" + highScoreTable.entries[0].name + ": " + highScoreTable.entries[0].score;
        bestScoreText.text = newString;
    }
}
