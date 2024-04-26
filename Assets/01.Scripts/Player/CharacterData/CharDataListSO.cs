using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Character/List")]
public class CharDataListSO : ScriptableObject
{
    public List<CharDataSO> list;

    private void OnValidate()
    {
        for(int i = 0; i < list.Count; i++)
        {
            list[i].characterIndex = i;
        }
    }
}
