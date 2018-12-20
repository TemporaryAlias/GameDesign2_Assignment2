using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{

    [SerializeField] Text stainsText, bunniesText, germsText, enemiesLeft;
    
    void Update()
    {
        stainsText.text = "Stains Cleaned: " + LevelManager.instance.stainsKilled.ToString();
        bunniesText.text = "Dust Bunnies Cleaned: " + LevelManager.instance.dustKilled.ToString();
        germsText.text = "Germs Cleaned: " + LevelManager.instance.germsKilled.ToString();
        enemiesLeft.text = "Enemies Remaining: " + LevelManager.instance.enemiesLeft;
    }
}
