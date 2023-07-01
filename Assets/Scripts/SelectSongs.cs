using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.IO.Pipes;
using System.Linq;
using UnityEngine.Networking;

public class SelectSongs : MonoBehaviour
{
    public FileStream filestream;

    public AudioSource source;
    public AudioClip clip;
    string[] files = new string[4];
    List<string> files_name = new List<string>();
    List<AudioClip> clips;

    TMPro.TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        TMPro.TMP_Dropdown dropdown = gameObject.GetComponent<TMP_Dropdown>();


        string path = "C:\\Users\\aprab\\AppData\\LocalLow\\DefaultCompany\\Avana Guitar";

        if (Directory.Exists(path))
        {
            files = Directory.GetFiles(path, @"*.mp3");
            Debug.Log(files);
        }
        List<string> files_list = files.ToList();


        for (int i =0; i< files_list.Count; i++)
        {
            files_name.Add("Song " + i);
        }
        dropdown.options.Clear();
        dropdown.AddOptions(files_name);
    }

    // Update is called once per frame
    void Update()
    {
        if(dropdown != null)
        {
            var current_file = files[dropdown.value];
            clip = (AudioClip)Resources.Load(current_file);
            source.clip = clip;
        }

    }
}
