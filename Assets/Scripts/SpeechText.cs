using HoloToolkit.Unity;
using UnityEngine;

public class SpeechText : MonoBehaviour
{
    void Start()
    {
        TextToSpeechManager tt = this.GetComponent<TextToSpeechManager>();
        tt.Voice = TextToSpeechVoice.Default;
        tt.SpeakText("Welcome To Explore HoloLens. A Holographic View of a HoloLens device. You can use Gaze, Gesture and Voice Command to explore different components. Walk around and start exploring!");
    }
}
