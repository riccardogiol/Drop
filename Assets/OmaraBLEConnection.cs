using UnityEngine;
using OVR;

public class OmaraBLEConnection : MonoBehaviour
{

    void Start()
    {
        TryConnect();
    }

    public void TryConnect()
    {
        if (ConnectionManager.instance == null)
            return;
        if (ConnectionManager.instance.isConnected)
            return;
        ConnectionManager.instance.OnNewDeviceFound.AddListener(OnDeviceFound);
        ConnectionManager.instance.OnBLEInitError.AddListener(
            reason => Debug.LogError("Omara BLE initialization failed: " + reason));
        ConnectionManager.instance.OnBLEConnectionAttemptFailed.AddListener(
            reason => Debug.LogWarning("Omara connection failed: " + reason));

        ConnectionManager.instance.Init(
            OVR.API.DeviceState.BLE_STATE,
            BLEConnectionStyle.SCAN);
    }

    private void OnDeviceFound(CrossPlatformDevice device)
    {
        ConnectionManager.instance.StopScan();
        ConnectionManager.instance.ConnectToDevice(device.nativeDeviceRef);
    }
}
