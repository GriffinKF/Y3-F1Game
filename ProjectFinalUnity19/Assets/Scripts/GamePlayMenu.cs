using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Button NextButton;
    public GameObject car;
    public GameObject wingCanvas;
    private void Awake()
    {
       // car.SetActive(false);
    }

    private void OnEnable()
    {
        NextButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        NextButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        car.SetActive(true);
        Time.timeScale = 1f;

        // Hides the button
        NextButton.gameObject.SetActive(false);
    }

}
