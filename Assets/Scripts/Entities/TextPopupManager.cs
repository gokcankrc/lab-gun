using Ky;
using UnityEngine;

public class TextPopupManager : Singleton<TextPopupManager>
{
    public TextPopupHandler prefab;

    public void SetUp(TextPopup dep)
    {
        var prf = dep.prefab == null ? prefab : dep.prefab;
        var newPopup = Instantiate(prf, dep.transform.position + Vector3.back, Quaternion.identity, dep.transform);
        newPopup.Init(dep);
    }
}