using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI scoreLimitText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI redTeamTitle;
    public TextMeshProUGUI blueTeamTitle;
    public TextMeshProUGUI greenTeamTitle;
    public TextMeshProUGUI goldTeamTitle;
    public TextMeshProUGUI redTeamTotalPoints;
    public TextMeshProUGUI blueTeamTotalPoints;
    public TextMeshProUGUI greenTeamTotalPoints;
    public TextMeshProUGUI goldTeamTotalPoints;

    public TextMeshProUGUI redTeamNamesText;
    public TextMeshProUGUI redTeamKillsText;
    public TextMeshProUGUI redTeamDeathsText;
    public TextMeshProUGUI redTeamPointsText;
    public TextMeshProUGUI blueTeamNamesText;
    public TextMeshProUGUI blueTeamKillsText;
    public TextMeshProUGUI blueTeamDeathsText;
    public TextMeshProUGUI blueTeamPointsText;
    public TextMeshProUGUI greenTeamNamesText;
    public TextMeshProUGUI greenTeamKillsText;
    public TextMeshProUGUI greenTeamDeathsText;
    public TextMeshProUGUI greenTeamPointsText;
    public TextMeshProUGUI goldTeamNamesText;
    public TextMeshProUGUI goldTeamKillsText;
    public TextMeshProUGUI goldTeamDeathsText;
    public TextMeshProUGUI goldTeamPointsText;


    //Non-team-game
    public TextMeshProUGUI[] namesTextArrays = new TextMeshProUGUI[16];
    public TextMeshProUGUI[] fragsTextArrays = new TextMeshProUGUI[16];
    public TextMeshProUGUI[] deathsTextArrays = new TextMeshProUGUI[16];
    public TextMeshProUGUI[] pointsTextArrays = new TextMeshProUGUI[16];

    //Team game
    public TextMeshProUGUI[] namesRedTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] fragsRedTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] deathsRedTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] pointsRedTeamTextArrays = new TextMeshProUGUI[8];
    public Image[] iconsRedTeamArray = new Image[8];
    public TextMeshProUGUI[] namesBlueTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] fragsBlueTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] deathsBlueTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] pointsBlueTeamTextArrays = new TextMeshProUGUI[8];
    public Image[] iconsBlueTeamArray = new Image[8];

    
    public TextMeshProUGUI[] namesGreenTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] fragsGreenTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] deathsGreenTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] pointsGreenTeamTextArrays = new TextMeshProUGUI[8];
    public Image[] iconsGreenTeamArray = new Image[8];

    public TextMeshProUGUI[] namesGoldTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] fragsGoldTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] deathsGoldTeamTextArrays = new TextMeshProUGUI[8];
    public TextMeshProUGUI[] pointsGoldTeamTextArrays = new TextMeshProUGUI[8];
    public Image[] iconsGoldTeamArray = new Image[8];

    public const int MAX_STATS_REGULAR = 16; // There are a maximum of 16 players on a regular scoreboard list.
    public const int MAX_STATS_TEAM = 8; // There are a maximum of 8 players on a team scoreboard list.

    bool adjustedTextXForMultiTeams = false; // If there are more than two teams in a team game, the X position is adjusted to fit all the teams. We only do this once (when the scoreboard is first initiated)

    bool disabledExcessTeamStats = false; // When we first initialize the team scoreboard and there is less than four teams in the maxTeam variable in PlatformGameManager, we turn off the stats for the unused teams. We only need to do this once and to avoid doing this unnecceasarily when updating the entire scoreboard set this variable to true.
   
    public List<PlatformGameManager.PlayerStatistic> positionsList = new List<PlatformGameManager.PlayerStatistic>();
    public List<PlatformGameManager.PlayerStatistic>[] teamPositionsList;
    public Sprite redFlagIcon;
    public Sprite blueFlagIcon;
    public Sprite bombIcon;

    //int totalPlayers;
    //int[] totalTeamPlayers;
    
    PlatformGameManager platformGameManager;

    void OnEnable()
    {
        platformGameManager = GameObject.FindWithTag("PlatformGameManager").GetComponent<PlatformGameManager>();
    }
    void Update()
    {

    }

    // Update is called once per frame
    public void DrawStatistics() // Called when the game is first started, and when a player connects or disconnects (the entire scoreboard is redrawn)
    {
        //totalPlayers = platformGameManager.statsDictionary.Count;

        if(!platformGameManager.teamGame)
        {
            if(platformGameManager.statsDictionary.Count < MAX_STATS_REGULAR) // There is less than the greatest amoun to of players than can be put on the scoreboard (16), disable all extra records.
            {
                for(int i = platformGameManager.statsDictionary.Count; i < MAX_STATS_REGULAR; i++)
                {
                    DisableScoreboardRecord(i);
                }
            }

            int position = 0;

            foreach(PlatformGameManager.PlayerStatistic statistic in platformGameManager.statsDictionary.Values)
            {
                if(position < platformGameManager.statsDictionary.Count && position < MAX_STATS_REGULAR)
                {
                    UpdateWholeScoreboardRecord(position,statistic);
                }

                platformGameManager.positionDictionary.Add(statistic.playerNum,position);
                positionsList.Add(statistic);
                position++;
            }
        }
        else
        {
            DisableAllIcons();
            if(platformGameManager.gameType != PlatformGameManager.GameType.VACCINATION)
            {
                if(platformGameManager.maxTeams == 2)
                {
                    teamPositionsList = new List<PlatformGameManager.PlayerStatistic>[2];
                    //totalTeamPlayers = new int[2];

                    teamPositionsList[0] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[1] = new List<PlatformGameManager.PlayerStatistic>();

                    if(!disabledExcessTeamStats)
                    {
                        DisableTeamStats(PlatformGameManager.TeamColor.GREEN_TEAM);
                        DisableTeamStats(PlatformGameManager.TeamColor.GOLD_TEAM);
                    }

                    if(platformGameManager.gameType != PlatformGameManager.GameType.TEAM_DEATHMATCH)
                        RearrangeColumnsTwoTeamColumnNonTDM();

                    EnableTeamStats(PlatformGameManager.TeamColor.RED_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.BLUE_TEAM);

                    UpdatePointsForTeam(PlatformGameManager.TeamColor.RED_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.BLUE_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    print(teamPositionsList[0].Count);
                    print(platformGameManager.teamsPositionDictionary[0].Count);
                    print(platformGameManager.teamStatsDictionary[0].Count);

                    print(teamPositionsList[1].Count);
                    print(platformGameManager.teamsPositionDictionary[1].Count);
                    print(platformGameManager.teamStatsDictionary[1].Count);
                }
                else if(platformGameManager.maxTeams == 3)
                {
                    teamPositionsList = new List<PlatformGameManager.PlayerStatistic>[3];
                    //totalTeamPlayers = new int[3];

                    teamPositionsList[0] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[1] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[2] = new List<PlatformGameManager.PlayerStatistic>();

                    if(!disabledExcessTeamStats)
                    {
                    DisableTeamStats(PlatformGameManager.TeamColor.GOLD_TEAM);
                    }
                    EnableTeamStats(PlatformGameManager.TeamColor.RED_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.BLUE_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.GREEN_TEAM);

                    UpdatePointsForTeam(PlatformGameManager.TeamColor.RED_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.BLUE_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.GREEN_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                }
                else if(platformGameManager.maxTeams == 4)
                {
                    teamPositionsList = new List<PlatformGameManager.PlayerStatistic>[4];
                    //totalTeamPlayers = new int[4];

                    teamPositionsList[0] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[1] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[2] = new List<PlatformGameManager.PlayerStatistic>();
                    teamPositionsList[3] = new List<PlatformGameManager.PlayerStatistic>();

                    EnableTeamStats(PlatformGameManager.TeamColor.RED_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.BLUE_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.GREEN_TEAM);
                    EnableTeamStats(PlatformGameManager.TeamColor.GOLD_TEAM);

                    UpdatePointsForTeam(PlatformGameManager.TeamColor.RED_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.BLUE_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.GREEN_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                    UpdatePointsForTeam(PlatformGameManager.TeamColor.GOLD_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                }

                disabledExcessTeamStats = true;

                if(platformGameManager.maxTeams > 2 && !adjustedTextXForMultiTeams) // 4 teams won't fit on the scoreboard normally, we must adjust the xPos of the text to make room.
                {

                    AdjustScoreboardTextXPosition();
                    adjustedTextXForMultiTeams = true;
                }
            }
            else
            {
                teamPositionsList = new List<PlatformGameManager.PlayerStatistic>[2];

                teamPositionsList[0] = new List<PlatformGameManager.PlayerStatistic>();
                teamPositionsList[1] = new List<PlatformGameManager.PlayerStatistic>();

                EnableTeamStats(PlatformGameManager.TeamColor.RED_TEAM);
                EnableTeamStats(PlatformGameManager.TeamColor.BLUE_TEAM);

                UpdatePointsForTeam(PlatformGameManager.TeamColor.RED_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
                UpdatePointsForTeam(PlatformGameManager.TeamColor.BLUE_TEAM); // Set frags (tdm) or points (non-tdm) text for the scorebord.
            
                DisableTeamStats(PlatformGameManager.TeamColor.GREEN_TEAM);
                DisableTeamStats(PlatformGameManager.TeamColor.GOLD_TEAM);
            }
        }
    }
    

    void UpdateWholeScoreboardRecord(int num, PlatformGameManager.PlayerStatistic statistic)
    {
        namesTextArrays[num].text = (num+1)+". "+statistic.playerName;
        fragsTextArrays[num].text = statistic.frags.ToString();
        deathsTextArrays[num].text = statistic.deaths.ToString();
    }

    void UpdateWholeScoreboardRecord(int num, PlatformGameManager.PlayerStatistic statistic, PlatformGameManager.TeamColor team)
    {
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                namesRedTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsRedTeamTextArrays[num].text = statistic.frags.ToString();
                deathsRedTeamTextArrays[num].text = statistic.deaths.ToString();
                if(platformGameManager.gameType != PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    pointsRedTeamTextArrays[num].text = statistic.points.ToString();
                break;
            
            case PlatformGameManager.TeamColor.BLUE_TEAM:
                namesBlueTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsBlueTeamTextArrays[num].text = statistic.frags.ToString();
                deathsBlueTeamTextArrays[num].text = statistic.deaths.ToString();
                if(platformGameManager.gameType != PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    pointsBlueTeamTextArrays[num].text = statistic.points.ToString();
                break;
            
            case PlatformGameManager.TeamColor.GREEN_TEAM:
                namesGreenTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsGreenTeamTextArrays[num].text = statistic.frags.ToString();
                deathsGreenTeamTextArrays[num].text = statistic.deaths.ToString();
                if(platformGameManager.gameType != PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    pointsGreenTeamTextArrays[num].text = statistic.points.ToString();
                break;

            case PlatformGameManager.TeamColor.GOLD_TEAM:
                namesGoldTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsGoldTeamTextArrays[num].text = statistic.frags.ToString();
                deathsGoldTeamTextArrays[num].text = statistic.deaths.ToString();
                if(platformGameManager.gameType != PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    pointsGoldTeamTextArrays[num].text = statistic.points.ToString();
                break;
            
            case PlatformGameManager.TeamColor.VACCINATED_TEAM:
                namesRedTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsRedTeamTextArrays[num].text = statistic.frags.ToString();
                deathsRedTeamTextArrays[num].text = statistic.deaths.ToString();
                pointsRedTeamTextArrays[num].text = statistic.points.ToString();
                break;
            
            case PlatformGameManager.TeamColor.UNVACCINATED_TEAM:
                namesBlueTeamTextArrays[num].text = (num+1)+". "+statistic.playerName;
                fragsBlueTeamTextArrays[num].text = statistic.frags.ToString();
                deathsBlueTeamTextArrays[num].text = statistic.deaths.ToString();
                pointsBlueTeamTextArrays[num].text = statistic.points.ToString();
                break;

        }
    }

    void UpdatePartialScoreboardRecord(int num, PlatformGameManager.PlayerStatistic statistic, bool updateFrags, bool updateDeaths)
    {
        namesTextArrays[num].text = (num+1)+". "+statistic.playerName;

        if(updateFrags)
            fragsTextArrays[num].text = statistic.frags.ToString();
        
        if(updateDeaths)
            deathsTextArrays[num].text = statistic.deaths.ToString();
    }
    void DisableScoreboardRecord(int num)
    {
        namesTextArrays[num].enabled = false;
        fragsTextArrays[num].enabled = false; 
        deathsTextArrays[num].enabled = false;
    }
    void DisableScoreboardRecord(int num, PlatformGameManager.TeamColor team)
    {
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                namesRedTeamTextArrays[num].enabled = false;
                fragsRedTeamTextArrays[num].enabled = false;
                deathsRedTeamTextArrays[num].enabled = false;
                if(pointsRedTeamTextArrays[num].enabled)
                    pointsRedTeamTextArrays[num].enabled = false;
                break;
            
            case PlatformGameManager.TeamColor.BLUE_TEAM:
                namesBlueTeamTextArrays[num].enabled = false;
                fragsBlueTeamTextArrays[num].enabled = false;
                deathsBlueTeamTextArrays[num].enabled = false;
                if(pointsBlueTeamTextArrays[num].enabled)
                    pointsBlueTeamTextArrays[num].enabled = false;
                break;
            
            case PlatformGameManager.TeamColor.GREEN_TEAM:
                namesGreenTeamTextArrays[num].enabled = false;
                fragsGreenTeamTextArrays[num].enabled = false;
                deathsGreenTeamTextArrays[num].enabled = false;
                if(pointsGreenTeamTextArrays[num].enabled)
                    pointsGreenTeamTextArrays[num].enabled = false;
                break;
            
            case PlatformGameManager.TeamColor.GOLD_TEAM:
                namesGoldTeamTextArrays[num].enabled = false;
                fragsGoldTeamTextArrays[num].enabled = false;
                deathsGoldTeamTextArrays[num].enabled = false;
                if(pointsGoldTeamTextArrays[num].enabled)
                    pointsGoldTeamTextArrays[num].enabled = false;
                break;
        }
    }
    void EnableTeamStats(PlatformGameManager.TeamColor team)
    {
        if(platformGameManager.gameType != PlatformGameManager.GameType.VACCINATION)
        {

        if(platformGameManager.teamStatsDictionary[(int)(team)].Count < MAX_STATS_TEAM) // There is less than the greatest amoun to of players than can be put on the scoreboard (16), disable all extra records.
        {
            for(int i = platformGameManager.teamStatsDictionary[(int)(team)].Count; i < MAX_STATS_TEAM; i++)
            {
                DisableScoreboardRecord(i,team);
            }
        }

        int position = 0;

        foreach(PlatformGameManager.PlayerStatistic statistic in platformGameManager.teamStatsDictionary[(int)(team)].Values)
        {

            if(position < platformGameManager.teamStatsDictionary[(int)(team)].Count && position < MAX_STATS_TEAM)
            {
                UpdateWholeScoreboardRecord(position,statistic,team);
            }

            //if(team == PlatformGameManager.TeamColor.BLUE_TEAM)
            //    print(position);
            platformGameManager.teamsPositionDictionary[(int)(team)].Add(statistic.playerNum,position);
            teamPositionsList[(int)(team)].Add(statistic);
            //if(team == PlatformGameManager.TeamColor.BLUE_TEAM)
            //    print(teamPositionsList[(int)(team)].Count);
            position++;
        }
        }
        else
        {
            if(platformGameManager.vaccinatedGameTeamStatDictionary[(int)(team)].Count < MAX_STATS_TEAM) // There is less than the greatest amoun to of players than can be put on the scoreboard (16), disable all extra records.
            {
                for(int i = platformGameManager.vaccinatedGameTeamStatDictionary[(int)(team)].Count; i < MAX_STATS_TEAM; i++)
                {
                    DisableScoreboardRecord(i,team);
                }
            }

            int position = 0;

            foreach(PlatformGameManager.PlayerStatistic statistic in platformGameManager.vaccinatedGameTeamStatDictionary[(int)(team)].Values)
            {

                if(position < platformGameManager.vaccinatedGameTeamStatDictionary[(int)(team)].Count && position < MAX_STATS_TEAM)
                {
                    UpdateWholeScoreboardRecord(position,statistic,team);
                }

                //if(team == PlatformGameManager.TeamColor.BLUE_TEAM)
                //    print(position);
                platformGameManager.vaccinatedGameTeamPositionDictionary[(int)(team)].Add(statistic.playerNum,position);
                teamPositionsList[(int)(team)].Add(statistic);
                //if(team == PlatformGameManager.TeamColor.BLUE_TEAM)
                //    print(teamPositionsList[(int)(team)].Count);
                position++;
            }
        }
    }

    void SwapRecords(int oldPosition, int newPosition, bool updatedFrag, bool updatedDeath)
    {
        
        //PlatformGameManager.PlayerStatistic temp = positionsList[oldPosition];

        int tempPos = platformGameManager.positionDictionary[positionsList[newPosition].playerNum]; //Swap out the positions in the platformgamemanger position dictionary for the two records.
        platformGameManager.positionDictionary[positionsList[newPosition].playerNum] = oldPosition; 
        platformGameManager.positionDictionary[positionsList[oldPosition].playerNum] = tempPos;

        PlatformGameManager.PlayerStatistic temp = new PlatformGameManager.PlayerStatistic(positionsList[newPosition].player,positionsList[newPosition].frags,positionsList[newPosition].deaths, positionsList[newPosition].points); // Swap out the positions in the position list.
        positionsList[newPosition] = new PlatformGameManager.PlayerStatistic(positionsList[oldPosition].player,positionsList[oldPosition].frags,positionsList[oldPosition].deaths, positionsList[oldPosition].points);
        positionsList[oldPosition] = temp;

        /*print(oldPosition);
        print(newPosition);

        print("old pos name pos list"+positionsList[oldPosition].playerName);
        print("old pos name pos list frags"+positionsList[oldPosition].frags);
        print("stats dict old pos name"+platformGameManager.statsDictionary[positionsList[oldPosition].playerNum].playerName);
        print("stats dict old pos frags"+platformGameManager.statsDictionary[positionsList[oldPosition].playerNum].frags);

        print("new pos name pos list"+positionsList[newPosition].playerName);
        print("new pos name pos frags lists"+positionsList[newPosition].frags);
        print("stats dict new pos name"+platformGameManager.statsDictionary[positionsList[newPosition].playerNum].playerName);
        print("stats dict new pos frags"+platformGameManager.statsDictionary[positionsList[newPosition].playerNum].frags);*/

        if(oldPosition <= MAX_STATS_REGULAR - 1) // If the statistic we are swapping for is within the scoreboard range, update the scoreboard.
        {
            UpdateWholeScoreboardRecord(oldPosition,positionsList[oldPosition]);
        }

        UpdateRecord(positionsList[newPosition].playerNum,updatedFrag,updatedDeath);
    }

    void SwapRecords(int oldPosition, int newPosition, bool updatedFrag, bool updatedDeath, bool updatedPoint, PlatformGameManager.TeamColor team, int playerNum)
    {
        if(platformGameManager.gameType != PlatformGameManager.GameType.VACCINATION)
        {
            if(platformGameManager.gameType == PlatformGameManager.GameType.CAPTURE_THE_FLAG || platformGameManager.gameType == PlatformGameManager.GameType.SABOTAGE)
            {
                if(newPosition < oldPosition)
                {
                    print("debug");
                    HandleIcon(true,oldPosition,team,playerNum);
                }
                else
                {
                    print("debug2");
                    HandleIcon(false,oldPosition,team,playerNum);
                }
            }

            int tempPos = platformGameManager.teamsPositionDictionary[(int)team][teamPositionsList[(int)team][newPosition].playerNum]; //Swap out the positions in the platformgamemanger position dictionary for the two records.
            platformGameManager.teamsPositionDictionary[(int)team][teamPositionsList[(int)team][newPosition].playerNum] = oldPosition; 
            platformGameManager.teamsPositionDictionary[(int)team][teamPositionsList[(int)team][oldPosition].playerNum] = tempPos;

            PlatformGameManager.PlayerStatistic temp = new PlatformGameManager.PlayerStatistic(teamPositionsList[(int)team][newPosition].player,teamPositionsList[(int)team][newPosition].frags,teamPositionsList[(int)team][newPosition].deaths, teamPositionsList[(int)team][newPosition].points); // Swap out the positions in the position list.
            teamPositionsList[(int)team][newPosition] = new PlatformGameManager.PlayerStatistic(teamPositionsList[(int)team][oldPosition].player,teamPositionsList[(int)team][oldPosition].frags,teamPositionsList[(int)team][oldPosition].deaths, teamPositionsList[(int)team][oldPosition].points);
            teamPositionsList[(int)team][oldPosition] = temp;

            if(oldPosition <= MAX_STATS_TEAM - 1) // If the statistic we are swapping for is within the scoreboard range, update the scoreboard.
            {
                UpdateWholeScoreboardRecord(oldPosition,teamPositionsList[(int)team][oldPosition],team);
            }

            UpdateRecord(teamPositionsList[(int)team][newPosition].playerNum,updatedFrag,updatedDeath,updatedPoint,team);
        }
        else
        {
            int vacTeamNum;

            if(team == PlatformGameManager.TeamColor.VACCINATED_TEAM)
                vacTeamNum = 0;
            else
                vacTeamNum = 1;

                int tempPos = platformGameManager.vaccinatedGameTeamPositionDictionary[vacTeamNum][teamPositionsList[vacTeamNum][newPosition].playerNum]; //Swap out the positions in the platformgamemanger position dictionary for the two records.
                platformGameManager.vaccinatedGameTeamPositionDictionary[vacTeamNum][teamPositionsList[vacTeamNum][newPosition].playerNum] = oldPosition; 
                platformGameManager.vaccinatedGameTeamPositionDictionary[vacTeamNum][teamPositionsList[vacTeamNum][oldPosition].playerNum] = tempPos;

                PlatformGameManager.PlayerStatistic temp = new PlatformGameManager.PlayerStatistic(teamPositionsList[vacTeamNum][newPosition].player,teamPositionsList[vacTeamNum][newPosition].frags,teamPositionsList[vacTeamNum][newPosition].deaths, teamPositionsList[vacTeamNum][newPosition].points); // Swap out the positions in the position list.
                teamPositionsList[vacTeamNum][newPosition] = new PlatformGameManager.PlayerStatistic(teamPositionsList[vacTeamNum][oldPosition].player,teamPositionsList[vacTeamNum][oldPosition].frags,teamPositionsList[vacTeamNum][oldPosition].deaths, teamPositionsList[vacTeamNum][oldPosition].points);
                teamPositionsList[vacTeamNum][oldPosition] = temp;

                if(oldPosition <= MAX_STATS_TEAM - 1) // If the statistic we are swapping for is within the scoreboard range, update the scoreboard.
                {
                    UpdateWholeScoreboardRecord(oldPosition,teamPositionsList[vacTeamNum][oldPosition],team);
                }

                UpdateRecord(teamPositionsList[vacTeamNum][newPosition].playerNum,updatedFrag,updatedDeath,updatedPoint,team);
        }
    }

    void HandleIcon(bool movePositionUp, int position, PlatformGameManager.TeamColor team, int playerNum)
    {
        if(position == MAX_STATS_TEAM - 1)
            print("last pos");

        if(position < MAX_STATS_TEAM - 1) //If our icon and the icon either below or above is off return. (Nothing needs changing)
        {
            switch(team)
            {
                case PlatformGameManager.TeamColor.RED_TEAM:
                    if(movePositionUp)
                    {
                        if(!iconsRedTeamArray[position].enabled && !iconsRedTeamArray[position - 1].enabled)
                        {

                            return;
                        }
                    }
                    else
                    {
                        if(!iconsRedTeamArray[position].enabled && !iconsRedTeamArray[position + 1].enabled)
                        {

                            return;
                        }
                    }
                    break;
            
                case PlatformGameManager.TeamColor.BLUE_TEAM:
                    if(movePositionUp)
                    {
                        if(!iconsBlueTeamArray[position].enabled && !iconsBlueTeamArray[position - 1].enabled)
                        {

                            return;
                        }
                    }
                    else
                    {
                        if(!iconsBlueTeamArray[position].enabled && !iconsBlueTeamArray[position + 1].enabled)
                        {
                            return;
                        }
                    }
                    break;
            
                case PlatformGameManager.TeamColor.GREEN_TEAM:
                    if(movePositionUp)
                    {
                        if(!iconsGreenTeamArray[position].enabled && !iconsGreenTeamArray[position - 1].enabled)
                        {

                            return;
                        }
                    }
                    else
                    {
                        if(!iconsGreenTeamArray[position].enabled && !iconsGreenTeamArray[position + 1].enabled)
                        {

                            return;
                        }
                    }
                    break;
            
                case PlatformGameManager.TeamColor.GOLD_TEAM:
                    if(movePositionUp)
                    {
                        if(!iconsGoldTeamArray[position].enabled && !iconsGoldTeamArray[position - 1].enabled)
                        {

                            return;
                        }
                    }
                    else
                    {
                        if(!iconsGoldTeamArray[position].enabled && !iconsGoldTeamArray[position + 1].enabled)
                        {

                            return;
                        }
                    }
                    break;
            }
        }

        if(movePositionUp && position > MAX_STATS_TEAM || !movePositionUp && position > MAX_STATS_TEAM - 1) // If we are moving up or down and the new position is outside the length of the array return. (Ex: There's no point in changing icons for player in position 11 and 10 since they're not on the scoreboard and its outside the length of the array anyways.)
        {
            return;
        }
        const string redFlag = "redflagicon"; // Sprite names of icons.
        const string blueFlag = "blueflagicon";
        const string bomb = "bombicon";
    
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM) //If we're at position 9 (8 in array, one more than it can hold) and are swapping up, turn on the last position icon (8th, 7th in array) if we are holding a flag or a bomb, otherwise turn off the icon.
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                        {
                            iconsRedTeamArray[position - 1].enabled = true;
                            iconsRedTeamArray[position - 1].sprite = bombIcon;
                        }
                    
                        else if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsRedTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsRedTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsRedTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                        else //Turn off the final scoreboard position icon we are not holding anything
                        {
                            iconsRedTeamArray[position - 1].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsRedTeamArray[position - 1].enabled)
                        {
                            if(iconsRedTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsRedTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsRedTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsRedTeamArray[position].enabled) //If our icon is enabled and we are swapping up, turn on the icon of the position above and set its sprite to our sprite.
                        {
                            iconsRedTeamArray[position - 1].enabled = true;
                            iconsRedTeamArray[position - 1].sprite = iconsRedTeamArray[position].sprite;
                        }
                        else // Turn off the icon above us because our icon is off.
                        {
                            iconsRedTeamArray[position - 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // The player above us has a bomb, put it into our position.
                        {
                            print("debug move bomb icon down moving up");
                            iconsRedTeamArray[position].sprite = bombIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // The player above us has a red flag, put it into our position.
                        {
                            iconsRedTeamArray[position].sprite = redFlagIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // The player above us has a blue flag, put it into our position.
                        {
                            iconsRedTeamArray[position].sprite = blueFlagIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else // The player above us does not have anything (vacinnated = nothing),turn off our position icon.
                        {
                            iconsRedTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {

                    if(position == MAX_STATS_TEAM - 1) // If we are at the final position on the scoreboard (8th, 7th in array) and are moving down, check if the player in the position below us has a bomb or flag and turn on the icon if so.
                    {
                        if(platformGameManager.playersDictionary[teamPositionsList[0][MAX_STATS_TEAM].playerNum].bigKaboomahCarried != null)
                        {
                            iconsRedTeamArray[position].sprite = bombIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }

                        else if(platformGameManager.playersDictionary[teamPositionsList[0][MAX_STATS_TEAM].playerNum].flagCarried != null)
                        {
                            if(platformGameManager.playersDictionary[teamPositionsList[0][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsRedTeamArray[position].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[teamPositionsList[0][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsRedTeamArray[position].sprite = blueFlagIcon;
                        
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else // The player below us does not have a bomb or flag, turn off our position icon.
                        {
                            iconsRedTeamArray[position].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsRedTeamArray[position + 1].enabled)
                        {
                            if(iconsRedTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsRedTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsRedTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsRedTeamArray[position].enabled) //If our icon is enabled and we are swapping down, turn on the icon of the position below and set its sprite to our sprite.
                        {
                            iconsRedTeamArray[position + 1].enabled = true;
                            iconsRedTeamArray[position + 1].sprite = iconsRedTeamArray[position].sprite;
                        }
                        else // Turn off the icon below us because our icon is off.
                        {
                            iconsRedTeamArray[position + 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // We have a bomb, turn on the bomb icon on the position below us.
                        {
                            iconsRedTeamArray[position].sprite = bombIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // We have a red flag, turn on the red flag icon on the position below us.
                        {
                            iconsRedTeamArray[position].sprite = redFlagIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // We have a blue flag, turn on the blue flag icon on the position below us.
                        {
                            iconsRedTeamArray[position].sprite = blueFlagIcon;
                            iconsRedTeamArray[position].enabled = true;
                        }
                        else // We don't have anything turn off the icon below us.
                        {
                            iconsRedTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

            case PlatformGameManager.TeamColor.BLUE_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM) //If we're at position 9 (8 in array, one more than it can hold) and are swapping up, turn on the last position icon (8th, 7th in array) if we are holding a flag or a bomb, otherwise turn off the icon.
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                        {
                            iconsBlueTeamArray[position - 1].enabled = true;
                            iconsBlueTeamArray[position - 1].sprite = bombIcon;
                        }
                    
                        else if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsBlueTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsBlueTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsBlueTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                        else //Turn off the final scoreboard position icon we are not holding anything
                        {
                            iconsBlueTeamArray[position - 1].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsBlueTeamArray[position - 1].enabled)
                        {
                            if(iconsBlueTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsBlueTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsBlueTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsBlueTeamArray[position].enabled) //If our icon is enabled and we are swapping up, turn on the icon of the position above and set its sprite to our sprite.
                        {
                            iconsBlueTeamArray[position - 1].enabled = true;
                            iconsBlueTeamArray[position - 1].sprite = iconsBlueTeamArray[position].sprite;
                        }
                        else // Turn off the icon above us because our icon is off.
                        {
                            iconsBlueTeamArray[position - 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // The player above us has a bomb, put it into our position.
                        {
                            print("debug move bomb icon down moving up");
                            iconsBlueTeamArray[position].sprite = bombIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // The player above us has a red flag, put it into our position.
                        {
                            iconsBlueTeamArray[position].sprite = redFlagIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // The player above us has a blue flag, put it into our position.
                        {
                            iconsBlueTeamArray[position].sprite = blueFlagIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else // The player above us does not have anything (vacinnated = nothing),turn off our position icon.
                        {
                            iconsBlueTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {

                    if(position == MAX_STATS_TEAM - 1) // If we are at the final position on the scoreboard (8th, 7th in array) and are moving down, check if the player in the position below us has a bomb or flag and turn on the icon if so.
                    {
                        if(platformGameManager.playersDictionary[teamPositionsList[1][MAX_STATS_TEAM].playerNum].bigKaboomahCarried != null)
                        {
                            iconsBlueTeamArray[position].sprite = bombIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }

                        else if(platformGameManager.playersDictionary[teamPositionsList[1][MAX_STATS_TEAM].playerNum].flagCarried != null)
                        {
                            if(platformGameManager.playersDictionary[teamPositionsList[1][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsBlueTeamArray[position].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[teamPositionsList[1][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsBlueTeamArray[position].sprite = blueFlagIcon;
                        
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else // The player below us does not have a bomb or flag, turn off our position icon.
                        {
                            iconsBlueTeamArray[position].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsBlueTeamArray[position + 1].enabled)
                        {
                            if(iconsBlueTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsBlueTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsBlueTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsBlueTeamArray[position].enabled) //If our icon is enabled and we are swapping down, turn on the icon of the position below and set its sprite to our sprite.
                        {
                            iconsBlueTeamArray[position + 1].enabled = true;
                            iconsBlueTeamArray[position + 1].sprite = iconsBlueTeamArray[position].sprite;
                        }
                        else // Turn off the icon below us because our icon is off.
                        {
                            iconsBlueTeamArray[position + 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // We have a bomb, turn on the bomb icon on the position below us.
                        {
                            iconsBlueTeamArray[position].sprite = bombIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // We have a red flag, turn on the red flag icon on the position below us.
                        {
                            iconsBlueTeamArray[position].sprite = redFlagIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // We have a blue flag, turn on the blue flag icon on the position below us.
                        {
                            iconsBlueTeamArray[position].sprite = blueFlagIcon;
                            iconsBlueTeamArray[position].enabled = true;
                        }
                        else // We don't have anything turn off the icon below us.
                        {
                            iconsBlueTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

            case PlatformGameManager.TeamColor.GREEN_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM) //If we're at position 9 (8 in array, one more than it can hold) and are swapping up, turn on the last position icon (8th, 7th in array) if we are holding a flag or a bomb, otherwise turn off the icon.
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                        {
                            iconsGreenTeamArray[position - 1].enabled = true;
                            iconsGreenTeamArray[position - 1].sprite = bombIcon;
                        }
                    
                        else if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsGreenTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGreenTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGreenTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                        else //Turn off the final scoreboard position icon we are not holding anything
                        {
                            iconsGreenTeamArray[position - 1].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGreenTeamArray[position - 1].enabled)
                        {
                            if(iconsGreenTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGreenTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGreenTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsGreenTeamArray[position].enabled) //If our icon is enabled and we are swapping up, turn on the icon of the position above and set its sprite to our sprite.
                        {
                            iconsGreenTeamArray[position - 1].enabled = true;
                            iconsGreenTeamArray[position - 1].sprite = iconsGreenTeamArray[position].sprite;
                        }
                        else // Turn off the icon above us because our icon is off.
                        {
                            iconsGreenTeamArray[position - 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // The player above us has a bomb, put it into our position.
                        {
                            print("debug move bomb icon down moving up");
                            iconsGreenTeamArray[position].sprite = bombIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // The player above us has a red flag, put it into our position.
                        {
                            iconsGreenTeamArray[position].sprite = redFlagIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // The player above us has a blue flag, put it into our position.
                        {
                            iconsGreenTeamArray[position].sprite = blueFlagIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else // The player above us does not have anything (vacinnated = nothing),turn off our position icon.
                        {
                            iconsGreenTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {

                    if(position == MAX_STATS_TEAM - 1) // If we are at the final position on the scoreboard (8th, 7th in array) and are moving down, check if the player in the position below us has a bomb or flag and turn on the icon if so.
                    {
                        if(platformGameManager.playersDictionary[teamPositionsList[2][MAX_STATS_TEAM].playerNum].bigKaboomahCarried != null)
                        {
                            iconsGreenTeamArray[position].sprite = bombIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }

                        else if(platformGameManager.playersDictionary[teamPositionsList[2][MAX_STATS_TEAM].playerNum].flagCarried != null)
                        {
                            if(platformGameManager.playersDictionary[teamPositionsList[2][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGreenTeamArray[position].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[teamPositionsList[2][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGreenTeamArray[position].sprite = blueFlagIcon;
                        
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else // The player below us does not have a bomb or flag, turn off our position icon.
                        {
                            iconsGreenTeamArray[position].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGreenTeamArray[position + 1].enabled)
                        {
                            if(iconsGreenTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGreenTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGreenTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsGreenTeamArray[position].enabled) //If our icon is enabled and we are swapping down, turn on the icon of the position below and set its sprite to our sprite.
                        {
                            iconsGreenTeamArray[position + 1].enabled = true;
                            iconsGreenTeamArray[position + 1].sprite = iconsGreenTeamArray[position].sprite;
                        }
                        else // Turn off the icon below us because our icon is off.
                        {
                            iconsGreenTeamArray[position + 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // We have a bomb, turn on the bomb icon on the position below us.
                        {
                            iconsGreenTeamArray[position].sprite = bombIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // We have a red flag, turn on the red flag icon on the position below us.
                        {
                            iconsGreenTeamArray[position].sprite = redFlagIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // We have a blue flag, turn on the blue flag icon on the position below us.
                        {
                            iconsGreenTeamArray[position].sprite = blueFlagIcon;
                            iconsGreenTeamArray[position].enabled = true;
                        }
                        else // We don't have anything turn off the icon below us.
                        {
                            iconsGreenTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

          case PlatformGameManager.TeamColor.GOLD_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM) //If we're at position 9 (8 in array, one more than it can hold) and are swapping up, turn on the last position icon (8th, 7th in array) if we are holding a flag or a bomb, otherwise turn off the icon.
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                        {
                            iconsGoldTeamArray[position - 1].enabled = true;
                            iconsGoldTeamArray[position - 1].sprite = bombIcon;
                        }
                    
                        else if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsGoldTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGoldTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGoldTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                        else //Turn off the final scoreboard position icon we are not holding anything
                        {
                            iconsGoldTeamArray[position - 1].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGoldTeamArray[position - 1].enabled)
                        {
                            if(iconsGoldTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGoldTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGoldTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsGoldTeamArray[position].enabled) //If our icon is enabled and we are swapping up, turn on the icon of the position above and set its sprite to our sprite.
                        {
                            iconsGoldTeamArray[position - 1].enabled = true;
                            iconsGoldTeamArray[position - 1].sprite = iconsGoldTeamArray[position].sprite;
                        }
                        else // Turn off the icon above us because our icon is off.
                        {
                            iconsGoldTeamArray[position - 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // The player above us has a bomb, put it into our position.
                        {
                            print("debug move bomb icon down moving up");
                            iconsGoldTeamArray[position].sprite = bombIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // The player above us has a red flag, put it into our position.
                        {
                            iconsGoldTeamArray[position].sprite = redFlagIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // The player above us has a blue flag, put it into our position.
                        {
                            iconsGoldTeamArray[position].sprite = blueFlagIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else // The player above us does not have anything (vacinnated = nothing),turn off our position icon.
                        {
                            iconsGoldTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {

                    if(position == MAX_STATS_TEAM - 1) // If we are at the final position on the scoreboard (8th, 7th in array) and are moving down, check if the player in the position below us has a bomb or flag and turn on the icon if so.
                    {
                        if(platformGameManager.playersDictionary[teamPositionsList[3][MAX_STATS_TEAM].playerNum].bigKaboomahCarried != null)
                        {
                            iconsGoldTeamArray[position].sprite = bombIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }

                        else if(platformGameManager.playersDictionary[teamPositionsList[3][MAX_STATS_TEAM].playerNum].flagCarried != null)
                        {
                            if(platformGameManager.playersDictionary[teamPositionsList[3][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGoldTeamArray[position].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[teamPositionsList[3][MAX_STATS_TEAM].playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGoldTeamArray[position].sprite = blueFlagIcon;
                        
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else // The player below us does not have a bomb or flag, turn off our position icon.
                        {
                            iconsGoldTeamArray[position].enabled = false;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGoldTeamArray[position + 1].enabled)
                        {
                            if(iconsGoldTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGoldTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGoldTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        if(iconsGoldTeamArray[position].enabled) //If our icon is enabled and we are swapping down, turn on the icon of the position below and set its sprite to our sprite.
                        {
                            iconsGoldTeamArray[position + 1].enabled = true;
                            iconsGoldTeamArray[position + 1].sprite = iconsGoldTeamArray[position].sprite;
                        }
                        else // Turn off the icon below us because our icon is off.
                        {
                            iconsGoldTeamArray[position + 1].enabled = false;
                        }

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM) // We have a bomb, turn on the bomb icon on the position below us.
                        {
                            iconsGoldTeamArray[position].sprite = bombIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM) // We have a red flag, turn on the red flag icon on the position below us.
                        {
                            iconsGoldTeamArray[position].sprite = redFlagIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM) // We have a blue flag, turn on the blue flag icon on the position below us.
                        {
                            iconsGoldTeamArray[position].sprite = blueFlagIcon;
                            iconsGoldTeamArray[position].enabled = true;
                        }
                        else // We don't have anything turn off the icon below us.
                        {
                            iconsGoldTeamArray[position].enabled = false;
                        }
                    }
                }
                break;
                
        }
    }

    /*void HandleIcon(bool movePositionUp, int position, PlatformGameManager.TeamColor team, int playerNum)
    {
        if(position == MAX_STATS_TEAM - 1)
            print("last pos");

        if(position < MAX_STATS_TEAM)
        {
            switch(team)
            {
                case PlatformGameManager.TeamColor.RED_TEAM:
                    if(!iconsRedTeamArray[position].enabled)
                    {
                        print("debug return");
                        return;
                    }
                    break;
            
                case PlatformGameManager.TeamColor.BLUE_TEAM:
                    if(!iconsBlueTeamArray[position].enabled)
                        return;
                    break;
            
                case PlatformGameManager.TeamColor.GREEN_TEAM:
                    if(!iconsGreenTeamArray[position].enabled)
                        return;
                    break;
            
                case PlatformGameManager.TeamColor.GOLD_TEAM:
                    if(!iconsGoldTeamArray[position].enabled)
                        return;
                    break;
            }
        }

        if(movePositionUp && position > MAX_STATS_TEAM || !movePositionUp && position > MAX_STATS_TEAM - 1) // Don't do anything if our position is 9 or greater (the player will not have an effect on the scoreboard)
            return;

        const string redFlag = "redflagicon";
        const string blueFlag = "blueflagicon";
        const string bomb = "bombicon";
    
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                if(movePositionUp)
                {
                    print("debug moving up");
                    if(position == MAX_STATS_TEAM)
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                            iconsRedTeamArray[position - 1].enabled = true;
                            iconsRedTeamArray[position - 1].sprite = bombIcon;
                    
                        if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsRedTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsRedTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsRedTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsRedTeamArray[position - 1].enabled)
                        {
                            if(iconsRedTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsRedTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsRedTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsRedTeamArray[position - 1].enabled = true;
                        iconsRedTeamArray[position - 1].sprite = iconsRedTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsRedTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {
                    print("debug moving down");
                    if(position == MAX_STATS_TEAM - 1)
                    {
                        print("debug last position on array");
                        iconsRedTeamArray[position].enabled = false;
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsRedTeamArray[position + 1].enabled)
                        {
                            if(iconsRedTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsRedTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsRedTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsRedTeamArray[position + 1].enabled = true;
                        iconsRedTeamArray[position + 1].sprite = iconsRedTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsRedTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsRedTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

                case PlatformGameManager.TeamColor.BLUE_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM)
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                            iconsBlueTeamArray[position - 1].enabled = true;
                            iconsBlueTeamArray[position - 1].sprite = bombIcon;
                    
                        if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsBlueTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsBlueTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsBlueTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsBlueTeamArray[position - 1].enabled)
                        {
                            if(iconsBlueTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsBlueTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsBlueTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsBlueTeamArray[position - 1].enabled = true;
                        iconsBlueTeamArray[position - 1].sprite = iconsBlueTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsBlueTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {
                    if(position == MAX_STATS_TEAM - 1)
                    {
                        print("debug last position on array");
                        iconsBlueTeamArray[position].enabled = false;
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsBlueTeamArray[position + 1].enabled)
                        {
                            if(iconsBlueTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsBlueTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsBlueTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsBlueTeamArray[position + 1].enabled = true;
                        iconsBlueTeamArray[position + 1].sprite = iconsBlueTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsBlueTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsBlueTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

                case PlatformGameManager.TeamColor.GREEN_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM)
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                            iconsGreenTeamArray[position - 1].enabled = true;
                            iconsGreenTeamArray[position - 1].sprite = bombIcon;
                    
                        if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsGreenTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGreenTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGreenTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGreenTeamArray[position - 1].enabled)
                        {
                            if(iconsGreenTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGreenTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGreenTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsGreenTeamArray[position - 1].enabled = true;
                        iconsGreenTeamArray[position - 1].sprite = iconsGreenTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsGreenTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {
                    if(position == MAX_STATS_TEAM - 1)
                    {
                        print("debug last position on array");
                        iconsGreenTeamArray[position].enabled = false;
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGreenTeamArray[position + 1].enabled)
                        {
                            if(iconsGreenTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGreenTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGreenTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsGreenTeamArray[position + 1].enabled = true;
                        iconsGreenTeamArray[position + 1].sprite = iconsGreenTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsGreenTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsGreenTeamArray[position].enabled = false;
                        }
                    }
                }
                break;

                case PlatformGameManager.TeamColor.GOLD_TEAM:
                if(movePositionUp)
                {
                    if(position == MAX_STATS_TEAM)
                    {
                        if(platformGameManager.playersDictionary[playerNum].bigKaboomahCarried != null)
                            iconsGoldTeamArray[position - 1].enabled = true;
                            iconsGoldTeamArray[position - 1].sprite = bombIcon;
                    
                        if(platformGameManager.playersDictionary[playerNum].flagCarried != null)
                        {
                            iconsGoldTeamArray[position - 1].enabled = true;
                            if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.RED_TEAM)
                                iconsGoldTeamArray[position - 1].sprite = redFlagIcon;
                            else if(platformGameManager.playersDictionary[playerNum].flagCarried.flagColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                                iconsGoldTeamArray[position - 1].sprite = blueFlagIcon;
                        }
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGoldTeamArray[position - 1].enabled)
                        {
                            if(iconsGoldTeamArray[position - 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGoldTeamArray[position - 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGoldTeamArray[position - 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsGoldTeamArray[position - 1].enabled = true;
                        iconsGoldTeamArray[position - 1].sprite = iconsGoldTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsGoldTeamArray[position].enabled = false;
                        }

                    }
                }
                else
                {
                    if(position == MAX_STATS_TEAM - 1)
                    {
                        print("debug last position on array");
                        iconsGoldTeamArray[position].enabled = false;
                    }
                    else
                    {
                        PlatformGameManager.TeamColor tempColor = PlatformGameManager.TeamColor.VACCINATED_TEAM;

                        if(iconsGoldTeamArray[position + 1].enabled)
                        {
                            if(iconsGoldTeamArray[position + 1].sprite.name.Equals(bomb))
                                tempColor = PlatformGameManager.TeamColor.NO_TEAM;
                            else if(iconsGoldTeamArray[position + 1].sprite.name.Equals(redFlag))
                                tempColor = PlatformGameManager.TeamColor.RED_TEAM;
                            else if(iconsGoldTeamArray[position + 1].sprite.name.Equals(blueFlag))
                                tempColor = PlatformGameManager.TeamColor.BLUE_TEAM;
                        }

                        iconsGoldTeamArray[position + 1].enabled = true;
                        iconsGoldTeamArray[position + 1].sprite = iconsGoldTeamArray[position].sprite;

                        if(tempColor == PlatformGameManager.TeamColor.NO_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = bombIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.RED_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = redFlagIcon;
                        }
                        else if(tempColor == PlatformGameManager.TeamColor.BLUE_TEAM)
                        {
                            iconsGoldTeamArray[position].sprite = blueFlagIcon;
                        }
                        else
                        {
                            iconsGoldTeamArray[position].enabled = false;
                        }
                    }
                }
                break;
        }
    }*/
    public void UpdateRecord(int playerNumber, bool updatedFrag, bool updatedDeath) // We are just updating one record and not updating all the records in a stat dictionary (called when a player registers a death since deaths don't change ranking)
    {
        int positionPlayer = platformGameManager.positionDictionary[playerNumber]; // Get the player's position in rank. (It is stored in the position dictionary in the platform game manager and can be looked up by the player's number.)
        print("debug player frag for swap is"+positionsList[positionPlayer].frags);
        print("positionPlayer"+positionPlayer);
        //positionsList[positionPlayer] = platformGameManager.statsDictionary[playerNumber];

        if(updatedFrag)
        {
            if(positionPlayer != 0) // Don't swap up if we're at the top of the scoreboard (There is no posision -1).
            {
                if(positionsList[positionPlayer - 1].frags < positionsList[positionPlayer].frags) // If the position above us has less frags we do, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath);
                }
                else if(positionsList[positionPlayer - 1].frags == positionsList[positionPlayer].frags && positionsList[positionPlayer - 1].deaths > positionsList[positionPlayer].deaths) // If the position above us has the same frags but more deaths, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath);
                }
            }

            if(positionPlayer != positionsList.Count - 1) // Don't swap down if we're at the bottom of the scoreboard
            {
                if(positionsList[positionPlayer + 1].frags > positionsList[positionPlayer].frags) //If the position below us has more frags, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath);
                }
                else if(positionsList[positionPlayer + 1].frags == positionsList[positionPlayer].frags && positionsList[positionPlayer + 1].deaths < positionsList[positionPlayer].deaths) // If the position below us has the same amount of frags but less deaths, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath);
                }
            }

            //if(positionPlayer != )
        }
        else if(updatedDeath)
        {
            if(positionPlayer != positionsList.Count - 1) // If we record a death but our position is the bottom, don't swap down.
            {
                if(positionsList[positionPlayer + 1].frags == positionsList[positionPlayer].frags && positionsList[positionPlayer + 1].deaths < positionsList[positionPlayer].deaths) // If the position below us has the same amount of frags but less deaths, swap our position down.
                {
                    print("debug swap records");
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath);
                }
            }
        }

        if(positionPlayer <= MAX_STATS_REGULAR - 1) // If the statistic we are swapped to is within the scoreboard range, update the scoreboard.
        {
            print("debug updated final position record");
            print("final player pos"+platformGameManager.positionDictionary[playerNumber]);
            UpdateWholeScoreboardRecord(positionPlayer,positionsList[positionPlayer]);
        }
    }

    public void UpdateRecord(int playerNumber, bool updatedFrag, bool updatedDeath, bool updatedPoint, PlatformGameManager.TeamColor team) // We are just updating one record and not updating all the records in a stat dictionary (called when a player registers a death since deaths don't change ranking)
    {
        if(platformGameManager.gameType != PlatformGameManager.GameType.VACCINATION)
        {
        int positionPlayer = platformGameManager.teamsPositionDictionary[(int)team][playerNumber];

        //int positionPlayer = platformGameManager.positionDictionary[playerNumber]; // Get the player's position in rank. (It is stored in the position dictionary in the platform game manager and can be looked up by the player's number.)

        //positionsList[positionPlayer] = platformGameManager.statsDictionary[playerNumber];

        if(updatedPoint)
        {
            if(positionPlayer != 0)
            {
                if(teamPositionsList[(int)team][positionPlayer - 1].points < teamPositionsList[(int)team][positionPlayer].points) // If the position above us has less points than we do, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[(int)team][positionPlayer - 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer - 1].frags < teamPositionsList[(int)team][positionPlayer].frags) // If the position above use has the same points but less frags, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[(int)team][positionPlayer - 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer - 1].frags == teamPositionsList[(int)team][positionPlayer].frags && teamPositionsList[(int)team][positionPlayer - 1].deaths > teamPositionsList[(int)team][positionPlayer].deaths) // If the position above use has the same points and frags but more deaths, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }

            if(positionPlayer != teamPositionsList[(int)team].Count - 1) // Don't swap down if we're at the bottom of the scoreboard
            {
                if(teamPositionsList[(int)team][positionPlayer + 1].points > teamPositionsList[(int)team][positionPlayer].points) // If the position below us has more points than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[(int)team][positionPlayer + 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer + 1].frags > teamPositionsList[(int)team][positionPlayer].frags) // If the position below us has the sane points but more frags than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[(int)team][positionPlayer + 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer + 1].frags == teamPositionsList[(int)team][positionPlayer].frags && teamPositionsList[(int)team][positionPlayer + 1].deaths < teamPositionsList[(int)team][positionPlayer].deaths) // If the position below us has the sane points and frags but less deaths than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }

        if(updatedFrag)
        {
            if(positionPlayer != 0) // Don't swap up if we're at the top of the scoreboard (There is no posision -1).
            {
                if(teamPositionsList[(int)team][positionPlayer - 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer - 1].frags < teamPositionsList[(int)team][positionPlayer].frags) // If the position above us has has the same points but less frags than we do, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }

                else if(teamPositionsList[(int)team][positionPlayer - 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer - 1].frags == teamPositionsList[(int)team][positionPlayer].frags && teamPositionsList[(int)team][positionPlayer - 1].deaths > teamPositionsList[(int)team][positionPlayer].deaths) // If the position above us has the same points and frags but more deaths, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }

            if(positionPlayer != teamPositionsList[(int)team].Count - 1) // Don't swap down if we're at the bottom of the scoreboard
            {
                if(teamPositionsList[(int)team][positionPlayer + 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer + 1].frags > teamPositionsList[(int)team][positionPlayer].frags) // If the position below us has has the same points but more frags than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }

                else if(teamPositionsList[(int)team][positionPlayer + 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer + 1].frags == teamPositionsList[(int)team][positionPlayer].frags && teamPositionsList[(int)team][positionPlayer + 1].deaths < teamPositionsList[(int)team][positionPlayer].deaths) // If the position below us has the same points and frags but less deaths, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }
        else if(updatedDeath)
        {
            if(positionPlayer != teamPositionsList[(int)team].Count - 1) // If we record a death but our position is the bottom, don't swap down.
            {
                if(teamPositionsList[(int)team][positionPlayer + 1].points == teamPositionsList[(int)team][positionPlayer].points && teamPositionsList[(int)team][positionPlayer + 1].frags == teamPositionsList[(int)team][positionPlayer].frags && teamPositionsList[(int)team][positionPlayer + 1].deaths < teamPositionsList[(int)team][positionPlayer].deaths) // If the position below us has has the same points and frags but less deaths than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }

        if(positionPlayer <= MAX_STATS_TEAM - 1) // If the statistic we are swapped to is within the scoreboard range, update the scoreboard.
        {
            UpdateWholeScoreboardRecord(positionPlayer,teamPositionsList[(int)team][positionPlayer],team);
        }
        }
        else
        {
        int vacTeamNum;
        if(team == PlatformGameManager.TeamColor.VACCINATED_TEAM)
            vacTeamNum = 0;
        else
            vacTeamNum = 1;
        int positionPlayer = platformGameManager.vaccinatedGameTeamPositionDictionary[vacTeamNum][playerNumber];

        //int positionPlayer = platformGameManager.positionDictionary[playerNumber]; // Get the player's position in rank. (It is stored in the position dictionary in the platform game manager and can be looked up by the player's number.)

        //positionsList[positionPlayer] = platformGameManager.statsDictionary[playerNumber];

        if(updatedPoint)
        {
            if(positionPlayer != 0)
            {
                if(teamPositionsList[vacTeamNum][positionPlayer - 1].points < teamPositionsList[vacTeamNum][positionPlayer].points) // If the position above us has less points than we do, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[vacTeamNum][positionPlayer - 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer - 1].frags < teamPositionsList[vacTeamNum][positionPlayer].frags) // If the position above use has the same points but less frags, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[vacTeamNum][positionPlayer - 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer - 1].frags == teamPositionsList[vacTeamNum][positionPlayer].frags && teamPositionsList[vacTeamNum][positionPlayer - 1].deaths > teamPositionsList[vacTeamNum][positionPlayer].deaths) // If the position above use has the same points and frags but more deaths, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }

            if(positionPlayer != teamPositionsList[vacTeamNum].Count - 1) // Don't swap down if we're at the bottom of the scoreboard
            {
                if(teamPositionsList[vacTeamNum][positionPlayer + 1].points > teamPositionsList[vacTeamNum][positionPlayer].points) // If the position below us has more points than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[vacTeamNum][positionPlayer + 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer + 1].frags > teamPositionsList[vacTeamNum][positionPlayer].frags) // If the position below us has the sane points but more frags than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
                else if(teamPositionsList[vacTeamNum][positionPlayer + 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer + 1].frags == teamPositionsList[vacTeamNum][positionPlayer].frags && teamPositionsList[vacTeamNum][positionPlayer + 1].deaths < teamPositionsList[vacTeamNum][positionPlayer].deaths) // If the position below us has the sane points and frags but less deaths than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }

        if(updatedFrag)
        {
            if(positionPlayer != 0) // Don't swap up if we're at the top of the scoreboard (There is no posision -1).
            {
                if(teamPositionsList[vacTeamNum][positionPlayer - 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer - 1].frags < teamPositionsList[vacTeamNum][positionPlayer].frags) // If the position above us has has the same points but less frags than we do, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }

                else if(teamPositionsList[vacTeamNum][positionPlayer - 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer - 1].frags == teamPositionsList[vacTeamNum][positionPlayer].frags && teamPositionsList[vacTeamNum][positionPlayer - 1].deaths > teamPositionsList[vacTeamNum][positionPlayer].deaths) // If the position above us has the same points and frags but more deaths, swap our position up.
                {
                    SwapRecords(positionPlayer, positionPlayer - 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }

            if(positionPlayer != teamPositionsList[vacTeamNum].Count - 1) // Don't swap down if we're at the bottom of the scoreboard
            {
                if(teamPositionsList[vacTeamNum][positionPlayer + 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer + 1].frags > teamPositionsList[vacTeamNum][positionPlayer].frags) // If the position below us has has the same points but more frags than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }

                else if(teamPositionsList[vacTeamNum][positionPlayer + 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer + 1].frags == teamPositionsList[vacTeamNum][positionPlayer].frags && teamPositionsList[vacTeamNum][positionPlayer + 1].deaths < teamPositionsList[vacTeamNum][positionPlayer].deaths) // If the position below us has the same points and frags but less deaths, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }
        else if(updatedDeath)
        {
            if(positionPlayer != teamPositionsList[vacTeamNum].Count - 1) // If we record a death but our position is the bottom, don't swap down.
            {
                if(teamPositionsList[vacTeamNum][positionPlayer + 1].points == teamPositionsList[vacTeamNum][positionPlayer].points && teamPositionsList[vacTeamNum][positionPlayer + 1].frags == teamPositionsList[vacTeamNum][positionPlayer].frags && teamPositionsList[vacTeamNum][positionPlayer + 1].deaths < teamPositionsList[vacTeamNum][positionPlayer].deaths) // If the position below us has has the same points and frags but less deaths than we do, swap our position down.
                {
                    SwapRecords(positionPlayer, positionPlayer + 1,updatedFrag,updatedDeath,updatedPoint,team,playerNumber);
                }
            }
        }

        if(positionPlayer <= MAX_STATS_TEAM - 1) // If the statistic we are swapped to is within the scoreboard range, update the scoreboard.
        {
            UpdateWholeScoreboardRecord(positionPlayer,teamPositionsList[vacTeamNum][positionPlayer],team);
        }   
        }
    }

    void AdjustScoreboardTextXPosition()
    {
        print("debug adjusting scoreboard");
        print("max teams"+platformGameManager.maxTeams);

        ShiftScoreboardText(PlatformGameManager.TeamColor.RED_TEAM);
        ShiftScoreboardText(PlatformGameManager.TeamColor.BLUE_TEAM);
        if(platformGameManager.maxTeams >= 3)
            ShiftScoreboardText(PlatformGameManager.TeamColor.GREEN_TEAM);
        if(platformGameManager.maxTeams >= 4)
        {
            print("debug shift gold");
            ShiftScoreboardText(PlatformGameManager.TeamColor.GOLD_TEAM);
        }
    }

    void RearrangeColumnsTwoTeamColumnNonTDM()
    {
        const float COLUMN_1_X = 0f;
        const float COLUMN_2_X = 300f;
        const float COLUMN_3_X = 600f;
 
        redTeamPointsText.rectTransform.localPosition = new Vector2(COLUMN_1_X,redTeamPointsText.rectTransform.localPosition.y);
        redTeamKillsText.rectTransform.localPosition = new Vector2(COLUMN_2_X,redTeamKillsText.rectTransform.localPosition.y);
        redTeamDeathsText.rectTransform.localPosition = new Vector2(COLUMN_3_X,redTeamDeathsText.rectTransform.localPosition.y);

        blueTeamPointsText.rectTransform.localPosition = new Vector2(COLUMN_1_X,blueTeamPointsText.rectTransform.localPosition.y);
        blueTeamKillsText.rectTransform.localPosition = new Vector2(COLUMN_2_X,blueTeamKillsText.rectTransform.localPosition.y);
        blueTeamDeathsText.rectTransform.localPosition = new Vector2(COLUMN_3_X,blueTeamDeathsText.rectTransform.localPosition.y);

        for(int i = 0; i < MAX_STATS_TEAM; i++)
        {
            pointsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_1_X,namesRedTeamTextArrays[i].rectTransform.localPosition.y);
            fragsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_2_X,fragsRedTeamTextArrays[i].rectTransform.localPosition.y);
            deathsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_3_X,deathsRedTeamTextArrays[i].rectTransform.localPosition.y);
        
            pointsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_1_X,namesBlueTeamTextArrays[i].rectTransform.localPosition.y);
            fragsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_2_X,fragsBlueTeamTextArrays[i].rectTransform.localPosition.y);
            deathsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(COLUMN_3_X,deathsBlueTeamTextArrays[i].rectTransform.localPosition.y);
        }
    }

    void ShiftScoreboardText(PlatformGameManager.TeamColor team)
    {
        const float LEFT_TITLE_TEXT_NAMES_TITLE_X = -693.5f;
        const float LEFT_NAMES_COLUMN_X = -668.5f;
        const float LEFT_COLUMN_1_AND_TITLE_X = -493.5f;
        const float LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X = -343.5f;
        const float LEFT_COLUMN_3_AND_TITLE_X = -193.5f;
        const float LEFT_ICON_COLUMN = -893.5f;

        const float RIGHT_TITLE_TEXT_NAMES_TITLE_X = 256.5f;
        const float RIGHT_TOTAL_POINTS_TITLE_X = 606.5f;
        const float RIGHT_NAMES_COLUMN_X = 291.5f;
        const float RIGHT_COLUMN_1_AND_TITLE_X = 466.5f;
        const float RIGHT_COLUMN_2_AND_TITLE_X = 641.5f;
        const float RIGHT_COLUMN_3_AND_TITLE_X = 836.5f;
        const float RIGHT_ICON_COLUMN = 66.5f;

        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                print("debug shifting red");
                redTeamTitle.rectTransform.localPosition = new Vector2(LEFT_TITLE_TEXT_NAMES_TITLE_X, redTeamTitle.rectTransform.localPosition.y);
                redTeamTotalPoints.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, redTeamTotalPoints.rectTransform.localPosition.y);
                redTeamNamesText.rectTransform.localPosition = new Vector2(LEFT_TITLE_TEXT_NAMES_TITLE_X,redTeamNamesText.rectTransform.localPosition.y);
                
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                {
                    print("debug team dm");
                    redTeamKillsText.rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X, redTeamKillsText.rectTransform.localPosition.y);
                    redTeamDeathsText.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, redTeamDeathsText.rectTransform.localPosition.y);

                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_NAMES_COLUMN_X,namesRedTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X,fragsRedTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X,deathsRedTeamTextArrays[i].rectTransform.localPosition.y);
                    }
                }
                else
                {
                    print("debug not team dm");
                    redTeamPointsText.rectTransform.localPosition = new Vector3(LEFT_COLUMN_1_AND_TITLE_X, redTeamPointsText.rectTransform.localPosition.y);
                    redTeamKillsText.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, redTeamKillsText.rectTransform.localPosition.y);
                    redTeamDeathsText.rectTransform.localPosition = new Vector2(LEFT_COLUMN_3_AND_TITLE_X, redTeamDeathsText.rectTransform.localPosition.y);
                
                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        print("debug shifting red icon");
                        namesRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_NAMES_COLUMN_X,namesRedTeamTextArrays[i].rectTransform.localPosition.y);
                        pointsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X,pointsRedTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X,fragsRedTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsRedTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_3_AND_TITLE_X,deathsRedTeamTextArrays[i].rectTransform.localPosition.y);
                        iconsRedTeamArray[i].rectTransform.localPosition = new Vector2(LEFT_ICON_COLUMN,iconsRedTeamArray[i].rectTransform.localPosition.y);
                    }
                }
                break;

            case PlatformGameManager.TeamColor.BLUE_TEAM:
                blueTeamTitle.rectTransform.localPosition = new Vector2(LEFT_TITLE_TEXT_NAMES_TITLE_X, blueTeamTitle.rectTransform.localPosition.y);
                blueTeamTotalPoints.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, blueTeamTotalPoints.rectTransform.localPosition.y);
                blueTeamNamesText.rectTransform.localPosition = new Vector2(LEFT_TITLE_TEXT_NAMES_TITLE_X,blueTeamNamesText.rectTransform.localPosition.y);
                
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                {
                    blueTeamKillsText.rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X, blueTeamKillsText.rectTransform.localPosition.y);
                    blueTeamDeathsText.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, blueTeamDeathsText.rectTransform.localPosition.y);

                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_NAMES_COLUMN_X,namesBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X,fragsBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X,deathsBlueTeamTextArrays[i].rectTransform.localPosition.y);
                    }
                }
                else
                {
                    blueTeamPointsText.rectTransform.localPosition = new Vector3(LEFT_COLUMN_1_AND_TITLE_X, blueTeamPointsText.rectTransform.localPosition.y);
                    blueTeamKillsText.rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X, blueTeamKillsText.rectTransform.localPosition.y);
                    blueTeamDeathsText.rectTransform.localPosition = new Vector2(LEFT_COLUMN_3_AND_TITLE_X, blueTeamDeathsText.rectTransform.localPosition.y);
                
                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_NAMES_COLUMN_X,namesBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        pointsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_1_AND_TITLE_X,pointsBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_TOTAL_POINTS_AND_LEFT_COLUMN_2_AND_TITLE_X,fragsBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsBlueTeamTextArrays[i].rectTransform.localPosition = new Vector2(LEFT_COLUMN_3_AND_TITLE_X,deathsBlueTeamTextArrays[i].rectTransform.localPosition.y);
                        iconsBlueTeamArray[i].rectTransform.localPosition = new Vector2(LEFT_ICON_COLUMN,iconsBlueTeamArray[i].rectTransform.localPosition.y);
                    }
                }
                break;

            case PlatformGameManager.TeamColor.GREEN_TEAM:
                greenTeamTitle.rectTransform.localPosition = new Vector2(RIGHT_TITLE_TEXT_NAMES_TITLE_X, greenTeamTitle.rectTransform.localPosition.y);
                greenTeamTotalPoints.rectTransform.localPosition = new Vector2(RIGHT_TOTAL_POINTS_TITLE_X, greenTeamTotalPoints.rectTransform.localPosition.y);
                greenTeamNamesText.rectTransform.localPosition = new Vector2(RIGHT_TITLE_TEXT_NAMES_TITLE_X,greenTeamNamesText.rectTransform.localPosition.y);
                
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                {
                    greenTeamKillsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X, greenTeamKillsText.rectTransform.localPosition.y);
                    greenTeamDeathsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X, greenTeamDeathsText.rectTransform.localPosition.y);

                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_NAMES_COLUMN_X,namesGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X,fragsGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X,deathsGreenTeamTextArrays[i].rectTransform.localPosition.y);
                    }
                }
                else
                {
                    greenTeamPointsText.rectTransform.localPosition = new Vector3(RIGHT_COLUMN_1_AND_TITLE_X, greenTeamPointsText.rectTransform.localPosition.y);
                    greenTeamKillsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X, greenTeamKillsText.rectTransform.localPosition.y);
                    greenTeamDeathsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_3_AND_TITLE_X, greenTeamDeathsText.rectTransform.localPosition.y);
                
                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_NAMES_COLUMN_X,namesGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        pointsGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X,pointsGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_TOTAL_POINTS_TITLE_X,fragsGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsGreenTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_3_AND_TITLE_X,deathsGreenTeamTextArrays[i].rectTransform.localPosition.y);
                        iconsGreenTeamArray[i].rectTransform.localPosition = new Vector2(RIGHT_ICON_COLUMN,iconsGreenTeamArray[i].rectTransform.localPosition.y);
                    }
                }
                break;

            case PlatformGameManager.TeamColor.GOLD_TEAM:
                goldTeamTitle.rectTransform.localPosition = new Vector2(RIGHT_TITLE_TEXT_NAMES_TITLE_X, goldTeamTitle.rectTransform.localPosition.y);
                goldTeamTotalPoints.rectTransform.localPosition = new Vector2(RIGHT_TOTAL_POINTS_TITLE_X, goldTeamTotalPoints.rectTransform.localPosition.y);
                goldTeamNamesText.rectTransform.localPosition = new Vector2(RIGHT_TITLE_TEXT_NAMES_TITLE_X,goldTeamNamesText.rectTransform.localPosition.y);
                
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                {
                    goldTeamKillsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X, goldTeamKillsText.rectTransform.localPosition.y);
                    goldTeamDeathsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X, goldTeamDeathsText.rectTransform.localPosition.y);

                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_NAMES_COLUMN_X,namesGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X,fragsGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X,deathsGoldTeamTextArrays[i].rectTransform.localPosition.y);
                    }
                }
                else
                {
                    goldTeamPointsText.rectTransform.localPosition = new Vector3(RIGHT_COLUMN_1_AND_TITLE_X, goldTeamPointsText.rectTransform.localPosition.y);
                    goldTeamKillsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_2_AND_TITLE_X, goldTeamKillsText.rectTransform.localPosition.y);
                    goldTeamDeathsText.rectTransform.localPosition = new Vector2(RIGHT_COLUMN_3_AND_TITLE_X, goldTeamDeathsText.rectTransform.localPosition.y);
                
                    for(int i = 0; i < MAX_STATS_TEAM; i++)
                    {
                        namesGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_NAMES_COLUMN_X,namesGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        pointsGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_1_AND_TITLE_X,pointsGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        fragsGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_TOTAL_POINTS_TITLE_X,fragsGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        deathsGoldTeamTextArrays[i].rectTransform.localPosition = new Vector2(RIGHT_COLUMN_3_AND_TITLE_X,deathsGoldTeamTextArrays[i].rectTransform.localPosition.y);
                        iconsGoldTeamArray[i].rectTransform.localPosition = new Vector2(RIGHT_ICON_COLUMN,iconsGoldTeamArray[i].rectTransform.localPosition.y);
                    }
                }
                break;
        }
    }
    
    void DisableTeamStats(PlatformGameManager.TeamColor team)
    {
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                redTeamTitle.enabled = false;
                redTeamTotalPoints.enabled = false;
                redTeamNamesText.enabled = false;
                redTeamKillsText.enabled = false;
                redTeamDeathsText.enabled = false;
                redTeamPointsText.enabled = false;
                for(int i = 0; i < MAX_STATS_TEAM; i++)
                {
                    namesRedTeamTextArrays[i].enabled = false;
                    fragsRedTeamTextArrays[i].enabled = false; 
                    deathsRedTeamTextArrays[i].enabled = false;
                    pointsRedTeamTextArrays[i].enabled = false;
                }
                break;
            case PlatformGameManager.TeamColor.BLUE_TEAM:
                blueTeamTitle.enabled = false;
                blueTeamTotalPoints.enabled = false;
                blueTeamNamesText.enabled = false;
                blueTeamKillsText.enabled = false;
                blueTeamDeathsText.enabled = false;
                blueTeamPointsText.enabled = false;
                for(int i = 0; i < MAX_STATS_TEAM; i++)
                {
                    namesBlueTeamTextArrays[i].enabled = false;
                    fragsBlueTeamTextArrays[i].enabled = false; 
                    deathsBlueTeamTextArrays[i].enabled = false;
                    pointsBlueTeamTextArrays[i].enabled = false;
                }
                break;
            case PlatformGameManager.TeamColor.GREEN_TEAM:
                greenTeamTitle.enabled = false;
                greenTeamTotalPoints.enabled = false;
                greenTeamNamesText.enabled = false;
                greenTeamKillsText.enabled = false;
                greenTeamDeathsText.enabled = false;
                greenTeamPointsText.enabled = false;
                for(int i = 0; i < MAX_STATS_TEAM; i++)
                {
                    namesGreenTeamTextArrays[i].enabled = false;
                    fragsGreenTeamTextArrays[i].enabled = false; 
                    deathsGreenTeamTextArrays[i].enabled = false;
                    pointsGreenTeamTextArrays[i].enabled = false;
                }
                break;
            case PlatformGameManager.TeamColor.GOLD_TEAM:
                goldTeamTitle.enabled = false;
                goldTeamTotalPoints.enabled = false;
                goldTeamNamesText.enabled = false;
                goldTeamKillsText.enabled = false;
                goldTeamDeathsText.enabled = false;
                goldTeamPointsText.enabled = false;
                for(int i = 0; i < MAX_STATS_TEAM; i++)
                {
                    namesGoldTeamTextArrays[i].enabled = false;
                    fragsGoldTeamTextArrays[i].enabled = false; 
                    deathsGoldTeamTextArrays[i].enabled = false;
                    pointsGoldTeamTextArrays[i].enabled = false;
                }
                break;
        }
    }
    public void DrawTimeLimitText(int limit)
    {
        int minutes = limit / 60;
        int seconds = limit % 60;

        string secondsString = string.Format("{0:00}",seconds);
        string minutesString = string.Format("{0:00}",minutes);

        string timerString = "Time Left: "+minutesString+":"+secondsString;

        timeLeftText.text = timerString;
    }

    public void UpdatePointsForTeam(PlatformGameManager.TeamColor team)
    {
        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    redTeamTotalPoints.text = "Total Frags : "+platformGameManager.pointsRed.ToString();
                else
                    redTeamTotalPoints.text = "Total Points : "+platformGameManager.pointsRed.ToString();
                break;
            
            case PlatformGameManager.TeamColor.BLUE_TEAM:
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    blueTeamTotalPoints.text = "Total Frags : "+platformGameManager.pointsBlue.ToString();
                else
                    blueTeamTotalPoints.text = "Total Points : "+platformGameManager.pointsBlue.ToString();
                break;
            
            case PlatformGameManager.TeamColor.GREEN_TEAM:
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    greenTeamTotalPoints.text = "Total Frags : "+platformGameManager.pointsGreen.ToString();
                else
                    greenTeamTotalPoints.text = "Total Points : "+platformGameManager.pointsGreen.ToString();
                break;
            
            case PlatformGameManager.TeamColor.GOLD_TEAM:
                if(platformGameManager.gameType == PlatformGameManager.GameType.TEAM_DEATHMATCH)
                    goldTeamTotalPoints.text = "Total Frags : "+platformGameManager.pointsGold.ToString();
                else
                    goldTeamTotalPoints.text = "Total Points : "+platformGameManager.pointsGold.ToString();
                break;

        }
    }

    void DisableAllIcons()
    {
        for(int i = 0; i < MAX_STATS_TEAM; i++)
        {
            iconsRedTeamArray[i].enabled = false;
            iconsBlueTeamArray[i].enabled = false;
            iconsGreenTeamArray[i].enabled = false;
            iconsGoldTeamArray[i].enabled = false;
        }
    }

    public void ChangeIconStatus(int playerNum, PlatformGameManager.TeamColor team, PlatformGameManager.TeamColor flagColor, bool enabled)
    {
        int pos = platformGameManager.teamsPositionDictionary[(int)team][playerNum];

        switch(team)
        {
            case PlatformGameManager.TeamColor.RED_TEAM:
                switch(flagColor)
                {
                    case PlatformGameManager.TeamColor.RED_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                                iconsRedTeamArray[pos].sprite = redFlagIcon;
                                iconsRedTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsRedTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.BLUE_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsRedTeamArray[pos].sprite = blueFlagIcon;
                            iconsRedTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsRedTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.NO_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsRedTeamArray[pos].sprite = bombIcon;
                            iconsRedTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsRedTeamArray[pos].enabled = false;
                            }
                        }
                        break;

                }
                break;

            case PlatformGameManager.TeamColor.BLUE_TEAM:
                switch(flagColor)
                {
                    case PlatformGameManager.TeamColor.RED_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsBlueTeamArray[pos].sprite = redFlagIcon;
                            iconsBlueTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsBlueTeamArray[pos].enabled = false;
                            }
                        }
                        break;

                    
                    case PlatformGameManager.TeamColor.BLUE_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsBlueTeamArray[pos].sprite = blueFlagIcon;
                            iconsBlueTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsBlueTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.NO_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsBlueTeamArray[pos].sprite = bombIcon;
                            iconsBlueTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsBlueTeamArray[pos].enabled = false;
                            }
                        }
                        break;

                }
                break;

            case PlatformGameManager.TeamColor.GREEN_TEAM:
                switch(flagColor)
                {
                    case PlatformGameManager.TeamColor.RED_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGreenTeamArray[pos].sprite = redFlagIcon;
                            iconsGreenTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGreenTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.BLUE_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGreenTeamArray[pos].sprite = blueFlagIcon;
                            iconsGreenTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGreenTeamArray[pos].enabled = false;
                            }
                            
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.NO_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGreenTeamArray[pos].sprite = bombIcon;
                            iconsGreenTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGreenTeamArray[pos].enabled = false;
                            }
                        }
                        break;

                }
                break;

            case PlatformGameManager.TeamColor.GOLD_TEAM:
                switch(flagColor)
                {
                    case PlatformGameManager.TeamColor.RED_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGoldTeamArray[pos].sprite = redFlagIcon;
                            iconsGoldTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGoldTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.BLUE_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGoldTeamArray[pos].sprite = blueFlagIcon;
                            iconsGoldTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGoldTeamArray[pos].enabled = false;
                            }
                        }
                        break;
                    
                    case PlatformGameManager.TeamColor.NO_TEAM:
                        if(pos <= 7)
                        {
                            if(enabled)
                            {
                            iconsGreenTeamArray[pos].sprite = bombIcon;
                            iconsGreenTeamArray[pos].enabled = true;
                            }
                            else
                            {
                                iconsGoldTeamArray[pos].enabled = false;
                            }
                        }
                        break;

                }
                break;
            
        }
    }
}
