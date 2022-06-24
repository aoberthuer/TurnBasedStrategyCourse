using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld _instance;
    
    private Camera _mainCamera;
    
    [SerializeField] private LayerMask _mousePlaneLayerMask;
    
    private void Awake()
    {
        _instance = this;
        
        _mainCamera = Camera.main;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = _instance._mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _instance._mousePlaneLayerMask);
        return raycastHit.point;
    }
    
}
