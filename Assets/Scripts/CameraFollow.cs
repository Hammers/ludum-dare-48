using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private MapGenerator mapGenerator;
    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = GameObject.Find("Grid").GetComponent<MapGenerator>();
        target = GameObject.Find("PlayerCharacter").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
