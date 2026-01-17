using UnityEngine;
using UnityEngine.Splines;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    [SerializeField] private Transform car;
    [SerializeField] private SplineContainer tarmacRoad;
    [SerializeField] private SplineContainer gravelRoad;
    [SerializeField, Range(0f, 1f)]
    private float startT = 0.057f;

    private float maxTarmac = 0f;
    private float maxGravel = 0f;

    private float tarmacWeight;
    private float gravelWeight;

    [SerializeField] private int zombiesKilled = 0;

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
        float tarmacLength =
            GetSplineLengthFromT(tarmacRoad.Spline, startT);

        float gravelLength =
            GetSplineLengthFromT(gravelRoad.Spline, 0f);

        float total = tarmacLength + gravelLength;

        tarmacWeight = tarmacLength / total;
        gravelWeight = gravelLength / total;
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

    float percent = totalProgress * 100f;
    //Debug.Log($"Прогресс трассы: {percent:0.0}%");
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

    public void HandleZombieKill()
    {
        zombiesKilled++;
    }

}
