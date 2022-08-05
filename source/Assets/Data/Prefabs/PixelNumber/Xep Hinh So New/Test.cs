using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Test : MonoBehaviour
{
    List<PlaneXepHinh> lsBlocks = new List<PlaneXepHinh>();
    public Sprite GrayTex;
    int blocks;
    Vector2 textureSize = new Vector2(512, 512);

    List<List<PlaneXepHinh>> blockMatrix = new List<List<PlaneXepHinh>>();
    void Awake()
    {
        lsBlocks = GetComponentsInChildren<PlaneXepHinh>().ToList();
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (i >= blockMatrix.Count)
                    blockMatrix.Add(new List<PlaneXepHinh>());
                blockMatrix[i].Add(lsBlocks[index]);
                index++;
            }
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.2f);
        textureSize.x = GrayTex.texture.width;
        textureSize.y = GrayTex.texture.height;
        blocks = (int)Mathf.Max(textureSize.x, textureSize.y) / 10 + ((int)Mathf.Max(textureSize.x, textureSize.y) % 10 == 0 ? 0 : 1);
        int current = -1;
        int delta = 0;
        for (int j = 0; j < (int)textureSize.y; j++)
        {
            for (int i = 0; i < (int)textureSize.x; i++)
            {
                Color tempColor =  GrayTex.texture.GetPixel(i, j);

                int xBlock = i / 10;
                int xIndex = i % 10;
                int yBlock = j / 10;
                int yIndex = j % 10;

                int blockNum = xBlock + yBlock * blocks;
                int indexNum = xIndex + yIndex * 10;
                //var index = blockNum / blocks;
                //if (current != blockNum)
                //{
                //    current = blockNum;
                //    delta = 0;
                //}
                GetPlane(blockNum,blocks).SetMeshColor(indexNum * 2, tempColor);
                //delta++;
            }
        }
    }

    PlaneXepHinh GetPlane(int blockIndex,int blockCount)
    {
        var x = blockIndex / blockCount;
        int y = blockIndex - (blockCount * x);
        return blockMatrix[x][y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
