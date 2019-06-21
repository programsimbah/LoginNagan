using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
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

    //go
    [Header("Panel Game Object")]
    public GameObject panelMenu;
    public GameObject panelCode;

    [Header("Penting")]
    public CodeText code;

    private void Start()
    {
        panelMenu.SetActive(true);
        panelCode.SetActive(false);

        pesan = GameObject.FindWithTag("pesan").GetComponent<Text>();
        pesan.gameObject.SetActive(false);
        paraUser = loadSaveLocal();

        Debug.Log(""+DateTime.Now.Date+"\n"
        +DateTime.Now.DayOfWeek+"\n"+DateTime.Now.Hour+"\n"+DateTime.Now.Minute+"\n"+DateTime.Now.Second+"\n"+DateTime.Now.Millisecond);
    }


    public void InputData()
    {
        if(nama.field.text == null)
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
            ModelData model = new ModelData();
            model.id = KomputerCode + paraUser.Count;
            model.nama = nama.field.text;
            model.email = email.field.text;
            model.instansi = instansi.field.text;
            model.noHp = noHp.field.text;
            code.field.text = code.tamabhanKaliamat += model.id;

            panelMenu.SetActive(false);
            panelCode.SetActive(true);

            paraUser.Add(model);
            localSave(paraUser);
            paraUser = loadSaveLocal();
        }
    }
    public void ShowMenu()
    {
        panelMenu.SetActive(true);
        panelCode.SetActive(false);
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
        var s = GameObject.Find("Debug");
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
