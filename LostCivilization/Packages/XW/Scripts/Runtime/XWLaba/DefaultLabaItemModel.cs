using UnityEngine;
namespace XWLaba
{
    public class DefaultLabaItemModel : ILabaItemModel
    {
        public Color color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
    }
}