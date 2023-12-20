using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDisplayMain : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField, Range(0.38f, 1.4f)] private float period = 1.4f;
    // [UnityEngine.SerializeField] private UnityEngine.AddressableAssets.AssetReference csvFile;

    [UnityEngine.SerializeField] private Activate activate;

    [System.Serializable]
    public class Activate {
        // Unit mm
        [UnityEngine.SerializeField] public bool lifter      = false;
        [UnityEngine.SerializeField] public bool leftPedal   = false;
        [UnityEngine.SerializeField] public bool leftSlider  = false;
        [UnityEngine.SerializeField] public bool rightPedal  = false;
        [UnityEngine.SerializeField] public bool rightSlider = false;
    }

    [UnityEngine.Header("Unit mm")]
    [UnityEngine.SerializeField] private Amptitude amptitude;

    [System.Serializable]
    private class Amptitude {
        // Unit mm
        [UnityEngine.SerializeField, Range(0, 10)] public double lift = 1;
        [UnityEngine.SerializeField, Range(0, 20)] public double pedal = 1;
        [UnityEngine.SerializeField, Range(0, 40)] public double slider = 1;
    }

    [UnityEngine.SerializeField] private Epos4Main epos4Main;
    public enum Status {
        stop, walking
    }
    [UnityEngine.SerializeField, ReadOnly] public Status status;

    private float halfPeriod = 0;
    private float quaterPeriod = 0;

    private UnityEngine.WaitForSeconds waitForQuaterPeriod;
    
    class LegCoroutines {
        public static UnityEngine.Coroutine lifter      = null;
        public static UnityEngine.Coroutine leftPedal   = null;
        public static UnityEngine.Coroutine leftSlider  = null;
        public static UnityEngine.Coroutine rightPedal  = null;
        public static UnityEngine.Coroutine rightSlider = null;

        public static void stop(WalkingDisplayMain arg_walkMain) {
            if (LegCoroutines.lifter != null) {
                arg_walkMain.StopCoroutine(LegCoroutines.lifter);
                LegCoroutines.lifter = null;
            }
            if (LegCoroutines.leftPedal != null) {
                arg_walkMain.StopCoroutine(LegCoroutines.leftPedal);
                LegCoroutines.leftPedal = null;
            }
            if (LegCoroutines.leftSlider != null) {
                arg_walkMain.StopCoroutine(LegCoroutines.leftSlider);
                LegCoroutines.leftSlider = null;
            }
            if (LegCoroutines.rightPedal != null) {
                arg_walkMain.StopCoroutine(LegCoroutines.rightPedal);
                LegCoroutines.rightPedal = null;
            }
            if (LegCoroutines.rightSlider != null) {
                arg_walkMain.StopCoroutine(LegCoroutines.rightSlider);
                LegCoroutines.rightSlider = null;
            }
            return;
        }
    }

    class LegThreads {
        public static System.Threading.Thread lifter      = null;
        public static System.Threading.Thread leftPedal   = null;
        public static System.Threading.Thread leftSlider  = null;
        public static System.Threading.Thread rightPedal  = null;
        public static System.Threading.Thread rightSlider = null;

        public static void start(WalkingDisplayMain arg_walkMain) {
            if (arg_walkMain.activate.lifter) {
                LegThreads.lifter = new System.Threading.Thread(new System.Threading.ThreadStart(arg_walkMain.WalkStraightLifterAsync));
                LegThreads.lifter.Start();
            }
            if (arg_walkMain.activate.leftPedal) {
                LegThreads.leftPedal = new System.Threading.Thread(new System.Threading.ThreadStart(arg_walkMain.WalkStraightLeftPedalAsync));
                LegThreads.leftPedal.Start();
            }
            if (arg_walkMain.activate.leftSlider) {
                LegThreads.leftSlider = new System.Threading.Thread(new System.Threading.ThreadStart(arg_walkMain.WalkStraightLeftSliderAsync));
                LegThreads.leftSlider.Start();
            }
            if (arg_walkMain.activate.rightPedal) {
                LegThreads.rightPedal = new System.Threading.Thread(new System.Threading.ThreadStart(arg_walkMain.WalkStraightRightPedalAsync));
                LegThreads.rightPedal.Start();
            }
            if (arg_walkMain.activate.rightSlider) {
                LegThreads.rightSlider = new System.Threading.Thread(new System.Threading.ThreadStart(arg_walkMain.WalkStraightRightSliderAsync));
                LegThreads.rightSlider.Start();
            }
        }
        public static void stop() {
            if (LegThreads.lifter != null) {
                LegThreads.lifter.Abort();
                LegThreads.lifter = null;
            }
            if (LegThreads.leftPedal != null) {
                LegThreads.leftPedal.Abort();
                LegThreads.leftPedal = null;
            }
            if (LegThreads.leftSlider != null) {
                LegThreads.leftSlider.Abort();
                LegThreads.leftSlider = null;
            }
            if (LegThreads.rightPedal != null) {
                LegThreads.rightPedal.Abort();
                LegThreads.rightPedal = null;
            }
            if (LegThreads.rightSlider != null) {
                LegThreads.rightSlider.Abort();
                LegThreads.rightSlider = null;
            }
            return;
        }
    }

    private void Start() {
    }

    private void setPeriod() {
        this.quaterPeriod = this.period/4.0f;
        this.halfPeriod = this.period/2.0f;
        return;
    }

    public void WalkStraight(float incdec_time)
    {
        this.period = this.period + incdec_time;
        epos4Main.AllNodeDefinePosition();
        this.setPeriod();
        LegThreads.start(this);
    }

    private void WalkStraightLifterAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.lifter.MoveToPositionInTime(-this.amptitude.lift, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            // System.Threading.Tasks.Task.Delay((int)(1000*this.quaterPeriod));
            epos4Main.lifter.MoveToPositionInTime(0, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            // System.Threading.Tasks.Task.Delay((int)(1000*this.quaterPeriod));
        }
    }

    private void WalkStraightLeftPedalAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.leftPedal.MoveToPositionInTime(this.amptitude.pedal, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            epos4Main.leftPedal.MoveToPositionInTime(0, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
        }
    }


    private void WalkStraightLeftSliderAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.leftSlider.MoveToPositionInTime(-this.amptitude.slider, this.halfPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            epos4Main.leftSlider.MoveToPositionInTime(this.amptitude.slider, this.halfPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
        }
    }

    private void WalkStraightRightPedalAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod)); //
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod)); //
            epos4Main.rightPedal.MoveToPositionInTime(this.amptitude.pedal, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            epos4Main.rightPedal.MoveToPositionInTime(0, this.quaterPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
        }
    }

    private void WalkStraightRightSliderAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            epos4Main.rightSlider.MoveToPositionInTime(-this.amptitude.slider, this.halfPeriod);
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            System.Threading.Thread.Sleep((int)(1000*this.quaterPeriod));
            epos4Main.rightSlider.MoveToPositionInTime(this.amptitude.slider, this.halfPeriod);
        }
    }

    public void WalkStop()
    {
        // LegCoroutines.stop(this);
        LegThreads.stop();
        this.status = Status.stop;
        epos4Main.AllNodeMoveToHome();
    }

    private void OnDestroy()
    {
        this.WalkStop();
    }
}