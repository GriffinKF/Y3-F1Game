using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    float m_lap = 3;

    public VehicleController m_mainPlayerCar;
    public Button PlayButton;
    public GameObject MainMenuPanel;
    private void Awake()
    {
        PlayButton.onClick.AddListener(OnClickStartButton);
    }
    private void Update()
    {
        if (m_mainPlayerCar.m_isRaceFinished)
            MainMenuPanel.SetActive(true);
    }
    void OnClickStartButton()
    {
        m_mainPlayerCar.m_isRaceFinished = false;
        //MainMenuPanel.SetActive(false);
    }
}
