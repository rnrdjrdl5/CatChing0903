using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnOffLightState
{
    [Header(" - 암전 패널입니다.")]
    [Tooltip(" - 암전 UI가 들어있는 패널입니다.")]
    public GameObject TrunOffPanel;

    [Header(" - 암전 지속시간입니다.")]
    [Tooltip(" - 불이 꺼지고 나서 유지되는 지속시간입니다.")]
    public float TurnOffTime;


}
