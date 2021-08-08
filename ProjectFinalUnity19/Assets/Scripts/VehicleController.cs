using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class VehicleController : CarController
{
    [System.NonSerialized]
    public float m_currentLap=0;
    [System.NonSerialized]
    public bool m_isRaceFinished=false;
    public GameObject driftcanvas, frontWingCanvas, rearWingCanvas;
    public float m_frontRightWheelSlipValue = 0;
    public float m_frontLeftWheelSlipValue = 0;
    public float m_rearRightWheelSlipValue = 0;
    public float m_rearLeftWheelSlipValue = 0;
    [SerializeField][Range(0,2)]public int frontWing, rearWing;
    /*private float m_frontRightWheelSlipValue = 0;
    private float m_frontLeftWheelSlipValue = 0;
    private float m_rearRightWheelSlipValue = 0;
    private float m_rearLeftWheelSlipValue = 0;*/
    WheelFrictionCurve WFC_FR, WFC_FL, WFC_RR, WFC_RL;
   // WheelCollider wheelCollider;
    public WheelCollider[] frontColliders, backColliders;
    bool isLeftWing=false, isRightWing=false;
    //FrontWingUpValues frontWingUpValues = new FrontWingUpValues();
    public FrontWheelValues frontWingsValues;
    public RearWheelValues rearWingsValues;
    public ChangeForFrontWing changeForFrontWing;
    public ChangeForRearWing changeForRearWing;
    public GameObject wingCanvas;
    public tyreValues[] tyre;
    public int m_tyreIndex = 3;
    public bool m_frontWing = false;
    public bool m_rearWing = false;
    public GameObject car, hud;
    public GameObject track1, track2;
    public GameObject LapNoText, TotalLapText, LastLapTimeText, TireText, CurrentLapTimeText, CurrentPos, pauseMenuCanvas, pauseBtn;
    public LapSystem lapSystem;
    float CurrentLapTime = 0;
    public Text DegradeTxt;
    public GameObject DegradeTxtUI;
    public Button StartBtn;
    public GameObject WingsObj;
    [Header("Current Tyre")]
     float fes, fev, fas, fav, res, rev, ras, rav;
    float fullTireVAlue = 10, currentDegradation = 0, currentTireValue = 0, degradeinEachLap = (1/3f);

    float CurrentTime;
    int tireLap = 1;
    float degPer = 8;
    public Text HighScoreTxt, TimeTxt;
   public RACE_TYPE state =0;
    public float m_maxAngle = 0;

    float m_softAngle=5, m_medAngle=10, m_roughAngle=15;
    int m_currentTireState = 0;

    [Serializable]public class FrontWheelValues
    {
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue;
        
    }
    [Serializable]public class ChangeForFrontWing
    {
        public float FExtremumSlip, FExtremumValue, FAsymptoteSlip, FAsymptoteValue;
        public float RExtremumSlip, RExtremumValue, RAsymptoteSlip, RAsymptoteValue;
    }
    [Serializable]public class ChangeForRearWing
    {
        public float FExtremumSlip, FExtremumValue, FAsymptoteSlip, FAsymptoteValue;
        public float RExtremumSlip, RExtremumValue, RAsymptoteSlip, RAsymptoteValue;
    }
    [Serializable]public class RearWheelValues
    {
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue;
    }
  
    [Serializable]public class tyreValues
    {
        public float FExtremumSlip, FExtremumValue, FAsymptoteSlip, FAsymptoteValue;
        public float RExtremumSlip, RExtremumValue, RAsymptoteSlip, RAsymptoteValue;
        
    }
    WheelFrictionCurve SideCurve = new WheelFrictionCurve();

    public Button LeftWing, RightWing;


    private void Awake()
    {
        m_isRaceFinished = true;
    }
    protected override void Start()
    {
        hud.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        CurrentLapTime = 0;
        m_currentTireState =3;
        // car.SetActive(false);
        fes = fev = fas = fav = res = rev = ras = rav = 0.5f;
      //  DegradeTxt.text = "DEGRADE:" +m_MaximumSteerAngle;
        m_maxAngle = m_MaximumSteerAngle;
        currentTireValue = m_maxAngle;
        driftcanvas.SetActive(false);
        base.Start();
       m_frontWing = RaceData.INSTANCE.m_frontWingsOn;
       m_rearWing = RaceData.INSTANCE.m_rearWingsOn;
        if (m_frontWing)
            OnFrontWingUp();
        if (m_rearWing)
            onRearWingUp();
        onNextButtonClick();
        //LeftWing.onClick.AddListener(LeftWingCallback);
        // RightWing.onClick.AddListener(RightWingCallback);
        if (RaceData.INSTANCE.m_track == TRACK.BEGINNER_TRACK)
        {
            transform.Rotate(0f, 0f, 0f);
            transform.localPosition = new Vector3(12.2f, 4f, 309.8f);
            track1.SetActive(true);
            track2.SetActive(false);
        }
        if (RaceData.INSTANCE.m_track == TRACK.ADVANCED_TRACK)
        {
           // transform.Rotate(0f, 60f, 0f);
            track1.SetActive(false);
            track2.SetActive(true);
        }
        
        if (RaceData.INSTANCE.m_raceType == RACE_TYPE.RACE)
        {
            Debug.Log("RAceType________________==   " + RACE_TYPE.RACE);
            Debug.Log("______________________Track type__________==   " + RaceData.INSTANCE.m_track);
            LapNoText.SetActive(true);
            TotalLapText.SetActive(true);
            LastLapTimeText.SetActive(true);
            TireText.SetActive(true);
            CurrentLapTimeText.SetActive(true);
            CurrentPos.SetActive(true);
        }
        if (RaceData.INSTANCE.m_raceType == RACE_TYPE.QUALIFIER)
        {
            Debug.Log("______________________Track type__________==   " + RaceData.INSTANCE.m_track);
            Debug.Log("RaceType________________==   " + RACE_TYPE.QUALIFIER);
            LapNoText.SetActive(false);
            TotalLapText.SetActive(false);
            LastLapTimeText.SetActive(false);
            TireText.SetActive(true);
            CurrentLapTimeText.SetActive(true);
            CurrentPos.SetActive(false);
            DegradeTxtUI.SetActive(false);
        }

    }
    void Update()
    {
        CurrentLapTime += Time.deltaTime;
        System.TimeSpan tm = System.TimeSpan.FromSeconds(CurrentLapTime);
        var CurrentLapTimeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", tm.Hours, tm.Minutes, tm.Seconds, tm.Milliseconds);
        //TimeTxt.text = "CurrentTime:" + CurrentLapTimeFormatted;
        LapNoText.GetComponent<Text>().text = m_currentLap.ToString();
        // TotalLapText.GetComponent<Text>().text = lapSystem.m_totalLap.ToString();
        TotalLapText.GetComponent<Text>().text = RaceData.INSTANCE.m_numLaps.ToString();
       // LastLapTimeText.GetComponent<Text>().text = CurrentLapTimeFormatted.ToString();
        CurrentLapTimeText.GetComponent<Text>().text = CurrentLapTimeFormatted.ToString();
        CurrentPos.SetActive(false);



       // DegradeTxt.text = "DEGRADE:" + currentDegradation.ToString("0") + "%";

        //  m_Rigidbody.velocity = m_Rigidbody.mass * new Vector3(0, -15, 0) * Time.deltaTime ;
        // m_Rigidbody.AddForce(new Vector3(0f,-150f,0f), ForceMode.Impulse);
        // float lift =downForce * m_Rigidbody.velocity.sqrMagnitude;
        // m_Rigidbody.AddForceAtPosition(lift * transform.up, transform.position);
        if (RaceData.INSTANCE.m_track == TRACK.ADVANCED_TRACK)
        {
            if (transform.localPosition.y != 3.6f)
                transform.localPosition = new Vector3(transform.localPosition.x, 3.6f, transform.localPosition.z);
        }
        if (RaceData.INSTANCE.m_track == TRACK.BEGINNER_TRACK)
        {
            if (transform.localPosition.y != 3.9f)
                transform.localPosition = new Vector3(transform.localPosition.x, 3.9f, transform.localPosition.z);
          
        }


        if (state == RACE_TYPE.QUALIFIER)
        {
           // PlayerPrefs.Save();
            if (m_isRaceFinished) return;
            CurrentTime += Time.deltaTime;
        }
        else if (state == RACE_TYPE.RACE)
        {


            if (m_isRaceFinished) return;


            CurrentTime += Time.deltaTime;


        }

    }
    protected override void GearChanging()
    {
        base.GearChanging();
    }
    public override void Move(float steering, float accel, float footbrake, float handbrake)
    {

        if (accel > 0.4f && handbrake < .01f)
        {
         
        }
        float brake = handbrake;
        if (Input.GetKey(KeyCode.Space))
        {
            brake *= 3;
        }
      
        

        base.Move( steering,  accel,  brake,  handbrake);
    }
    public void drift0(int a_index)
    {
        m_tyreIndex = a_index;
        if (a_index ==3)
        {
            m_MaximumSteerAngle = 15;
            degPer = 8;
           // degradeinEachLap = 15 / 30;
            //currentDegradation = 0;
            //fullTireVAlue = 15;
            TireText.GetComponent<Text>().text = "SOFT";
        }
        else if (a_index == 2)
        {
            m_MaximumSteerAngle = 10;
            degPer = 5;
            degradeinEachLap = 10 / 30;
            currentDegradation = 0;
            fullTireVAlue = 10;
            TireText.GetComponent<Text>().text = "MEDIUM";
        }
        else
        {
            m_MaximumSteerAngle = 5;
            degPer = 2;
            degradeinEachLap = 5 / 30;
            currentDegradation = 0;
            fullTireVAlue = 5;

            TireText.GetComponent<Text>().text = "HARD";
        }
        tireLap = 1;
       // m_MaximumSteerAngle = a_index == 2 ? 
       //     (LapSystem.m_maxSteerAngle- LapSystem.m_minSteerAngle) : 
        //    (a_index == 3)?LapSystem.m_maxSteerAngle: LapSystem.m_minSteerAngle;
        m_currentTireState = a_index;
        DegradeTxt.text = "DEGRADE:0" + "%";

    }
    public void SetTraction(float value)
    {
        m_TractionControl = value;
    }
    public void LeftWingCallback()
    {
        if (!isLeftWing)
        {
            m_TractionControl = m_TractionControl + 0.15f;
            isLeftWing = true;
            LeftWing.GetComponentInChildren<Text>().text = "LeftWingUp";
        }
        else
        {
            isLeftWing = false;

            m_TractionControl = m_TractionControl - 0.15f;
            LeftWing.GetComponentInChildren<Text>().text = "LeftWingDown";

        }
    }
    public void RightWingCallback()
    {
        if (!isRightWing)
        {
            isRightWing = true;
            RightWing.GetComponentInChildren<Text>().text = "RightWingUp";

            m_TractionControl = m_TractionControl + 0.15f;
        }
        else
        {
            isRightWing = false;
            RightWing.GetComponentInChildren<Text>().text = "RightWingDown";

            m_TractionControl = m_TractionControl - 0.15f;
        }
    }
    private void setSlip()
    {
        WFC_FR = new WheelFrictionCurve();
        WFC_FR.extremumSlip = m_frontRightWheelSlipValue;
        WFC_FR.extremumValue = 1;
        WFC_FR.asymptoteSlip = .25f;
        WFC_FR.asymptoteValue = .75f;
        WFC_FR.stiffness = 1;

        WFC_FL = WFC_RR = WFC_RL = WFC_FR;
        WFC_FL.extremumSlip = m_frontLeftWheelSlipValue;
        WFC_RR.extremumSlip = m_rearRightWheelSlipValue;
        WFC_RL.extremumSlip = m_rearLeftWheelSlipValue;

        m_WheelColliders[0].sidewaysFriction = WFC_FR;
        m_WheelColliders[1].sidewaysFriction = WFC_FL;
        m_WheelColliders[2].sidewaysFriction = WFC_RR;
        m_WheelColliders[3].sidewaysFriction = WFC_RL;
    }
    public void setFrontWheel(float a, float b, float c, float d)
    {

        foreach (WheelCollider collider in frontColliders)
        {
            
           
            SideCurve.extremumSlip = a;// frontWingsValues.SExtremumSlip;
            SideCurve.extremumValue = b;// frontWingsValues.SExtremumValue;
            SideCurve.asymptoteSlip = c;// frontWingsValues.SAsymptoteSlip;
            SideCurve.asymptoteValue = d;// frontWingsValues.SAsymptoteValue;
            SideCurve.stiffness = 1;
            collider.sidewaysFriction = SideCurve;
        }
    //    tyre = new tyreValues(frontWingsValues.SExtremumSlip,frontWingsValues.SExtremumValue,frontWingsValues.SAsymptoteSlip,frontWingsValues.SAsymptoteValue,
      //      rearWingsValues.SExtremumSlip,rearWingsValues.SExtremumValue,rearWingsValues.SAsymptoteSlip,rearWingsValues.SAsymptoteValue);
    }
    public void setRearWheel(float a, float b, float c, float d)
    {
       // rearWingsValues= new RearWingsValues(a, b, c, d);
        foreach (WheelCollider collider in backColliders)
        {
            
            //SerializeClass.RearWingDownValues rearWingDownValues = new SerializeClass.RearWingDownValues(1f, 2f, 3f, 4f);
            SideCurve.extremumSlip = a;// rearWingsValues.SExtremumSlip;
            SideCurve.extremumValue = b;// rearWingsValues.SExtremumValue;
            SideCurve.asymptoteSlip = c;// rearWingsValues.SAsymptoteSlip;
            SideCurve.asymptoteValue = d;// rearWingsValues.SAsymptoteValue;
            SideCurve.stiffness = 1;
            collider.sidewaysFriction = SideCurve;
        }
     //   tyre = new tyreValues(frontWingsValues.SExtremumSlip, frontWingsValues.SExtremumValue, frontWingsValues.SAsymptoteSlip, frontWingsValues.SAsymptoteValue,
         //   rearWingsValues.SExtremumSlip, rearWingsValues.SExtremumValue, rearWingsValues.SAsymptoteSlip, rearWingsValues.SAsymptoteValue);
    }
    public void OnFrontWingUp()
{
        //  setFrontWheel(frontWingsValues.SExtremumSlip,frontWingsValues.SExtremumValue,frontWingsValues.SAsymptoteSlip,frontWingsValues.SAsymptoteValue);
        // m_frontWing = a_wing;
        /*  if (m_frontWing)
          {
           //   m_frontWing = false;
            //  RightWing.GetComponentInChildren<Text>().text = "FronttWingDown";
              fes -= frontWingsValues.SExtremumSlip;
              fev -= frontWingsValues.SExtremumValue;
              fas -= frontWingsValues.SAsymptoteSlip;
              fav -= frontWingsValues.SAsymptoteValue;
          }
          else
          {
            //  m_frontWing = true;
            //  RightWing.GetComponentInChildren<Text>().text = "FronttWingUp";


              fes += frontWingsValues.SExtremumSlip;
              fev += frontWingsValues.SExtremumValue;
              fas += frontWingsValues.SAsymptoteSlip;
              fav += frontWingsValues.SAsymptoteValue;
          }*/
        if (m_frontWing)
        {
            //  m_frontWing = true;
            //  RightWing.GetComponentInChildren<Text>().text = "FronttWingUp";


            fes += frontWingsValues.SExtremumSlip;
            fev += frontWingsValues.SExtremumValue;
            fas += frontWingsValues.SAsymptoteSlip;
            fav += frontWingsValues.SAsymptoteValue;
        }

        // drift0(m_tyreIndex);
        // frontWingCanvas.SetActive(false);
    }
    public void onRearWingUp()
    {

        //setRearWheel(frontWingsValues.SExtremumSlip, frontWingsValues.SExtremumValue, frontWingsValues.SAsymptoteSlip, frontWingsValues.SAsymptoteValue);
        /* if (m_rearWing)
         {
             m_rearWing = false;
            // LeftWing.GetComponentInChildren<Text>().text = "RearWingDown";
             res -= rearWingsValues.SExtremumSlip;
             ras -= rearWingsValues.SAsymptoteSlip;
             rav -= rearWingsValues.SAsymptoteValue;
             rev -= rearWingsValues.SExtremumValue;
         }
         else
         {
             m_rearWing = true;
           //  LeftWing.GetComponentInChildren<Text>().text = "RearWingUp";


             res += rearWingsValues.SExtremumSlip;
             rev += rearWingsValues.SExtremumValue;
             ras += rearWingsValues.SAsymptoteSlip;
             rav += rearWingsValues.SAsymptoteValue;
         }*/
        if (m_rearWing)
        {
            res += rearWingsValues.SExtremumSlip;
            rev += rearWingsValues.SExtremumValue;
            ras += rearWingsValues.SAsymptoteSlip;
            rav += rearWingsValues.SAsymptoteValue;
        }
        //drift0(m_tyreIndex);
        // rearWingCanvas.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag.Equals("EntryPoint"))
        {
            driftcanvas.SetActive(true);
        }
        if (other.gameObject.tag.Equals("LapObj"))
        {
           
           
            //var rem = 100 / (m_currentTireState == 2 ? m_medAngle : (m_currentTireState == 3 ? m_roughAngle : m_softAngle));
            //Debug.LogError(rem);
           // DegradeTxt.text = "DEGRADE:" + (((m_currentTireState == 2 ? m_medAngle : (m_currentTireState == 3 ? m_roughAngle : m_softAngle)) - m_MaximumSteerAngle) *
            // rem).ToString("f00") + "%";
           
            //  m_MaximumSteerAngle -= degradeinEachLap;
            //  currentDegradation =100f-( (m_MaximumSteerAngle * 100f) / m_maxAngle);
            // DegradeTxt.text = "DEGRADE:" + currentDegradation.ToString("f00") + "%";
           
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("EntryPoint"))
        {
            driftcanvas.SetActive(false) ;
            setFrontWheel(fes, fev, fas, fav);
            setRearWheel(res, rev, ras, rav);
        }
        if (other.gameObject.tag.Equals("LapObj"))
        {
            Debug.Log("+++++++++++++++++++++++++++++");
            CurrentLapTime = 0;
            System.TimeSpan lstLpTm = System.TimeSpan.FromSeconds(lapSystem.lastLapTime);
            var lastLapTimeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", lstLpTm.Hours, lstLpTm.Minutes, lstLpTm.Seconds, lstLpTm.Milliseconds);
            if (m_currentLap == 2)
            {
                LastLapTimeText.GetComponent<Text>().text = lastLapTimeFormatted.ToString();
            }
            
        }

    }
    public void onNextButtonClick()
    {
       // float fes, fev, fas, fav, res, rev, ras, rav;
        setFrontWheel(fes, fev, fas, fav);
        setRearWheel(res, rev, ras, rav);
        driftcanvas.SetActive(false);
        m_isRaceFinished = false;
        car.SetActive(true);
     //   WingsObj.SetActive(false);
      //  StartBtn.gameObject.SetActive(false);



        //SceneManager.LoadScene("SampleScene");
    }
    public void OnFinishRace()
    {
        hud.SetActive(false);
        if (state == RACE_TYPE.QUALIFIER)
        {

            if (!PlayerPrefs.HasKey("hsQualify"))
            {
                PlayerPrefs.SetFloat("hsQualify", CurrentTime);
                PlayerPrefs.Save();

            }
            else
            {
                if (PlayerPrefs.GetFloat("hsQualify", 0.5f) > CurrentTime)
                {
                    PlayerPrefs.SetFloat("hsQualify", CurrentTime);
                    PlayerPrefs.Save();
                }
            }

            System.TimeSpan t = System.TimeSpan.FromSeconds(CurrentTime);
            var timerFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            TimeTxt.text = "CurrentTime:" + timerFormatted;

            System.TimeSpan h = System.TimeSpan.FromSeconds(PlayerPrefs.GetFloat("hsQualify"));
            var timerFormattedh = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", h.Hours, h.Minutes, h.Seconds, h.Milliseconds);
            HighScoreTxt.text = "HighScore:" + timerFormattedh;
        }
        else if (state == RACE_TYPE.RACE)
        {
            if (!PlayerPrefs.HasKey("hsRace"))
            {
                PlayerPrefs.SetFloat("hsRace", CurrentTime);
                PlayerPrefs.Save();

            }
            else
            {
                if (PlayerPrefs.GetFloat("hsRace", 0.5f) > CurrentTime)
                {
                    PlayerPrefs.SetFloat("hsRace", CurrentTime);
                    PlayerPrefs.Save();
                }
            }

            System.TimeSpan t = System.TimeSpan.FromSeconds(CurrentTime);
            var timerFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
            TimeTxt.text = "CurrentTime:" + timerFormatted;

            System.TimeSpan h = System.TimeSpan.FromSeconds(PlayerPrefs.GetFloat("hsRace"));
            var timerFormattedh = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", h.Hours, h.Minutes, h.Seconds, h.Milliseconds);
            HighScoreTxt.text = "HighScore:" + timerFormattedh;
        }
    }

    public void OnPauseClick()
    {
        Time.timeScale = 0;
        pauseBtn.SetActive(false);
        pauseMenuCanvas.SetActive(true);
    }
    public void OnClickResume()
    {
        Time.timeScale = 1;
        pauseBtn.SetActive(true);
        pauseBtn.SetActive(true);
        pauseMenuCanvas.SetActive(false);
    }
    public void OnReturnToMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void ONClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    public void Degradation()
    {
       
            m_MaximumSteerAngle = (m_maxAngle - (m_maxAngle * tireLap * degPer) / 100);
            CurrentLapTime = 0;
            float degNow = degPer * tireLap;
        if (degNow < 100)
        {
            DegradeTxt.text = "DEGRADE:" + degNow.ToString("f00") + "%";
        }
        else
        {
            DegradeTxt.text = "DEGRADE: 100%";
            DegradeTxt.fontSize = 80;
        }
            
            tireLap++;

    
       
    }
   
}
