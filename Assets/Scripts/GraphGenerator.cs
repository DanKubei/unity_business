using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform netTransform, graphTransform, linesContainer, pointsContainer,
        textsContainer, graphTextPrefab, graphLinePrefab, meshPrefab, meshContainer;
    [SerializeField]
    private Sprite circleSprite;
    [SerializeField]
    private Color netColor;
    [SerializeField]
    private float circleSize, lineThickness, divisionThickness, graphMax = 0.95f, minDivisionPrice = 1f, fallCof = 10f;
    [SerializeField]
    private int divisionCount, valueCount, verticesCount;
    [SerializeField]
    private Material lineRiseMaterial, lineStayMaterial, lineFallMaterial, groundRiseMaterial, groundStayMaterial, groundFallMaterial;

    private Vector2 graphSize, graphPosition, graphWorldSize, graphWorldPosition;
    private GraphPoint[] points;
    private float maxValue, divisionPrice;
    private List<RectTransform> circles = new List<RectTransform>();
    private List<MeshFilter> groundMeshes = new List<MeshFilter>();
    private List<MeshFilter> linesMeshes = new List<MeshFilter>();
    private List<MeshRenderer> groundMeshesRender = new List<MeshRenderer>();
    private List<MeshRenderer> linesMeshesRender = new List<MeshRenderer>();
    private List<Transform> divisionTransforms = new List<Transform>();
    private List<Text> graphTexts = new List<Text>();
    private RectTransform graphRect;

    private void Start()
    {
        divisionPrice = minDivisionPrice;
        graphRect = graphTransform.GetComponent<RectTransform>();
        Vector3[] worldCorners = new Vector3[4];
        graphRect.GetWorldCorners(worldCorners);
        graphWorldSize = new Vector2((worldCorners[2] - worldCorners[1]).x, (worldCorners[1] - worldCorners[0]).y);
        graphWorldPosition = worldCorners[0];
        graphPosition = new Vector2(graphRect.rect.position.x, graphRect.rect.position.y);
        graphSize = new Vector2(graphRect.rect.size.x, graphRect.rect.size.y * graphMax);
        maxValue = minDivisionPrice * divisionCount * graphMax;
        points = new GraphPoint[valueCount];
        DrawNet();
    }

    public void DrawNet()
    {
        if (divisionTransforms.Count > 0)
        {
            foreach (Transform transform in divisionTransforms)
            {
                Destroy(transform.gameObject);
            }
            divisionTransforms.Clear();
        }
        float yOffset = graphSize.y / divisionCount / graphMax;
        for (int i = 0; i < divisionCount; i++)
        {
            GameObject division = new GameObject("Division", typeof(Image));
            division.transform.SetParent(netTransform, false);
            division.GetComponent<Image>().color = netColor;
            RectTransform divisionRect = division.GetComponent<RectTransform>();
            divisionRect.anchorMin = Vector2.zero;
            divisionRect.anchorMax = Vector2.zero;
            divisionRect.sizeDelta = new Vector2(graphSize.x, divisionThickness);
            divisionRect.anchoredPosition = new Vector2(graphPosition.x + graphSize.x, yOffset * i - divisionThickness * .5f);
        }
        InstantiateTextValues();
    }

    public void DrawGraph(float newValue, int index = -1, GraphPoint point = null, bool updateY = false)
    {
        float xOffset = graphSize.x / (valueCount - 1);
        if (newValue > maxValue && newValue > minDivisionPrice * divisionCount * graphMax)
        {
            maxValue = newValue;
            divisionPrice = newValue / graphMax / divisionCount;
            updateY = true;
            SetTextValues();
        }
        else if (newValue < maxValue / fallCof)
        {
            bool fall = true;
            float max = newValue;
            for (int i = 0; i < valueCount; i++)
            {
                if (points[i] == null)
                {
                    break;
                }
                if (points[i].value < maxValue / fallCof)
                {
                    if (points[i].value > max)
                    {
                        max = points[i].value;
                    }
                }
                else
                {
                    fall = false;
                    break;
                }
            }
            if (fall && max > minDivisionPrice * divisionCount / graphMax)
            {
                maxValue = max;
                divisionPrice = max * graphMax / divisionCount;
                updateY = true;
            }
            SetTextValues();
        }
        if (index == -1)
        {
            index = valueCount - 1;
        }
        if (points[index] == null || points[index].IsNull)
        {
            if (index == 0)
            {
                points[0] = new GraphPoint(newValue, new Vector2(graphPosition.x,
                    graphPosition.y + (newValue / maxValue * graphSize.y)));
                return;
            }
            else if (points[index - 1] == null || points[index - 1].IsNull)
            {
                DrawGraph(newValue, index - 1, null, updateY);
                return;
            }
            else
            {
                points[index] = new GraphPoint(newValue, new Vector2(graphPosition.x + xOffset * index,
                    graphPosition.y + newValue / maxValue * graphSize.y));
                if (updateY)
                {
                    DrawGraph(-1, index - 1, null, true);
                    return;
                }
            }
        }
        else
        {
            if (newValue == -1)
            {
                float y = graphPosition.y + points[index].value / maxValue * graphSize.y;
                if (points[index].value > maxValue)
                {
                    y = graphPosition.y + graphSize.y * graphMax;
                }
                points[index] = new GraphPoint(points[index].value, new Vector2(graphPosition.x + xOffset * index, y));
                if (index > 0)
                {
                    DrawGraph(newValue, index - 1, point, updateY);
                    return;
                }
            }
            else if (point == null)
            {
                point = points[index];
                points[index] = new GraphPoint(newValue, new Vector2(graphPosition.x + xOffset * index,
                    graphPosition.y + newValue / maxValue * graphSize.y));
                DrawGraph(newValue, index - 1, point, updateY);
                return;
            }
            else
            {
                GraphPoint oldPoint = points[index];
                if (!updateY)
                {
                    points[index] = new GraphPoint(point.value, new Vector2(graphPosition.x + xOffset * index,
                        point.position.y));
                }
                else
                {
                    float y = graphPosition.y + point.value / maxValue * graphSize.y;
                    if (points[index].value > maxValue)
                    {
                        y = graphPosition.y + graphSize.y * graphMax;
                    }
                    points[index] = new GraphPoint(point.value, new Vector2(graphPosition.x + xOffset * index, y));
                }
                point = oldPoint;
                if (index > 0)
                {
                    DrawGraph(newValue, index - 1, point, updateY);
                    return;
                }
            }
        }
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i] == null || points[i].IsNull)
            {
                break;
            }
            if (circles.Count == i)
            {
                GameObject circle = new GameObject("Point", typeof(Image));
                circle.transform.SetParent(pointsContainer);
                circle.GetComponent<Image>().sprite = circleSprite;
                RectTransform circleRect = circle.GetComponent<RectTransform>();
                circleRect.sizeDelta = Vector2.one * circleSize;
                circleRect.localScale = Vector3.one;
                circleRect.anchoredPosition = points[i].position;
                circles.Add(circleRect);
                if (i > 0)
                {
                    Transform transform = Instantiate(graphLinePrefab, linesContainer);
                    linesMeshes.Add(transform.GetComponent<MeshFilter>());
                    linesMeshesRender.Add(transform.GetComponent<MeshRenderer>());
                    StartCoroutine(ChangeLine(i));
                    transform = Instantiate(meshPrefab, meshContainer);
                    groundMeshes.Add(transform.GetComponent<MeshFilter>());
                    groundMeshesRender.Add(transform.GetComponent<MeshRenderer>());
                    ChangeMesh(i - 1);
                }
            }
            else
            {
                circles[i].anchoredPosition = points[i].position;
                if (i > 0) {
                    StartCoroutine(ChangeLine(i));
                    ChangeMesh(i - 1);
                }
            }
        }
    }

    private IEnumerator ChangeLine(int index)
    {
        yield return new WaitForSeconds(0.01f);
        if (verticesCount < 1)
        {
            verticesCount = 1;
        }
        int localVertices = 0;
        int halfVertices = Mathf.CeilToInt((float)verticesCount / 2);
        Vector2 dir = circles[index].anchoredPosition - circles[index - 1].anchoredPosition;
        Vector2 normalizedDir = dir.normalized;
        Vector2 bottomDir = Vector2.zero, topDir = Vector2.zero;
        if (index > 1)
        {
            bottomDir = circles[index - 1].anchoredPosition - circles[index - 2].anchoredPosition;
            localVertices += halfVertices;
        }
        if (index < linesMeshes.Count)
        {
            topDir = circles[index + 1].anchoredPosition - circles[index].anchoredPosition;
            localVertices += halfVertices;
        }
        Vector2 startPos = circles[index - 1].anchoredPosition;
        float xPos = lineThickness / 2 * normalizedDir.y;
        float yPos = lineThickness / 2 * normalizedDir.x;

        Mesh mesh = new Mesh();
        Vector2[] uv = new Vector2[4 + localVertices];
        Vector3[] vertices = new Vector3[4 + localVertices];
        int[] triangles = new int[6 + localVertices * 3];

        uv[0] = new Vector2(1, 0);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(0, 1);

        vertices[0] = new Vector2(startPos.x + dir.x + xPos, startPos.y + dir.y - yPos);
        vertices[1] = new Vector2(startPos.x + dir.x - xPos, startPos.y + dir.y + yPos);
        vertices[2] = new Vector2(startPos.x + xPos, startPos.y - yPos);
        vertices[3] = new Vector2(startPos.x - xPos, startPos.y + yPos);

        if (topDir != Vector2.zero && (dir.y != 0 || topDir.y != 0))
        {
            Vector2 perpendicularVector = Utility.GetPerpendicularVector(Utility.RotateVector(dir, 180), topDir);
            bool isInnerAngel = Utility.GetAngleFromVector(topDir) < Utility.GetAngleFromVector(dir);
            if (perpendicularVector.y < 0)
            {
                perpendicularVector *= -1;
            }
            if (isInnerAngel)
            {
                vertices[0] = circles[index].anchoredPosition - perpendicularVector * lineThickness / 2;
                vertices[1] = vertices[0] + new Vector3(-xPos, yPos, 0) * 2;
            }
            else
            {
                vertices[1] = circles[index].anchoredPosition + perpendicularVector * lineThickness / 2;
                vertices[0] = vertices[1] + new Vector3(xPos, -yPos, 0) * 2;
            }
            float angle = Utility.GetAngleFromVectors(dir, topDir) / halfVertices / 2;
            for (int i = 0; i < halfVertices; i++)
            {
                if (isInnerAngel)
                {
                    vertices[4 + i] = vertices[0] +
                        (Vector3)Utility.RotateVector(perpendicularVector, angle * i * 180f / Mathf.PI) * lineThickness;
                }
                else
                {
                    vertices[4 + i] = vertices[1] -
                        (Vector3)Utility.RotateVector(perpendicularVector, -angle * i * 180f / Mathf.PI) * lineThickness;
                }
            }
            if (isInnerAngel)
            {
                for (int i = 0; i < halfVertices; i++)
                {
                    if (i >= halfVertices - 1)
                    {
                        triangles[6 + i * 3] = 1;
                    }
                    else
                    {
                        triangles[6 + i * 3] = 5 + i;
                    }
                    triangles[7 + i * 3] = 4 + i;
                    triangles[8 + i * 3] = 0;
                }
            }
            else
            {
                for (int i = 0; i < halfVertices; i++)
                {
                    triangles[6 + i * 3] = 1;
                    triangles[7 + i * 3] = 4 + i;
                    if (i == halfVertices - 1)
                    {
                        triangles[8 + i * 3] = 0;
                    }
                    else
                    {
                        triangles[8 + i * 3] = 5 + i;
                    }
                }
            }
        }
        if (bottomDir != Vector2.zero && (dir.y != 0 || bottomDir.y != 0))
        {
            Vector2 perpendicularVector = Utility.GetPerpendicularVector(Utility.RotateVector(bottomDir, 180), dir);
            int indexOffset = topDir == Vector2.zero ? 0 : halfVertices;
            bool isInnerAngel = Utility.GetAngleFromVector(dir) > Utility.GetAngleFromVector(bottomDir);
            if (perpendicularVector.y < 0)
            {
                perpendicularVector *= -1;
            }
            if (isInnerAngel)
            {
                vertices[3] = circles[index - 1].anchoredPosition + perpendicularVector * lineThickness / 2;
                vertices[2] = vertices[3] + new Vector3(xPos, -yPos, 0) * 2;
            }
            else
            {
                vertices[2] = circles[index - 1].anchoredPosition - perpendicularVector * lineThickness / 2;
                vertices[3] = vertices[2] + new Vector3(-xPos, yPos, 0) * 2;
            }
            float angle = Utility.GetAngleFromVectors(bottomDir, dir) / halfVertices / 2;
            for (int i = indexOffset; i < halfVertices + indexOffset; i++)
            {
                if (isInnerAngel)
                {
                    vertices[4 + i] = vertices[3] -
                        (Vector3)Utility.RotateVector(perpendicularVector, angle * (i - indexOffset) * 180f / Mathf.PI) * lineThickness;
                }
                else
                {
                    vertices[4 + i] = vertices[2] +
                        (Vector3)Utility.RotateVector(perpendicularVector, -angle * (i - indexOffset) * 180f / Mathf.PI) * lineThickness;
                }
            }
            if (isInnerAngel)
            {
                for (int i = indexOffset; i < halfVertices + indexOffset; i++)
                {
                    triangles[6 + i * 3] = 4 + i;
                    triangles[7 + i * 3] = 3;
                    if (i == halfVertices - 1 + indexOffset)
                    {
                        triangles[8 + i * 3] = 2;
                    }
                    else
                    {
                        triangles[8 + i * 3] = 5 + i;
                    }
                }
            }
            else
            {
                for (int i = indexOffset; i < halfVertices + indexOffset; i++)
                {
                    if (i == halfVertices - 1 + indexOffset)
                    {
                        triangles[6 + i * 3] = 3;
                    }
                    else
                    {
                        triangles[6 + i * 3] = 5 + i;
                    }
                    triangles[7 + i * 3] = 2;
                    triangles[8 + i * 3] = 4 + i;
                }
            }
        }
        triangles[0] = 1;
        triangles[1] = 2;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 0;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        linesMeshes[index - 1].mesh = mesh;
        Material material = null;
        if (dir.y > 0)
        {
            material = lineRiseMaterial;
        }
        else if (dir.y < 0)
        {
            material = lineFallMaterial;
        }
        else
        {
            material = lineStayMaterial;
        }
        linesMeshesRender[index - 1].material = material;
    }

    private void ChangeMesh(int index)
    {
        float xOffset = graphSize.x / (valueCount - 1);
        float yUv1 = points[index].value / maxValue;
        float yUv2 = points[index + 1].value / maxValue;
        yUv1 = yUv1 > 1 ? 1 : yUv1;
        yUv2 = yUv2 > 1 ? 1 : yUv2;
        float yPosition1 = yUv1 * graphSize.y + graphPosition.y;
        float yPosition2 = yUv2 * graphSize.y + graphPosition.y;
        Mesh mesh = new Mesh();
        Vector2[] uv = new Vector2[4];
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[6];

        uv[0] = new Vector2(1, 0);
        uv[1] = new Vector2(1, yUv2);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(0, yUv1);

        vertices[0] = new Vector2(graphPosition.x + xOffset * (index + 1), graphPosition.y);
        vertices[1] = new Vector2(graphPosition.x + xOffset * (index + 1), yPosition2);
        vertices[2] = new Vector2(graphPosition.x + xOffset * index, graphPosition.y);
        vertices[3] = new Vector2(graphPosition.x + xOffset * index, yPosition1);

        triangles[0] = 1;
        triangles[1] = 2;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 0;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        groundMeshes[index].mesh = mesh;
        Material material = null;
        if (yUv1 > yUv2)
        {
            material = groundFallMaterial;
        }
        else if (yUv1 < yUv2)
        {
            material = groundRiseMaterial;
        }
        else
        {
            material = groundStayMaterial;
        }
        groundMeshesRender[index].material = material;
    }

    private void InstantiateTextValues()
    {
        float left = graphRect.rect.position.x;
        float yOffset = graphSize.y / divisionCount / graphMax;
        for (int i = 0; i < divisionCount; i++)
        {
            Transform text = Instantiate(graphTextPrefab, textsContainer);
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.localPosition = new Vector3(left + textRect.rect.width / 2, graphPosition.y + yOffset * i + textRect.rect.height / 2 + divisionThickness);
            graphTexts.Add(text.GetComponent<Text>());
        }
        SetTextValues();
    }

    private void SetTextValues()
    {
        for (int i = 0; i < divisionCount; i++)
        {
            graphTexts[i].text = string.Format("{0:f2}", divisionPrice * i);
        }
    }
}

[System.Serializable]
public class GraphPoint
{
    public float value = -1;
    public Vector2 position;

    public GraphPoint(float value, Vector2 position)
    {
        this.value = value;
        this.position = position;
    }

    public bool IsNull
    {
        get { return value == 0 && position == Vector2.zero; }
    }
}
