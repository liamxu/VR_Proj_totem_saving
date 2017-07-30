using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;

public class VoiceRecog : MonoBehaviour {
    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_Recognizer;
    public GameObject Cube;
    public GameObject Sphere;

    void Start() {
        m_Keywords = new string[3];
        m_Keywords[0] = "Cube";
        m_Keywords[1] = "Sphere";
        m_Keywords[2] = "Hand";

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();

    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args) {
        float newX = UnityEngine.Random.Range(-3, 3);
        float newZ = UnityEngine.Random.Range(-3, 3);

        Debug.Log("=========="+args.text);

        if (args.text == m_Keywords[0]) {
            Instantiate(Cube, new Vector3(newX, newZ, 1), Quaternion.identity);
        }
        if (args.text == m_Keywords[1]) {
            Instantiate(Sphere, new Vector3(newX, newZ, 1), Quaternion.identity);
        }
        if (args.text == m_Keywords[2]) {
            if (GameObject.FindGameObjectWithTag("HandModels").active == true) {
                GameObject.FindGameObjectWithTag("HandModels").SetActive(false);
            }
            if (GameObject.FindGameObjectWithTag("HandModels").active == false) {
                GameObject.FindGameObjectWithTag("HandModels").SetActive(true);
            }
        }
    }
}
