using UnityEngine;


public class VoxelsTest : MonoBehaviour
{

	// Use this for initialization
	void Start()
    {
        // Get rasterizer component or create a new one and disable it to prevent automatic processing
        var rasterizer = gameObject.GetComponent<Voxels.RasterizerBase>();
        if (rasterizer == null)
        {
            rasterizer = gameObject.AddComponent<Voxels.Rasterizer>();
        }
        if (rasterizer)
        {
            rasterizer.enabled = false;
        }

        // Scan this game object by Voxels converter engine and transfer result data to attached processor instances
        bool success = Voxels.Rasterizer.Engine.Process(
            rasterizer,
            new Voxels.Rasterizer.Settings(
                Voxels.Rasterizer.Engine.ComputeBounds(gameObject),
                new Vector3(0.05f, 0.05f, 0.05f),
                0,
                1,
                Voxels.BakingOperation.MostFrequentColor,
                null,
                null,
                true,
                false,
                false,
                1.0f,
                1.0f
                ),
            null,
            OnObjectProcessed
            );

        Debug.Log("Voxels.Rasterizer.Engine.Process() returned: " + success.ToString());
	}

    void OnObjectProcessed(UnityEngine.Object[] objects, object parameter)
    {
        // Hide original object
        gameObject.SetActive(false);
    }

}
