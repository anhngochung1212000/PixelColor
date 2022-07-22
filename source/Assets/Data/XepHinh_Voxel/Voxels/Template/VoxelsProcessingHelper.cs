using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class containing helper methods for Voxels
/// </summary>
public class VoxelsProcessingHelper : MonoBehaviour
{

    /// <summary>
    /// Recompute bounding box for the rasterizer
    /// </summary>
    /// <param name="rasterizer">Rasterizer instance</param>
    /// <param name="source">Game object to be processed</param>
    public virtual void AdaptBoundaries(Voxels.RasterizerBase rasterizer, GameObject source)
    {
        rasterizer.RecomputeBounds();
    }

    /// <summary>
    /// Helper method to enforce shader recompilation for materials with an emissive component
    /// </summary>
    /// <param name="source">Object to process</param>
    public virtual void ReenableMaterialEmission(Object source)
    {
        // Check for game object instance
        var gameObject = source as GameObject;
        if (gameObject)
        {
            var materials = new List<Material>();

            // Get all materials used by renderers, which are attached to this instance
            CollectMaterials(materials, gameObject);

            // Reset emissive color map keyword to force shader recompilation
            for (int index = 0; index < materials.Count; ++index)
            {
                materials[index].DisableKeyword("_EMISSIVE_COLOR_MAP");
                materials[index].EnableKeyword("_EMISSIVE_COLOR_MAP");
            }
        }
    }

    /// <summary>
    /// Store materials of renderers of the given object to given list
    /// </summary>
    /// <param name="materials">List to store materials to</param>
    /// <param name="source">Object to process</param>
    protected virtual void CollectMaterials(List<Material> materials, GameObject source)
    {
        // Try to get mesh renderers
        var meshRenderers = source.GetComponentsInChildren<MeshRenderer>(true);
        if (meshRenderers != null)
        {
            // Store shared materials, which are not already in the list
            for (int index = 0; index < meshRenderers.Length; ++index)
            {
                AddUniqueElements(materials, meshRenderers[index].sharedMaterials);
            }
        }
        else
        {
            // Try to get particle system renderers
            var particleSystemRenderers = source.GetComponentsInChildren<ParticleSystemRenderer>(true);
            if (particleSystemRenderers != null)
            {
                // Store shared materials, which are not already in the list
                for (int index = 0; index < particleSystemRenderers.Length; ++index)
                {
                    AddUniqueElements(materials, particleSystemRenderers[index].sharedMaterials);
                }
            }
        }
    }

    /// <summary>
    /// Add given elements to given list, if they not already included
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="elements">List to store to</param>
    /// <param name="sources">Enumeration interface</param>
    protected void AddUniqueElements<T>(List<T> elements, IEnumerable<T> sources)
    {
        foreach (var source in sources)
        {
            AddUniqueElement(elements, source);
        }
    }

    /// <summary>
    /// Add given element to given list, if they not already included
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="elements">List to store to</param>
    /// <param name="source">Element to process</param>
    protected void AddUniqueElement<T>(List<T> elements, T source)
    {
        if (!elements.Contains(source))
        {
            elements.Add(source);
        }
    }

}
