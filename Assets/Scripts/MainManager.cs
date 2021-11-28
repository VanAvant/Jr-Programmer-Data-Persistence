using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    // // // // // // 
    [SerializeField] StartMenuUI menuUI;
    [SerializeField] Text playerNameText;
    private string playerNameString;

    [SerializeField] BestScore bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        playerNameString = StartMenuUI.playerNameString;
        //Debug.Log("Player name: " + playerNameString);
        playerNameText.text = playerNameString;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateHighScoreTable();
    }

    public void UpdateHighScoreTable()
    {
        StartMenuUI.HighScoreTable newHighScoreTable = new StartMenuUI.HighScoreTable();
        StartMenuUI.HighScoreTable existingTable = StartMenuUI.highScoreTable;
        bool addedNewEntry = false;

        for (int i =0; i < StartMenuUI.highScoreTable.entries.Count; i++)
        {
            if (!addedNewEntry && m_Points > existingTable.entries[i].score)
            {
                addedNewEntry = true;
                StartMenuUI.HighScoreTable.Entry newEntry = new StartMenuUI.HighScoreTable.Entry
                {
                    name = playerNameString,
                    score = m_Points
                    };

                newHighScoreTable.entries.Add(newEntry);

                i++;
            }
            newHighScoreTable.entries.Add(existingTable.entries[i]);
        }
        if (addedNewEntry)
        {
            menuUI.ScoreDataSave(newHighScoreTable);
            StartMenuUI.highScoreTable = newHighScoreTable;
            bestScoreText.SetBestScore(StartMenuUI.highScoreTable);
        }
    }
}
