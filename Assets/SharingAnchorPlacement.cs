using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.SpatialMapping;

public class SharingAnchorPlacement : MonoBehaviour
{
    public GameObject VisualClue;

    GameStateManager _gameStateManager;

    GestureRecognizer gestureRecognizer;

    bool isPlacing = true;

    void Start()
    {
        _gameStateManager = GameStateManager.Instance;

#if UNITY_EDITOR
        Debug.Log("Sharing position is disabled in Unity Editor.");
        this.gameObject.transform.position = new Vector3(0, 0, 1);
        this.gameObject.transform.LookAt(new Vector3(0, 0, 0));

        VisualClue.SetActive(false);
        _gameStateManager.SetGameState(GameState.Configuration);
        return;
#endif


        gestureRecognizer = new GestureRecognizer();
        _gameStateManager.gameStateChangedEvent += GameStateChanged;
        gestureRecognizer.TappedEvent += OnTappedEvent;
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        if (_gameStateManager.CurrentGameState == GameState.Sharing)
        {
            gestureRecognizer.StartCapturingGestures();
            SpatialMappingManager.Instance.DrawVisualMeshes = true;
        }
    }

    private void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (isPlacing)
        {
            this.gameObject.transform.position = GazeManager.Instance.HitInfo.point;
            isPlacing = false;
        }
        else
        {
            this.gameObject.transform.LookAt(GazeManager.Instance.HitInfo.point);

            gestureRecognizer.StopCapturingGestures();
            VisualClue.SetActive(false);
            SpatialMappingManager.Instance.DrawVisualMeshes = false;
            _gameStateManager.SetGameState(GameState.Configuration);
        }
    }

    private void GameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Sharing)
        {
            gestureRecognizer.StartCapturingGestures();
            SpatialMappingManager.Instance.DrawVisualMeshes = true;
            VisualClue.SetActive(true);
            isPlacing = true;
        }
    }

    private void Update()
    {
        VisualClue.transform.position = GazeManager.Instance.HitInfo.point;
    }
}
