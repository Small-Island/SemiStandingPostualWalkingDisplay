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
    private enum Status {
        stop, walking
    }
    [UnityEngine.SerializeField, ReadOnly] private Status status;

    private float halfPeriod = 0;
    private float quaterPeriod = 0;

    private UnityEngine.WaitForSeconds waitForQuaterPeriod;
    private UnityEngine.WaitForSeconds waitForHalfPeriod;
    private UnityEngine.WaitForSeconds waitForThreeQuaterPeriod;
    
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

    private void Start() {
    }

    private void setPeriod() {
        this.quaterPeriod = this.period/4.0f;
        this.halfPeriod = this.period/2.0f;
        this.waitForQuaterPeriod = new UnityEngine.WaitForSeconds((float)this.quaterPeriod);
        this.waitForHalfPeriod = new UnityEngine.WaitForSeconds((float)this.halfPeriod);
        this.waitForThreeQuaterPeriod = new UnityEngine.WaitForSeconds((float)(this.period*3.0f/4.0f));
        return;
    }

    public void WalkStraight()
    {
        epos4Main.AllNodeDefinePosition();
        this.setPeriod();
        if (LegCoroutines.lifter == null && activate.lifter) {
            LegCoroutines.lifter = StartCoroutine(this.WalkStraightLifterAsync());
        }
        if (LegCoroutines.leftPedal == null && activate.leftPedal) {
            LegCoroutines.leftPedal = StartCoroutine(this.WalkStraightLeftPedalAsync());
        }
        if (LegCoroutines.leftSlider == null && activate.leftSlider) {
            LegCoroutines.leftSlider = StartCoroutine(this.WalkStraightLeftSliderAsync());
        }
        if (LegCoroutines.rightPedal == null && activate.rightPedal) {
            LegCoroutines.rightPedal = StartCoroutine(WalkStraightRightPedalAsync());
        }
        if (LegCoroutines.rightSlider == null && activate.rightSlider) {
            LegCoroutines.rightSlider = StartCoroutine(WalkStraightRightSliderAsync());
        }
    }

    private System.Collections.IEnumerator WalkStraightLifterAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.lifter.MoveToPositionInTime(-this.amptitude.lift, this.quaterPeriod);
            yield return this.waitForQuaterPeriod;
            epos4Main.lifter.MoveToPositionInTime(0, this.quaterPeriod);
            yield return this.waitForQuaterPeriod;
        }
    }

    private System.Collections.IEnumerator WalkStraightLeftPedalAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.leftPedal.MoveToPositionInTime(this.amptitude.pedal, this.quaterPeriod);
            // epos4Main.leftSlider.MoveToPositionInTime(this.amptitude.leftSlider, this.halfPeriod);
            yield return this.waitForQuaterPeriod;
            epos4Main.leftPedal.MoveToPositionInTime(0, this.quaterPeriod);
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            // yield return this.waitForQuaterPeriod;
            // epos4Main.leftSlider.MoveToPositionInTime(-this.amptitude.leftSlider, this.halfPeriod);
            // yield return this.waitForHalfPeriod;
            // yield return this.waitForThreeQuaterPeriod;
        }
    }

    private System.Collections.IEnumerator WalkStraightLeftSliderAsync() {
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            // System.DateTime dt = System.DateTime.Now;
            // UnityEngine.Debug.Log(dt.Second.ToString() + "." + dt.Millisecond.ToString());
            epos4Main.leftSlider.MoveToPositionInTime(-this.amptitude.slider, this.halfPeriod);
            // yield return this.waitForHalfPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            epos4Main.leftSlider.MoveToPositionInTime(this.amptitude.slider, this.halfPeriod);
            // yield return this.waitForHalfPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
        }
    }

    private System.Collections.IEnumerator WalkStraightRightPedalAsync() {
        // yield return this.waitForHalfPeriod;
        yield return this.waitForQuaterPeriod;
        yield return this.waitForQuaterPeriod;
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            epos4Main.rightPedal.MoveToPositionInTime(this.amptitude.pedal, this.quaterPeriod);
            yield return this.waitForQuaterPeriod;
            epos4Main.rightPedal.MoveToPositionInTime(0, this.quaterPeriod);
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            // yield return this.waitForQuaterPeriod;
            // yield return this.waitForHalfPeriod;
            // yield return this.waitForThreeQuaterPeriod;
        }
    }

    private System.Collections.IEnumerator WalkStraightRightSliderAsync() {
        yield return this.waitForQuaterPeriod;
        yield return this.waitForQuaterPeriod;
        while (true) {
            this.status = Status.walking;
            this.setPeriod();
            // System.DateTime dt = System.DateTime.Now;
            // UnityEngine.Debug.Log(dt.Second.ToString() + "." + dt.Millisecond.ToString());
            epos4Main.rightSlider.MoveToPositionInTime(-this.amptitude.slider, this.halfPeriod);
            // yield return this.waitForHalfPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
            epos4Main.rightSlider.MoveToPositionInTime(this.amptitude.slider, this.halfPeriod);
            // yield return this.waitForHalfPeriod;
            yield return this.waitForQuaterPeriod;
            yield return this.waitForQuaterPeriod;
        }
    }

    public void WalkStop()
    {
        this.status = Status.stop;
        LegCoroutines.stop(this);
        // if (LegCoroutines.lifter != null) {
        //     this.StopCoroutine(LegCoroutines.lifter);
        //     LegCoroutines.lifter = null;
        // }
        // if (LegCoroutines.leftPedal != null) {
        //     StopCoroutine(LegCoroutines.leftPedal);
        //     LegCoroutines.leftPedal = null;
        // }
        // if (LegCoroutines.leftSlider != null) {
        //     StopCoroutine(LegCoroutines.leftSlider);
        //     LegCoroutines.leftSlider = null;
        // }
        // if (LegCoroutines.rightPedal != null) {
        //     StopCoroutine(LegCoroutines.rightPedal);
        //     LegCoroutines.rightPedal = null;
        // }
        // if (LegCoroutines.rightSlider != null) {
        //     StopCoroutine(LegCoroutines.rightSlider);
        //     LegCoroutines.rightSlider = null;
        // }
        epos4Main.AllNodeMoveToHome();
        new UnityEngine.WaitForSeconds(0.5f);
    }

    private void OnDestroy()
    {
        this.WalkStop();
    }
}