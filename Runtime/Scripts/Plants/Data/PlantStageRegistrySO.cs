using UnityEngine;

namespace Eughc.Farm {
    [CreateAssetMenu(fileName = "PlantStageRegistry", menuName = "Farm/PlantStageRegistry")]
    public class PlantStageRegistrySO : ScriptableObject {
        private static PlantStageRegistrySO _instance;
        public static PlantStageRegistrySO Instance {
            get {
                if (_instance == null)
                    _instance = Resources.Load<PlantStageRegistrySO>("PlantStageRegistry");
                return _instance;
            }
        }

        [SerializeField] private PlantStageInfo[] definitions;

        public PlantStageInfo Get(PlantStage stage) {
            foreach (var def in definitions)
                if (def.Stage == stage) return def;
            return null;
        }
    }
}