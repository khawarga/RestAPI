using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System;
using UnityEditor;
using UnityEngine.Networking;

public class putMethod : MonoBehaviour
{
    InputField outputArea;

    void Start()
    {
        outputArea = GameObject.Find("Output").GetComponent<InputField>();
        GameObject.Find("Put").GetComponent<Button>().onClick.AddListener(PutData);
		GameObject.Find("Download").GetComponent<Button>().onClick.AddListener(downloadData);
		GameObject.Find("Delete").GetComponent<Button>().onClick.AddListener(deleteData);
		GameObject.Find("Download2").GetComponent<Button>().onClick.AddListener(downloadData2);
	}

    void PutData() => Put();
	void downloadData() => DownloadMusic();
	void downloadData2() => DownloadImage();
	void deleteData() => Delete();


    private void LogMessage(string title, string message)
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }

    public void Put()
    {
		RequestHelper currentRequest = new RequestHelper
		{
			Uri = "https://jsonplaceholder.typicode.com/posts/1",
			Body = new Fruit
			{
				genus = "testing",
				family = "famili",
				order = "order",
				name = "jeruk",
				id = 90
			},
			Retries = 5,
			RetrySecondsDelay = 1,
			RetryCallback = (err, retries) => {
				Debug.Log(string.Format("Retry #{0} Status {1}\nError: {2}", retries, err.StatusCode, err));
			}
		};

		RestClient.Put<Fruit>(currentRequest, (err, res, body) => {
			if (err != null)
			{
				this.LogMessage("Error", err.Message);
			}
			else
			{
				string ea = JsonUtility.ToJson(body, true);
				
				Fruit buah = JsonUtility.FromJson<Fruit>(ea);

				Image img = GameObject.Find("Image").GetComponent<Image>();
				img.sprite = null;

				outputArea.text = "";

				outputArea.text = outputArea.text + "Name : " + buah.name + "\nGenus : " + buah.genus + "\nFamily : " + buah.family + "\nOrder : " + buah.order + "\n\n";
			}
		});
	}

	public void DownloadImage()
	{

		//var fileUrl = "https://cdn.pixabay.com/download/audio/2023/02/28/audio_550d815fa5.mp3?filename=waterfall-140894.mp3&g-recaptcha-response=03AFY_a8Vmdwz0bXzl_sAb0IQ74vS3E2eCB0NhymU-drPAtNlMmSptzsnO7eN-6aQrCbEZ6JbI-JDiu-r-8gdTcHRkwiBhhYg_BfE27pIJbwOTsS5NQ2HAQ91CPk81TcMROkceLK2gHvD-72NY2E3RX07QhWNV4H0yND8q3s5I9-0_4nuGO6yg9dYnl3LmFoKDAZQOzSGq06nYeMdZcJLOf4kNaB2IAuGSuVR_NgQJR6US3Mw55zOtIzIoJFRE1OXCbue7Dwk1Vm81BqqtnWfcP9I51PVoF0JmZaDSacSPgbH4ykmYQDuku5sNmxHS7sQRaSvruLlwdr596MmATBgAH5uzfm-D0d-yTxOu6e9hABjFJUlG2Si5DohtfVs8X_S7b4qwEku0Je6cnPBzBXADaSDPzddxSf7XuvHYRyK4fmGZRCAUZs1IKM55y0DejHEj2xaQKZolapCnfd9929c8mlXrzB-3591DnlyTpDpr7zLmMPdxtbQXwxo&remote_template=1";
		var fileUrlImg = "https://static.wikia.nocookie.net/cartoons/images/e/ed/Profile_-_SpongeBob_SquarePants.png/revision/latest?cb=20230305115632";
		//var fileType = AudioType.MPEG;

		RestClient.Get(new RequestHelper
		{
			Uri = fileUrlImg,
			Method = "GET",
			DefaultContentType = false,
			DownloadHandler = new DownloadHandlerTexture(true)
		}).Then(res => {
			outputArea.text = "";
			Image img = GameObject.Find("Image").GetComponent<Image>();
			Texture2D texture = ((DownloadHandlerTexture)res.Request.downloadHandler).texture;
			img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}).Catch(err => {
			this.LogMessage("Error", err.Message);
		});
	}

	public void DownloadMusic()
	{

		var fileUrl = "https://cdn.pixabay.com/download/audio/2023/02/28/audio_550d815fa5.mp3?filename=waterfall-140894.mp3&g-recaptcha-response=03AFY_a8Vmdwz0bXzl_sAb0IQ74vS3E2eCB0NhymU-drPAtNlMmSptzsnO7eN-6aQrCbEZ6JbI-JDiu-r-8gdTcHRkwiBhhYg_BfE27pIJbwOTsS5NQ2HAQ91CPk81TcMROkceLK2gHvD-72NY2E3RX07QhWNV4H0yND8q3s5I9-0_4nuGO6yg9dYnl3LmFoKDAZQOzSGq06nYeMdZcJLOf4kNaB2IAuGSuVR_NgQJR6US3Mw55zOtIzIoJFRE1OXCbue7Dwk1Vm81BqqtnWfcP9I51PVoF0JmZaDSacSPgbH4ykmYQDuku5sNmxHS7sQRaSvruLlwdr596MmATBgAH5uzfm-D0d-yTxOu6e9hABjFJUlG2Si5DohtfVs8X_S7b4qwEku0Je6cnPBzBXADaSDPzddxSf7XuvHYRyK4fmGZRCAUZs1IKM55y0DejHEj2xaQKZolapCnfd9929c8mlXrzB-3591DnlyTpDpr7zLmMPdxtbQXwxo&remote_template=1";
		var fileType = AudioType.MPEG;

		RestClient.Get(new RequestHelper
		{
			Uri = fileUrl,
			DownloadHandler = new DownloadHandlerAudioClip(fileUrl,fileType)
		}).Then(res => {
			Image img = GameObject.Find("Image").GetComponent<Image>();
			img.sprite = null;
			AudioSource audio = GetComponent<AudioSource>();
			audio.clip = ((DownloadHandlerAudioClip)res.Request.downloadHandler).audioClip;
			audio.Play();
			outputArea.text = "Music Playing";
		}).Catch(err => {
			this.LogMessage("Error", err.Message);
		});
	}

	public void Delete()
	{

		RestClient.Delete("https://jsonplaceholder.typicode.com/posts/1", (err, res) => {
			if (err != null)
			{
				this.LogMessage("Error", err.Message);
			}
			else
			{
				Image img = GameObject.Find("Image").GetComponent<Image>();
				img.sprite = null;
				outputArea.text = "Data has been deleted";
			}
		});
	}

	[Serializable]
	public class Fruit
	{
		public string genus;
		public string name;
		public int id;
		public string family;
		public string order;
		//public Nutritions nutritions;
	}
	[Serializable]
	public class Nutritions
	{
		public float carbohydrates;
		public float protein;
		public float fat;
		public float calories;
		public float sugar;
	}
}
