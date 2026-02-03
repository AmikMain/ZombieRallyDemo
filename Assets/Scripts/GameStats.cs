using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    [SerializeField] private Transform car;
    [SerializeField] private SplineContainer tarmacRoad;
    [SerializeField] private SplineContainer gravelRoad;
    public string COIN_BANK_AMOUNT = "COIN_BANK_AMOUNT";

    private float lapPercent;
    private float startT = 0.057f;

    private float maxTarmac = 0f;
    private float maxGravel = 0f;

    private float tarmacWeight;
    private float gravelWeight;

    [SerializeField] private int zombiesKilled = 0;

    private int lapEndMoney = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayerPrefs.SetInt(COIN_BANK_AMOUNT, 0); //DELETE

        Car.Instance.OnCarDied += CalculateLapEndMoney;
        GameManager.Instance.OnLapStart += ResetLapEndMoney;
        GameArea.OnPlayerLeftGameArea += ReturnCarToRoad;

        float tarmacLength =
            GetSplineLengthFromT(tarmacRoad.Spline, startT);

        float gravelLength =
            GetSplineLengthFromT(gravelRoad.Spline, 0f);

        float total = tarmacLength + gravelLength;

        tarmacWeight = tarmacLength / total;
        gravelWeight = gravelLength / total;
    }

    void OnDisable()
    {
        Car.Instance.OnCarDied -= CalculateLapEndMoney;
        GameManager.Instance.OnLapStart -= ResetLapEndMoney;
    }

    void Update()
    {
        // --- TARMAC ---
        Vector3 localTarmacPos =
            tarmacRoad.transform.InverseTransformPoint(car.position);

        float tarmacT;
        SplineUtility.GetNearestPoint(
            tarmacRoad.Spline,
            localTarmacPos,
            out _,
            out tarmacT
        );

        float tarmacProgress =
            Mathf.Clamp01(Mathf.InverseLerp(startT, 1f, tarmacT));

        maxTarmac = Mathf.Max(maxTarmac, tarmacProgress);

        // --- GRAVEL ---
        Vector3 localGravelPos =
            gravelRoad.transform.InverseTransformPoint(car.position);

        float gravelT;
        SplineUtility.GetNearestPoint(
            gravelRoad.Spline,
            localGravelPos,
            out _,
            out gravelT
        );

        float gravelProgress = Mathf.Clamp01(gravelT);
        maxGravel = Mathf.Max(maxGravel, gravelProgress);

        // --- TOTAL ---
        float totalProgress =
            maxTarmac * tarmacWeight +
            maxGravel * gravelWeight;

        lapPercent = totalProgress * 100f;
    }

    float GetSplineLengthFromT(Spline spline, float fromT, int steps = 100)
    {
        Vector3 prev = spline.EvaluatePosition(fromT);
        float length = 0f;

        for (int i = 1; i <= steps; i++)
        {
            float t = Mathf.Lerp(fromT, 1f, i / (float)steps);
            Vector3 pos = spline.EvaluatePosition(t);
            length += Vector3.Distance(prev, pos);
            prev = pos;
        }

        return length;
    }

    private void CalculateLapEndMoney()
    {
        lapEndMoney = Mathf.RoundToInt((float)lapPercent * 1.5f + (float)zombiesKilled * 1.5f) * 100;

        AddMoneyToBank(lapEndMoney);
    }

    public void HandleZombieKill()
    {
        zombiesKilled++;
    }

    public float GetLapPercent()
    {
        return lapPercent;
    }

    public int GetZombiesKilled()
    {
        return zombiesKilled;
    }

    public int GetLapEndMoney()
    {
        return lapEndMoney;
    }

    private void AddMoneyToBank(int m)
    {
        int currentAmount = PlayerPrefs.GetInt(COIN_BANK_AMOUNT, 0);
        PlayerPrefs.SetInt(COIN_BANK_AMOUNT, currentAmount + m);
        
        Debug.Log("Adding money to bank called");    
    }

    private void ResetLapEndMoney()
    {
        lapEndMoney = 0;
        zombiesKilled = 0;
        lapPercent = 0;
    }

    public void ReturnCarToRoad()
    {
        Debug.Log("event catched");
        if (car == null || tarmacRoad == null || gravelRoad == null) return;

        // --- Асфальт ---
        Vector3 localTarmacPos = tarmacRoad.transform.InverseTransformPoint(car.position);
        float tTarmac;
        SplineUtility.GetNearestPoint(tarmacRoad.Spline, localTarmacPos, out _, out tTarmac);
        Vector3 nearestTarmacPos = tarmacRoad.Spline.EvaluatePosition(tTarmac);
        float tarmacDistance = Vector3.Distance(car.position, tarmacRoad.transform.TransformPoint(nearestTarmacPos));

        // --- Гравий ---
        Vector3 localGravelPos = gravelRoad.transform.InverseTransformPoint(car.position);
        float tGravel;
        SplineUtility.GetNearestPoint(gravelRoad.Spline, localGravelPos, out _, out tGravel);
        Vector3 nearestGravelPos = gravelRoad.Spline.EvaluatePosition(tGravel);
        float gravelDistance = Vector3.Distance(car.position, gravelRoad.transform.TransformPoint(nearestGravelPos));

        // --- Выбираем ближайшую дорогу ---
        SplineContainer nearestRoad = tarmacDistance <= gravelDistance ? tarmacRoad : gravelRoad;
        float nearestT = tarmacDistance <= gravelDistance ? tTarmac : tGravel;
        Vector3 nearestPos = nearestRoad.Spline.EvaluatePosition(nearestT);

        // --- Телепортируем машину ---
        car.position = nearestRoad.transform.TransformPoint(nearestPos);

        // --- Выровнять машину по сплайну ---
        Vector3 tangent = nearestRoad.Spline.EvaluateTangent(nearestT);
        if (tangent.sqrMagnitude > 0.0001f)
            car.rotation = Quaternion.LookRotation(tangent.normalized, Vector3.up);
    }

}
