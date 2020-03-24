using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class generates and assigns names to the 2 dance teams in our dance off battle.
/// It also controls the number of dancers on each team via the inspector
/// It also uses the name generator to pass character names to the teams so they can initialise
/// 
/// TODO:
///     Generate unique team names for both teams and assign them via team_.SetTroupeName(str);
///     Use the nameGenerator to get enough names for the number of dancers on both teams and pass the required names via array to each team for init (InitaliseTeamFromNames)
/// </summary>
public class DanceTeamInit : MonoBehaviour
{
    public DanceTeam teamA, teamB;

    public GameObject dancerPrefab;
    public int dancersPerSide = 3;
    public CharacterNameGenerator nameGenerator;

    private void OnEnable()
    {
        GameEvents.OnBattleInitialise += InitTeams;
    }
    private void OnDisable()
    {
        GameEvents.OnBattleInitialise -= InitTeams;
    }

    void InitTeams()
    {
        teamA.SetTroupeName("Team A");
        teamB.SetTroupeName("Team B");

        teamA.InitaliseTeamFromNames(dancerPrefab, -1, nameGenerator.GenerateNames(dancersPerSide));
        teamB.InitaliseTeamFromNames(dancerPrefab, 1, nameGenerator.GenerateNames(dancersPerSide));
    }
}
