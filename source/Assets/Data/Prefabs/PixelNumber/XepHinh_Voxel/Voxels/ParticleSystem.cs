//--------------------------------
//
// Voxels for Unity
//  Version: 1.2.4
//
// © 2014-21 by Ronny Burkersroda
//
//--------------------------------


using UnityEngine;


namespace Voxels
{

    // Processor class to convert voxels to particles
    [AddComponentMenu(""), RequireComponent(typeof(Rasterizer)), System.Obsolete("Component has been renamed to Particle System Creator because of a conflict with internal Particle System class.")]
    public class ParticleSystem : ParticleSystemCreator
    {
    }

}