//--------------------------------
//
// Voxels for Unity
//  Version: 1.2.4
//
// © 2014-21 by Ronny Burkersroda
//
//--------------------------------


using UnityEngine;
using UnityEditor;


namespace Voxels
{

    // Editor extension for Voxel Particle System component
    [CustomEditor(typeof(ParticleSystemCreator))]
    public class VoxelParticleSystemEditor : Editor
    {

        // Show and process inspector
        public override void OnInspectorGUI()
        {
            ParticleSystemCreator voxelParticleSystem = (ParticleSystemCreator)target;

            //// Show title at first
            //EditorGUILayout.LabelField(Information.Title, EditorStyles.centeredGreyMiniLabel);

            // Object selection for game object to use as template
            GameObject template = (GameObject)EditorGUILayout.ObjectField("Object Template", voxelParticleSystem.template, typeof(GameObject), true);
            if (voxelParticleSystem.template != template)
            {
                Undo.RecordObject(voxelParticleSystem, "Template Object Change");
                voxelParticleSystem.template = template;
            }

            // Sizing factor for the voxel particle
            EditorGUILayout.BeginHorizontal();
            float sizeFactor = EditorGUILayout.FloatField("Size Factor", voxelParticleSystem.sizeFactor);
            if (GUILayout.Button("Reset"))
            {
                sizeFactor = 1;
            }
            if (voxelParticleSystem.sizeFactor != sizeFactor)
            {
                Undo.RecordObject(voxelParticleSystem, "Size Factor Change");
                voxelParticleSystem.sizeFactor = sizeFactor;
            }
            EditorGUILayout.EndHorizontal();

            // Name of the main target container
            string targetName = EditorGUILayout.TextField("Target Name", voxelParticleSystem.targetName);
            if (voxelParticleSystem.targetName != targetName)
            {
                Undo.RecordObject(voxelParticleSystem, "Target Object Name Change");
                voxelParticleSystem.targetName = targetName;
            }
        }

    }

}