using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeClass
{
   public class FrontWingUpValues
    {
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue, SStifness;
    }

    [Serializable]public class FrontWingDownValues
    {
        public float FExtremumSlip, FExtremumValue, FAsymptoteSlip, FAsymptoteValue, FStifness;
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue, SStifness;
    }
    [Serializable]public class RearWingUpValues
    {
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue, SStifness;
        // public float extremumSlip, extremumValue, asymptoteSlip, asymptoteValue, stifness;
         RearWingUpValues(float a,float b,float c, float d)
         {
            SExtremumSlip = a;
            SExtremumValue = b;
            SAsymptoteSlip = c;
            SAsymptoteValue = d;
         }

    }
    [Serializable]public class RearWingDownValues
    {
        public float SExtremumSlip, SExtremumValue, SAsymptoteSlip, SAsymptoteValue, SStifness;
       public  RearWingDownValues(float a, float b, float c, float d)
        {
            SExtremumSlip = a;
            SExtremumValue = b;
            SAsymptoteSlip = c;
            SAsymptoteValue = d;
        }
    }
   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
