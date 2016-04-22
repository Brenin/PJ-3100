using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamesLoader : MonoBehaviour {

	public static GamesLoader current;
	public GameObject genericButton;
	public EventSystem eventSystem;
	private GUIStyle borders;
	private Rect rect;

	public string rootFolderName;
	private string _path;
	private string _url;
	private string usbPath = "D:\\Games";
	private bool buttonIsSelected;
	private bool driveInserted;
	private int numberOfGames = 0;
	private object yield;

	void Awake() {
		Cursor.visible = false;
		Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, 
						Screen.resolutions[Screen.resolutions.Length - 1].height + 150, false);

		driveInserted = Directory.Exists(usbPath);
		InvokeRepeating("LookForUSBDrive", 0f, 5f);
		current = this;

		_path = Directory.GetCurrentDirectory() + "\\" + rootFolderName;
	}

	void Start() {
		loadButtons();
	}

	void LookForUSBDrive() 
	{
		if (driveInserted == Directory.Exists(usbPath)) return;

		driveInserted = Directory.Exists(usbPath);
		RefreshScene();
    }
	
	public void loadButtons() {
		if (Directory.Exists(usbPath)) {
			readingGames(_path);
			readingGames(usbPath);
		} else {
			readingGames(_path);
		}
	}

	public void readingGames(string _tmp) {

		foreach (string s in Directory.GetDirectories(_tmp)) {
			string gameName = s.Remove(0, _tmp.Length + 1);
			numberOfGames++;

			// instatiate object
			GameObject lastButton = Instantiate(genericButton, transform.position, Quaternion.identity) as GameObject;

			// Set button name
			lastButton.name = gameName;
			lastButton.transform.GetChild(0).GetComponent<Text>().text = gameName;
			lastButton.transform.SetParent(transform);

			// Set actionlistener
			lastButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(lastButton.GetComponent<Button>()));

			//Set Button Image
			string imageLocation = _tmp + "\\" + gameName + "\\image.png";

			byte[] bytes;
			bytes = System.IO.File.ReadAllBytes(imageLocation);
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(bytes);
			lastButton.GetComponent<Image>().sprite = Sprite.Create(texture,
								new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

			//Set Description
			string descriptionLocation = _tmp + "\\" + gameName + "\\description.txt";
			StreamReader reader = new StreamReader(descriptionLocation);
			lastButton.transform.GetChild(1).GetComponent<Text>().text = reader.ReadToEnd();

			//Set Initial Button
			if (!buttonIsSelected) {
				buttonIsSelected = true;
				eventSystem.SetSelectedGameObject(lastButton);
			}
		}
	}

	public void OnButtonClick(Button b) {
		_url = _path + "\\" + b.gameObject.name;
		b.interactable = false;
		System.Diagnostics.Process.Start(_url + "\\application.lnk");
		StartCoroutine(WaitTime(3, b));
	}

	IEnumerator WaitTime(float seconds, Button b) {
		yield return new WaitForSeconds(seconds);
		b.interactable = true;
	}

	private void RefreshScene() {
		SceneManager.LoadScene(0);
		loadButtons();
	}

	public int GetNumberOfGames() {
		return numberOfGames;
    }

	void onEnable() {
		numberOfGames = 0;
	}
}
