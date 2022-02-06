using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public bool spawnTree;

    [SerializeField]
    private Transform tree;

    private float randomXPos;
    private float randomYPos;

    private float screenEdgeHorizontal;
    private float screenEdgeVertical;

    private int numOfRandomTrees;

    // the time waited before spawning a new tree
    private float randomTimeToWait;

    // the min and max values of the number of trees that should spawn
    private int numOfTreesMin;
    private int numOfTreesMax;

    // score
    [HideInInspector]
    public int playerScore;
    public TextMeshProUGUI textMesh;
    [SerializeField]
    private TextMeshProUGUI highscore;
    [SerializeField]
    private TextMeshProUGUI endHighscore;

    private GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        spawnTree = false;
        screenEdgeHorizontal = 5f;
        screenEdgeVertical = 3f;
        //numOfRandomTrees = Random.Range(1, 3);
        numOfRandomTrees = 0;
        randomTimeToWait = 5;
        numOfTreesMin = 1;
        numOfTreesMax = 3;
        playerScore = 0;
        gameOverScreen = GameObject.Find("game over screen");
        gameOverScreen.transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(SpawnTrees());
    }

    // Update is called once per frame
    void Update()
    {
        //CheckTime();
        Debug.Log(playerScore);
        if (spawnTree)
        {
            StartCoroutine(SpawnTrees());
            spawnTree = false;
        }
        textMesh.text = "Score: " + playerScore.ToString();
        highscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();

        if (playerScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
        }

        if (playerScore < 0)
        {
            GameOver();
        }
    }

    public void SpawnTree()
    {
        randomXPos = Random.Range(-screenEdgeHorizontal, screenEdgeHorizontal);
        randomYPos = Random.Range(-screenEdgeVertical, screenEdgeVertical);
        Instantiate(tree, new Vector3(randomXPos, randomYPos, 0), Quaternion.identity);
    }

    private IEnumerator SpawnTrees()
    {
        numOfRandomTrees = Random.Range(numOfTreesMin, numOfTreesMax);
        //Debug.Log(numOfRandomTrees);
        for (int i = 0; i < numOfRandomTrees; i++)
        {
            yield return new WaitForSeconds(randomTimeToWait);
            SpawnTree();
        }
    }

    private void CheckTime()
    {
        if (Time.timeSinceLevelLoad % 10f >= 0.001f && Time.timeSinceLevelLoad % 10f <= 0.05f && Time.timeSinceLevelLoad >= 10f)
        {
            
            numOfTreesMin += 3;
            numOfTreesMax += 5;
            randomTimeToWait -= 0.5f;
        }
    }

    private void GameOver()
    {
        if (playerScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", playerScore);
        }
        endHighscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        Time.timeScale = 0;
        gameOverScreen.transform.localScale = new Vector3(1, 1, 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
