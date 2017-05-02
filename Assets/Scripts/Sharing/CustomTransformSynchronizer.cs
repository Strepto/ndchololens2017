using HoloToolkit.Sharing.SyncModel;
using UnityEngine;

public class CustomTransformSynchronizer : MonoBehaviour
{
    Vector3 _lastPosition = Vector3.zero;
    Quaternion _lastRotation = Quaternion.identity;
    Vector3 _lastScale = Vector3.one;
    Transform _transform;

    #region SyncTransform
    SyncTransform _transformDataModel;

    /// <summary>
    /// Data model to which this transform synchronization is tied to.
    /// </summary>
    public SyncTransform TransformDataModel
    {
        private get { return _transformDataModel; }
        set
        {
            enabled = true;
            if (_transformDataModel != value)
            {
                UnregisterDataModelEvents();

                _transformDataModel = value;
                Initialize();

                if (_transformDataModel != null)
                {
                    // Set the position, rotation and scale to what they should be
                    _transform.localPosition = _transformDataModel.Position.Value;
                    _transform.localRotation = _transformDataModel.Rotation.Value;
                    _transform.localScale = _transformDataModel.Scale.Value;

                    // Register to changes
                    _transformDataModel.PositionChanged += OnPositionChanged;
                    _transformDataModel.RotationChanged += OnRotationChanged;
                    _transformDataModel.ScaleChanged += OnScaleChanged;
                }
            }
        }
    }

    private void Awake()
    {
        enabled = false;
    }

    private void UnregisterDataModelEvents()
    {
        if (_transformDataModel != null)
        {
            _transformDataModel.PositionChanged -= OnPositionChanged;
            _transformDataModel.RotationChanged -= OnRotationChanged;
            _transformDataModel.ScaleChanged -= OnScaleChanged;
        }
    }

    private void OnDestroy()
    {
        UnregisterDataModelEvents();
    }

    private void OnPositionChanged()
    {
        _transform.localPosition = _transformDataModel.Position.Value;
        _lastPosition = _transformDataModel.Position.Value;
    }

    private void OnRotationChanged()
    {
        _lastRotation = _transformDataModel.Rotation.Value;
        _transform.localRotation = TransformDataModel.Rotation.Value;
    }

    private void OnScaleChanged()
    {
        _lastScale = _transformDataModel.Scale.Value;
        _transform.localScale = _transformDataModel.Scale.Value;
    }
    #endregion

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        // Determine if the transform has changed locally, in which case we need to update the data model
        if (_transform.localPosition != _lastPosition)
        {
            _lastPosition = _transform.localPosition;
            _transformDataModel.Position.Value = _transform.localPosition;
        }
        if (Quaternion.Angle(_transform.localRotation, _lastRotation) > 0.002f)
        {
            _lastRotation = _transform.localRotation;
            _transformDataModel.Rotation.Value = _transform.localRotation;
        }
        if (_transform.localScale != _lastScale)
        {
            _lastScale = _transform.localScale;
            _transformDataModel.Scale.Value = _transform.localScale;
        }
    }
}
