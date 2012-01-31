using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System.Xml.Serialization;

namespace ERMotA
{
    [Serializable]
    public struct SaveGameData
    {
        public Int16 Strength, Dexterity, Speed, Health, Level;
        public Int32 XP;
        public Vector2 Pos;
    }

    class Storage
    {
        StorageDevice Device;
        public ArrayList PlayersLoaded;
        IAsyncResult result;

        public Storage()
        {
            PlayersLoaded = new ArrayList();
            result = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            Device = Guide.EndShowStorageDeviceSelector(result);

            // Open a storage container.
            StorageContainer container = Device.OpenContainer("Savegames");

            // Add the container path to our file name.
            string filename = Path.Combine(container.Path, "savegame.sav");

            // Create a new file.
            if (!File.Exists(filename))
            {
                FileStream file = File.Create(filename);
                file.Close();
            }
            // Dispose the container, to commit the data.
            container.Dispose();
        }
        public void DoLoadGame()
        {
            // Open a storage container.
            StorageContainer container = Device.OpenContainer("Savegames");

            // Get the path of the save game.
            string filename = Path.Combine(container.Path, "savegame.sav");

            // Open the file, creating it if necessary.
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer reader = new XmlSerializer(typeof(SaveGameData));
            while (stream.Position < stream.Length)
            {
                try
                {
                    SaveGameData data = (SaveGameData)reader.Deserialize(stream);
                    LivingEntity.PlayerEntity temp = new LivingEntity.PlayerEntity();
                    temp.Health = data.Health;
                    temp.Level = data.Level;
                    temp.XP = data.XP;
                    temp.Pos = data.Pos;
                    temp.Strength = data.Strength;
                    temp.Speed = data.Speed;
                    temp.Dexterity = data.Dexterity;
                    PlayersLoaded.Add(temp);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }
        public void DoSaveGame(LivingEntity Entity)
        {
            // Create the data to save.
            SaveGameData data = new SaveGameData();
            data.Level = Entity.Level;
            data.XP = Entity.XP;
            data.Pos = Entity.Pos;
            data.Strength = Entity.Strength;
            data.Speed = Entity.Speed;
            data.Dexterity = Entity.Dexterity;
            data.Health = Entity.Health;

            // Open a storage container.
            StorageContainer container = Device.OpenContainer("Savegames");

            // Get the path of the save game.
            string filename = Path.Combine(container.Path, "savegame.sav");

            // Open the file, creating it if necessary.
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);

            // Convert the object to XML data and put it in the stream.
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);

            // Close the file.
            stream.Close();

            // Dispose the container, to commit changes.
            container.Dispose();
        }
    }
}