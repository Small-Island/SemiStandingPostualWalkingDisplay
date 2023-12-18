// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.IO;
// using System.Text;
// using UnityEngine.AddressableAssets;

public class Epos4Main : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField]
    public Epos4Node lifter, leftPedal, leftSlider, rightPedal, rightSlider;

    private UnityEngine.Coroutine coroutineActualPosition = null;

    private UnityEngine.WaitForSeconds waitForSeconds;

    void Start() {
        EposCmd.Net.DeviceManager connector = null;
        try {
            connector = new EposCmd.Net.DeviceManager("EPOS4", "MAXON SERIAL V2", "USB", "USB0");
        }
        catch (EposCmd.Net.DeviceException) {
        }
        this.lifter      = new Epos4Node(connector, 1, "Lifter",       2);
        this.lifter.MotorInit();
        this.leftPedal   = new Epos4Node(connector, 2, "Left Pedal",   6);
        this.leftPedal.MotorInit();
        this.leftSlider  = new Epos4Node(connector, 3, "Left Slider",  12);
        this.leftSlider.MotorInit();
        this.rightPedal  = new Epos4Node(connector, 4, "Right Pedal",  6);
        this.rightPedal.MotorInit();
        this.rightSlider = new Epos4Node(connector, 5, "Right Slider", 12);
        this.rightSlider.MotorInit();
        this.waitForSeconds = new UnityEngine.WaitForSeconds(0.1f);
        this.coroutineActualPosition = StartCoroutine(this.getActualPositionAsync());
    }

    public void clearError() {
        EposCmd.Net.DeviceManager connector = null;
        try {
            connector = new EposCmd.Net.DeviceManager("EPOS4", "MAXON SERIAL V2", "USB", "USB0");
        }
        catch (EposCmd.Net.DeviceException) {
        }
        this.lifter      = new Epos4Node(connector, 1, "Lifter",       2);
        this.leftPedal   = new Epos4Node(connector, 2, "Left Pedal",   6);
        this.leftSlider  = new Epos4Node(connector, 3, "Left Slider",  12);
        this.rightPedal  = new Epos4Node(connector, 4, "Right Pedal",  6);
        this.rightSlider = new Epos4Node(connector, 5, "Right Slider", 12);
    }

    // void Update() {
    // }

    private System.Collections.IEnumerator getActualPositionAsync() {
        while (true) {
            this.lifter.actualPosition      = this.lifter.getPositionIs();
            this.leftPedal.actualPosition   = this.leftPedal.getPositionIs();
            this.leftSlider.actualPosition  = this.leftSlider.getPositionIs();
            this.rightPedal.actualPosition  = this.rightPedal.getPositionIs();
            this.rightSlider.actualPosition = this.rightSlider.getPositionIs();
            yield return this.waitForSeconds;
        }
    }

    public void AllNodeMoveToHome()
    {
        this.lifter.MoveToHome();
        this.leftPedal.MoveToHome();
        this.leftSlider.MoveToHome();
        this.rightPedal.MoveToHome();
        this.rightSlider.MoveToHome();
    }

    public void AllNodeDefinePosition()
    {
        this.lifter.definePosition();
        this.leftPedal.definePosition();
        this.leftSlider.definePosition();
        this.rightPedal.definePosition();
        this.rightSlider.definePosition();
    }
}
