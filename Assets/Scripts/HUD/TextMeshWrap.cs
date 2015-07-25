using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 
 TextSize for Unity3D by thienhaflash (thienhaflash@gmail.com)
 
 Version	: 0.1
 Update		: 18.Jun.2012
 Features	:
	Return perfect size for any TextMesh
 	Memoize the size of each character to speed up the size
	Evaluate and cache only when there are requirements
 
 You are free to use the code or modify it any way you want (even remove this block of comments) but if it's good
 please give it back to the community.
 
 */

public class TextMeshWrap : MonoBehaviour {

    public float textWidth;

    private TextSize textSize;
    private TextMesh textMesh;

    private string text;

    void Start() {
        textMesh = GetComponent<TextMesh>();
        textSize = new TextSize(textMesh);
        setText(textMesh.text);
    }

    void Update() {
        if (textMesh.text != text) {
            setText(textMesh.text);
        }
    }

    public void setText(string s) {
        text = textSize.splitIntoLines(s, textWidth);
        textMesh.text = text;
    }
}

public class TextSize {

    private Dictionary<char, float> dict;

    private TextMesh textMesh;
    private Renderer renderer;

    public TextSize(TextMesh tm) {
        textMesh = tm;
        renderer = tm.GetComponent<Renderer>();
        dict = new Dictionary<char, float>();
        getSpace();
    }

    void getSpace() {
        string oldText = textMesh.text;

        Quaternion oldRotation = renderer.transform.rotation;
        renderer.transform.rotation = Quaternion.identity;

        textMesh.text = "a";
        float aw = renderer.bounds.size.x;
        textMesh.text = "a a";
        float cw = renderer.bounds.size.x - 2 * aw;

        renderer.transform.rotation = oldRotation;

        dict.Add(' ', cw);
        dict.Add('a', aw);

        textMesh.text = oldText;
    }

    float getTextWidth(string s) {
        char[] charList = s.ToCharArray();
        float w = 0;
        char c;
        string oldText = textMesh.text;

        for (int i = 0; i < charList.Length; i++) {
            c = charList[i];

            if (dict.ContainsKey(c)) {
                w += dict[c];
            }
            else {
                Quaternion oldRotation = renderer.transform.rotation;
                renderer.transform.rotation = Quaternion.identity;
                textMesh.text = "" + c;
                float cw = renderer.bounds.size.x;
                dict.Add(c, cw);
                w += cw;
                renderer.transform.rotation = oldRotation;
            }
        }

        textMesh.text = oldText;
        return w;
    }

    public string splitIntoLines(string s, float width) {
        string lines = "";
        string[] words = s.Split(' ');

        float currentWidth = 0;
        foreach (string word in words) {
            float wordWidth = getTextWidth(word);

            if (wordWidth > width) {
                if (currentWidth == 0) {
                    lines += word + "\n";
                }
                else {
                    lines += "\n" + word + "\n";
                }
                currentWidth = 0;
            }
            else {
                currentWidth += wordWidth;
                if (currentWidth > width) {
                    lines += "\n" + word + " ";
                    currentWidth = wordWidth + dict[' '];
                }
                else {
                    lines += word + " ";
                    currentWidth += dict[' '];
                }
            }
        }

        return lines;
    }
}