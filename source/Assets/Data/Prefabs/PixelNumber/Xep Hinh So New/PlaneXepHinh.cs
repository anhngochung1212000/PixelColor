using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorData
{
    public int index;
    public Color color;
    public Color colorGray;
}
public class PlaneXepHinh : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public Vector2 TexNumberSize = new Vector2(6, 3);
    public int blockIndex;
    Mesh cloneMesh;
    Color[] verColor;
    Vector3[] verNormal;
    Vector2[] meshUV;
    Vector2 cellSize;
    Vector3[] vertices;
    public List<ColorData> colorDatas = new List<ColorData>();

    private void Awake()
    {
        MeshFilter = GetComponent<MeshFilter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //blockIndex = int.Parse(gameObject.name.Substring(1, gameObject.name.Length - 1)) - 1;
        //cellSize = new Vector2(1f / TexNumberSize.x, 1f / TexNumberSize.y);
        //cloneMesh = MeshFilter.mesh;
        //meshUV = cloneMesh.uv;
        //verColor = new Color[cloneMesh.vertexCount];
        //verNormal = new Vector3[cloneMesh.vertexCount];
        //vertices = cloneMesh.vertices;
        //for (int i = 0; i < cloneMesh.vertexCount; i++)
        //{
        //    verColor[i] = cloneMesh.colors[i];
        //    verNormal[i] = Vector3.zero;
        //    meshUV[i] = cloneMesh.uv[i];
        //}
        //for (int i = 0; i < cloneMesh.triangles.Length; i += 3)
        //    SetMeshNumber(i / 3, Random.Range(0, 17));
    }

    public void Init(Mesh mesh)
    {
        cellSize = new Vector2(1f / TexNumberSize.x, 1f / TexNumberSize.y);
        cloneMesh = Instantiate(mesh);
        cloneMesh.name = blockIndex.ToString();
        meshUV = cloneMesh.uv;
        verColor = new Color[cloneMesh.vertexCount];
        verNormal = new Vector3[cloneMesh.vertexCount];

        for (int i = 0; i < cloneMesh.vertexCount; i++)
        {
            verColor[i] = cloneMesh.colors[i];
            verNormal[i] = Vector3.zero;
            meshUV[i] = cloneMesh.uv[i];
        }

        this.MeshFilter.mesh = cloneMesh;

        for (int i = 0; i < cloneMesh.triangles.Length; i += 3)
            SetMeshNumber(i / 3, Random.Range(0, 17));

    }

    public void SetMeshNumber(int index, int number)
    {
        Vector2 offset = new Vector2(number % (int)TexNumberSize.x, (int)TexNumberSize.y - (number / (int)TexNumberSize.x));
        Vector2 triUV = new Vector2(cellSize.x * offset.x, cellSize.y * offset.y - cellSize.y);

        if (index % 2 > 0)
            index -= 1;

        meshUV[cloneMesh.triangles[index * 3 + 0]] = new Vector2(triUV.x, triUV.y);
        meshUV[cloneMesh.triangles[index * 3 + 1]] = new Vector2(triUV.x, triUV.y + cellSize.y);
        meshUV[cloneMesh.triangles[index * 3 + 2]] = new Vector2(triUV.x + cellSize.x, triUV.y + cellSize.y);

        meshUV[cloneMesh.triangles[index * 3 + 3]] = new Vector2(triUV.x, triUV.y);
        meshUV[cloneMesh.triangles[index * 3 + 4]] = new Vector2(triUV.x + cellSize.x, triUV.y + cellSize.y);
        meshUV[cloneMesh.triangles[index * 3 + 5]] = new Vector2(triUV.x + cellSize.x, triUV.y);

        cloneMesh.uv = meshUV;
    }

    public void SetMeshColor(int index, Color color)
    {
        //if (color.CompareTwoColor(Color.black))
        //{
        //    var a = color.a;
        //    color = Color.gray;
        //    color.a = a;
        //}
        if (index % 2 > 0)
            index -= 1;

        verColor[cloneMesh.triangles[index * 3 + 0]] = color;
        verColor[cloneMesh.triangles[index * 3 + 1]] = color;
        verColor[cloneMesh.triangles[index * 3 + 2]] = color;
        verColor[cloneMesh.triangles[index * 3 + 3]] = color;
        verColor[cloneMesh.triangles[index * 3 + 4]] = color;
        verColor[cloneMesh.triangles[index * 3 + 5]] = color;

        cloneMesh.colors = verColor;
    }

    public void SetMeshNormal(int index, Vector3 normal)
    {
        if (index % 2 > 0)
            index -= 1;

        verNormal[cloneMesh.triangles[index * 3 + 0]] = normal;
        verNormal[cloneMesh.triangles[index * 3 + 1]] = normal;
        verNormal[cloneMesh.triangles[index * 3 + 2]] = normal;
        verNormal[cloneMesh.triangles[index * 3 + 3]] = normal;
        verNormal[cloneMesh.triangles[index * 3 + 4]] = normal;
        verNormal[cloneMesh.triangles[index * 3 + 5]] = normal;

        cloneMesh.normals = verNormal;
    }

    // Update is called once per frame
    //void Update()
    //{     
    //}
}
