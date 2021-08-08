using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    public Text m_frontWingText;
    public Text m_rearWingText;
    public GameObject m_startScreen;
    public GameObject m_wingsScreen;
    public GameObject m_trackSelectionUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Race()
    {
        RaceData.INSTANCE.m_numLaps = 50;
        RaceData.INSTANCE.m_raceType = RACE_TYPE.RACE;
        m_startScreen.SetActive(false);
        m_trackSelectionUI.SetActive(true);
       // m_wingsScreen.SetActive(true);
    }
    public void Qualifier()
    {
        RaceData.INSTANCE.m_numLaps = 1;
        RaceData.INSTANCE.m_raceType = RACE_TYPE.QUALIFIER;
        m_startScreen.SetActive(false);
        m_trackSelectionUI.SetActive(true);
        //m_wingsScreen.SetActive(true);
    }
    public void FrontWingOn()
    {
        if (RaceData.INSTANCE.m_frontWingsOn)
            m_frontWingText.text = "Front Wing Down";
        else
            m_frontWingText.text = "Front Wing Up";
        RaceData.INSTANCE.m_frontWingsOn = !RaceData.INSTANCE.m_frontWingsOn;
      
    }
    public void RearWingOn()
    {
        if (RaceData.INSTANCE.m_rearWingsOn)
            m_rearWingText.text = "Rear Wing Down";
        else
            m_rearWingText.text = "Rear Wing Up";
        RaceData.INSTANCE.m_rearWingsOn = !RaceData.INSTANCE.m_rearWingsOn;
       
    }

    // Update is called once per frame
    public void MoveToGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnClickBeginnerTrack()
    {
        m_trackSelectionUI.SetActive(false);
        RaceData.INSTANCE.m_track = TRACK.BEGINNER_TRACK;
        m_wingsScreen.SetActive(true);
    }
    public void OnClickAdvancedTrack()
    {
        m_trackSelectionUI.SetActive(false);
        RaceData.INSTANCE.m_track = TRACK.ADVANCED_TRACK;
        m_wingsScreen.SetActive(true);
    }
}
