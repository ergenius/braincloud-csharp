﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

namespace BrainCloudUnity.HUD
{
	public class HUDPlayer : IHUDElement
	{
		SortedDictionary<string, string> m_attributes = new SortedDictionary<string, string>();
		Vector2 m_scrollPosition = new Vector2(0,0);

		public void OnHUDActivate()
		{
			BrainCloudLoginPF.BrainCloud.PlayerStateService.GetAttributes (GetAttributesSuccess, Failure);
		}
		
		public void OnHUDDeactivate()
		{

		}
		
		public string GetHUDTitle()
		{
			return "Player";
		}

		void GetAttributesSuccess(string json, object cb)
		{
			m_attributes.Clear ();

			JsonData jObj = JsonMapper.ToObject(json);
			JsonData jStats = jObj["data"]["attributes"];
			IDictionary dStats = jStats as IDictionary;
			if (dStats != null)
			{
				foreach (string key in dStats.Keys)
				{
					string name = (string) key;
					string value = (string) dStats[key];
					m_attributes[name] = value;
				}
			}
		}

		void ResetPlayerSuccess(string json, object cb)
		{
			// probably need to refresh game state... todo add a callback handler
		}

		void DeletePlayerSuccess(string json, object cb)
		{
			// definitely need to refresh game state... todo add a callback handler
		}
		
		void Failure(int statusCode, int reasonCode, string statusMessage, object cb)
		{
			Debug.LogError("Failed: " + statusMessage);
		}
		
		public void OnHUDDraw()
		{
			m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			foreach (string key in m_attributes.Keys)
			{
				GUILayout.Label(key);
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			foreach (string value in m_attributes.Values)
			{
				GUILayout.Box(value);
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();

			//spacer
			GUILayout.BeginVertical();
			GUILayout.Space(5);
			GUILayout.EndVertical();

			GUILayout.TextArea ("Reseting your player will delete all player data but will keep identities intact.");
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Reset Player"))
			{
				BrainCloudLoginPF.BrainCloud.PlayerStateService.ResetUser (ResetPlayerSuccess, Failure);
			}
			GUILayout.EndHorizontal ();

			GUILayout.TextArea ("Deleting your player will delete the player entirely. Player will need to reauthenticate and create new account");
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Delete Player"))
			{
				BrainCloudLoginPF.BrainCloud.PlayerStateService.DeleteUser (DeletePlayerSuccess, Failure);
			}
			
			if (GUILayout.Button ("Get Friend Example"))
			{
				BrainCloudLoginPF.BrainCloud.FriendService.FindUserByUniversalId("User", 4, (response, cbObject) =>
				{
				
					/**
 {
   "data":{
      "matchedCount":5,
      "message":"Result count exceeds maximum.",
      "matches":[
         {
            "profileId":"40683b0c-f3a7-4fc2-8ba1-d065247ec4b6",
            "profileName":"",
            "summaryFriendData":null,
            "pictureUrl":null,
            "externalId":"userb-14585208"
         },
         {
            "profileId":"36fccd98-26d7-4b74-a9c1-3de64be525a5",
            "profileName":"",
            "summaryFriendData":null,
            "pictureUrl":null,
            "externalId":"userb-14732606"
         },
         {
            "profileId":"67c1f191-c74f-4dfe-b8c0-61b74236fb51",
            "profileName":"",
            "summaryFriendData":null,
            "pictureUrl":null,
            "externalId":"user"
         },
         {
            "profileId":"b420b910-e8b1-4d3c-a9ad-c50a7fa832df",
            "profileName":"",
            "summaryFriendData":null,
            "pictureUrl":null,
            "externalId":"usera-10971514"
         }
      ]
   },
   "status":200
}
					 */
					
				}, (status, code, error, cbObject) =>
				{
					
				});
			}
			
			GUILayout.EndHorizontal ();

			GUILayout.EndScrollView();

			GUILayout.EndVertical ();
		}
		
	}
}