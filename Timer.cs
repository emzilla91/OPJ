using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public Text bestTime;
    public Text newBest;

    public float startTimeCount;
    public float pointsPerSec;
    public bool scoreIncreasing = true;

    public static float bestTimeCount = -1;


    void Start()
    {
        newBest.text = "";
        ReadFromFile();

        if (bestTimeCount != -1)
            bestTime.text = "Best time: " + Mathf.Round(Timer.bestTimeCount);
    }

    void Update()
    {

        if (scoreIncreasing)
        {
            startTimeCount += pointsPerSec + Time.deltaTime;
        }


        timerText.text = "Time: " + Mathf.Round(startTimeCount);
    }

    public void GameEnd()
    {

        if ((startTimeCount < Timer.bestTimeCount) || (Timer.bestTimeCount == -1))
        {
            Timer.bestTimeCount = startTimeCount;
            PlayerPrefs.SetFloat("BestTime", (Timer.bestTimeCount));

            newBest.text = "!!New best time!!";
        }
        bestTime.text = "Best time: " + Mathf.Round(Timer.bestTimeCount);

        StoreToFile();
    }

    private void StoreToFile()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        string specificFolder = Path.Combine(folder, "Soko");
        Directory.CreateDirectory(specificFolder);

        File.WriteAllText(Path.Combine(specificFolder, "besttime"), Timer.bestTimeCount.ToString());

    }

    private void ReadFromFile()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        string specificFolder = Path.Combine(folder, "Soko");

        if (File.Exists(Path.Combine(specificFolder, "besttime")))
        {
            Timer.bestTimeCount = float.Parse(File.ReadAllText(Path.Combine(specificFolder, "besttime")));
        }
    }
}
