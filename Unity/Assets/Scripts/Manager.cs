using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.EventSystems;
using System.Text;
using System.IO;



public class Manager : MonoBehaviour
{
    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    public const string Slax = "!";
    public const string Slax2 = "*";

    public static string keyUserPara = "mdaosjdjdadjfrjwndvj";


    [System.Serializable]
    public class UserData
    {
        public InputField field;
        public string pesanKosong;
    }

    [System.Serializable]
    public class CodeText
    {
        public Text field;
        public string tamabhanKaliamat;
    }

    [Header("Komputer")]
    [SerializeField]
    public string KomputerCode = "A";
    [Header("Input Data")]
    [SerializeField]
    public UserData nama;
    public UserData email;
    public UserData instansi;
    public UserData noHp;

    //Variane
    Text pesan;
    List<ModelData> paraUser ;
    GameObject s;
    EventSystem system;

    //go
    [Header("Panel Game Object")]
    public GameObject panelMenu;
    public GameObject panelCode;

    [Header("Penting")]
    public CodeText code;

    private void Start()
    {
        s = GameObject.Find("Debug");
        panelMenu.SetActive(true);
        panelCode.SetActive(false);

      //  system = EventSystemManager

        pesan = GameObject.FindWithTag("pesan").GetComponent<Text>();
        pesan.gameObject.SetActive(false);
        paraUser = loadSaveLocal();

        Debug.Log(""+DateTime.Now.Date+"\n"
        +DateTime.Now.DayOfWeek+"\n"+DateTime.Now.Hour+"\n"+DateTime.Now.Minute+"\n"+DateTime.Now.Second+"\n"+DateTime.Now.Millisecond);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))
        {
            //Selectable next = system.currentSelectedGameObject.GetComponent<InputField>();
            //if (next != null)
            //{
            //    InputField inputField = next.GetComponent<InputField>();
            //    if(inputField != null)
            //    {
            //        inputField.OnPointerClick(new PointerEventData(system));
            //        system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            //    }
            //}

            if (nama.field.isFocused)
            {
                if (nama.field.text == "")
                {
                    warning("anda belum mengisi nama");
                    return;
                }
                else
                {
                    email.field.Select();
                }
            }
            else if (email.field.isFocused)
            {
                if (!validEmail(email.field.text))
                {
                    warning(email.pesanKosong);
                    return;
                }
                else
                {
                    instansi.field.Select();
                }
            }
            else if (instansi.field.isFocused)
            {
                if (instansi.field.text == "")
                {
                    warning("anda belum mengisi instansi");
                    return;
                }
                else
                {
                    noHp.field.Select();
                }
            }
            else
            {
                if (nama.field.text == "")
                {
                    nama.field.Select();
                }
                else if (noHp.field.text == "")
                {
                    warning("anda belum mengisi hp");
                    return;
                }
                else
                {
                    InputData();
                }
            }
        }
    }


    public void InputData()
    {
        if(nama.field.text == "")
        {
            warning(nama.pesanKosong);
            return;
        }else if (!validEmail(email.field.text)){
            warning(email.pesanKosong);
            return;
        }else if (instansi.field.text == ""){
            warning(instansi.pesanKosong);
            return;
        }else if (noHp.field.text == ""){
            warning(noHp.pesanKosong);
            return;
        }
        else
        {
            ModelData _model = new ModelData();
            _model.id = KomputerCode + paraUser.Count;
            _model.nama = nama.field.text;
            _model.email = email.field.text;
            _model.instansi = instansi.field.text;
            _model.noHp = noHp.field.text;

            var f = code.tamabhanKaliamat;
            code.field.text = f += _model.id;

            panelMenu.SetActive(false);
            panelCode.SetActive(true);

            paraUser.Add(_model);
            localSave(paraUser);
            paraUser = loadSaveLocal();
        }
    }
    public void ShowMenu()
    {
        panelMenu.SetActive(true);
        panelCode.SetActive(false);

        nama.field.text = "";
        email.field.text = "";
        instansi.field.text = "";
        noHp.field.text = "";
        code.field.text = "";


    }

    void localSave(List<ModelData> datas)
    {
        string temp = "";
        for (var i = 0; i < datas.Count; i++)
        {

            if (i != datas.Count - 1)
                temp += datas[i].id
                    + Slax + datas[i].nama
                    + Slax + datas[i].email
                    + Slax + datas[i].instansi
                    + Slax + datas[i].noHp + Slax2;

            else
                temp += datas[i].id
                    + Slax + datas[i].nama
                    + Slax + datas[i].email
                    + Slax + datas[i].instansi
                    + Slax + datas[i].noHp;
        }

        PlayerPrefs.SetString(keyUserPara, temp);
    }
    List<ModelData> loadSaveLocal()
    {
        string temp = "";
        string[] tempArray;
        List<ModelData> modelDatas = new List<ModelData>();
        temp = PlayerPrefs.GetString(keyUserPara);
        if (temp == null || temp == "") return modelDatas;
        tempArray = temp.Split(Slax2.ToCharArray());
        for (int i = 0; i < tempArray.Length; i++)
        {
            string[] temp2Array;
            temp2Array = tempArray[i].Split(Slax.ToCharArray());

            ModelData modelData = new ModelData();
            modelData.id = temp2Array[0];
            modelData.nama = temp2Array[1];
            modelData.email = temp2Array[2];
            modelData.instansi = temp2Array[3];
            modelData.noHp = temp2Array[4];

            modelDatas.Add(modelData);
        }

        return modelDatas;
    }
    void warning(string mes)
    {
        pesan.text = mes;
        StartCoroutine(Hide(3, pesan.gameObject));
    }

    IEnumerator Hide (int time, GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    bool validEmail(string emailnya)
    {
        if (emailnya != "")
            return Regex.IsMatch(emailnya, MatchEmailPattern);
        else
            return false;
    }

    public void DownloadData(string name)
    {
        List<string[]> rowData= new List<string[]>();

        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "ID";
        rowDataTemp[1] = "Nama";
        rowDataTemp[2] = "Email";
        rowDataTemp[3] = "Instansi";
        rowDataTemp[4] = "NoHp";
        rowData.Add(rowDataTemp);

        for (int i = 0; i <paraUser.Count; i++)
        {
           
            rowDataTemp = new string[5];
            rowDataTemp[0] = paraUser[i].id;
            rowDataTemp[1] = paraUser[i].nama;
            rowDataTemp[2] = paraUser[i].email;
            rowDataTemp[3] = paraUser[i].instansi;
            rowDataTemp[4] = paraUser[i].noHp;
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

       // string filePath = Application.dataPath + "/" +name+DateTime.Now.DayOfWeek+ ".csv";
        string filePath = Application.persistentDataPath + "/" +name+DateTime.Now.DayOfWeek+ ".csv";
        Debug.Log(filePath);

        s.GetComponent<Text>().text = filePath;
        StartCoroutine(Hide(3, s));

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        
    }
    public void ResetPerfs()
    {
        DownloadData(DateTime.Now.DayOfWeek+"_on_"+DateTime.Now.Hour+":"+DateTime.Now.Minute+":"+DateTime.Now.Second+":"+DateTime.Now.Millisecond+"_backup_daftar_hadir_");
        PlayerPrefs.DeleteAll();
        paraUser = loadSaveLocal();
    }
}
