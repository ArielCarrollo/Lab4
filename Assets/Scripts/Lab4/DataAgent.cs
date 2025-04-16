using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[System.Serializable]
public class Data
{
    [Range(0f, 1f)]  
    public float value;
    public float valueMax=1;
    public float time;
    public float timeRate;
    public float timeFrameRate =0;
    public Data() { 
    
    
    }
}
public class DataAgent : MonoBehaviour
{
    public Data Energy = new Data();
    public Data Sleep = new Data();
    public Data WC = new Data();
    Coroutine CoroutineEnergy=null;
    Coroutine CoroutineSleep = null;
    Coroutine CoroutineWC = null;

    public bool CantLoadEnergy { get => CoroutineEnergy == null; }
    public bool IsSleeping { get; set; }
    public bool IsInBathroom { get; set; } // Nueva propiedad para estado en baño

    public bool IsInTransit { get; set; } // Nuevo flag para movimiento

    public bool IsMoving { get; set; } // Nuevo flag para movimiento

    void Update()
    {
        if (IsMoving) return; // Si está en movimiento, no actualizar barras

        // Disminuir sueño (solo si no está durmiendo/moviéndose)
        if (!IsSleeping && !IsInBathroom)
        {
            Sleep.value = Mathf.Max(0, Sleep.value - Time.deltaTime * 0.01f);
        }

        // Aumentar necesidad de WC (solo si no está durmiendo/moviéndose)
        if (!IsSleeping)
        {
            WC.value = Mathf.Max(0, WC.value - Time.deltaTime * 0.02f);
        }
    }
    IEnumerator LoadEnergyTime(float time)
    {

        while(time>0)
        {
            time--;
            Energy.value = Mathf.Lerp(Energy.value, Energy.valueMax, Time.deltaTime * 20f);
            yield return new WaitForSecondsRealtime(1);
        }
        Energy.value = Energy.valueMax;
        StopCoroutine(CoroutineEnergy);
        CoroutineEnergy = null;

    }
    public void LoadEnergy()
    {
        if(CoroutineEnergy==null)
            CoroutineEnergy = StartCoroutine(LoadEnergyTime(Energy.time));
    }

    public void DiscountEnergy()
    {
        if(Energy.timeFrameRate > Energy.timeRate)
        {
            Energy.timeFrameRate = 0;
            Energy.value-=0.03f;
        }
        Energy.timeFrameRate += Time.deltaTime;
    }
    // Métodos para recuperar sueño (similar a LoadEnergy)
    public void Rest()
    {
        if (CoroutineSleep == null)
            CoroutineSleep = StartCoroutine(RecoverSleepTime(Sleep.time));
    }

    IEnumerator RecoverSleepTime(float time)
    {
        while (time > 0)
        {
            time--;
            Sleep.value = Mathf.Lerp(Sleep.value, Sleep.valueMax, Time.deltaTime * 20f);
            yield return new WaitForSecondsRealtime(1);
        }
        Sleep.value = Sleep.valueMax;
        StopCoroutine(CoroutineSleep);
        CoroutineSleep = null;
    }
}
