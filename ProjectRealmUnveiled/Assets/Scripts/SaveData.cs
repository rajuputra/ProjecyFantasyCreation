using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SaveData
{
    public static SaveData Instance;

    //map stuff
    public HashSet<string> sceneNames;

    //bench stuff
    public string saveStuffSceneName;
    public Vector2 saveStuffPos;


    //Player stuff
    public int playerHealth;
    public int playerHeartShards;
    public float playerMana;
    public bool playerHalfMana;
    public Vector2 playerPosition;
    public string lastScene;
    public int playerMaxHealth;

    public bool playerUnlockedWallJump;
    public bool playerUnlockedDash;
    public bool playerUnlockedVarJump;

    //enemy stuff
    //shade
    public Vector2 shadePos;
    public string sceneWithShade;
    public Quaternion shadeRot;

    public void Initialize()
    {
        if(!File.Exists(Application.persistentDataPath + "/save.savestuff.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.savestuff.data"));
        }

        if (!File.Exists(Application.persistentDataPath + "/save.player.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.player.data"));
        }

        if (!File.Exists(Application.persistentDataPath + "/save.shade.data"))
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Application.persistentDataPath + "/save.shade.data"));
        }

        if (sceneNames == null)
        {
            sceneNames = new HashSet<string>();
        }
    }

    public void SaveSaveStuff()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.savestuff.data")))
        {
            writer.Write(saveStuffSceneName);
            writer.Write(saveStuffPos.x);
            writer.Write(saveStuffPos.y);
        }
    }

    public void LoadSaveStuff()
    {
        if(File.Exists(Application.persistentDataPath + "/save.savestuff.data"))
        {
            using(BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.savestuff.data")))
            {
                saveStuffSceneName = reader.ReadString();
                saveStuffPos.x = reader.ReadSingle();
                saveStuffPos.y = reader.ReadSingle();
            }
        }
    }

    public void SavePlayerData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.player.data")))
        {
            playerHealth = Player.Instance.Health;
            writer.Write(playerHealth);

            playerHeartShards = Player.Instance.heartShards;
            writer.Write(playerHeartShards);

            playerMaxHealth = Player.Instance.maxHealth;
            writer.Write(playerMaxHealth);

            playerMana = Player.Instance.Mana;
            writer.Write(playerMana);

            playerHalfMana = Player.Instance.halfMana;
            writer.Write(playerHalfMana);

            playerUnlockedWallJump = Player.Instance.unlockedWallJump;
            writer.Write(playerUnlockedWallJump);
            
            playerUnlockedDash = Player.Instance.unlockedDash;
            writer.Write(playerUnlockedDash);

            playerUnlockedVarJump = Player.Instance.unlockedVarJump;
            writer.Write(playerUnlockedVarJump);

            playerPosition = Player.Instance.transform.position;
            writer.Write(playerPosition.x);
            writer.Write(playerPosition.y);

            lastScene = SceneManager.GetActiveScene().name;
            writer.Write(lastScene);
        }
    }

    
    public void LoadPlayerData()
    {
        if(File.Exists(Application.persistentDataPath + "/save.player.data"))
        {
            using(BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.player.data")))
            {
                playerHealth = reader.ReadInt32();
                playerHeartShards = reader.ReadInt32();
                playerMaxHealth = reader.ReadInt32();
                playerMana = reader.ReadSingle();
                playerHalfMana = reader.ReadBoolean();

                playerUnlockedWallJump = reader.ReadBoolean();
                playerUnlockedVarJump= reader.ReadBoolean();
                playerUnlockedDash= reader.ReadBoolean();

                playerPosition.x = reader.ReadSingle();
                playerPosition.y = reader.ReadSingle();
                lastScene = reader.ReadString();

                SceneManager.LoadScene(lastScene);
                Player.Instance.transform.position = playerPosition;
                Player.Instance.halfMana = playerHalfMana;
                Player.Instance.Health = playerHealth;
                Player.Instance.heartShards = playerHeartShards;
                Player.Instance.maxHealth = playerMaxHealth;
                
                Player.Instance.Mana = playerMana;

                Player.Instance.unlockedWallJump = playerUnlockedWallJump;
                Player.Instance.unlockedDash = playerUnlockedDash;
                Player.Instance.unlockedVarJump = playerUnlockedVarJump;
            }
        }
        else
        {
            Debug.Log("File doesn't exist");
            Player.Instance.halfMana = false;
            Player.Instance.Health = Player.Instance.maxHealth;
            Player.Instance.maxHealth = 5;
            Player.Instance.Mana = 0.5f;

            Player.Instance.heartShards = 0;
            Player.Instance.unlockedWallJump = false;
            Player.Instance.unlockedDash = false;
        }
    }

    public void SaveShadeData()
    {
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Application.persistentDataPath + "/save.shade.data")))
        {
            sceneWithShade = SceneManager.GetActiveScene().name;
            shadePos = Shade.Instance.transform.position;
            shadeRot = Shade.Instance.transform.rotation;

            writer.Write(sceneWithShade);

            writer.Write(shadePos.x);
            writer.Write(shadePos.y);

            writer.Write(shadeRot.x);
            writer.Write(shadeRot.y);
            writer.Write(shadeRot.z);
            writer.Write(shadeRot.w);
        }
    }

    public void LoadShadeData()
    {
        if (File.Exists(Application.persistentDataPath + "/save.shade.data"))
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(Application.persistentDataPath + "/save.shade.data")))
            {
                sceneWithShade = reader.ReadString();
                shadePos.x = reader.ReadSingle();
                shadePos.y = reader.ReadSingle();

                float rotationX = reader.ReadSingle();
                float rotationY = reader.ReadSingle();
                float rotationZ = reader.ReadSingle();
                float rotationW = reader.ReadSingle();

                shadeRot = new Quaternion(rotationX, rotationY, rotationZ, rotationW);

            }
        }
        else
        {
            Debug.Log("Debug doesn't exist");
        }
    }

}
