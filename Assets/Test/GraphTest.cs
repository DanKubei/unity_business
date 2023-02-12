using System.Collections;
using UnityEngine;

public class GraphTest : MonoBehaviour
{
    public float timeOffset, maxValue, minValue;
    public GraphGenerator graphGenerator;

    private float num;
    private float[] testValues = { 10, 8, 2, 0, 1, 5, 1, 15 };
    private int tick;

    void Start()
    {
        num = Random.Range(minValue, maxValue);
        StartCoroutine(Clock());
        StartCoroutine(Counter());
    }

    private IEnumerator Counter()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            graphGenerator.ClearGraph();
        }
    }

    private IEnumerator Clock()
    {
        /*foreach (float value in testValues)
        {
            yield return new WaitForSeconds(timeOffset);
            graphGenerator.DrawGraph(value);
        }*/
        
        while (true)
        {
            yield return new WaitForSeconds(timeOffset);
            num += Random.Range(-1, 2) * Random.Range(0, 50f);
            if (num < 0) num = 0;
            graphGenerator.DrawGraph(num);
            tick++;
        }
    }

}
