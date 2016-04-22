using UnityEngine;
using UnityEngine.EventSystems;

public class Enlarging : MonoBehaviour, ISelectHandler, IDeselectHandler {

	private RectTransform rect;

	public void OnSelect(BaseEventData eventData) {
		rect = GetComponent<RectTransform>();
		rect.localScale += new Vector3(0.5f, 0.5f, 0);
	}

	public void OnDeselect(BaseEventData eventData) {
		rect.localScale += new Vector3(-0.5f, -0.5f, 0);
	}
}
