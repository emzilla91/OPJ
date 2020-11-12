using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelsBuilder m_LevelsBuilder;
    public GameObject m_ResetButton;
    private bool m_ReadyForInput;
    private Player m_Player;
    public int brLevela;

    private Timer theTimer;

    void Start()
    {
        theTimer = FindObjectOfType<Timer>();
        m_ResetButton.SetActive(true);
        ResetScene();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
        if (moveInput.sqrMagnitude > 0.5)
        {
            if (m_ReadyForInput)
            {
                m_ReadyForInput = false;
                m_Player.Move(moveInput);
                EndGame();
            }
        }
        else
        {
            m_ReadyForInput = true;
        }
    }

    public void NextLevel()
    {
        m_LevelsBuilder.NextLevel();
        StartCoroutine(ResetSceneASync());
        brLevela++;
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneASync());
    }

    public void EndGame()
    {
        if (brLevela == 4 && IsLevelComplete())
        {          
            m_ResetButton.SetActive(false);
            theTimer.scoreIncreasing = false;
            theTimer.GameEnd();
        }
        else if (IsLevelComplete())
        {
            NextLevel();
        }
    }
    bool IsLevelComplete()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach (var box in boxes)
        {
            if (!box.m_OnCross) return false;
        }
        return true;
    }

    IEnumerator ResetSceneASync()
    {
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncUnload.isDone)
            {                                                                                         
                yield return null;
                Debug.Log("UnLoading...");
            }
            Debug.Log("Unload Done");
            Resources.UnloadUnusedAssets();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading...");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        m_LevelsBuilder.Build();
        m_Player = FindObjectOfType<Player>();
        Debug.Log("Level loaded " + brLevela);
    }
}