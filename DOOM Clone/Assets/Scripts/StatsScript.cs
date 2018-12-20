using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{

    [SerializeField] Text stainsText, bunniesText, germsText;
    
    void Update()
    {
        stainsText.text = "Stains Cleaned: " + LevelManager.instance.stainsKilled.ToString();
        bunniesText.text = "Stains Cleaned: " + LevelManager.instance.dustKilled.ToString();
        germsText.text = "Stains Cleaned: " + LevelManager.instance.germsKilled.ToString();
    }
}
