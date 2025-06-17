using Cinemachine;
using UnityEngine;

namespace Core.Service
{
    public interface ICameraService
    {
        Camera UnityCamera { get; }
        CinemachineVirtualCamera MainVirtualCamera { get; }
    }
}