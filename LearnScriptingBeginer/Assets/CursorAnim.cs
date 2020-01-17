using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorAnim : MonoBehaviour
{

    [SerializeField] private float multiplayer;
    [SerializeField] private RectTransform myTr;
    [SerializeField] private Transform shipForward;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color farColor;
    [SerializeField] private Color nearColor;

    private Color standColor;

    private void Start() {
        standColor = image.color;
    }

    private void Update()
    {

        Vector3 dir = new Vector3(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        dir = Vector3.ClampMagnitude(dir,1.01f);

        Vector3 cursorPos = dir * multiplayer;
        
        myTr.anchoredPosition = Vector3.Lerp(myTr.anchoredPosition,cursorPos,Time.deltaTime * 1.5f );

       // Vector3 chek = Camera.main.ScreenToWorldPoint(transform.position);

        bool isHit = Physics.Raycast(shipForward.position,shipForward.forward,out RaycastHit hit);

        if(isHit){
            float distance = (hit.point - shipForward.position).magnitude + 0.01f;

            float red = 150 / distance;
            red = red > 1 ? 1 : red;
            Debug.Log(red + ":" + hit.transform.name);
            Color tCol = image.color;

            text.text = red > .05f ? distance + "" : "";

            image.color = Color.LerpUnclamped(farColor,nearColor,red);
        }else{
            image.color = standColor;
            text.text = "";
        }

        
    }
}
