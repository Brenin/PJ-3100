using UnityEngine;
using UnityEngine.EventSystems;

public class Enlarging : MonoBehaviour, ISelectHandler, IDeselectHandler {

	private RectTransform rect;
	private GamesLoader games;

	public void OnSelect(BaseEventData eventData) {
		games = GamesLoader.current;
		rect = GetComponent<RectTransform>();
		rect.localScale += new Vector3(0.5f, 0.5f, 0);

		if (games.getGameIsRunning() == true) {
			games.setGameIsRunning(false);
			games.reload();
		}
		
	}

	public void OnDeselect(BaseEventData eventData) {
		rect.localScale += new Vector3(-0.5f, -0.5f, 0);
	}
}
