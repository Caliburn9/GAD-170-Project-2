﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BattlesSystem handles the organisation of rounds, selecting the dancers to dance off from each side.
/// It then hands off to the fightManager to determine the outcome of 2 dancers dance off'ing.
/// 
/// TODO:
///     Needs to hand the request for a dance off battle round by selecting a dancer from each side and 
///         handing off to the fight manager, via GameEvents.RequestFight
///     Needs to handle GameEvents.OnFightComplete so that a new round can start
///     Needs to handle a team winning or loosing
///     This may be where characters are set as selected when they are in a dance off and when they leave the dance off
/// </summary>
public class BattleSystem : MonoBehaviour
{
    public DanceTeam TeamA,TeamB;

    public float battlePrepTime = 2;
    public float fightWinTime = 2;

    private void OnEnable()
    {
        GameEvents.OnRequestFighters += RoundRequested;
        GameEvents.OnFightComplete += FightOver;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestFighters -= RoundRequested;
        GameEvents.OnFightComplete -= FightOver;
    }

    void RoundRequested()
    {
        //calling the coroutine so we can put waits in for anims to play
        StartCoroutine(DoRound());
    }

    IEnumerator DoRound()
    {
        yield return new WaitForSeconds(battlePrepTime);

        if (TeamA.activeDancers.Count > 0 && TeamB.activeDancers.Count > 0)
        {
            int i = Random.Range(0, TeamA.activeDancers.Count);
            int j = Random.Range(0, TeamB.activeDancers.Count);

            Character characterA = TeamA.activeDancers[i];
            Character characterB = TeamB.activeDancers[j];

            GameEvents.RequestFight(new FightEventData(characterA, characterB));
        }
        else
        {
            DanceTeam winner;

            winner = TeamA.activeDancers.Count <= 0 ? TeamB : TeamA;

            GameEvents.BattleFinished(winner);
            winner.EnableWinEffects();

            BattleLog.Log(new DefaultLogMessage("The Winning Team is " + winner.troupeNameText.text, winner.teamColor));
        }
    }

    void FightOver(FightResultData data)
    {
        if (data.outcome != 0)
        {
            data.defeated.myTeam.RemoveFromActive(data.defeated);
            data.winner.myTeam.EnableWinEffects();
        }
        
        StartCoroutine(HandleFightOver());
    }

    IEnumerator HandleFightOver()
    {
        yield return new WaitForSeconds(fightWinTime);
        TeamA.DisableWinEffects();
        TeamB.DisableWinEffects();

        //TeamA.activeDancers.Clear();
        //TeamB.activeDancers.Clear();

        Debug.LogWarning("HandleFightOver called, may need to prepare or clean dancers or teams and checks before doing GameEvents.RequestFighters()");
        GameEvents.RequestFighters();
    }
}
