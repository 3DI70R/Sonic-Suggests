using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCompletionCounter : MonoBehaviour
{
    public Text textToUpdate;
    public string[] lines;

    public void Start()
    {
        try
        {
            var completionCount = PlayerPrefs.GetInt("CompletionCount", 0);
            textToUpdate.text = lines[completionCount].Replace("\\n", "\n");
            PlayerPrefs.SetInt("CompletionCount", completionCount + 1);
        }
        catch (Exception e)
        {
            textToUpdate.text = e.ToString();
        }
    }
}
