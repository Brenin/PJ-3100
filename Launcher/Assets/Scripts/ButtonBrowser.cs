using UnityEngine;

public class ButtonBrowser : MonoBehaviour {

	private GamesLoader games;
	private RectTransform rect;

	public float scrollSpeed = 0.5f;
	private float buttonSpacing = 20f;
	private float buttonWidth = 400f;
	private float maxLeft = 200f;
	private float maxRight;
	private float currentPosition = 200f;
	private float targetPosition;
	private float currentVelocity;

	void Start () {
		games = GamesLoader.current;
		int nrOfGames = games.GetNumberOfGames() - 1;
		maxRight = (buttonSpacing + buttonWidth) * nrOfGames;

		rect = GetComponent<RectTransform>();
		targetPosition = currentPosition;
        rect.anchoredPosition = new Vector2(currentPosition, 0f);
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.RightArrow))
			Move(1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
			Move(-1);

		currentPosition = Mathf.SmoothDamp(currentPosition, targetPosition, ref currentVelocity, scrollSpeed);
		rect.anchoredPosition = new Vector2(currentPosition, 0f);
	}

	void Move(int direction) {
		float newPos = (buttonSpacing + buttonWidth) * -direction;
		targetPosition += newPos;
		targetPosition = Mathf.Clamp(targetPosition, -maxRight + maxLeft, maxLeft);
	}
}
