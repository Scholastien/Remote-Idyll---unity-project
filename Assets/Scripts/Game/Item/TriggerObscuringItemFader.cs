
using UnityEngine;

public class TriggerObscuringItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision){
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFaders.Length > 0){
            foreach (ObscuringItemFader obj in obscuringItemFaders){
                obj.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        ObscuringItemFader[] obscuringItemFaders = collision.gameObject.GetComponentsInChildren<ObscuringItemFader>();

        if(obscuringItemFaders.Length > 0){
            foreach (ObscuringItemFader obj in obscuringItemFaders){
                obj.FadeIn();
            }
        }
    }
}
