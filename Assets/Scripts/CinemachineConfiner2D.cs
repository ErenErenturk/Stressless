using UnityEngine;
using Cinemachine;

[AddComponentMenu("Cinemachine/Confiners/Cinemachine Confiner 2D")]
[DisallowMultipleComponent]
[ExecuteAlways]
[SaveDuringPlay]
public class CinemachineConfiner2D : CinemachineExtension
{
    public Collider2D m_BoundingShape2D;
    public float m_Damping = 0f;

    Vector3 m_Delta;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (m_BoundingShape2D == null || stage != CinemachineCore.Stage.Body)
            return;

        var pos = state.RawPosition;
        var closest = (Vector3)m_BoundingShape2D.ClosestPoint(pos);  // 🔧 casting fix
        m_Delta = closest - pos;

        if (deltaTime >= 0 && m_Damping > 0)
            m_Delta = Vector3.Lerp(Vector3.zero, m_Delta, 1 - Mathf.Exp(-m_Damping * deltaTime));

        state.PositionCorrection += m_Delta;
    }
}
