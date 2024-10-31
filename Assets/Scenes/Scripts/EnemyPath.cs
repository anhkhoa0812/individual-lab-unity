using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints; //List WayPoints
    [SerializeField] private Color color; //Màu của đường vẽ hỗ trợ debug
    [SerializeField] private bool show; //Show đường vẽ hay không hỗ trợ debug

    public Transform[] WayPoints => this.wayPoints; //Get list WayPoints
    private void OnDrawGizmos() //Dùng để vẽ đường đi
    {
        if (!show) return; //Nếu show = false thì không hiển thị đường vẽ

        if(wayPoints != null && wayPoints.Length > 1)//Đảm bảo ít nhất 1 điểm WayPoint
        {
            for(int i=0; i < wayPoints.Length - 1; i++ )
            {
                Transform from = wayPoints[i]; //Điểm bắt đầu của đường vẽ
                Transform to = wayPoints[i + 1]; //Điểm kết thúc của đường vẽ
                Gizmos.color = color; //Qui định màu cho đường vẽ
                Gizmos.DrawLine(from.position, to.position);
            }

            Gizmos.DrawLine(wayPoints[0].position, wayPoints[wayPoints.Length - 1].position); //Nối 2 điểm đầu và cuối của mảng WayPoints
        }
    }
}
