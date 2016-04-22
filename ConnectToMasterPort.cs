using System;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class ConnectToMasterPort
{
    public static string getServerPorts
    {
        get
        {
            return returnServerPorts(true);
        }
    }

    /*Estimated 2 packets per second per player*/
    private static Func<bool, string> returnServerPorts = (bool onlyPort) => {
        System.IO.File.WriteAllLines("PortSwap.plat", new string[] {
            Regex.Match(PhotonNetwork.networkingPeer.ServerAddress, @"\:(\w{4})").Groups[0].Value.Substring(1), 
        onlyPort ? Convert.ToString(PhotonNetwork.room.playerCount) : "\0" });
        return "Complete";
    };

    /*Returns a list of all the players properties*/
    private static Func<PhotonPlayer, bool, string[]> returnProps = (PhotonPlayer player, bool keysOnly) => {
        return keysOnly ? player.allProperties.Select(x => Convert.ToString(x.Key)).ToArray()
        : player.allProperties.Select(x => x.Key + ": " + x.Value + "\n").ToArray();
    };

    #region AdvancedBugging //For properties that need to be dealt with instantly
    /*Check all properties, if does not match, return the Keys of them (And value if needed)*/
    private static Func<PhotonPlayer, string> returnUnknownProps = (PhotonPlayer player) => {
        string[] props = returnProps.Invoke(PhotonNetwork.player, true);
        string[] eprops = returnProps.Invoke(player, true);
        string[] result = eprops.Select(d => !props.Contains(d) ? d + "\n" : "").ToArray();
        return string.Concat(result);
    };
    #endregion
}


