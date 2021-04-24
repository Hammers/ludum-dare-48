using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Material seenMat;
    [SerializeField] private Material searchingMat;
    [SerializeField] private Material alertMat;
    public Vision vision;
    private Mesh coneMesh;
    private MeshRenderer mRenderer;

    private const int RAY_COUNT = 20;
    private float totalAngle;
    private float angleStep;
    
    private float distance;
    private Vision.VisionState lastState = Vision.VisionState.Normal;

    void Start()
    {
        coneMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = coneMesh;
        mRenderer = GetComponent<MeshRenderer>();
        mRenderer.sortingLayerID = vision.GetComponent<SpriteRenderer>().sortingLayerID;
        mRenderer.sortingLayerName = vision.GetComponent<SpriteRenderer>().sortingLayerName;
        mRenderer.material = normalMat;
        mRenderer.sortingOrder = -1;
        totalAngle = vision.angle;
        
        angleStep = totalAngle / RAY_COUNT;

        distance = vision.distance;
    }

    Vector3 GetVectorFromAngle(float angle){
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    float GetAngleFromVectorFloat(Vector2 dir){
        Vector3 dirNorm = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        var origin = vision.transform.position;
        float angle = GetAngleFromVectorFloat(vision.transform.up)+(totalAngle/2);

        Vector3[] vertices = new Vector3[RAY_COUNT + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[RAY_COUNT * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= RAY_COUNT; i++){
            Vector3 vertext;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(vision.transform.position, GetVectorFromAngle(angle), distance);
            if (raycastHit2D.collider == null){
                vertext = origin + GetVectorFromAngle(angle) * distance;
            }
            else{
                //Debug.Log("Colliding with "+raycastHit2D.collider.name+" setting point to "+raycastHit2D.point);
                vertext = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertext;

            if(i > 0){
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleStep;
        }

        coneMesh.vertices = vertices;
        coneMesh.uv = uvs;
        coneMesh.triangles = triangles;
        
        if(lastState != vision.state){
            switch(vision.state){
                case Vision.VisionState.Normal:
                    mRenderer.material = normalMat;
                    break;
                case Vision.VisionState.Seen:
                    mRenderer.material = seenMat;
                    break;
                case Vision.VisionState.Searching:
                    mRenderer.material = searchingMat;
                    break;
                case Vision.VisionState.Alert:
                    mRenderer.material = alertMat;
                    break;
            }
            lastState = vision.state;
        }
    }
}
