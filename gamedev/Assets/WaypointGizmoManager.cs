using System.Collections;
using UnityEngine;

public class WaypointGizmoManager : MonoBehaviour
{


    [SerializeField] private Transform[] m_waypointTransforms;
    [SerializeField] private float m_gizmoSize = 1;
    
    // Start is called before the first frame update
    public void OnValidate()
    {
      


    }
 
    void OnDrawGizmos() {
        m_waypointTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in m_waypointTransforms)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(t.position, m_gizmoSize);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
