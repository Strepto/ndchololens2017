using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using HoloToolkit.Unity.SpatialMapping;

public class SharingAnchorPlacement : MonoBehaviour
{
    public GameObject VisualClue;

    GameStateManager _gameStateManager;

    GestureRecognizer gestureRecognizer;

    bool isPlaceing = true;

    void Start()
    {
        _gameStateManager = GameStateManager.Instance;

#if UNITY_EDITOR
        this.gameObject.transform.position = new Vector3(0, 0, 1);
        this.gameObject.transform.LookAt(new Vector3(0, 0, 0));

        VisualClue.SetActive(false);
        _gameStateManager.SetGameState(GameState.Configuration);
        return;
#endif

        _gameStateManager.gameStateChangedEvent += GameStateChanged;

        gestureRecognizer = new GestureRecognizer();
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
        if (isPlaceing)
        {
            this.gameObject.transform.position = GazeManager.Instance.HitInfo.point;
            isPlaceing = false;
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

    private void GameStateChanged(GameState obj)
    {
        if (_gameStateManager.CurrentGameState == GameState.Sharing)
        {
            gestureRecognizer.StartCapturingGestures();
            SpatialMappingManager.Instance.DrawVisualMeshes = true;
            VisualClue.SetActive(true);
            isPlaceing = true;
        }
    }

    private void Update()
    {
        VisualClue.transform.position = GazeManager.Instance.HitInfo.point;
    }
}
