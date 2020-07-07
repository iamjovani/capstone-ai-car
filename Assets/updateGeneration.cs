using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class updateGeneration : MonoBehaviour
{
    public GameObject genCount;
    public GameObject FitnessCount2;
    public GameObject jeep;
    public int genCountVal = 0;
    public float genFitnessVal = 0f;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        CarController genCountValtemp = jeep.GetComponent<CarController> ();
        genCountVal = genCountValtemp.generationCount;

        genCount.GetComponent<Text>().text = genCountVal.ToString("0");


        //------------------------------------------------------------------
        CarController genFitnessValtemp = jeep.GetComponent<CarController>();
        genFitnessVal = genFitnessValtemp.overallFitness;

        FitnessCount2.GetComponent<Text>().text = genFitnessVal.ToString("0");


    }
}
