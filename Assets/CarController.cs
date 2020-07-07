using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System;
using System.IO;
using System.Text;

[RequireComponent(typeof(NNet))]
public class CarController : MonoBehaviour
{
    private Vector3 startPosition, startRotation;
    private NNet network;

    [Range(-1f,1f)]
    public float a,t;

    public float timeSinceStart = 0f;

    [Header("Fitness")]
    public float overallFitness;
    public float distanceMultipler = 1.4f;
    public float avgSpeedMultiplier = 0.2f;
    public float sensorMultiplier = 0.1f;


    [Header("Generation Count")]
    public int generationCount = 0;


    [Header("Network Options")]
    public int LAYERS = 3;
    public int NEURONS = 30;

    private Vector3 lastPosition;
    private float totalDistanceTravelled;
    private float avgSpeed;

    private float aSensor,bSensor,cSensor;

    public StringBuilder csv = new StringBuilder();

    public static int i = 0;


    private void Awake() 
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        //genText.text = generationCount.ToString("0");
        network = GetComponent<NNet>();
    }

    public void ResetWithNetwork (NNet net)
    {
        network = net;
        Reset();
    }

    

    public void Reset() {

        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0f;
        transform.position = startPosition;
        generationCount += 1; // add value to  TextUI 
        transform.eulerAngles = startRotation;

        /*
        string path = @"C:\DEV\saveTrainingData\trainingData.csv";

            // Calling the ReadAllLines() function 
            string[] readText = File.ReadAllLines(path);

            foreach (string s in readText)
            {
               
             String[] arr = s.Split(',');
             (a, t) = network.RunNetwork(float.Parse(arr[0]), float.Parse(arr[0]), float.Parse(arr[0]));
            }
        */
    }

    private void OnCollisionEnter (Collision collision) {
        Death();
    }

    private void FixedUpdate() {

        InputSensors();
        lastPosition = transform.position;


        (a, t) = network.RunNetwork(aSensor, bSensor, cSensor);

     
      

        MoveCar(a,t, timeSinceStart);

        timeSinceStart += Time.deltaTime;

        CalculateFitness();

    }

    private void Death ()
    {
        GameObject.FindObjectOfType<GeneticManager>().Death(overallFitness, network);
    }

    private void CalculateFitness() {

        totalDistanceTravelled += Vector3.Distance(transform.position,lastPosition);
        avgSpeed = totalDistanceTravelled/timeSinceStart;

       overallFitness = (totalDistanceTravelled*distanceMultipler)+(avgSpeed*avgSpeedMultiplier)+(((aSensor+bSensor+cSensor)/3)*sensorMultiplier);

        if (timeSinceStart > 20 && overallFitness < 40) {
            Death();
        }

        if (overallFitness >= 1000) {
            Death();
        }

    }

    private void InputSensors() {

        Vector3 a = (transform.forward+transform.right);
        Vector3 b = (transform.forward);
        Vector3 c = (transform.forward-transform.right);

        Ray r = new Ray(transform.position,a);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit)) {
            aSensor = hit.distance/20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = b;

        if (Physics.Raycast(r, out hit)) {
            bSensor = hit.distance/20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = c;

        if (Physics.Raycast(r, out hit)) {
            cSensor = hit.distance/20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        saveData(aSensor, bSensor, cSensor, getOverallFitness());

    }

    // save training data from model
    public void saveData(float aSensor, float bSensor, float cSensor, float overallFitness)
    {
            var newLine = string.Format("{0},{1},{2},{3}", aSensor, bSensor, cSensor, overallFitness);
            csv.AppendLine(newLine);

            File.WriteAllText(@"C:\DEV\saveTrainingData\trainingData.csv", csv.ToString());
    }

    
    private Vector3 inp;
    public void MoveCar (float v, float h, float time) {
        inp = Vector3.Lerp(Vector3.zero,new Vector3(0,0,v*11.4f),0.02f);
        inp = transform.TransformDirection(inp);
        transform.position += inp;

        transform.eulerAngles += new Vector3(0, (h*90)*0.02f,0);

        //saveData(v, h, time);
    }

    public int genCount()
    {
        return generationCount;
    }

    public float getOverallFitness()
    {
        return overallFitness;
    }

}
