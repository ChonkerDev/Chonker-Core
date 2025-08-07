using UnityEngine;

namespace Chonker.Core.Scripts.Physics
{
    public static class PhysicsLayerRegistry
    {
        public const string PlayerLayerName = "Local Player";
        public const string LevelGeometryLayerName = "Level Geometry";
        public const string InteractionLayerName = "Interaction";
        public const string UILayerName = "UI";
        public static readonly int LevelGeometryLayerIndex;
        public static readonly int LocalPlayerLayerIndex;
        public static readonly int InteractionLayerIndex = LayerMask.NameToLayer(InteractionLayerName);
        public static readonly int UILayerIndex = LayerMask.NameToLayer(UILayerName);
        public static readonly LayerMask AllLayers;
        public static readonly LayerMask LevelGeometryMask;
        public static readonly LayerMask LocalPlayer;
        public static readonly LayerMask InteractionMask;
        public static readonly LayerMask UIMask;

        static PhysicsLayerRegistry() {
            AllLayers = ~0;
            LevelGeometryLayerIndex = LayerMask.NameToLayer(LevelGeometryLayerName);
            LocalPlayerLayerIndex = LayerMask.NameToLayer(PlayerLayerName);
            LevelGeometryMask = LayerMask.GetMask(LevelGeometryLayerName);
            LocalPlayer = LayerMask.GetMask(PlayerLayerName);
            InteractionMask = LayerMask.GetMask(InteractionLayerName);
            UIMask = LayerMask.GetMask(UILayerName);

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