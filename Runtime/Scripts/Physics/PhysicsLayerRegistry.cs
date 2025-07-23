using UnityEngine;

namespace Chonker.Core.Scripts.Physics
{
    public static class PhysicsLayerRegistry
    {
        public const string PlayerLayerName = "Local Player";
        public const string LevelGeometryLayerName = "Level Geometry";
        public static readonly LayerMask AllLayers;
        public static readonly LayerMask LevelGeometry;
        public static readonly LayerMask LocalPlayer;

        static PhysicsLayerRegistry() {
            AllLayers = ~0;
            LevelGeometry = LayerMask.GetMask(LevelGeometryLayerName);
            LocalPlayer = LayerMask.GetMask(PlayerLayerName);

#if UNITY_EDITOR
            validateLayer(PlayerLayerName);
            validateLayer(LevelGeometryLayerName);
#endif
        }

#if UNITY_EDITOR
        private static void validateLayer(string layerName) {
            if (LayerMask.NameToLayer(layerName) == -1)
                Debug.LogWarning($"[PhysicsLayerRegistry] Missing Layer: '{PlayerLayerName}'");
        }
#endif
    }
}