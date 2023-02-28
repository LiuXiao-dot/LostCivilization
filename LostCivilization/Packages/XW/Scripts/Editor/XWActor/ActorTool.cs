using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using XWActor;
using XWTool;
namespace XWActorEditor
{
    [Tool("Actor管理")]
    public class ActorTool : ITool
    {
        [Searchable]
        [LabelText("全部角色数据")]
        public ActorConfigs actorConfigs;

        public string newActorType;
        [Button("生成新的Actor类型")]
        public void Generate()
        {
            if (!newActorType.IsNullOrWhitespace()) {
                actorConfigs.configs.Add(newActorType, null);
                actorConfigs.OnValidate();
            }
        }
        
        private void RefreshActorConfigs()
        {
            actorConfigs = ActorConfigs.Instance;
        }
        
        public void InitWithTree(OdinMenuTree tree)
        {
            RefreshActorConfigs();
            foreach (var temp in actorConfigs.configs) {
                tree.Add($"Actor管理/{temp.Key}", temp.Value);
            }
        }
    }
}