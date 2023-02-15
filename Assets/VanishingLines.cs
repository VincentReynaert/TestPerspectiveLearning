using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VanishingLines : MonoBehaviour
{
    Vector3[] vertices = new Vector3[8];
    [SerializeField] private float lineLength = 100;
    [SerializeField] Color lineColor = Color.white;
    Color currentLineColor;
    BoxCollider boxCollider;
    [SerializeField] private bool showAxesColors;
    [SerializeField] private LineRenderer[] lines = new LineRenderer[12];
    [SerializeField, Range(0f, 1f)] private float lineWidth = 0.02f;
    [SerializeField] private Material lineMaterial;
    private bool showLines = true;
    private bool showingLines = true;

    //public static bool ShowLines { get => showLines; set => showLines = !showLines; }

    private void OnEnable()
    {
        if (lines == null) return;
        showLines = true;
        foreach (var item in lines)
        {
            if (item != null) item.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (lines == null) return;
        showLines = false;
        foreach (var item in lines)
        {
            if (item != null) item.gameObject.SetActive(false);
        }

    }
    //public static void ToggleShowLines()
    //{
    //    print("toggle");
    //    showLines = !showLines;
    //}
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentLineColor = lineColor;
        for (int i = 0; i < 12; i++)
        {
            GameObject go = Instantiate<GameObject>(new GameObject(), transform);
            lines[i] = go.AddComponent<LineRenderer>();
            lines[i].alignment = LineAlignment.View;
            lines[i].startWidth = lineWidth;
            lines[i].endWidth = lineWidth;
            lines[i].material = lineMaterial;
        }
    }

    void Update()
    {
        if (!showLines) return;
        //if (!showLines)
        //{
        //    if (showingLines)
        //    {
        //        showingLines = false;
        //        foreach (var item in lines)
        //        {
        //            item.gameObject.SetActive(false);
        //        }
        //    }
        //    return;
        //}
        //else if (!showingLines)
        //{
        //    showingLines = true;
        //    foreach (var item in lines)
        //    {
        //        item.gameObject.SetActive(true);
        //    }
        //}
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            ComputeBoundingBoxVertices(boxCollider.size, boxCollider.center);
        }
        DrawEdges();
    }
    private Vector3 Multiply(Vector3 v, Vector3 u)
    {
        return new Vector3(v.x * u.x, v.y * u.y, v.z * u.z);
    }
    private void DrawEdges()
    {
        int lineIndex = 0;
        Vector3 worldPta = vertices[0];
        foreach (var worldPtb in new Vector3[] { vertices[1], vertices[2], vertices[4] })
        {
            lineIndex = HandleLine(lineIndex, worldPta, worldPtb);
        }
        worldPta = vertices[3];
        foreach (var worldPtb in new Vector3[] { vertices[1], vertices[2], vertices[7] })
        {
            lineIndex = HandleLine(lineIndex, worldPta, worldPtb);
        }
        worldPta = vertices[5];
        foreach (var worldPtb in new Vector3[] { vertices[1], vertices[4], vertices[7] })
        {
            lineIndex = HandleLine(lineIndex, worldPta, worldPtb);
        }
        worldPta = vertices[6];
        foreach (var worldPtb in new Vector3[] { vertices[2], vertices[4], vertices[7] })
        {
            lineIndex = HandleLine(lineIndex, worldPta, worldPtb);
        }
    }

    private int HandleLine(int lineIndex, Vector3 worldPta, Vector3 worldPtb)
    {
        UpdateLineColor(worldPta, worldPtb);
        DrawDebugLine(worldPta, worldPtb);
        UpdateLine(worldPta, worldPtb, lines[lineIndex]);
        lineIndex++;
        return lineIndex;
    }

    private void ComputeBoundingBoxVertices(Vector3 boxSize, Vector3 boxCenter)
    {
        vertices[0] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(1, 1, 1)));
        vertices[1] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(1, 1, -1)));
        vertices[2] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(1, -1, 1)));
        vertices[3] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(1, -1, -1)));
        vertices[4] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(-1, 1, 1)));
        vertices[5] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(-1, 1, -1)));
        vertices[6] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(-1, -1, 1)));
        vertices[7] = transform.TransformPoint(boxCenter + Multiply(boxSize * 0.5f, new Vector3(-1, -1, -1)));
    }
    private void UpdateLineColor(Vector3 worldPta, Vector3 worldPtb)
    {
        if (showAxesColors)
        {
            if (Vector3.Angle((worldPtb - worldPta), transform.forward) % 180 == 0) currentLineColor = Color.blue;
            else if (Vector3.Angle((worldPtb - worldPta), transform.up) % 180 == 0) currentLineColor = Color.green;
            else if (Vector3.Angle((worldPtb - worldPta), transform.right) % 180 == 0) currentLineColor = Color.red;
        }
        else if (lineColor != currentLineColor) currentLineColor = lineColor;
    }
    private void UpdateLine(Vector3 worldPta, Vector3 worldPtb, LineRenderer lineRenderer)
    {
        lineRenderer.startColor = currentLineColor;
        lineRenderer.endColor = currentLineColor;
        lineRenderer.SetPosition(0, worldPta - (worldPtb - worldPta) * lineLength);
        lineRenderer.SetPosition(1, worldPtb + (worldPtb - worldPta) * lineLength);

    }
    private void DrawDebugLine(Vector3 worldPta, Vector3 worldPtb)
    {
        Debug.DrawLine(worldPta - (worldPtb - worldPta) * lineLength, worldPtb + (worldPtb - worldPta) * lineLength, currentLineColor);
    }
}
