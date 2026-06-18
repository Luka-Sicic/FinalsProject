using UnityEngine;

namespace Project.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class VisionConeVisual : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float fov = 90f;
        [SerializeField] private float viewDistance = 5f;
        [SerializeField] private int rayCount = 50;
        [SerializeField] private LayerMask obstacleLayer;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;
        private Color[] _colors;
        private Color _currentColor = Color.white;

        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.name = "VisionConeMesh";
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter.mesh = _mesh;

            
            if (_meshRenderer.sharedMaterial == null)
            {
                _meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"));
            }

            
            if (_meshRenderer.sortingLayerName == "Default" && _meshRenderer.sortingOrder == 0)
            {
                _meshRenderer.sortingLayerName = "Ground";
                _meshRenderer.sortingOrder = 1;
            }
        }

        private void LateUpdate()
        {
            UpdateMesh();
        }

        public void SetColor(Color color)
        {
            _currentColor = color;
        }

        public void SetFov(float newFov)
        {
            fov = newFov;
        }

        public void SetViewDistance(float newDistance)
        {
            viewDistance = newDistance;
        }

        private void UpdateMesh()
        {
            int vertexCount = rayCount + 1 + 1; 
            if (_vertices == null || _vertices.Length != vertexCount)
            {
                _vertices = new Vector3[vertexCount];
                _uv = new Vector2[vertexCount];
                _colors = new Color[vertexCount];
                _triangles = new int[rayCount * 3];
            }

            float angleStep = fov / rayCount;
            float currentAngle = -fov / 2f;

            _vertices[0] = Vector3.zero;
            _uv[0] = new Vector2(0.5f, 0.5f);
            _colors[0] = _currentColor;

            for (int i = 0; i <= rayCount; i++)
            {
                float angleRad = (currentAngle + transform.eulerAngles.z) * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
                
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, obstacleLayer);
                
                Vector3 vertex;
                if (hit.collider != null)
                {
                    vertex = transform.InverseTransformPoint(hit.point);
                }
                else
                {
                    vertex = transform.InverseTransformPoint(transform.position + direction * viewDistance);
                }

                _vertices[i + 1] = vertex;
                _colors[i + 1] = _currentColor;
                
                
                _uv[i + 1] = new Vector2(0.5f + vertex.x / (viewDistance * 2), 0.5f + vertex.y / (viewDistance * 2));

                if (i < rayCount)
                {
                    _triangles[i * 3 + 0] = 0;
                    _triangles[i * 3 + 1] = i + 1;
                    _triangles[i * 3 + 2] = i + 2;
                }

                currentAngle += angleStep;
            }

            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.uv = _uv;
            _mesh.colors = _colors;
            _mesh.triangles = _triangles;
            _mesh.RecalculateBounds();
        }
    }
}
