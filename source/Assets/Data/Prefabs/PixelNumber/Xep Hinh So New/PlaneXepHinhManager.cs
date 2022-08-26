using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlaneXepHinhManager : MonoBehaviour
{
    public Sprite MainTex;
    public Sprite GrayTex;
    public Mesh mesh;
    Vector2 Size = new Vector2(69, 65);
    public PlaneXepHinh PlaneXepHinh;

    List<PlaneXepHinh> lsBlocks = new List<PlaneXepHinh>();
    int blocks;

    bool isDown;
    Vector3 curMousePos;
    Vector3 camPos;
    public float mouseSpeed = 0.1f;
    public List<int> indexs = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        Size = new Vector2(GrayTex.texture.width, GrayTex.texture.height);
        blocks = (int)Mathf.Max(this.Size.x, this.Size.y) / 10 + ((int)Mathf.Max(this.Size.x, this.Size.y) % 10 == 0 ? 0 : 1);
        var blockX = (int)this.Size.x / 10 + ((int)this.Size.x % 10 == 0 ? 0 : 1);
        var blockY = (int)this.Size.y / 10 + ((int)this.Size.y % 10 == 0 ? 0 : 1);
      
        Init();

        for (int j = 0; j < (int)this.Size.y; j++)
        {
            for (int i = 0; i < (int)this.Size.x; i++)
            {
                Color tempColorGray = this.GrayTex.texture.GetPixel(i, j);
                Color tempColor = this.MainTex.texture.GetPixel(i, j);
                int xBlock = i / 10;
                int xIndex = i % 10;
                int yBlock = j / 10;
                int yIndex = j % 10;

                int blockNum = xBlock + yBlock * blockX;
                int indexNum = xIndex + yIndex * 10;

                var index = indexNum * 2;
                var indexData = new ColorData() {
                    index = indexNum * 2,
                    color = tempColor,
                    colorGray = tempColorGray
                };
                indexs.Add(indexData.index);
                lsBlocks[blockNum].colorDatas.Add(indexData);
                lsBlocks[blockNum].SetMeshColor(indexNum * 2, tempColorGray);
            }
        }
    }

    void Init()
    {
        blocks = (int)Mathf.Max(this.Size.x, this.Size.y) / 10 + ((int)Mathf.Max(this.Size.x, this.Size.y) % 10 == 0 ? 0 : 1);
        var blockX = (int)this.Size.x / 10 + ((int)this.Size.x % 10 == 0 ? 0 : 1);
        var blockY = (int)this.Size.y / 10 + ((int)this.Size.y % 10 == 0 ? 0 : 1);
        for (int j = 0; j < blockY ; j++)
        {
            for (int i = 0; i < blockX; i++)
            {
                Vector3 pos = Vector3.zero;

                pos.x = i;
                pos.z = j;

                PlaneXepHinh planeXepHinh = Instantiate(this.PlaneXepHinh,transform);

                planeXepHinh.blockIndex = i + j * blocks;
                planeXepHinh.gameObject.transform.position = pos;
                planeXepHinh.Init(mesh);
                lsBlocks.Add(planeXepHinh);

                //planeXepHinh.gameObject.name = name;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDown = true;
            curMousePos = Input.mousePosition;
            camPos = Camera.main.transform.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }

        if (isDown)
        {
            Vector3 pos = Vector3.zero;
            Vector3 mouseDir = curMousePos - Input.mousePosition;

            pos.x = mouseDir.x;
            pos.z = mouseDir.y;

            Camera.main.transform.position = camPos + pos * mouseSpeed * Screen.dpi;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Color pixelColor = GetPixelColor(hit.transform, hit.triangleIndex, MainTex);
 
                //hit.transform.GetComponent<PlaneXepHinh>().SetMeshColor(hit.triangleIndex, pixelColor);
                //hit.transform.GetComponent<PlaneXepHinh>().SetMeshNormal(hit.triangleIndex, Vector3.one);
            }
        }
    }

    Color GetPixelColor(Transform block, int index, Texture2D texture) 
    {
        Color tempPos = Color.black;
        PlaneXepHinh planeXepHinh = block.GetComponent<PlaneXepHinh>();

        if (index % 2 > 0)
            index -= 1;

        int blockX = planeXepHinh.blockIndex % blocks;
        int blockY = planeXepHinh.blockIndex / blocks;
        int x = (index % 20 / 2) + (blockX * 10);
        int y = (index / 20) + (blockY * 10);

        tempPos = texture.GetPixel(x, y);

        return tempPos;
    }

#if UNITY_EDITOR

    public void ExportMesh()
    {

        List<Mesh> tempMeshL = new List<Mesh>();

        Mesh firstMesh = Instantiate(lsBlocks[0].MeshFilter.sharedMesh);
        tempMeshL.Add(firstMesh);

        AssetDatabase.CreateAsset(firstMesh, "Assets/"+ GrayTex.name+ ".asset");

        for (int i = 1; i < lsBlocks.Count; i++)
        {
            Mesh tempMesh = Instantiate(lsBlocks[i].MeshFilter.sharedMesh);

            AssetDatabase.AddObjectToAsset(tempMesh, firstMesh);

            tempMeshL.Add(tempMesh);


        }

        for (int i = 0; i < lsBlocks.Count; i++)
        {
            lsBlocks[i].MeshFilter.sharedMesh = tempMeshL[i];
        }

        PrefabUtility.CreatePrefab("Assets/" + GrayTex.name + ".prefab", this.gameObject);


        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(firstMesh));
        AssetDatabase.Refresh();
    }
#endif
}
