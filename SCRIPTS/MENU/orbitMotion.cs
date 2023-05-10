using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class orbitMotion : MonoBehaviour
{
    [SerializeField] private GameObject _moon;
    [SerializeField] private GameObject _sunPoint;
    [SerializeField] private GameObject _shadow;

    private TrailRenderer _trail;
    
    private float centerX;
    private float centerY;
    
    private float rotateRand;

    const float speedUpDown = 1.2f;
    private const float distance = 5;
    private const float CAR_ITSELF_ROTATION = -0.186f;

    private void Awake()
    {
        _trail = GetComponentInChildren<TrailRenderer>();
        
        _trail.enabled = false;
        _trail.emitting = false;
        _trail.time = 0;
         //_trail. = true;
        
        startOrbitMotion();
    }

    private void OnEnable()
    {
        _trail.Clear();
    }

    void Start()
    {
        _trail.time = 0;
        centerX = _sunPoint.transform.position.x;
        centerY = _sunPoint.transform.position.y;
        
        RotationRandomization();// рассчет рандома, чтобы не одинаково вращались
        
        startOrbitMotion();
        StartCoroutine(PostponeTrail()); // хвост после паузы
    }


    private IEnumerator PostponeTrail()
    {
        yield return new  WaitForSeconds(2f);
        // _trail.time = 3;
        // _trail.enabled = true;
    }

    void Update()
    {
        startOrbitMotion();
    }

    private void startOrbitMotion()
    {
        // эксперименты:
        // ++++++++++++ Движение мухи по восьмерке ++++++++++++
        // Vector3 v = new Vector3 (Mathf.Cos(speedUpDown * Time.time * distance), Mathf.Sin(speedUpDown * Time.time) * distance, 0);
        // Движение шетки
        // Vector3 v = new Vector3 (Mathf.Cos(speedUpDown * Time.time * distance * 6), Mathf.Sin(speedUpDown * Time.time) * distance, 0);

        // ++++++++++++ Вращение против часовой стрелки ++++++++++++
        // Vector3 v = new Vector3 (Mathf.Sin(speedUpDown * Time.time) * distance, Mathf.Cos(speedUpDown * Time.time) * distance, 0);
        // Вращенме по часовой стрелке
        // Vector3 v = new Vector3 (Mathf.Sin(speedUpDown * Time.time) * distance, Mathf.Cos(speedUpDown * Time.time) * distance, 0);
        
        // ++++++++++++ Вращение по кругу ++++++++++++
        // рассчитали:
         Vector3 v = new Vector3 (Mathf.Sin(speedUpDown * Time.time + rotateRand) * distance + centerX, 
            Mathf.Cos(speedUpDown * Time.time + rotateRand) * distance + centerY,
            0);
         
        // применили:
        _moon.transform.position = v;
        
        float axis1 = 360 * Time.time * CAR_ITSELF_ROTATION;
        _moon.transform.rotation = Quaternion.Euler(0, 0, axis1);;
    }

    private void RotationRandomization()
    {
        rotateRand = Random.Range(0, 100);
    }
}