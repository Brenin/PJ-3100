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
	private bool _gameIsRunning = false;

	void Awake() {
		startLED();
		Cursor.visible = false;
		Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width,
				Screen.resolutions[Screen.resolutions.Length - 1].height + 150, false);

		driveInserted = Directory.Exists(usbPath);
		InvokeRepeating("LookForUSBDrive", 0f, 5f);
		current = this;

		_path = Directory.GetCurrentDirectory() + "\\" + rootFolderName;

		startLEDIdle();
	}

	public void reload() {
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
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
		startLEDGame();
		setGameIsRunning(true);
		System.Diagnostics.Process.Start(_url + "\\application.lnk");
		StartCoroutine(WaitTime(3, b));
	}

	public void startLED() {
		string cmd = "/C C:\\users\\arcade\\dropbox\\LEDBlinky\\ledblinky\\LEDBlinky.exe 1";

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = cmd;
		process.StartInfo = startInfo;
		process.Start();
	}

	public void startLEDIdle() {
		string cmd = "/C C:\\users\\arcade\\dropbox\\LEDBlinky\\ledblinky\\LEDBlinky.exe Opening01.lwax n";

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = cmd;
		process.StartInfo = startInfo;
		process.Start();
	}

	public void startLEDGame() {
		string cmd = "/C C:\\users\\arcade\\dropbox\\LEDBlinky\\ledblinky\\LEDBlinky.exe playanim01.lwax n";

		System.Diagnostics.Process process = new System.Diagnostics.Process();
		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		startInfo.FileName = "cmd.exe";
		startInfo.Arguments = cmd;
		process.StartInfo = startInfo;
		process.Start();
	}

	public void setGameIsRunning(bool startedGame) {
		this._gameIsRunning = startedGame;
	}

	public bool getGameIsRunning() {
		return _gameIsRunning;
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
