using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    // 路径脚本
    [SerializeField]
    public WaypointCircuit circuit;

    //移动距离
    public float dis = 0;
    //移动速度
    public float speed = 1;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //计算距离
        dis += Time.deltaTime * speed;
        //获取相应距离在路径上的位置坐标
        transform.position = circuit.GetRoutePoint(dis).position;
        //获取相应距离在路径上的方向
        transform.rotation = Quaternion.LookRotation(circuit.GetRoutePoint(dis).direction);
    }
}
