using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RACE_TYPE
{
    NONE = 0,
    RACE = 1,
    QUALIFIER = 2
}
public enum TRACK
{
    NONE=0,
    BEGINNER_TRACK=1,
    ADVANCED_TRACK=2
}
public class LapSystem : MonoBehaviour
{
    Rigidbody m_rigidBody;
    [System.NonSerialized]
    public float m_totalLap = 0;
    public GameObject StartScene, WingSelectionScene;
    public GameObject ReloadScene;
    public VehicleController Player;
    public float time=0,lastLapTime = 0;
    bool isCounterStart;

    public int m_degradeTillLap = 20;
    [HideInInspector]
    public static int m_maxSteerAngle = 15, m_minSteerAngle = 5;
    private void Start()
    {
        m_totalLap = RaceData.INSTANCE.m_numLaps;
        Player.state = RaceData.INSTANCE.m_raceType;
        WingSelectionScene.SetActive(false);
    }
    public void OnClickRaceBtn()
    {
        Debug.Log("Race Button____________________");
        m_totalLap = 5;
        Player.state = RACE_TYPE.RACE;
        StartScene.SetActive(false);
        WingSelectionScene.SetActive(true);
    }
    public void OnClickQualifierBtn()
    {
        m_totalLap = 1;
        Player.state = RACE_TYPE.QUALIFIER;
        StartScene.SetActive(false);
        WingSelectionScene.SetActive(true);

    }
    public void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void Update()
    {
        time += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        lastLapTime = time;
        Debug.LogWarning(other.gameObject.name);

        // Debug.LogError(other.gameObject.name);
        if (other.gameObject.tag.Equals("PlayerCollider"))
        {
            if (other.gameObject.GetComponent<VehicleController>().m_currentLap == m_totalLap)
            {
                other.gameObject.GetComponent<VehicleController>().m_isRaceFinished = true;
                Debug.Log("finished");
                ReloadScene.transform.parent.gameObject.SetActive(true);
                ReloadScene.SetActive(true);
                other.gameObject.GetComponent<VehicleController>().OnFinishRace();
            }
            else
            {
                other.gameObject.GetComponent<VehicleController>().m_currentLap++;
                Debug.Log("incrementing"+ other.gameObject.GetComponent<VehicleController>().m_currentLap);
                Debug.LogError(other.gameObject.GetComponent<VehicleController>().m_currentLap);
               if(other.gameObject.GetComponent<VehicleController>().m_currentLap>1)
                    other.gameObject.GetComponent<VehicleController>().Degradation();
                // other.gameObject.GetComponent<VehicleController>().Degrade((m_maxSteerAngle -m_minSteerAngle)/ m_degradeTillLap);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("PlayerCollider"))
        {

            time = 0;
        }
    }
}
