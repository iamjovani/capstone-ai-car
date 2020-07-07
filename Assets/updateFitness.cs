using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class scr : MonoBehaviour
{
    public GameObject genFitness;
    public GameObject jeep;
    public float genFitnessVal = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*CarController genFitnessValtemp = jeep.GetComponent<CarController>();
        genFitnessVal = genFitnessValtemp.overallFitness;

        genFitness.GetComponent<Text>().text = genFitnessVal.ToString("0");*/
    }
}
