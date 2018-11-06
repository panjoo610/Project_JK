using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour {

    Text Text;
	void Start ()
    {
         Text = GetComponent<Text>();
        StartCoroutine(BinkText());
	}
	public IEnumerator BinkText()
    {
        while (true)
        {
            Text.text = " ";
            yield return new WaitForSeconds(0.5f);
            Text.text = "게임을 시작하려면 클릭하세요.";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
