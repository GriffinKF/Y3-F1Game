using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceData : MonoBehaviour
{
    public int m_numLaps = 1;
    //public int m_RtotalLap;
    public RACE_TYPE m_raceType = RACE_TYPE.QUALIFIER;
    public TRACK m_track = TRACK.ADVANCED_TRACK;
    public bool m_frontWingsOn = false;
    public bool m_rearWingsOn = false;
    static RaceData g_instance;
    private void Start()
    {
        g_instance = this;
        DontDestroyOnLoad(gameObject);

    }
    public static RaceData INSTANCE
    {
        get
        {
            return g_instance;
        }
    }
}
