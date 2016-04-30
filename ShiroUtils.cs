#region checkingPropertiesFunc //Check for similar prop values
    public Func<bool, string> propValMatchDead = (string matcher) =>
    {
        bool properties =  Array.TrueForAll(PhotonNetwork.playerList.Select(x =>
        (bool)x.customProperties[PhotonPlayerProperty.dead] && (int)x.customProperties[PhotonPlayerProperty.isTitan] == 1 
        ? true : false).ToArray(), delegate (bool t) { return t; });
    };
    #endregion checkingPropertiesFunc //End

/*The options in the configuration.txt*/
    public static string Chatname = string.Empty;

    /*Returns a list of all the players properties*/
    #region returnPlayerProperties //Return their objects
    private static Func<PhotonPlayer, bool, string[]> returnProps = (PhotonPlayer player, bool keysOnly) => {
        return keysOnly ? player.allProperties.Select(x => Convert.ToString(x.Key)).ToArray()
        : player.allProperties.Select(x => x.Key + ": " + x.Value + "\n").ToArray();
    };
    #endregion returnPlayerProperties //End return objects

    /*Check all properties, if does not match, return the Keys of them (And value if needed)*/
    #region returnUnknownProps //Return unknown objects
    public static Func<PhotonPlayer, string> returnUnknownProps = (PhotonPlayer player) => {
        string[] props = returnProps.Invoke(PhotonNetwork.player, true);
        string[] eprops = returnProps.Invoke(player, true);
        string[] result = eprops.Select(d => !props.Contains(d) ? d + "\n" : "").ToArray();
        return string.Concat(result);
    };
    #endregion returnUnknownProps //End return unknown objects

    /*Name Config.txt*/
    #region returnConfigurationLines //Return name objects
    public static Func<string, string> optionsFill = (string line) => {
        string word = File.ReadAllLines("Configuration").Single(d => d.ToLower().Contains(line.ToLower()));
        word = Regex.Replace(word, @"\w+:", "\0");
        Chatname = line.ToLower() == "chatname" ? word : Chatname;
        return word;
    };
    #endregion returnConfigurationLines //End return name objects

    #region colorShiro //Color a string from pink to white
    public static Func<string, string> colorizeShiro = (string inputString) =>
    {
        List<Color> colorsList = new List<Color>();
        char[] letters = inputString.ToCharArray();
        inputString = string.Empty;
        float[] colors = new float[] { 0f}; //Hot pink
        for (int i = 0; i < letters.Length; i++)
        {
            colors[0] += 255 / letters.Length;
            Mathf.Clamp(colors[0], 0f, 255f);
            colorsList.Add(new Color(255, colors[0], 255, 0f));
            string colors2 = "FF" + Convert.ToInt32(colorsList[i].g).ToString ("X2")+"FF";
            inputString += "<color=#" + colors2 + ">" + letters[i] + "</color>";
        }
        return inputString;
    };
    #endregion colorShiro //End

    #region CheckOrRepairColorCodes //Repair the inputstring argument
    public static Func<string, string> checkForBadColors = (string inputstring) =>
    {
        int[] amt = new int[] { Regex.Matches(inputstring, @"\<color\=\#\w{6}\>").Count, 
            Regex.Matches(inputstring, @"\<\/color\>").Count };
        if (amt[0] > amt[1])
        {
            for (int i = amt[1]; i < amt[0]; i++)
            {
                inputstring += "</color>";
            }
        }
        else if (amt[0] < amt[1])
        {
            for (int i = amt[0]; i < amt[1]; i++)
            {
                inputstring = "<color=#ffffff>" + inputstring;
            }
        }
        return inputstring;
    };
    #endregion CheckOrRepairColorCodes //End
