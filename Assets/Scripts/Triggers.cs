using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Triggers : MonoBehaviour
{

    public GameObject player;
    bool respawn;
    bool nextLevel;

    public Text timerText;
    private float startTime;
    private float t;
    private float dummyTime = 180f;

    public Text bestText;
    string bestTimeKey;
    private float bestTime = 0f;

    //GUI Variables
    public int buttonWidth;
    public int buttonHeight;

    private int origin_x;
    private int origin_y;

    void Start()
    {
       //PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        bestTimeKey = "Best" + SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.HasKey(bestTimeKey))
            bestTime = PlayerPrefs.GetFloat(bestTimeKey);
        else
        {
            PlayerPrefs.SetFloat(bestTimeKey, dummyTime);
            bestTime = PlayerPrefs.GetFloat(bestTimeKey);
        }
        startTime = Time.time;
        respawn = false;
        buttonWidth = 200;
        buttonHeight = 50;
        origin_x = Screen.width / 2 - buttonWidth / 2;
        origin_y = Screen.height / 2 - buttonHeight * 2;
        HighScore();
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        if (t > 60)
            timerText.text ="Current: " + minutes + ":" + seconds + " s";
        else
            timerText.text ="Current: " + seconds + " s";
    }

    void HighScore()
    {
        float currTime = PlayerPrefs.GetFloat(bestTimeKey);
        string minutes = ((int)currTime / 60).ToString();
        string seconds = (currTime % 60).ToString("f2");
        if (currTime > 60)
            bestText.text ="Best: " + minutes + ":" + seconds + " s" ;
        else
            bestText.text ="Best: " + seconds + " s";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Time.timeScale = 0;
            respawn = true;
            
        }
        else if(other.gameObject.tag == "Door")
        {
            nextLevel = true;
            if (t < bestTime)
            {
                print("NEW HIGH SCORE");
                PlayerPrefs.SetFloat(bestTimeKey, t);
                PlayerPrefs.Save();                
            }
            Time.timeScale = 0;
            HighScore();
        }
    }

    void OnGUI()
    {
        if (respawn)
        {
            if (GUI.Button(new Rect(origin_x, origin_y + 50, buttonWidth, buttonHeight), "Retry?"))
            {
                if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
                SceneManager.LoadScene(1);
                else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2"))
                    SceneManager.LoadScene(2);
            }
            if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 60, buttonWidth, buttonHeight), "Main Menu"))
            {
                SceneManager.LoadScene(0);
            }
            if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight * 2 + 70, buttonWidth, buttonHeight), "Exit"))
            {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
				Application.Quit();
            #endif
            }
        }
        else if (nextLevel)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1"))
            {
                if (GUI.Button(new Rect(origin_x, origin_y + 50, buttonWidth, buttonHeight), "Next Level"))
                {
                    SceneManager.LoadScene(2);
                }
                if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 60, buttonWidth, buttonHeight), "Replay Level"))
                {
                    //   highScore();
                    SceneManager.LoadScene(1);
                }
                if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight * 2 + 70, buttonWidth, buttonHeight), "Exit"))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
                }
            }
            else if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2"))
            {
                if (GUI.Button(new Rect(origin_x, origin_y + 50, buttonWidth, buttonHeight), "Replay Level"))
                {
                    SceneManager.LoadScene(2);
                }
                if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight + 60, buttonWidth, buttonHeight), "Main Menu"))
                {
                    //   highScore();
                    SceneManager.LoadScene(0);
                }
                if (GUI.Button(new Rect(origin_x, origin_y + buttonHeight * 2 + 70, buttonWidth, buttonHeight), "Exit"))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
                }
            }
        }
    }
}
