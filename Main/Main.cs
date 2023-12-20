using UnityEngine;
// [ExecuteInEditMode]
public enum Controller {
    stop,
    play,
    pause
}
public class Main : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Vector2 thumbStick;
    [SerializeField, ReadOnly]
    public Controller status = Controller.stop;
    // public Epos4Lift epos4Seat;
    public WalkingDisplayMain walkingDisplayMain;
    public UnityEngine.Video.VideoPlayer videoPlayer;
    [SerializeField, UnityEngine.Header("Unit s")]
    private float walkDelayTime = 0f;

    private bool button_play_disabled = false;
    public bool get_button_play_disabled() {
        return this.button_play_disabled;
    }

    private Coroutine playLoopCoroutine = null;
    public void button_play() {
        // if (this.button_play_disabled) {
        //     return;
        // }
        this.main_play();
        // if (this.playLoopCoroutine != null) {
        //     StopCoroutine(this.playLoopCoroutine);
        // }
        // this.playLoopCoroutine = StartCoroutine(playLoopAsync());
    }
    public void button_pause() {
        this.button_play_disabled = false;
        this.main_pause();
    }
    public void button_stop() {
        this.button_play_disabled = false;
        if (playLoopCoroutine != null) {
            StopCoroutine(playLoopCoroutine);
        }
        this.main_stop();
    }

    private async void main_play() {
        this.status = Controller.play;
        // if (videoPlayer) {
            videoPlayer.Play();
        // }
        // yield return new UnityEngine.WaitForSeconds(1.0f);
        await System.Threading.Tasks.Task.Delay((int)(1000*this.walkDelayTime));
        // if (walkingDisplayMain) {
            walkingDisplayMain.WalkStraight();
        // }
        // yield return new UnityEngine.WaitForSeconds(0.1f);
        // if (epos4Seat) {
        //     epos4Seat.WalkStraight();
        // }
        // if (lowerLimbMotorSerial) {
            // lowerLimbMotorSerial.WalkStraight();
        // }
    }

    private System.Collections.IEnumerator playLoopAsync() {
        while (true) {
            this.button_play_disabled = true;
            main_stop();
            // float oldHalfTime = lowerLimbMotorSerial.getOldHalfTime();
            // yield return new UnityEngine.WaitForSeconds((float)3*oldHalfTime/1000.0f);

            // yield return this.main_play();
            // yield return new UnityEngine.WaitForSeconds(1.5f);
            this.button_play_disabled = false;
            yield return new UnityEngine.WaitForSeconds(18f);
        }
    }

    public void main_pause() {
        this.status = Controller.pause;
        if (videoPlayer) {
            videoPlayer.Pause();
        }
        if (walkingDisplayMain) {
            walkingDisplayMain.WalkStop();
        }
        // if (epos4Seat) {
        //     epos4Seat.WalkStop();
        // }
        // if (lowerLimbMotorSerial) {
            // lowerLimbMotorSerial.WalkStop();
        // }
    }

    public void main_stop() {
        this.status = Controller.stop;
        if (videoPlayer) {
            videoPlayer.Stop();
        }
        if (walkingDisplayMain) {
            walkingDisplayMain.WalkStop();
        }
        // if (epos4Seat) {
        //     epos4Seat.WalkStop();
        // }
        // if (lowerLimbMotorSerial) {
            // lowerLimbMotorSerial.WalkStop();
        // }
    }
    
    void Start() {
        this.status = Controller.stop;
    }

    private bool thumbStickFlag = false;
    void Update() {
        this.thumbStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        if (!this.button_play_disabled) {
            if (this.thumbStick.y > 0.9 && !thumbStickFlag) {
                thumbStickFlag = true;
                // lowerLimbMotorSerial.incrementalHallfTime();
                button_play();
            }
            if (this.thumbStick.y < -0.9 && !thumbStickFlag) {
                thumbStickFlag = true;
                // lowerLimbMotorSerial.decrementalHallfTime();
                button_stop();
            }
        }
        if (System.Math.Abs(this.thumbStick.y) < 0.1) {
            thumbStickFlag = false;
        }
    }
}