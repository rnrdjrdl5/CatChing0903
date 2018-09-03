using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect{

    public Vector3 originalLocalPosition { get; set; }

    private List<UIEffectNode> listUIEffectNode;
    public List<UIEffectNode> GetListUIEffectNode() { return listUIEffectNode; }

    public UIEffect()
    {
        listUIEffectNode = new List<UIEffectNode>();
    }


    // 아래의 Lobby용 이벤트와 합칠수 있음. 개선바람.
    public void EffectEvent()
    {

        if (!listUIEffectNode[0].NodeUpdate())
        {

            listUIEffectNode.RemoveAt(0);


            if (listUIEffectNode.Count == 0)
            {
                UIManager.GetInstance().UpdateEvent -= EffectEvent;
                return;
            }
        }

    }

    public void EffectEventLobby()
    {


        if (!listUIEffectNode[0].NodeUpdate())
        {

            listUIEffectNode.RemoveAt(0);


            if (listUIEffectNode.Count == 0)
            {
                LobbyUIManager.GetInstance().UpdateEvent -= EffectEventLobby;
                return;
            }
        }
    }

    public void CheckResetUIEffect(GameObject original)
    {
        if (listUIEffectNode.Count > 0)
        {
            original.transform.localPosition = originalLocalPosition;
            listUIEffectNode.Clear();
            UIManager.GetInstance().UpdateEvent -= EffectEvent;
        }
    }

    public void AddMoveEffectNode(
        GameObject targetObject, 
        Vector2 FirstPosition,
        Vector2 LastPosition, 
        float MaxTime
        )
    {

        UIEffectNode uIEffectNode = new UIEffectNode();
        uIEffectNode.SetMoveData(targetObject, FirstPosition, LastPosition, MaxTime);
        listUIEffectNode.Add(uIEffectNode);

    }

    // 시간, 타입만 지정
    public void AddWaitEffectNode(float MaxTime)
    {
        UIEffectNode uIEffectNode = new UIEffectNode();
        uIEffectNode.SetWaitData(MaxTime);

        listUIEffectNode.Add(uIEffectNode);
    }

    public void AddScaleEffectNode(
        GameObject targetObject,
        float minScale,
        float maxScale,
        float maxTime)
    {

        UIEffectNode uIEffectNode = new UIEffectNode();
        uIEffectNode.SetScaleData(targetObject, minScale, maxScale, maxTime);
        listUIEffectNode.Add(uIEffectNode);
    }

    public void AddShakeEffectNode(GameObject targetObject , int count , int ShakeWeight , UIEffectNode.ShakeType st)
    {
        UIEffectNode uIEffectNode = new UIEffectNode();

        uIEffectNode.SetShakeData(targetObject, count, ShakeWeight , st);
        listUIEffectNode.Add(uIEffectNode);

    }

    // 커스텀 이펙트, 노드 사이에 사용하고 싶은 함수 등록해서 사용
    // 1프레임을 낭비하는 구조임. 커스텀 이벤트 발견 시 continue로 다시 체크하도록 구조변경
    public void AddUIEffectCustom(UIEffectNode.DeleCustom deleCustom)
    {
        UIEffectNode uIEffectNode = new UIEffectNode();
        uIEffectNode.SetCustomEvent(deleCustom);

        uIEffectNode.SetEnumUIEffectType(UIEffectNode.EnumUIEffectType.CUSTOM_EVENT);

        listUIEffectNode.Add(uIEffectNode);
        
    }

    public void AddFadeEffectNode(GameObject targetObject, float maxTime, UIEffectNode.EnumFade enumFade)
    {

        UIEffectNode uIEffectNode = new UIEffectNode();
        uIEffectNode.SetFadeData(targetObject, maxTime, enumFade);

        listUIEffectNode.Add(uIEffectNode);
    }
}
