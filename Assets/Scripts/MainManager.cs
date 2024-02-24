using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private int m_bestPoint;
    private PlayerData m_playerData;

    private bool m_GameOver = false;

    private void Awake()
    {
        LoadPlayerDataJSON();
    }

    // Start is called before the first frame update

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
        AssignBestPoint();
        GameOverText.SetActive(true);
    }

    private void AssignBestPoint()
    {
        if (m_playerData.bestScore < m_Points)
        {
            m_playerData.bestScore = m_Points;
            SavePlayerDataJSON();
        }
        BestScoreText.gameObject.SetActive(true);
        BestScoreText.text = $"Best Score {MenuManager.Instance.playerName} : {m_playerData.bestScore}";
    }

    private void SavePlayerDataJSON()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        string json = JsonUtility.ToJson(m_playerData);
        File.WriteAllText(path, json);
    }

    private void LoadPlayerDataJSON()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            m_playerData = JsonUtility.FromJson<PlayerData>(json);
            AssignBestPoint();
            BestScoreText.gameObject.SetActive(true);
        }
        else
        {
            BestScoreText.gameObject.SetActive(false);
        }
    }
}

public class PlayerData
{
    public int bestScore;
}