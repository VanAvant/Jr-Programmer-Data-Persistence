using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] Text nameInputField;
    public static string playerNameString;
    private string saveFileLocation;
    private int numberOfHighScores = 10;
    public static HighScoreTable highScoreTable;

    //High score text fields
    [SerializeField] Text highScoreNames;
    [SerializeField] Text highScoreScores;
    [SerializeField] Text highScoreColons;

    void Start()
    {

        if (saveFileLocation == null)
        {
            saveFileLocation = Application.dataPath + "SaveFile.json";
            highScoreTable = ScoreDataLoad();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DisplayHighScores(highScoreTable);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameButton(int scene)
    {
        if (nameInputField.text.Length > 0)
        {
            LoadGameScene(scene);
        }
    }
    public void LoadGameScene(int scene)
    {
        playerNameString = nameInputField.text;
        SceneManager.LoadScene(scene);
    }

    private void DisplayHighScores(HighScoreTable highScoreTable)
    {
        string newHighScoreNames = "";
        string newHighScoreScores = "";
        string newColons = "";
        for (int i = 0; i < highScoreTable.entries.Count; i++)
        {
            newHighScoreNames += highScoreTable.entries[i].name + "\n";
            newHighScoreScores += highScoreTable.entries[i].score + "\n";
            newColons += ":\n";
        }
        highScoreNames.text = newHighScoreNames;
        highScoreScores.text = newHighScoreScores;
        highScoreColons.text = newColons;
    }

    //SAVE AND LOAD 

    [System.Serializable]
    public class HighScoreTable
    {
        
        public List<Entry> entries = new List<Entry>();

        [System.Serializable]
        public class Entry
        {
            public string name;
            public int score;
        }
    }

    public void ScoreDataSave(HighScoreTable highScoreTable)
    {
        string highScoreTableString = JsonUtility.ToJson(highScoreTable);
        File.WriteAllText(saveFileLocation, highScoreTableString);
    }

    public HighScoreTable ScoreDataLoad()
    {
        if (File.Exists(saveFileLocation))
        {
            string saveFileJson = File.ReadAllText(saveFileLocation);
            HighScoreTable highScoreTable = JsonUtility.FromJson<HighScoreTable>(saveFileJson);
            return highScoreTable;
        }
        else
        {
            HighScoreTable newHighScoreTable = new HighScoreTable();

            for (int i = 0; i < numberOfHighScores; i++)
            {
                HighScoreTable.Entry newEntry = new HighScoreTable.Entry
                {
                    name = "AAAAAA",
                    score = (numberOfHighScores - i) * 5
                };

                newHighScoreTable.entries.Add(newEntry);
            }
            ScoreDataSave(newHighScoreTable);
            return newHighScoreTable;
        }
    }
}
