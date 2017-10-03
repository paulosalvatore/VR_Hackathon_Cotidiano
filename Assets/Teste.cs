/*
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{
	[SerializeField]
	private AudioClip m_AudioClip = new AudioClip();
	private SpeechToText m_SpeechToText = new SpeechToText();

	private string microphoneName = "Built-in Microphone";

	private void Start()
	{
		//m_SpeechToText.Recognize(m_AudioClip, HandleOnRecognize);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			Tocar();
			//StartCoroutine(Tocar());
		}
	}

	private void Tocar()
	{
		//  Streaming
		m_SpeechToText.StartListening(OnRecognize);
		//  Stop listening
		m_SpeechToText.StopListening();
	}

	//  Callback for the listen functions.
	private void OnRecognize(SpeechRecognitionEvent result)
	{
		if (result != null && result.results.Length > 0)
		{
			foreach (var res in result.results)
			{
				foreach (var alt in res.alternatives)
				{
					string text = alt.transcript;
					Debug.Log(string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));
				}
			}
		}
	}

	public static void ExecutarAcao(string acao)
	{
		if (acao.Contains("fecha"))
		{
			Debug.Log("Executar Ação de Fechar");
		}
		else if (acao.Contains("abre") ||
			acao.Contains("abri"))
		{
			Debug.Log("Executar Ação de Abrir");
		}
		else if (acao.Contains("leitura"))
		{
			Debug.Log("Executar Ação de Leitura");
		}
		else if (acao.Contains("prender"))
		{
			Debug.Log("Executar Ação de Prender");
		}
	}

	/*
	private IEnumerator Tocar()
	{
		var aud = GetComponent<AudioSource>();

		AudioClip clip = Microphone.Start(microphoneName, true, 10, 44100);
		aud.clip = clip;
		aud.loop = true;

		while (!(Microphone.GetPosition(null) > 0))
		{
			yield return null;
		}

		while (Microphone.IsRecording(microphoneName))
		{
			yield return null;
		}

		aud.Play();

		m_SpeechToText.Recognize(clip, HandleOnRecognize);
	}

	private void HandleOnRecognize(SpeechRecognitionEvent result)
	{
		Debug.Log("HandleOnRecognize");
		if (result != null && result.results.Length > 0)
		{
			foreach (var res in result.results)
			{
				foreach (var alt in res.alternatives)
				{
					string text = alt.transcript;
					Debug.Log(string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));
				}
			}
		}
	}
	*/

	/*
	private TextToSpeech m_TextToSpeech = new TextToSpeech();
	private string m_TestString = "O rato roeu a roupa do rei de roma.";
	private string m_ExpressiveText = "<speak version=\"1.0\"><prosody pitch=\"150Hz\">Hello! This is the </prosody><say-as interpret-as=\"letters\">IBM</say-as> Watson <express-as type=\"GoodNews\">Unity</express-as></prosody><say-as interpret-as=\"letters\">SDK</say-as>!</speak>";
	private string m_ExpressiveText2 = "<speak version=\"1.0\"><prosody pitch=\"150Hz\">Olá! Esse é o </prosody><say-as interpret-as=\"letters\">IBM</say-as> Watson <express-as type=\"GoodNews\">Unity</express-as></prosody><say-as interpret-as=\"letters\">SDK</say-as>!</speak>";

	private void Start()
	{
		//m_TextToSpeech.Voice = VoiceType.en_US_Allison;
		//m_TextToSpeech.ToSpeech(m_ExpressiveText, HandleToSpeechCallback);
	}

	private void Tocar()
	{
		m_TextToSpeech.Voice = VoiceType.pt_BR_Isabela;
		m_TextToSpeech.ToSpeech(m_ExpressiveText2, HandleToSpeechCallback);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
			Tocar();
	}

	private void HandleToSpeechCallback(AudioClip clip)
	{
		PlayClip(clip);
	}

	private void PlayClip(AudioClip clip)
	{
		if (Application.isPlaying && clip != null)
		{
			GameObject audioObject = new GameObject("AudioObject");
			AudioSource source = audioObject.AddComponent<AudioSource>();
			source.spatialBlend = 0.0f;
			source.loop = false;
			source.clip = clip;
			source.Play();

			GameObject.Destroy(audioObject, clip.length);
		}
	}
	*/
//}
